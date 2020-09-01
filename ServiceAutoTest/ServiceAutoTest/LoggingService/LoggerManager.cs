﻿using NLog;
using System;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager()
        {
        }
        public LoggerManager(string name)
        {
            logger = LogManager.GetLogger(name);
        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
