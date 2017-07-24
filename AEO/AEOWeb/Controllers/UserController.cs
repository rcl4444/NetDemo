using AEOPoco.Domain;
using AEOService.Interface;
using AEOWeb.Controllers;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEOWeb.Controllers
{
    public class UserController : AuthorizeController
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService AccountService, IWorkContext workContext):base(workContext)
        {
            this._accountService = AccountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchUser(string PersonName, string AccountName, int? DeparementID)
        {
            var list = _accountService.SearchAccout(currentAccount.CustomerCompanyID, PersonName, AccountName, DeparementID,currentAccount.Id);
            return StandardJson(list);
        }

        public ActionResult Update(int? DeparementID, string PersonName, string AccountName,string Pwd, int id)
        {
            string message = "";
            var success = _accountService.UpdateAccount(DeparementID, PersonName, AccountName,Pwd, currentAccount.CustomerCompanyID, id, out message) == true ? 1 : 0;
            return StandardJson("",success, message);
        }

        public ActionResult Delete(int id)
        {
            var message = "";
            var success = _accountService.DelAccount(currentAccount.CustomerCompanyID,id,out message)==true?1:0;
            return StandardJson("", success,message);
        }

        public ActionResult Insert(int? DeparementID, string PersonName, string AccountName, string Pwd)
        {
            var message = "";
            var success = _accountService.InsertAccount(DeparementID,PersonName,AccountName,Pwd, currentAccount.CustomerCompanyID,out message)==true?1:0;
            return StandardJson("", success,message);
        }

        public ActionResult GetUserList()
        { 
            var list = _accountService.GetAccoutList(currentAccount.CustomerCompanyID);
            return StandardJson(list);
        }

        public ActionResult ChangePassWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassWord(string oldpassword, string pwd, string password)
        {
            string message;
            if (this._accountService.ChangePassWord(currentAccount, oldpassword, pwd, password, null, out message))
            {
                return StandardJson();
            }
            return StandardJson(null, 0, message);
        }
    }
}