using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Models;

namespace Web.Framework.Controllers
{
    public abstract class BasePublicController: BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            IController errorController = EngineContext.Current.Resolve<CommonController>();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");
            errorController.Execute(new RequestContext(this.HttpContext, routeData));
            return new EmptyResult();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            LogException(filterContext.Exception);
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.ExceptionHandled = true;
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write((new OperateResult()
            {
                Success = 0,
                Message = "请求异常,请联系管理员"
            }).ToJson());
        }

        /// <summary>
        /// 根据配置路径来动态创建绝对路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="uploadPath">父目录</param>
        /// <returns></returns>
        protected string GetPhysicalPath(string path, string uploadPath)
        {
            string basePath;
            if (path.Contains("/") && path.Contains("\\"))
            {
                throw new Exception("输入地址格式错误");
            }
            Func<string, string, string> createPath = (basepath, subpath) =>
            {
                subpath = subpath.Replace('/', '\\');
                while (!string.IsNullOrEmpty(subpath) && subpath[0] == '\\')
                {
                    subpath = subpath.Substring(1, subpath.Length - 1);
                }
                while (subpath.IndexOf("..\\") >= 0)
                {
                    int index = subpath.IndexOf("..") + 3;
                    subpath = subpath.Substring(index, subpath.Length - index);
                    if (basepath.IndexOf(":\\") + 3 < basepath.Length)
                    {
                        basepath = basepath.Substring(0, basepath.LastIndexOf('\\', basepath.Length - 2));
                    }
                }
                return System.IO.Path.Combine(basepath, subpath);
            };
            if (!uploadPath.Contains(":"))
            {
                basePath = createPath(Server.MapPath("~/"), uploadPath);
            }
            else
            {
                basePath = uploadPath;
            }
            return createPath(basePath, path);
        }
    }
}