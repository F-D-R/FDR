using FDR.Tools.Library;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace FDR.Web
{
    public class DummyProcess
    {
        private readonly int run = new Random().Next(100, 999);

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
            }, token);
        }
    }

    public class ProcessInfo
    {
        private static int lastId = 0;

        public ProcessInfo(Operation operation, CancellationTokenSource cancellationTokenSource, Task task)
        {
            Id = Interlocked.Increment(ref lastId);
            StartedAt = DateTime.Now;
            Operation = operation;
            CancellationTokenSource = cancellationTokenSource;
            Task = task;
        }

        public int Id { get; }

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

        public ProcessInfo Start(Operation operation, string? folder = null, string? reffolder = null, bool verbose = false, bool force = false, bool auto = false, bool noactions = false, ConfigPartBase? tmpConfig = null)
        {
            CancellationTokenSource tokenSource = new();
            tokenSource.Token.ThrowIfCancellationRequested();

            Process process = new();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                process.StartInfo.FileName = "FDR.exe";
            else
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = "FDR.dll";
            }

            string param = string.Empty;
            switch (operation)
            {
                case Operation.Cleanup:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
                    param += $" {Common.param_cleanup} \"{folder}\"";
                    break;
                case Operation.Diff:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
                    ArgumentException.ThrowIfNullOrWhiteSpace(reffolder);
                    param += $" {Common.param_diff} \"{folder}\"";
                    param += $" {Common.param_reference} \"{reffolder}\"";
                    break;
                case Operation.Hash:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
                    param += $" {Common.param_hash} \"{folder}\"";
                    break;
                case Operation.Help:
                    param += $" {Common.param_help}";
                    break;
                case Operation.Import:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
                    param += $" {Common.param_import} \"{folder}\"";
                    break;
                case Operation.Rename:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
                    param += $" {Common.param_rename} \"{folder}\"";
                    break;
                case Operation.Resize:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
                    param += $" {Common.param_resize} \"{folder}\"";
                    break;
                case Operation.Verify:
                    ArgumentException.ThrowIfNullOrWhiteSpace(folder);
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
            if (auto) param += $" {Common.param_auto}";
            if (noactions) param += $" {Common.param_noactions}";

            string? tmpFile = null;
            if (tmpConfig != null)
            {
                tmpFile = CreateTmpConfigFile(tmpConfig);
                Console.WriteLine($"Temporary config file: {tmpFile}");
                param += $" {Common.param_config} {tmpKey} {Common.param_configfile} \"{tmpFile}\"";
            }

            Console.WriteLine($"Arguments: {param}");
            process.StartInfo.Arguments += param;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                Console.WriteLine(e.Data);

                if (sender is Process proc)
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

            ProcessInfo proc = new(operation, tokenSource, task) { PID = process.Id };
            this.Add(proc);

            return proc;
        }

        public string? CreateTmpConfigFile(ConfigPartBase config)
        {
            AppConfig appConfig = new();
            var file = Path.GetTempFileName();

            if (config is ImportConfig importConfig)
                appConfig.ImportConfigs.Add(tmpKey, importConfig);
            else if (config is ResizeConfig resizeConfig)
                appConfig.ResizeConfigs.Add(tmpKey, resizeConfig);
            else if (config is MoveConfig moveConfig)
                appConfig.MoveConfigs.Add(tmpKey, moveConfig);
            else if (config is RenameConfig renameConfig)
                appConfig.RenameConfigs.Add(tmpKey, renameConfig);

            AppConfig.SaveToFile(appConfig, file);
            return file;
        }
    }
}
