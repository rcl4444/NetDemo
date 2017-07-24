using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    /// <summary>
    /// Default logger
    /// </summary>
    public partial class DefaultLogger : ILogger
    {
        private static readonly log4net.ILog logInfo = log4net.LogManager.GetLogger("myLogger");
        private static readonly log4net.ILog logError = log4net.LogManager.GetLogger("myErrorLogger");

        public void ErrorLog(string shortMessage, Exception ex)
        {
            logError.Error(shortMessage,ex);
        }

        public void RecordLog(string message)
        {
            logInfo.Info(message);
        }
    }
}
