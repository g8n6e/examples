using LoggerService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAutoTest.Services
{
    public class AutoTestCaseProcessor : IDisposable
    {
        private readonly AutoTestCaseProducer producer;

        private readonly AutoTestCaseConsumer consumer;
        private bool disposed;
        protected readonly ILoggerManager logger;
        private readonly string directoryPath;

        private ConcurrentDictionary<string, string> testCaseDirectories = new ConcurrentDictionary<string, string>();

        public bool ProcessingCompleted { get; set; }

        public AutoTestCaseProcessor(string directoryPath, Func<string, Task> processTestCaseCallback, TimeSpan pollingInterval, int degreesOfParallelism,  ILoggerManager logger)
        {
            this.ProcessingCompleted = false;
            this.directoryPath = directoryPath;
            this.producer = new AutoTestCaseProducer(pollingInterval, logger);
            var subDirs = Directory.EnumerateDirectories(directoryPath);

            foreach (var subDir in subDirs)
            {
                testCaseDirectories.TryAdd(subDir, subDir);
            }

            var test = testCaseDirectories.Select(x => x.Value).ToList();
            var test2 = string.Empty;

            this.producer.BatchAvailable += (s, e) => this.ProducerOnBatchAvailable(testCaseDirectories.Select(x => x.Value).ToList());
            this.consumer = new AutoTestCaseConsumer(processTestCaseCallback, degreesOfParallelism, logger);
            this.consumer.RemoveTestCaseDirectory += (s, e) => this.RemoveTestCase(e.EventData);
            this.logger = logger;
        }

        public void Start()
        {
            this.ProcessingCompleted = false;     
            this.producer.Start();
            this.consumer.Start();
        }

        public void Stop()
        {
            this.producer.Stop();
            this.consumer.Stop();
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.Stop();
            this.producer.Dispose();
            this.consumer.Dispose();
            this.disposed = true;
        }

        private void ProducerOnBatchAvailable(List<string> directoryPaths)
        {
            if (!this.consumer.Ready)
            {
                return;
            }

            logger.LogInfo($"File producer is ready for another Batch of test case directories from {directoryPath}");

            var batch = this.producer.GetNextBatch(testCaseDirectories.Select(x => x.Value).ToList()).ToList();

            if (batch.Count == 0 && this.testCaseDirectories.Count == 0)
            {
                logger.LogInfo($"All test case subdirectories have been processed in {directoryPath}");
                this.ProcessingCompleted = true;             
            }

            batch.ForEach(this.consumer.Add);
        }

        public void RemoveTestCase(string testCaseDir)
        {
            bool result = testCaseDirectories.TryRemoveConditionally(testCaseDir, testCaseDir);
            if (!result)
            {
                logger.LogInfo($"Failed to remove test case directory [{testCaseDir}] from concurrent dictionary in AutoTestCaseProcessor... probably means another thread already did.");
            }
        }
    }
}
