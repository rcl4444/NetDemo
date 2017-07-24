using AEOPoco.Domain;
using AEOService.Interface;
using AEOWeb.Infrastructure;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Controllers;

namespace AEOWeb.Controllers
{
    public class AccountController : BasePublicController
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerCompanyService _customerCompanyService;

        public AccountController(IAccountService accountService, 
            IAuthenticationService authenticationService,
            IWorkContext workContext,
            ICustomerCompanyService customerCompanyService)
        {
            this._accountService = accountService;
            this._authenticationService = authenticationService;
            this._workContext = workContext;
            this._customerCompanyService = customerCompanyService;
        }

        [HttpGet]
        public ActionResult Login(int? id)
        {
            ViewBag.CompanyID = id;
            ViewBag.Message = id.HasValue ? "" : "缺少公司标识";
            return View();
        }

        [HttpPost]
        public ActionResult Login(int? companyID, string userName, string passWord, bool? isremember)
        {
            if (!companyID.HasValue || companyID.Value <= 0)
            {
                ViewBag.Message = "缺少公司标识";
                return View();
            }
            string message = string.Empty;
            CustomerAccount currentUser;
            if (_accountService.CustomerCompanyGeneralAccountLogin(companyID.Value, userName, passWord, out currentUser, out message))
            {
                this._authenticationService.SignIn(currentUser, isremember.HasValue ? isremember.Value : false);
                this._workContext.CurrentUser = currentUser;
                if (currentUser.IsManager && !currentUser.CustomerCompany.CustomsAuthenticationID.HasValue)
                {
                    return RedirectToAction("serviceinitialize", "home", new { customercompanyid = currentUser.CustomerCompanyID });
                } 
                return RedirectToAction("index", "home", new { customercompanyid = currentUser.CustomerCompanyID });
            }
            else
            {
                ViewBag.UserName = userName;
                ViewBag.Message = message;
                ViewBag.CompanyID = companyID;
                return View();
            }
        }

        public ActionResult Logout()
        {
            _authenticationService.SignOut();
            string customerCompanyID = this.Request.UrlReferrer == null ?"": this.Request.UrlReferrer.AbsolutePath.Substring(1, this.Request.UrlReferrer.AbsolutePath.IndexOf('/', 1) - 1);
            if (!string.IsNullOrEmpty(customerCompanyID))
            {
                return RedirectToAction("login", "account", new { id = customerCompanyID });
            }
            else
            {
                return RedirectToAction("login", "account");
            }
        }
    }
}