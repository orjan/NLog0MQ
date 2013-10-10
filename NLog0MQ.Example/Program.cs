using System;

namespace NLog0MQ.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DoMagic();

            GC.Collect();

            Console.ReadLine();
        }

        public static void DoMagic()
        {
            var log = NLog.LogManager.GetLogger("NLog0MQ.Example");
            log.Info("Hello...");
           
        }
    }
}
