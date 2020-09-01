
using LoggerService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAutoTest.Services
{
   
    public class AutoTestCaseConsumer : IDisposable
    {
        private readonly BlockingCollection<string> directories = new BlockingCollection<string>();

        private CancellationTokenSource tokenSource;

        private Task task;

        private readonly Func<string, Task> ProcessTestCaseCallback;

        private readonly int dop;

        private readonly ILoggerManager logger;

        public event EventHandler<GenericEventArgs<string>> RemoveTestCaseDirectory;

        public AutoTestCaseConsumer(Func<string, Task> processTestCaseCallback, int degreesOfParallelism, ILoggerManager logger)
        {
            ProcessTestCaseCallback = processTestCaseCallback;
            this.dop = degreesOfParallelism;
            this.logger = logger;
        }

        public void Start()
        {
            try
            {
            
                this.tokenSource = new CancellationTokenSource();

                this.task = this.directories.GetConsumingEnumerable().ForEachAsyncConcurrent(
                async directoryPath =>
                        {
                            if (!this.tokenSource.IsCancellationRequested)
                            {                               
                                await ProcessTestCaseCallback(directoryPath);                                   
                                this.RemoveTestCaseDirectory?.Invoke((AutoTestCaseConsumer)this, new GenericEventArgs<string>(directoryPath));

                                // Yeah, yeah, I know...
                                //GC.Collect();
                                //GC.WaitForPendingFinalizers();
                            }
                            else
                            {
                                this.directories.CompleteAdding();
                            }
                        },
                    this.dop);
            }
            catch (OperationCanceledException oce)
            {
                logger.LogInfo($"file processing stopped -> {oce.ToDetailedString()}");
            }
        }

        public void Stop()
        {
            this.Dispose();
        }

        public void Add(string folder)
        {
            this.directories.Add(folder);
        }

        public void Dispose()
        {
            if (this.task == null)
            {
                return;
            }

            this.tokenSource.Cancel();
            while (!this.task.IsCanceled)
            {
            }

            this.task.Dispose();
            this.tokenSource.Dispose();
            this.task = null;
        }

        public bool Ready
        {
            get
            {
                return this.directories.Count == 0;
            }
        }
    }
}
