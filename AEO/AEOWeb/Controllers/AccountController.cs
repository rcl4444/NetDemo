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
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Web.SessionState;
using System.Reflection;

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
            //Session.Abandon();
            SessionIDManager manager = new SessionIDManager();
            manager.RemoveSessionID(System.Web.HttpContext.Current);
            var oldId = manager.GetSessionID(System.Web.HttpContext.Current);
            var newId = manager.CreateSessionID(System.Web.HttpContext.Current);
            var isRedirected = true;
            var isAdded = true;
            manager.SaveSessionID(System.Web.HttpContext.Current, newId, out isRedirected, out isAdded);
            System.Web.HttpContext.Current.Session["sessionid"] = newId;
            HttpApplication ctx = (HttpApplication)System.Web.HttpContext.Current.ApplicationInstance;
            HttpModuleCollection mods = ctx.Modules;
            System.Web.SessionState.SessionStateModule ssm = (SessionStateModule)mods.Get("Session");
            System.Reflection.FieldInfo[] fields = ssm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            SessionStateStoreProviderBase store = null;
            System.Reflection.FieldInfo rqIdField = null, rqLockIdField = null, rqStateNotFoundField = null;
            foreach (System.Reflection.FieldInfo field in fields)
            {
                if (field.Name.Equals("_store")) store = (SessionStateStoreProviderBase)field.GetValue(ssm);
                if (field.Name.Equals("_rqId")) rqIdField = field;
                if (field.Name.Equals("_rqLockId")) rqLockIdField = field;
                if (field.Name.Equals("_rqSessionStateNotFound")) rqStateNotFoundField = field;
            }
            object lockId = rqLockIdField.GetValue(ssm);
            if ((lockId != null) && (oldId != null)) store.ReleaseItemExclusive(System.Web.HttpContext.Current, oldId, lockId);
            rqStateNotFoundField.SetValue(ssm, true);
            rqIdField.SetValue(ssm, newId);

            ViewBag.CompanyID = id;
            ViewBag.Message = id.HasValue ? "" : "缺少公司标识";
            ViewBag.ThirdLoginUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&state={3}&response_type=code", authorizeUrl,clinetId, redirectUri,state);
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

        //public string authorizeUrl = "http://git.yunbaoguan.cn/oauth/authorize";
        //public string tokenUrl = "http://git.yunbaoguan.cn/oauth/token";
        //public string redirectUri = "http://localhost:3111/Account/ThirdParty";
        //public string state = "123456";
        //public string clinetId = "cd23b6cfc15af35003ef94c78b3218114ae3cfc2f17994294cd309c71385eb63";
        //public string clientSecret = "2f50dbcfd9c0dfc9fa67fd58c75f04622f3c22edba46fcef4cfcd213b2dfbe80";
        public string authorizeUrl = "http://localhost:8080/oauth/authorize";
        public string tokenUrl = "http://localhost:8080/oauth/token";
        public string redirectUri = "http://localhost:3111/Account/ThirdParty";
        public string state = "123456";
        public string clinetId = "test1";
        public string clientSecret = "test1";


        public ActionResult ThirdParty(string code,string state)
        {
            if (state.Equals("123456"))
            {
                HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "client_id", clinetId},
                    { "client_secret", clientSecret},
                    { "redirect_uri", redirectUri},
                    { "code", code},
                    { "grant_type", "authorization_code"}
                });
                HttpResponseMessage result = client.PostAsync(tokenUrl, content).Result;
                string resultstr = result.Content.ReadAsStringAsync().Result;
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(resultstr);
                try
                {
                    dynamic o = serializer.Deserialize(new JsonTextReader(sr));
                    string token = o.access_token.ToString();
                    using (client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = client.GetAsync("http://localhost:8080/oauth/applicationoperation").Result;
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        ViewBag.TokenMessage = resultstr;
                        ViewBag.UserMessage = responseContent;
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            return View();
        }
    }
}