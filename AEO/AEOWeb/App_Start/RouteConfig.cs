using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AEOWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = false;
            //忽略chrome里样式表和js文件的压缩源码map的请求捕获
            //忽略Chrome中对于网站图标的请求捕获
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            //routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");//忽略aspx后缀
            //routes.IgnoreRoute("{*allmap}", new { allmap = @".*\.map(/.*)?" });//忽略后缀包含map
            //routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });//忽略后缀包含favicon.ico
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;
            routes.MapRoute(
                "customer_login",
                "login/{id}",
                new { id = UrlParameter.Optional, controller = "account", action = "login" },
                new string[] { "AEOWeb.Controllers" }
            );
            routes.MapRoute(
                "customer_logout",
                "logout",
                new { controller = "account", action = "logout" },
                new string[] { "AEOWeb.Controllers" }
            );
            routes.MapRoute(
                "Main",
                "{customercompanyid}/{controller}/{action}/{id}",
                new { action="Index", id = UrlParameter.Optional },
                new { customercompanyid = @"\d+" },
                new string[] { "AEOWeb.Controllers" }
            );
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "AEOWeb.Controllers" }
            );
        }
    }
}