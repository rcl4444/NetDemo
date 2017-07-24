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
    public class ItemController : AuthorizeController
    {
        private readonly IClausesService _clausesService;
        private readonly IScoreTaskService _scoreTaskService;
        private readonly IItemService _itemService;
        private readonly IFineItemService _fineItemService;
        private readonly IFileScheduleService _fileScheduleService;

        public ItemController(IClausesService clausesService, IItemService itemService,IScoreTaskService scoreTaskService,IFineItemService fineItemService,IFileScheduleService fileScheduleService, IWorkContext workContext):base(workContext)
        {
            this._clausesService = clausesService;
            this._scoreTaskService = scoreTaskService;
            this._itemService = itemService;
            this._fineItemService = fineItemService;
            this._fileScheduleService = fileScheduleService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchItem()
        {
            var list = _clausesService.GetTaskConut(currentAccount.CustomerCompanyID,currentAccount.Id,currentAccount.IsManager);
            return StandardJson(list);
        }

        public ActionResult ItemDetail(int id)
        {
            var clauses = this._clausesService.GetByID(id);
            if (clauses != null)
            {
                ViewBag.ClausesName = clauses.ClausesName;
            }
            ViewBag.ClausesID = id;
            return View();
        }

        public ActionResult GetItemList()
        {
            var list = _itemService.GetItemList(currentAccount.CustomerCompanyID, currentAccount.Id, currentAccount.IsManager);
            return StandardJson(list);
        }

        public ActionResult FineItemDetail(int id)
        {
            var item = this._itemService.GetByID(id);
            if (item != null)
            {
                ViewBag.ClausesName = item.Clauses.ClausesName;
                ViewBag.ClausesID = item.ClausesID;
                ViewBag.ItemName = item.ItemName;
            }
            ViewBag.ItemID = id;
            return View();
        }

        public ActionResult SetScoerPerosn(List<int> itemIDs, int id)
        {
            var message = "";
            var success = _scoreTaskService.SetScoerPerosn(itemIDs, id, currentAccount.CustomerCompanyID, out message) == true ? 1 : 0;
            return StandardJson("", success, message);
        }

        public ActionResult GetFineItemList(int itemID)
        {
            var list = _fineItemService.GetFineItemList(itemID, currentAccount.CustomerCompanyID);
            return StandardJson(list);
        }

        public ActionResult SetChargeAndReviewer(string id, int? reviewerPersonID, int? chargePersonID, string finishTime)
        {
            var message = "";
            var success = _fileScheduleService.SetChargeAndReviewer
                (currentAccount.CustomerCompanyID, reviewerPersonID, chargePersonID, finishTime, id, out message)==true?1:0;
            return StandardJson("",success,message);
        }
    }
}