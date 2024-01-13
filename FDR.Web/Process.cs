using FDR.Tools.Library;

namespace FDR.Web
{
    public class Process
    {
        public async static Task Start(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            await Task.Run(() =>
            {
                int i = 0;
                while (true)
                {
                    i++;
                    Console.WriteLine($"Process: {i}");
                    Thread.Sleep(1000);
                }
            });
        }
    }

    public class ProcessInfo
    {
        public ProcessInfo(string name, Operation operation)
        {
            StartedAt = DateTime.Now;
            Name = name;
            Operation = operation;
        }

        public string Name { get; }

        public Operation Operation { get; }

        public DateTime StartedAt { get; }

        public Task? ProcessTask { get; set; }

        public CancellationToken Token { get; set; }

        public Stream? Output { get; set; }
    }
}
