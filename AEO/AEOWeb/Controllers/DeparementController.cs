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
    public class DeparementController : AuthorizeController
    {
        private readonly IDeparementService _DeparementService;
        private readonly ICustomerCompanyService _CustomerCompanyService;

        public DeparementController(IDeparementService IDeparementService,
            ICustomerCompanyService CustomerCompanyService,IWorkContext workContext):base(workContext)
        {
            this._DeparementService = IDeparementService;
            this._CustomerCompanyService = CustomerCompanyService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchDeparement()
        {
            var data = _CustomerCompanyService.GetDeparement(currentAccount.CustomerCompanyID);
            return StandardJson(new
            {
                CompanyName = data.CompanyName,
                Deparements = data.CustomerDeparements.Select(o => new
                {
                    o.Id,
                    o.DeparementName,
                    o.Description
                })
            });
        }

        public ActionResult Delete(int deparementid)
        {
            var message = "";
            var success = _DeparementService.DelDeparement(currentAccount.CustomerCompanyID, deparementid, out message) == true ? 1 : 0;
            return StandardJson("",success,message);
        }

        public ActionResult Update(int id, string deparementname, string description)
        {
            var success = 1;
            var message = "";
            if (string.IsNullOrEmpty(deparementname))
            {
                success = 0;
                message = "部门名称不能为空";
            }
            else
            {
                success = _DeparementService.UpdateDeparement(currentAccount.CustomerCompanyID, id, deparementname, description, out message) == true ? 1 : 0;
            }
            return StandardJson("", success, message);
        }

        public ActionResult Insert(string deparementname, string description)
        {
            var success =1;
            var message = "";
            if (string.IsNullOrEmpty(deparementname))
            {
                success = 0;
                message = "部门名称不能为空";
            }
            else 
            {
                success = _DeparementService.AddDeparement(currentAccount.CustomerCompanyID, deparementname, description, out message) == true ? 1 : 0;
            }
            return StandardJson("", success, message);
        }

        public ActionResult GetDeparementList()
        {
            var list = _DeparementService.GetDeparementList(currentAccount.CustomerCompanyID);
            return StandardJson(list);
        }
    }
}