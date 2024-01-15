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
        public Processes() { }

        public void Add(Operation operation, CancellationTokenSource cancellationTokenSource, Task task)
        {
            this.Add(new(operation, cancellationTokenSource, task));
        }

        //public Task Start(CancellationTokenSource tokenSource, Operation operation, string folder, bool verbose)
        public Task Start(Operation operation, string? folder = null, string? config = null, bool verbose = false, bool force = false)
        {
            CancellationTokenSource tokenSource = new();
            tokenSource.Token.ThrowIfCancellationRequested();

            Process process = new Process();
            process.StartInfo.FileName = "FDR.exe";

            string param = string.Empty;
            switch (operation)
            {
                case Operation.Cleanup:
                    param += $"-cleanup \"{folder}\"";
                    break;
                case Operation.Diff:
                    param += $"-diff \"{folder}\"";
                    break;
                case Operation.Hash:
                    param += $"-hash \"{folder}\"";
                    break;
                case Operation.Help:
                    param += "-help";
                    break;
                case Operation.Import:
                    param += $"-import \"{folder}\"";
                    break;
                case Operation.Rename:
                    param += $"-rename \"{folder}\"";
                    break;
                case Operation.Resize:
                    param += $"-resize \"{folder}\"";
                    break;
                case Operation.Verify:
                    param += $"-verify \"{folder}\"";
                    break;
                case Operation.Web:
                    param += "-web";
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (verbose) param += " -verbose";
            if (force) param += " -force";

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

            process.Start();
            process.BeginOutputReadLine();

            //Console.WriteLine(process.StandardOutput.ReadToEnd());

            tokenSource.Token.Register(() => { process.Kill(); });

            //return process.WaitForExitAsync();
            var task = process.WaitForExitAsync();

            //this.Add(operation, tokenSource, task);
            ProcessInfo proc = new(operation, tokenSource, task);
            proc.PID = process.Id;
            this.Add(proc);

            return task;
        }
    }
}
