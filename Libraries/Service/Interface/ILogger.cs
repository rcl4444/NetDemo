using System;
using System.Collections.Generic;
using Core;

namespace Service.Interface
{
    /// <summary>
    /// Logger interface
    /// </summary>
    public partial interface ILogger
    {
        void RecordLog(string message);

        void ErrorLog(string shortMessage,Exception ex);
    }
}
