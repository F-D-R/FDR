using FDR.Tools.Library;

namespace FDR.Web
{
    public class DummyProcess
    {
        public async static Task Start(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            await Task.Run(() =>
            {
                int i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                    i++;
                    Console.WriteLine($"Process: {i}");
                    Thread.Sleep(1000);
                }
            });
        }
    }

    public class ProcessInfo
    {
        public ProcessInfo(Operation operation, CancellationTokenSource tokenSource)
        {
            StartedAt = DateTime.Now;
            Operation = operation;
            TokenSource = tokenSource;
        }

        public Operation Operation { get; }

        public DateTime StartedAt { get; }

        public CancellationTokenSource? TokenSource { get; }
    }
}
