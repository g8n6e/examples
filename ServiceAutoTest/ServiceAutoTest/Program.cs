

using ServiceAutoTest.Models;
using ServiceAutoTest.Services;
using LoggerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace ServiceAutoTest
{
    enum ExitCode : int
    {
        [Description("Success")]
        Success = 0,
        [Description("UnknownError")]
        UnknownError = 10
    }

    class Program
    {
        private static IConfiguration configuration;
        private static ILoggerManager logger;
        private static int StatusCode;

        public static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
//#if DEBUG
//            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile($"appsettings.dev.json", false, true);
//#else
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile($"appsettings.json", false, true);
//#endif

            configuration = builder.Build();
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);


            var serviceProvider = serviceCollection.BuildServiceProvider();

            logger = serviceProvider.GetService<ILoggerManager>();
            var serviceName = Assembly.GetEntryAssembly().GetName().Name;

            logger.LogInfo($"Beginning {serviceName}");

            var testCaseBaseFolder = configuration.GetValue<string>("TestCaseBaseFolder");
            var testCaseDirectories = Directory.EnumerateDirectories(testCaseBaseFolder);
            var pollingIntervalSeconds = configuration.GetValue<int>("PollingIntervalSeconds");
            var pollingInterval = new TimeSpan(0, 0, pollingIntervalSeconds);
            var degreesOfParallelism = configuration.GetValue<int>("DegreesOfParallelism");


            Func<string, Task> processTestCaseCallback = async (testCaseDirectory) =>
            {

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var logger = serviceProvider.GetService<ILoggerManager>();           

                var testCaseName = Path.GetFileName(testCaseDirectory);
                var configFileName = configuration.GetValue<string>("ConfigFileName");
                var payloadFileName = configuration.GetValue<string>("PayloadFileName");
                var reportFileName = configuration.GetValue<string>("ReportFileName");
                var archiveTestCaseRuns = configuration.GetValue<bool>("ArchiveTestCaseRuns");

                // I hate this part but lets build a custom logger for each test case
                var logTarget = new FileTarget();
                logTarget.Name = testCaseName;
                logTarget.FileName = testCaseDirectory + "/${shortdate}_logfile.txt";
                logTarget.Layout = "${longdate} ${level:uppercase=true} ${message}";

                LogManager.Configuration.AddTarget(testCaseName, logTarget);
                LogManager.Configuration.AddRule(LogLevel.Info, LogLevel.Fatal, logTarget, testCaseName);
                LogManager.ReconfigExistingLoggers();

                var testCaseLogger = new LoggerManager(testCaseName);


                testCaseLogger.LogInfo($"{testCaseName} Logger is initialized, retrieving config file");
                TestCaseConfig testCaseConfig = null;

                try
                {
                    var configPath = Path.Combine(testCaseDirectory, configFileName);
                    var tccBuilder = new ConfigurationBuilder().AddJsonFile(configPath);

                    var iConfig = tccBuilder.Build();
                    
                    testCaseConfig = new TestCaseConfig(iConfig, configFileName);
                }
                catch(Exception e)
                {
                    testCaseLogger.LogError($"Configuration Failure -> {e.ToDetailedString()}");
                    logger.LogError($"{testCaseName} Failed -> Configuration Failure -> {e.ToDetailedString()}");
                }

                var resultFilePath = string.Empty;
                try
                {
                    var autoTestCase = new AutoTestCase(testCaseDirectory, testCaseConfig, payloadFileName, reportFileName, testCaseLogger, archiveTestCaseRuns);

                    resultFilePath = await autoTestCase.Run();
                }
                catch (Exception e)
                {
                    testCaseLogger.LogError($"Test Case Compare Failure -> {e.ToDetailedString()}");
                    logger.LogError($"{testCaseName} Failed -> Test Case Compare Failure -> {e.ToDetailedString()}");
                }

                stopwatch.Stop();

                logger.LogInfo($"Finished processing test case: {testCaseName} with report at {resultFilePath} elapsed time: { stopwatch.Elapsed.TotalMilliseconds } ms.");
            };

            var autoTestCaseProcessor = new AutoTestCaseProcessor(testCaseBaseFolder, processTestCaseCallback, pollingInterval, degreesOfParallelism, logger);            
            autoTestCaseProcessor.Start();

            // Because its a console app and not a service
            while (autoTestCaseProcessor.ProcessingCompleted == false)
            {
                continue;
            }
            if (autoTestCaseProcessor.ProcessingCompleted == true)
            {
                //autoTestCaseProcessor.Stop();
                return StatusCode;
            }


            return StatusCode;

        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;
            //Console.WriteLine($"Something went wrong: {exception.ToDetailedString()}");
            logger.LogError($"Something went wrong: {exception.ToDetailedString()}");
            StatusCode = (int)ExitCode.UnknownError;
            Environment.Exit(StatusCode);
        }

    }
}