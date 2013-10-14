using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace NLog0MQ.Example
{
    public class Program
    {
        private static readonly Logger Log = LogManager.GetLogger("NLog0MQ.Example");
        public static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(()=>LogMessages(cancellationTokenSource.Token), TaskCreationOptions.LongRunning);

            Console.ReadLine();
            cancellationTokenSource.Cancel();
        }

        private static Action LogMessages(CancellationToken token)
        {
            int j = 0;
            while (true)
            {
                token.ThrowIfCancellationRequested();
                j++;
                var message = "Log message: " + j;
                Log.Info(message);
                Console.WriteLine(message);
                Thread.Sleep(10);
            }

            return null;
        }
    }
}
