using System;
using NLog;
using NLog.Targets;
using ZeroMQ;

namespace NLog0MQ
{
    /// <summary>
    ///     https://github.com/nlog/NLog/wiki/How%20to%20write%20a%20Target
    /// </summary>
    [Target("NLog0Mq")]
    public class NLog0MqTarget : TargetWithLayout
    {
        private ZmqContext context;
        private ZmqSocket publisher;
        private bool targetClosed;
        private readonly object locker;

        public string Host { get; set; }

        public NLog0MqTarget()
        {
            locker = new object();
        }

        ~NLog0MqTarget()
        {
            CloseTarget();
        }
        
        protected override void InitializeTarget()
        {
            context = ZmqContext.Create();
            publisher = context.CreateSocket(SocketType.PUB);
            publisher.Bind("tcp://*:5563");
        }

        protected override void Write(LogEventInfo logEvent)
        {
            throw new Exception("sadfdsafdsafds");
        }

        protected override void CloseTarget()
        {
            lock (locker)
            {
                if (targetClosed)
                    return;
                
                if (publisher != null)
                {
                    publisher.Close();
                    publisher.Dispose();
                    publisher = null;
                }                
                
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }

                targetClosed = true;
            }
        }
    }
}