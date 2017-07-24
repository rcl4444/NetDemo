using AEOWeb.Controllers;
using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AEOWeb.Infrastructure
{
    public class AEOAuthorizeAttribute: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
                filterContext.Result = new HttpUnauthorizedResult("请重新登录");
            }
            else
            {
                var customercompanyid = filterContext.RequestContext.RouteData.Values["customercompanyid"];
                filterContext.Result = new RedirectToRouteResult("customer_login", new RouteValueDictionary(new Dictionary<string, object>() { { "id", customercompanyid } }));
            }
        }
    }
}