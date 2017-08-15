using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Core.Infrastructure;
using Core;
using Service.Extend;
using Service.Interface;

namespace AEOWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

        }

        protected void Application_PreSendRequestHeaders(Object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response != null)
            {
                response.Headers.Remove("Server");
            }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            //log error
            LogException(exception);
            //process 404 HTTP errors
            HttpException httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                if (!webHelper.IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;
                    Response.Write("page not find");
                }
            }
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            //ignore 404 HTTP errors
            HttpException httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

            try
            {
                //log
                var logger = EngineContext.Current.Resolve<ILogger>();
                logger.Error(exc.Message, exc);
            }
            catch
            {
                //don't throw new exception if occurs
            }
        }
    }
}