using System;
using System.Text;
using NLog;
using NLog.Common;
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
            if (string.IsNullOrEmpty(Host))
            {
                throw new ArgumentException("Host must be specified for the target e.g. tcp://*:5563");
            }

            InternalLogger.Info("Connecting to host: " + Host);

            context = ZmqContext.Create();
            publisher = context.CreateSocket(SocketType.PUB);
            publisher.Bind(Host);
        }

        protected override void Write(LogEventInfo logEvent)
        {
            publisher.Send(logEvent.FormattedMessage, Encoding.UTF8);
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