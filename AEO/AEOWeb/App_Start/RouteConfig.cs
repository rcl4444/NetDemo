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