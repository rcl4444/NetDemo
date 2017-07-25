using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using AEOPoco.Domain;
using AEOWeb.Controllers;
using Core.Configuration;
using System.IO;

namespace AEOWeb.Controllers
{
    public class FileUploadRequireController : AuthorizeController
    {
        private readonly IFileRequireService _fileRequireService;
        protected readonly IFileScheduleService _fileScheduleService;
        private readonly IFileResultService _fileResultService;
        private readonly IFileOperationNoteService _fileOperationNoteService;
        private readonly MyConfig _myConfig;
        private readonly IPreviewTokenService _previewTokenService;

        public FileUploadRequireController(IFileRequireService fileRequireService,
            IFileResultService fileResultService,
            IFileScheduleService fileScheduleService,
            IFileOperationNoteService fileOperationNoteService,
            IWorkContext workContext,
            IPreviewTokenService previewTokenService,
            MyConfig myConfig)
            : base(workContext)
        {
            this._fileScheduleService = fileScheduleService;
            this._fileRequireService = fileRequireService;
            this._fileResultService = fileResultService;
            this._fileOperationNoteService = fileOperationNoteService;
            this._myConfig = myConfig;
            this._previewTokenService = previewTokenService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(int? status)
        {
            var data = this._fileScheduleService.FileUploadRequireSearch(currentAccount, status);
            return StandardJson(data);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.FileRequireID = id;
            return View();
        }

        [HttpPost]
        public ActionResult GetDetailData(int id)
        {
            var fileRequire = _fileRequireService.GetByID(id);
            if (fileRequire != null)
            {
                var fileResults = _fileResultService.FilterQuery(o => o.FileRequireID == fileRequire.Id).OrderByDescending(o => o.UploadTime);
                var fileOperationNotes = _fileOperationNoteService.FilterQuery(o => o.FileRequireID == fileRequire.Id).OrderByDescending(o => o.CreateTime);
                return StandardJson(new
                {
                    ID = fileRequire.Id,
                    ClausesName = fileRequire.FineItem.Item.Clauses.ClausesName,//条
                    ItemName = fileRequire.FineItem.Item.ItemName,//项
                    FineItemName = fileRequire.FineItem.FineItemName,//细项
                    ChargePersonName = fileRequire.FileSchedule.ChargePerson.PersonName,//主办人
                    ReviewerName = fileRequire.FileSchedule.AuditorID.HasValue? fileRequire.FileSchedule.Auditor.PersonName:"",//审批人
                    FinishTime = fileRequire.FileSchedule.FinishTime.HasValue==true?fileRequire.FileSchedule.FinishTime.Value.ToString("yyyy-MM-dd"):"",//预计完成
                    FileRequire = fileRequire.Description,//文件要求
                    FileResults = fileResults.Select(o => new {
                        ID = o.Id,//ID
                        UploadPersonName = o.UploadPerson.PersonName,//上传人
                        UploadTime = o.UploadTime,//上传日期
                        Status = o.Status//状态
                    }),
                    OperationNotes = fileOperationNotes.Select(o => new {
                        CreateTime = o.CreateTime,//时间
                        Description = o.Description,//操作内容
                        OperationerPersonName = o.Operationer.PersonName//操作者
                    })
                });
            }
            else
            {
                return StandardJson("文件要求不存在");
            }
        }

        public ActionResult SetApplyAudit(int id)
        {
            string message;
            if (this._fileResultService.SetApplyAudit(id, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(message);
            }
        }

        public ActionResult SetCancel(int id)
        {
            string message;
            if (this._fileResultService.SetCancel(id, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(message);
            }
        }

        public ActionResult UploadFile(int id)
        {
            string message = "未上传文件或文件内容为空";
            var file = Request.Files["file"];
            if ( file != null && file.ContentLength >0)
            {

            }
            return StandardJson(message);
        }

        public ActionResult DownloadFile(int fileResultId)
        {
            var fileResult = this._fileResultService.GetByID(fileResultId);
            if (fileResult != null)
            {

            }
            return Content("文件不存在");
        }

        public ActionResult AccreditationPack()
        {
            if (currentAccount.IsManager)
            {
                string physicalBasePath = this.GetPhysicalPath("", this._myConfig.UploadPath);
                return File(this._fileResultService.PackFile(currentAccount.CustomerCompanyID,physicalBasePath), "application/octet-stream", "AEO认证.zip");
            }
            return Content("只有系统管理员才可打包下载");
        }

        public ActionResult GetToken(int Id)
        {
            var fileResult = this._fileResultService.GetByID(Id);
            if (fileResult != null)
            {
                var type = fileResult.ContentType;
                var Token = Guid.NewGuid().ToString();
                _previewTokenService.Add(new PreviewToken
                {
                    Token = Token,
                    CreateTime = DateTime.Now,
                    Path = fileResult.PhysicalFullPath,
                    ContentType = fileResult.ContentType
                });
                return StandardJson(new { Type = type.Contains("vnd.openxmlformats-officedocument") ? "false" : "true", token = Token }, 1, "执行成功");
            }
            return StandardJson("", 0, "文件不存在");
        }
    }
}