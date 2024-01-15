using FDR.Tools.Library;
using System.Diagnostics;
using System.Text;

namespace FDR.Web
{
    public class DummyProcess
    {
        private int run = new Random().Next(100, 999);

        public async Task Start(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            await Task.Run(() =>
            {
                int i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    i++;
                    Console.WriteLine($"Process: {run}/{i}");
                    Thread.Sleep(1000);
                    if (i >= 100) break;
                }
            });
        }
    }

    public class ProcessInfo
    {
        public ProcessInfo(Operation operation, CancellationTokenSource cancellationTokenSource, Task task)
        {
            Id = Guid.NewGuid();
            StartedAt = DateTime.Now;
            Operation = operation;
            CancellationTokenSource = cancellationTokenSource;
            Task = task;
        }

        public Guid Id { get; }

        public Operation Operation { get; }

        public DateTime StartedAt { get; }

        public CancellationTokenSource? CancellationTokenSource { get; }

        public Task Task { get; }

        public int PID { get; set; }

        public StringBuilder Output { get; } = new StringBuilder();
    }

    public class Processes : List<ProcessInfo>
    {
        private const string tmpKey = "tmp";

        public Processes() { }

        public void Add(Operation operation, CancellationTokenSource cancellationTokenSource, Task task)
        {
            this.Add(new(operation, cancellationTokenSource, task));
        }

        public Task Start(Operation operation, string? folder = null, string? reffolder = null, string? config = null, bool verbose = false, bool force = false, ConfigPartBase? tmpConfig = null)
        {
            CancellationTokenSource tokenSource = new();
            tokenSource.Token.ThrowIfCancellationRequested();

            Process process = new Process();
            //TODO: Linux! (dotnet FDR.dll [options])
            process.StartInfo.FileName = "FDR.exe";

            string param = string.Empty;
            switch (operation)
            {
                case Operation.Cleanup:
                    if(string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    param += $" {Common.param_cleanup} \"{folder}\"";
                    break;
                case Operation.Diff:
                    if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    if (string.IsNullOrEmpty(reffolder)) throw new ArgumentNullException(nameof(reffolder));
                    param += $" {Common.param_diff} \"{folder}\"";
                    param += $" {Common.param_reference} \"{reffolder}\"";
                    break;
                case Operation.Hash:
                    if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    param += $" {Common.param_hash} \"{folder}\"";
                    break;
                case Operation.Help:
                    param += $" {Common.param_help}";
                    break;
                case Operation.Import:
                    if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    param += $" {Common.param_import} \"{folder}\"";
                    break;
                case Operation.Rename:
                    if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    param += $" {Common.param_rename} \"{folder}\"";
                    break;
                case Operation.Resize:
                    if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    param += $" {Common.param_resize} \"{folder}\"";
                    break;
                case Operation.Verify:
                    if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));
                    param += $" {Common.param_verify} \"{folder}\"";
                    break;
                case Operation.Web:
                    param += $" {Common.param_web}";
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (verbose) param += $" {Common.param_verbose}";
            if (force) param += $" {Common.param_force}";

            string? tmpFile = null;
            if (tmpConfig != null)
            {
                tmpFile = CreateTmpConfigFile(tmpConfig);
                Console.WriteLine($"Temporary config file: {tmpFile}");
                param += $" {Common.param_config} {tmpKey} {Common.param_configfile} \"{tmpFile}\"";
            }

            Console.WriteLine($"Arguments: {param}");
            process.StartInfo.Arguments = param;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                Console.WriteLine(e.Data);

                Process? proc = sender as Process;
                if (proc != null)
                {
                    var pi = this.Find(pi => pi.PID == proc.Id);
                    pi?.Output?.AppendLine(e.Data);
                }
            });

            process.Exited += new EventHandler((sender, e) =>
            {
                if (!string.IsNullOrEmpty(tmpFile) && File.Exists(tmpFile)) 
                    File.Delete(tmpFile);
            });

            process.Start();
            process.BeginOutputReadLine();

            tokenSource.Token.Register(() => { process.Kill(); });

            var task = process.WaitForExitAsync();

            ProcessInfo proc = new(operation, tokenSource, task);
            proc.PID = process.Id;
            this.Add(proc);

            return task;
        }

        public string? CreateTmpConfigFile(ConfigPartBase config)
        {
            AppConfig appConfig = new();
            var file = Path.GetTempFileName();

            if (config is ImportConfig)
                appConfig.ImportConfigs.Add(tmpKey, (ImportConfig)config);
            else if (config is ResizeConfig)
                appConfig.ResizeConfigs.Add(tmpKey, (ResizeConfig)config);
            else if (config is MoveConfig)
                appConfig.MoveConfigs.Add(tmpKey, (MoveConfig)config);
            else if (config is RenameConfig)
                appConfig.RenameConfigs.Add(tmpKey, (RenameConfig)config);

            AppConfig.SaveToFile(appConfig, file);
            return file;
        }
    }
}
