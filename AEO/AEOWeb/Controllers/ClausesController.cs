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
    public class ClausesController : AuthorizeController
    {
        private readonly IClausesService _clausesService;
        private readonly IFileRequireService _fileRequireService;
        private readonly IClausesPersonLiableService _clausesPersonLiableService;
        private readonly IAccountService _accountService;

        public ClausesController(IClausesService clausesService, 
            IClausesPersonLiableService clausesPersonLiableService,
            IFileRequireService fileRequireService,
            IUserRoleService userRoleService,
            IAccountService accountService,
            IWorkContext workContext):base(workContext)
        {
            this._clausesService = clausesService;
            this._clausesPersonLiableService = clausesPersonLiableService;
            this._fileRequireService = fileRequireService;
            this._accountService = accountService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchClauses()
        {;
            var list = _clausesService.ClausesSearch(currentAccount.CustomerCompanyID);
            
            return StandardJson(list);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.ClausesId = id;
            return View();
        }

        public ActionResult GetTaskList(int id)
        {
            var clauses = _clausesService.GetTaskList(currentAccount.CustomerCompanyID, id);
            return StandardJson(clauses);
        }

        public ActionResult DelFileRequire(int id)
        {
            var message = "";
            var success = _fileRequireService.DeleteFileRequire(currentAccount.CustomerCompanyID,id,out message)==true?1:0;
            return StandardJson("", success, message);
        }

        public ActionResult AddFileRequire(int id, string Description, string SuggestFileName)
        {
            var message = "";
            var success = _fileRequireService.AddFileRequire(currentAccount.CustomerCompanyID, id, SuggestFileName,Description, out message) == true ? 1 : 0;
            return StandardJson("", success, message);
        }

        public ActionResult UpdateFileRequire(int id, string Description, string SuggestFileName)
        {
            var message = "";
            var success = _fileRequireService.UpdateFileRequire(currentAccount.CustomerCompanyID, id, SuggestFileName, Description, out message) == true ? 1 : 0; 
            return StandardJson(success);
        }

        public ActionResult SortFileRequire(int oldId,int newId)
        {
            var message = "";
            var success = _fileRequireService.SortFileRequire(currentAccount.CustomerCompanyID, oldId, newId, out message) == true ? 1 : 0;
            return StandardJson(success);
        }

        public ActionResult SetPerson(int id, int AccountID)
        {
            var message = "";
            var success = _clausesPersonLiableService.SetPersonLiable(currentAccount.CustomerCompanyID, AccountID, id, out message) == true ? 1 : 0;
            return StandardJson("", success, message);
        }

        /// <summary>
        /// 获取所有条信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClausesList()
        {
            var query = this._clausesService.FilterQuery(o => o.OutlineClass.CustomsAuthenticationID == currentComapny.CustomsAuthenticationID);
            if (!currentAccount.IsManager)
            {
                var clausesIDs = this._accountService.GetChargeClausesID(currentAccount);
                query = query.Where(o => clausesIDs.Contains(o.Id));
            }
            var data = query.Select(o => new {
                o.Id,
                o.ClausesName
            });
            return StandardJson(data);
        }
    }
}