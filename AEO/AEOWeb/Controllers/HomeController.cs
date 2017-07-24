using Core;
using Core.Help;
using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AEOWeb.Controllers;
using System.IO;
using AEOPoco.Other;
using AEOWeb.Models;

namespace AEOWeb.Controllers
{
    public class HomeController : AuthorizeController
    {
        private readonly ICorrectiveTaskService _correctiveTaskService;
        private readonly IFileScheduleService _fileScheduleService;
        private readonly ICustomerCompanyService _customerCompanyService;
        private readonly IAccountService _accountService;
        private readonly IScoreTaskService _scoreTaskService;
        private readonly IClausesPersonLiableService _clausesPersonLiableService;

        public HomeController(IWorkContext workContext,
            IFileScheduleService fileScheduleService,
            ICustomerCompanyService customerCompanyService,
            ICorrectiveTaskService correctiveTaskService,
            IAccountService accountService,
            IScoreTaskService scoreTaskService,
            IClausesPersonLiableService clausesPersonLiableService) :base(workContext)
        {
            this._fileScheduleService = fileScheduleService;
            this._customerCompanyService = customerCompanyService;
            this._correctiveTaskService = correctiveTaskService;
            this._accountService = accountService;
            this._scoreTaskService = scoreTaskService;
            this._clausesPersonLiableService = clausesPersonLiableService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CompanyName = currentAccount.CustomerCompany.CompanyName;
            ViewBag.PersonName = currentAccount.PersonName;
            return View();
        }

