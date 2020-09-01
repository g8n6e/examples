using LoggerService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace ServiceAutoTest.Services
{
    public class AutoTestCaseProducer : IDisposable
    {
        private readonly Timer timer;

        private ILoggerManager logger;

        private bool disposed;

        public event EventHandler BatchAvailable;

        public AutoTestCaseProducer(TimeSpan pollingInterval, ILoggerManager logger)
        {
            this.logger = logger;
            this.timer = new Timer(pollingInterval.TotalMilliseconds);
        }

        public void Start()
        {
            this.timer.Elapsed += this.TimerOnElapsed;
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
            this.timer.Elapsed -= this.TimerOnElapsed;
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.Stop();
            this.timer.Dispose();
            this.disposed = true;
        }

        public IEnumerable<string> GetNextBatch(List<string> directoryPaths)
        {           
            return directoryPaths;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            this.BatchAvailable?.Invoke(sender, e);
        }

    }
}

