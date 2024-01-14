using FDR.Tools.Library;

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
        public ProcessInfo(Operation operation, CancellationTokenSource tokenSource, Task task)
        {
            StartedAt = DateTime.Now;
            Operation = operation;
            TokenSource = tokenSource;
            Task = task;
        }

        public Operation Operation { get; }

        public DateTime StartedAt { get; }

        public CancellationTokenSource? TokenSource { get; }

        public Task? Task { get; }
    }
}
