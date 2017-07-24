using Service.Interface;
using System;

namespace Service.Extend
{
    /// <summary>
    /// Represents a log level
    /// </summary>
    public enum LogLevel
    {
        Debug = 10,
        Information = 20,
        Warning = 30,
        Error = 40 ,
        Fatal = 50
    }

    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception);
        }
        public static void Information(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception);
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception);
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;
            if (level == LogLevel.Error)
            {
                logger.ErrorLog(message,exception);
            }
            else
            {
                logger.RecordLog(message);
            }
        }
    }
}
