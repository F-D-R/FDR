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
}
