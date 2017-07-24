using AEOPoco.Domain;
using AEOPoco.Other;
using AEOService.Interface;
using AEOWeb.Controllers;
using Core;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEOWeb.Controllers
{
    public class ItemScoreController : AuthorizeController
    {
        private readonly IScoreTaskService _scoreTaskService;
        private readonly IFileRequireService _fileRequireService;
        private readonly IFileResultService _fileResultService;
        private readonly IScoreOperationNoteService _scoreOperationNoteService;
        private readonly IClausesPersonLiableService _clausesPersonLiableService;
        private readonly IOutlineclassService _outlineclassService;
        private readonly IItemService _itemService;
        

        public ItemScoreController(IScoreTaskService scoreTaskService,
            IFileResultService fileResultService,
            IScoreOperationNoteService scoreOperationNoteService,
            IClausesPersonLiableService clausesPersonLiableService,
            IFileRequireService fileRequireService,
            IOutlineclassService outlineclassService,
            IItemService itemService,
            IWorkContext workContext):base(workContext)
        {
            this._scoreTaskService = scoreTaskService;
            this._fileResultService = fileResultService;
            this._scoreOperationNoteService = scoreOperationNoteService;
            this._clausesPersonLiableService = clausesPersonLiableService;
            this._fileRequireService = fileRequireService;
            this._outlineclassService = outlineclassService;
            this._itemService = itemService;

        }

        public ActionResult Index()
        {
            ViewBag.ExportSetting = currentComapny.ExportSetting.HasValue? currentComapny.ExportSetting.Value : false;
            return View();
        }

        [HttpPost]
        public ActionResult Search()
        {
            var data = _scoreTaskService.ItemScoreSearch(currentAccount);
            return StandardJson(data);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.ScoreTaskID = id;
            return View();
        }

        [HttpPost]
        public ActionResult GetDetailData(int id)
        {
            string message = "";
            var scoreTask = _scoreTaskService.GetByID(id);
            if (scoreTask != null)
            {
                var fileResults = _fileResultService.GetEffectiveRecords(scoreTask.ItemID,currentAccount.CustomerCompanyID).OrderByDescending(o => o.UploadTime);
                var fileOperationNotes = _scoreOperationNoteService.FilterQuery(o => o.ScoreTaskID == scoreTask.Id).OrderByDescending(o => o.CreateTime);
                var clausesCharge = _clausesPersonLiableService.FilterQuery(o => o.CustomerCompanyID == currentAccount.CustomerCompanyID && o.ClausesID == scoreTask.Item.ClausesID).FirstOrDefault();
                DateTime? finshtime = _fileRequireService.FilterQuery(o => o.CustomerCompanyID == currentAccount.CustomerCompanyID && o.FineItem.Item.ClausesID == scoreTask.Item.ClausesID).Select(o=>(DateTime?)o.FileSchedule.FinishTime).Max();
                return StandardJson(new
                {
                    ID = scoreTask.Id,
                    scoreTask.Item.Clauses.ClausesName,//条
                    scoreTask.Item.ItemName,//项
                    ClausesChargePersonName = clausesCharge == null ? "" : clausesCharge.CustomerAccount.PersonName,//条负责人
                    IsScorePerson = scoreTask.ScorePersonID == currentAccount.Id,
                    scoreTask.ScorePerson.PersonName,//评分人
                    FinshTime = finshtime.HasValue ? finshtime.Value.ToString("yyyy-MM-dd") : "",//预计完成
                    FileRequireCount = fileResults.Count(),
                    FileResults = fileResults.Select(o => new
                    {
                        FileResultId = o.FileResultId.HasValue ? o.FileResultId.Value.ToString() : "",
                        o.ItemName,//细项
                        FinishTime = o.FinishTime.HasValue ? o.FinishTime.Value.ToString("yyyy-MM-dd") : "",//预计完成
                        UploadTime = o.UploadTime.HasValue ? o.UploadTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",//实际完成
                        o.Description,//文件要求
                        Status = o.Status.HasValue ? ((int)o.Status.Value).ToString() : "",//状态
                        AuditTime = o.AuditTime.HasValue ? o.AuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",//审批时间
                        o.PhysicalFullPath,//文件url
                        o.SuggestFileName,
                        o.AuditPerson
                    }),
                    OperationNotes = fileOperationNotes.Select(o => new {
                        o.CreateTime,//时间
                        o.Description,
                        Score = (ScoreLevel?)o.Score
                    }).ToList().Select(o => new
                    {
                        CreateTime = o.CreateTime,//时间
                        Description = o.Description,
                        Score = o.Score == null?"": this._scoreTaskService.GetScoreLevelDescription(o.Score.Value)//评分
                    })
                });
            }
            else
            {
                message = "不存在该评分任务";
            }
            return StandardJson(message);
        }

        [HttpPost]
        public ActionResult Score(ScoreRequire score)
        {
            string message;
            if (_scoreTaskService.Score(score, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(message);
            }
        }

        [HttpPost]
        public ActionResult GetScoreLevel(int id)
        {
            var st = this._scoreTaskService.GetByID(id);
            return StandardJson(st.Item.ScoreConfigure.Select(o => new { 
                text = this._scoreTaskService.GetScoreLevelDescription(o.ScoreValue),
                value = o.ScoreValue 
            }));
        }

        public ActionResult ExportScoreResult()
        {

            if (!_itemService.IsItemScore(currentComapny.Id))
                return StandardJson("全部项评分还未完成，请继续完成评分");

            int count = 0;
            NPOI.SS.UserModel.IWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            count = _itemService.GetItemCount(currentComapny.Id);
            //判断是否高级认证、一般认证
            if (count == 32)
            {
                string[] CellArray = { "3", "5", "7", "8","10","11","13",
                                   "15","17","18","20","24","26","28","29",
                                   "32","33","36","37","38","41","43","47",
                                   "49","50","51","52","53","54","55","56","58"};
                string templetFileName = Server.MapPath("~/resource/高级认证评估表.xls");
                book = _outlineclassService.GetExportData(currentComapny.Id, book, CellArray, templetFileName,true);
            }
            else
            {
                string[] CellArray = { "3", "4", "6","7","9","11","13",
                                   "15","19","21","23","24","27","28",
                                   "31","32","33","36","38","42","44","45",
                                   "46","47","48","49","50","51","53"};
                string templetFileName = Server.MapPath("~/resource/一般认证评估表.xls");
                book = _outlineclassService.GetExportData(currentComapny.Id, book, CellArray, templetFileName,false);
            }

            using (var memoryStream = new System.IO.MemoryStream())
            {
                book.Write(memoryStream);
                string exportFileName = string.Format("{0}{1}.xls", currentComapny.CompanyName,"认证评估表");
                return File(memoryStream.ToArray(), "application/octet-stream", exportFileName);
            }
        }
    }
}