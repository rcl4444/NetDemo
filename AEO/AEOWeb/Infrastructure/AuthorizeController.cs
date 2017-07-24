using Core;
using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Controllers;
using AEOWeb.Infrastructure;
using System.Web.Routing;

namespace AEOWeb.Controllers
{
    [AEOAuthorize]
    public abstract class AuthorizeController : BasePublicController
    {
        protected readonly IWorkContext _workContext;

        protected static Dictionary<Role,List<string>> RoleModuleMap = new Dictionary<Role,List<string>>(){
            {Role.Manager,new List<string>(){"account","clauses","deparement","fileaudit","fileuploadrequire","item","itemscore","user"}},
            {Role.Clause,new List<string>(){"account","fileaudit","fileuploadrequire","item","itemscore","user"}},
            {Role.Score,new List<string>(){"account","itemscore"}},
            {Role.Reviewer,new List<string>(){"account","fileaudit"}},
            {Role.Charge,new List<string>(){"account","fileuploadrequire"}}
        };

        protected CustomerAccount currentAccount
        {
            get {
                return (CustomerAccount)_workContext.CurrentUser;
            }
        }

        protected CustomerCompany currentComapny
        {
            get {
                return currentAccount.CustomerCompany;
            }
        }

        public AuthorizeController(IWorkContext workContext)
        {
            this._workContext = workContext;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }
            if (this.currentAccount == null)
            {
                var customercompanyid = filterContext.RequestContext.RouteData.Values["customercompanyid"];
                filterContext.Result = new RedirectToRouteResult("customer_login", new RouteValueDictionary(new Dictionary<string, object>() { { "id", customercompanyid } }));
                return;
            }
            string[] ignoreActionNames = new string[] { "changepassword" };
            if (!ignoreActionNames.Contains(filterContext.ActionDescriptor.ActionName.ToLower()) && !this.currentAccount.HasChange)
            {
                var customercompanyid = filterContext.RequestContext.RouteData.Values["customercompanyid"];
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new Dictionary<string, object>() { { "action", "changepassword" } }));
                return;
            }
            var account = this.currentAccount;
            ViewBag.CompanyID = account.CustomerCompanyID;
            ViewBag.CompanyName = this.currentComapny.CompanyName;
            ViewBag.PersonName = account.PersonName;
            ViewBag.IsManager = account.IsManager;
            ViewBag.CreateTime = account.CreateTime.ToString("yyyy-MM-dd");
            ViewBag.RoleType = account.IsManager ? Role.Manager : account.UserRoles.Count > 0 ? account.UserRoles.Select(o => o.RoleID).Min() : Role.Charge;
            if (account.IsManager == true)
            {
                ViewBag.Module = RoleModuleMap[Role.Manager];
            }
            else
            {
                var RolesList = account.UserRoles.ToList();
                var Modeles = new List<string>();
                for (int i = 0; i < RolesList.Count; i++)
                {
                    foreach (var item in RoleModuleMap[RolesList[i].RoleID])
                    {
                        Modeles.Add(item);
                    }
                }
                ViewBag.Module = Modeles.Distinct().ToList();
            }
        }
    }
}