        public ActionResult TaskProgress(string status)
        {
            List<TaskItem> result = new List<TaskItem>();
            IEnumerable<TaskSchedule> correctiveSchedules = this._correctiveTaskService.GetCorrectiveSchedule(currentAccount).ToList();
            IEnumerable<TaskSchedule> fileTaskSchedules = this._fileScheduleService.GetFileTaskStatus(currentAccount.CustomerCompanyID, currentAccount, currentAccount.IsManager).OrderBy(o => o.ClausesId).ThenBy(o => o.ItemId).ThenBy(o => o.ItemName).ThenBy(o => o.FileRequireID).ToList();
            if (!string.IsNullOrEmpty(status) && status.ToLower() == "finish")
            {
                correctiveSchedules = correctiveSchedules.Where(o => o.RealFinishTime.HasValue);
                fileTaskSchedules = fileTaskSchedules.Where(o => o.RealFinishTime.HasValue);
            }
            var schedules = Enumerable.Union(correctiveSchedules, fileTaskSchedules).OrderBy(o => o.ClausesId).ThenBy(o=>o.Type).ThenBy(o => o.ItemId).ThenBy(o => o.ItemName).ThenBy(o => o.FileRequireID).ToList();
            Dictionary<int, int> fineitems = new Dictionary<int, int>();
            Dictionary<int, int> items = new Dictionary<int, int>();
            Dictionary<int, int> clauses = new Dictionary<int, int>();
            int fatherId = schedules.Count + 1;
            int index = 1;
            var showCorrectiveIds = this._correctiveTaskService.Search(o => o.ChargePersonID == currentAccount.Id).Select(o => o.CorrectiveObjectID);
            var showItemIds = this._scoreTaskService.Search(o => o.ScorePersonID == currentAccount.Id).Select(o => o.ItemID);
            var showFileIds = this._fileScheduleService.Search(o => o.ChargePersonID == currentAccount.Id || o.AuditorID == currentAccount.Id).Select(o => o.Id);
            Dictionary<int, bool> taskOpen = new Dictionary<int, bool>();
            Action<int> setTaskOpen = id => {
                if (!taskOpen.ContainsKey(id))
                {
                    taskOpen.Add(id,true);
                }
            };
            foreach (var item in schedules)
            {
                if (!clauses.ContainsKey(item.ClausesId))
                {
                    clauses.Add(item.ClausesId, ++fatherId);
                    result.Add(new TaskItem
                    {
                        pID = clauses[item.ClausesId],
                        pName = item.ClausesName.Length > 10 ? item.ClausesName.Substring(0, 11) + "..." : item.ClausesName,
                        pStart = "",
                        pEnd = "",
                        pClass = "ggroupblack",
                        pLink = "",
                        pMile = 0,
                        pRes = item.ClausesChargePerson,
                        pComp = 0,
                        pGroup = 1,
                        pParent = currentAccount.IsManager ? 1: 0,
                        pOpen = 0,
                        pDepend = "",
                        pCaption = "",
                        pNotes = item.ClausesName
                    });
                }
                if (item.ItemId > 0 && !items.ContainsKey(item.ItemId))
                {
                    items.Add(item.ItemId, ++fatherId);
                    result.Add(new TaskItem
                    {
                        pID = items[item.ItemId],
                        pName = item.ItemName.Length > 7 ? item.ItemName.Substring(0, 7) + "..." : item.ItemName,
                        pStart = "",
                        pEnd = "",
                        pClass = "ggroupblack",
                        pLink = "",
                        pMile = 0,
                        pRes = "",
                        pComp = 0,
                        pGroup = 1,
                        pParent = clauses[item.ClausesId],
                        pOpen = 0,
                        pDepend = "",
                        pCaption = "",
                        pNotes = ""
                    });
                }
                if (item.FineItemId > 0 && !fineitems.ContainsKey(item.FineItemId))
                {
                    fineitems.Add(item.FineItemId, ++fatherId);
                    result.Add(new TaskItem
                    {
                        pID = fineitems[item.FineItemId],
                        pName = item.FineItemName.Length > 8 ? item.FineItemName.Substring(0, 8) + "..." : item.FineItemName,
                        pStart = "",
                        pEnd = "",
                        pClass = "ggroupblack",
                        pLink = "",
                        pMile = 0,
                        pRes = "",
                        pComp = 0,
                        pGroup = 1,
                        pParent = items[item.ItemId],
                        pOpen = 0,
                        pDepend = "",
                        pCaption = "",
                        pNotes = item.FineItemName
                    });
                }
                string pClass = "gtaskblue";
                if (!string.IsNullOrEmpty(item.SuggestFileName) && item.SuggestFileName.Contains("."))
                {
                    item.SuggestFileName = item.SuggestFileName.Substring(0, item.SuggestFileName.IndexOf('.'));
                }
                if (item.Type == AEOPoco.Other.ScheduleType.FileRequire)
                {
                    result.Add(new TaskItem
                    {
                        pID = ++index,
                        pName = item.SuggestFileName.Length > 5 ? item.SuggestFileName.Substring(0, 5) + "..." : item.SuggestFileName,
                        pStart = currentComapny.CreateTime.ToString("yyyy-MM-dd"),
                        pEnd =  item.FinishTime.HasValue==true?item.FinishTime.Value.ToString("yyyy-MM-dd"):"",
                        pClass = pClass,
                        pLink = "",
                        pMile = 0,
                        pRes = item.ChargePerson,
                        pComp = item.RealFinishTime.HasValue ? 100 : 0,
                        pGroup = 0,
                        pParent = fineitems[item.FineItemId],
                        pDepend = "",
                        pCaption = "",
                        pNotes = item.SuggestFileName
                    });
                }
                else
                {
                    result.Add(new TaskItem
                    {
                        pID = ++index,
                        pName = item.ItemName.Length > 7 ? item.ItemName.Substring(0, 7) + "..." : item.ItemName,
                        pStart = currentComapny.CreateTime.ToString("yyyy-MM-dd"),
                        pEnd = item.FinishTime.HasValue==true ? item.FinishTime.Value.ToString("yyyy-MM-dd"):"",
                        pClass = pClass,
                        pLink = "",
                        pMile = 0,
                        pRes = item.ChargePerson,
                        pComp = item.RealFinishTime.HasValue ? 100 : 0,
                        pGroup = 0,
                        pParent = clauses[item.ClausesId],
                        pDepend = "",
                        pCaption = "",
                        pNotes = item.ItemName
                    });
                }
                if (showCorrectiveIds.Contains(item.ClausesId))
                {
                    setTaskOpen(clauses[item.ClausesId]);
                }
                if (showItemIds.Contains(item.ItemId))
                {
                    setTaskOpen(clauses[item.ClausesId]);
                }
                if (showFileIds.Contains(item.FileRequireID))
                {
                    if (clauses.ContainsKey(item.ClausesId))
                    {
                        setTaskOpen(clauses[item.ClausesId]);
                    }
                    if (items.ContainsKey(item.ItemId))
                    {
                        setTaskOpen(items[item.ItemId]);
                    }
                    if (fineitems.ContainsKey(item.FineItemId))
                    {
                        setTaskOpen(fineitems[item.FineItemId]);
                    }
                }
            }
            foreach (var item in taskOpen)
            {
                result.First(o => o.pID == item.Key).pOpen = 1;
            }
            if (result.Count > 0 && currentAccount.IsManager)
            {
                result.Insert(0, new TaskItem
                {
                    pID = 1,
                    pName = "AEO认证",
                    pStart = "",
                    pEnd = "",
                    pClass = "ggroupblack",
                    pLink = "",
                    pMile = 0,
                    pRes = "",
                    pComp = 0,
                    pGroup = 1,
                    pParent = 0,
                    pOpen = 1,
                    pDepend = "",
                    pCaption = "",
                    pNotes = ""
                });
            }
            return Content(((System.Xml.Linq.XElement)new { task = result }.ToXml("project")).ToString(), "application/text");
        }

        public ActionResult ServiceInitialize()
        {
            return View();
        }

        public ActionResult ChooseService(string servicePath)
        {
            string message = "未上传文件或文件内容为空";
            string filePath = Server.MapPath(servicePath);
            if (System.IO.File.Exists(filePath))
            {
                using (FileStream fs = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    System.Xml.Serialization.XmlSerializer xmldes = new System.Xml.Serialization.XmlSerializer(typeof(AEOPoco.Other.XmlDocument));
                    var obj = (AEOPoco.Other.XmlDocument)xmldes.Deserialize(fs);
                    if (this._customerCompanyService.ServiceInitialize(currentAccount.CustomerCompany, obj, out message))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ViewBag.Message = message;
            return View("ServiceInitialize");
        }

        public ActionResult ChangePassWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassWord(string oldpassword,string pwd,string password)
        {
            string message;
            if (this._accountService.ChangePassWord( currentAccount, oldpassword, pwd, password, true, out message))
            {
                return StandardJson(currentAccount.IsManager&& !currentComapny.CustomsAuthenticationID.HasValue ? false: true);
            }
            return StandardJson(null, 0, message);
        }
    }
}