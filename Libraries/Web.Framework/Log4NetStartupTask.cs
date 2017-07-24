using Core.Infrastructure;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    public class Log4NetStartupTask : IStartupTask
    {
        public int Order { get { return 0; } }

        public void Execute()
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Log4net.config");
            FileInfo fileInfo = new FileInfo(filePath);
            XmlConfigurator.ConfigureAndWatch(fileInfo);
        }
    }
}