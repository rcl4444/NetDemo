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
    public class FileAuditController : AuthorizeController
    {
        private readonly IFileRequireService _fileRequireService;
        private readonly IFileResultService _fileResultService;
        private readonly IFileOperationNoteService _fileOperationNoteService;

        public FileAuditController(IFileRequireService fileRequireService,
            IFileResultService fileResultService,
            IFileOperationNoteService fileOperationNoteService,
            IWorkContext workContext):base(workContext)
        {
            this._fileRequireService = fileRequireService;
            this._fileResultService = fileResultService;
            this._fileOperationNoteService = fileOperationNoteService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search()
        {
            var data = _fileRequireService.FileAuditSearch(currentAccount.CustomerCompanyID, currentAccount.Id,currentAccount.IsManager).Select(o => new {
                Id = o.Id,
                FinishTime = o.FinishTime.HasValue ? o.FinishTime.Value.ToString("yyyy-MM-dd"):"",
                ClausesName = o.ClausesName,
                ItemName = o.ItemName,
                FineItemName = o.FineItemName,
                Description = o.Description,
                PersonName = o.PersonName
            });
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
                var fileResult = _fileResultService.FilterQuery(o => o.FileRequireID == fileRequire.Id&& o.Status != FileStatus.Cancel).OrderByDescending(o=>o.CreateTime).FirstOrDefault();
                var fileOperationNotes = _fileOperationNoteService.FilterQuery(o => o.FileRequireID == fileRequire.Id).OrderByDescending(o => o.CreateTime);
                return StandardJson(new
                {
                    ID = fileRequire.Id,
                    ClausesName = fileRequire.FineItem.Item.Clauses.ClausesName,//条
                    ItemName = fileRequire.FineItem.Item.ItemName,//项
                    FineItemName = fileRequire.FineItem.FineItemName,//细项
                    ChargePersonName = fileRequire.FileSchedule.ChargePerson.PersonName,//主办人
                    ReviewerName = fileRequire.FileSchedule.AuditorID.HasValue? fileRequire.FileSchedule.Auditor.PersonName : "",//审批人
                    FinishTime = fileRequire.FileSchedule.FinishTime.HasValue==true?fileRequire.FileSchedule.FinishTime.Value.ToString("yyyy-MM-dd"):"",//预计完成
                    FileRequire = fileRequire.Description,//文件要求
                    FileResultID = fileResult == null ? "":fileResult.Id.ToString(),//文件结果ID
                    FileResultUploadPersonName = fileResult == null ? "" : fileResult.UploadPerson.PersonName,//上传人
                    FileResultApplyAuditTime = fileResult == null ? "" : fileResult.ApplyAuditTime.HasValue? fileResult.ApplyAuditTime.Value.ToString("yyyy-MM-dd HH:mm:ss"):"",//提审日期
                    FileResultStatus = fileResult == null ? "" : ((int)fileResult.Status).ToString(),//状态
                    FileUrl = fileResult == null ? "" : fileResult.PhysicalFullPath,//文件路径
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

        public ActionResult SetAudit(int id)
        {
            string message;
            if (this._fileResultService.SetAudit(id, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(message);
            }
        }

        public ActionResult SetReject(int id,string rejectReason)
        {
            string message;
            if (this._fileResultService.SetReject(id, rejectReason, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(message);
            }
        }
    }
}