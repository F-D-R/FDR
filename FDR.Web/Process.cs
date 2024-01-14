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
    }

    public class Processes : List<ProcessInfo>
    {
        public Processes () { }

        public void  Add(Operation operation, CancellationTokenSource cancellationTokenSource, Task task)
        {
            this.Add(new(operation, cancellationTokenSource, task));
        }
    }
}
