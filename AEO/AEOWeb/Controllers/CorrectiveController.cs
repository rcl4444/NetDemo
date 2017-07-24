using AEOPoco.Domain;
using AEOService.Interface;
using AEOWeb.Controllers;
using Core;
using Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AEOWeb.Controllers
{
    public class CorrectiveController : AuthorizeController
    {
        private readonly ICorrectiveTaskService _correctiveTaskService;
        private readonly IAccountService _accountService;
        private readonly MyConfig _myConfig;

        public CorrectiveController(IWorkContext workContext,
            ICorrectiveTaskService correctiveTaskService,
            IAccountService accountService,
            MyConfig myConfig) : base(workContext)
        {
            this._correctiveTaskService = correctiveTaskService;
            this._accountService = accountService;
            this._myConfig = myConfig;
        }

        /// <summary>
        /// 整改列表模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ShowAdd = currentAccount.IsManager ? true : this._accountService.GetChargeClausesID(currentAccount).Any() ? true : false;
            ViewBag.CurrUserId = currentAccount.Id;
            return View();
        }

        /// <summary>
        /// 整改列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSearchData()
        {
            return StandardJson(this._correctiveTaskService.GetDataByAccount(currentAccount));
        }

        /// <summary>
        /// 整改详情模板
        /// </summary>
        /// <param name="id">整改ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int id)
        {
            ViewBag.CorrectiveTaskID = id;
            return View();
        }

        /// <summary>
        /// 整改详情数据源
        /// </summary>
        /// <param name="id">整改ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDetailData(int id)
        {
            var task = this._correctiveTaskService.GetByID(id);
            if (task == null || !task.IsDelete)
            {
                var taskResults = this._correctiveTaskService.GetCorrectiveTaskResult(task.Id);
                var operationNotes = this._correctiveTaskService.GetOperationNote(task.Id);
                return StandardJson(new {
                    ClausesName = task.CorrectiveObject.ClausesName,//条
                    Content = task.CorrectiveContent,//整改内容
                    FinishTime = task.FinishTime.ToString("yyyy-MM-dd"),//预计完成时间
                    ChargePerson = task.ChargePerson.PersonName,//主办人
                    Auditor = task.Auditor.PersonName,//审核人
                    Status = task.Status,//状态
                    Uploads = taskResults.Select(o=>new {
                        o.Id,
                        UploadPerson = o.UploadPerson.PersonName,//上传人
                        UploadTime = o.UploadTime,//上传日期
                        FileName = o.FileName//文件名称
                    }).ToList().Select(o=>new {
                        o.Id,
                        o.UploadPerson,
                        UploadTime = o.UploadTime.ToString("yyyy-MM-dd"),
                        o.FileName
                    }),
                    OperationNotes = operationNotes.Select(o=>new {
                        o.CreateTime,//时间
                        o.Description,//操作内容
                        Operationer = o.Operationer.PersonName//操作者
                    })
                });
            }
            else
            {
                return StandardJson(null, 0, "该整改数据不存在");
            }
        }

        /// <summary>
        /// 整改上传文件列表模板
        /// </summary>
        /// <param name="id">整改ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadList(int id)
        {
            ViewBag.CorrectiveTaskID = id;
            return View();
        }

        /// <summary>
        /// 整改上传文件列表数据源
        /// </summary>
        /// <param name="id">整改ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUploadData(int id)
        {
            var task = this._correctiveTaskService.GetByID(id);
            if (task == null || !task.IsDelete)
            {
                var taskResults = this._correctiveTaskService.GetCorrectiveTaskResult(task.Id);
                var operationNotes = this._correctiveTaskService.GetOperationNote(task.Id);
                return StandardJson(new
                {
                    ClausesName = task.CorrectiveObject.ClausesName,//条
                    Content = task.CorrectiveContent,//整改内容
                    FinishTime = task.FinishTime.ToString("yyyy-MM-dd"),//预计完成时间
                    ChargePerson = task.ChargePerson.PersonName,//主办人
                    Auditor = task.Auditor.PersonName,//审核人
                    Status = task.Status,//状态
                    Uploads = taskResults.Select(o => new
                    {
                        o.Id,
                        UploadPerson = o.UploadPerson.PersonName,//上传人
                        UploadTime = o.UploadTime,//上传日期
                        FileName = o.FileName//文件名称
                    }).ToList().Select(o => new
                    {
                        o.Id,
                        o.UploadPerson,
                        UploadTime = o.UploadTime.ToString("yyyy-MM-dd"),
                        o.FileName
                    })
                });
            }
            else
            {
                return StandardJson(null, 0, "该整改数据不存在");
            }
        }

        /// <summary>
        /// 整改文件作废
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            string message;
            if (this._correctiveTaskService.FileCancel(id, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }

        /// <summary>
        /// 整改文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileUpload(int id)
        {
            string message = "未上传文件或文件内容为空";
            var file = Request.Files["file"];
            if (file != null && file.ContentLength > 0)
            {
                Action<string, string> fileSave = (physicalContents, physicalFileName) =>
                {
                    string directoryPath = this.GetPhysicalPath(physicalContents, this._myConfig.UploadPath);
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }
                    file.SaveAs(System.IO.Path.Combine(directoryPath, physicalFileName));
                };
                if (this._correctiveTaskService.UploadFile(id, currentAccount, file.FileName, file.ContentType, fileSave, out message))
                {
                    return StandardJson();
                }
            }
            return StandardJson(message);
        }

        /// <summary>
        /// 整改文件下载
        /// </summary>
        /// <param name="id">文件ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FileDownLoad(int id)
        {
            var fileResult = this._correctiveTaskService.GetUploadFile(id);
            if (fileResult != null)
            {
                string filePath = this.GetPhysicalPath(fileResult.PhysicalFullPath, this._myConfig.UploadPath);
                if (System.IO.File.Exists(filePath))
                {
                    return File(filePath, "application/octet-stream", fileResult.FileName);
                }
            }
            return Content("文件不存在");
        }

        /// <summary>
        /// 整改新增
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(int clausesID,int auditorID, int chargePersonID,DateTime finishTime,string content)
        {
            string message;
            if (this._correctiveTaskService.Add(currentAccount, clausesID, auditorID, chargePersonID, finishTime, content,out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }

        /// <summary>
        /// 获取整改任务数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCorrectiveTaskData(int id)
        {
            var data = this._correctiveTaskService.GetByID(id);
            if (data == null || data.IsDelete)
            {
                return StandardJson(null, 0, "数据不存在");
            }
            else
            {
                return StandardJson(new {
                    ClausesID = data.CorrectiveObjectID,
                    FinishTime = data.FinishTime.ToString("yyyy-MM-dd"),
                    ChargePerson = data.ChargePersonID,
                    Auditor = data.AuditorID,
                    Content = data.CorrectiveContent
                });
            }
        }

        /// <summary>
        /// 整改修改
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(int id, int clausesID, int auditorID, int chargePersonID, DateTime finishTime, string content)
        {
            string message;
            if (this._correctiveTaskService.Update(currentAccount, id, clausesID, auditorID, chargePersonID, finishTime, content,out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }

        /// <summary>
        /// 整改删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(List<int> id)
        {
            string message;
            if (this._correctiveTaskService.Delete(currentAccount, id, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }

        /// <summary>
        /// 整改完成
        /// </summary>
        /// <param name="id">整改ID集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Finish(List<int> id)
        {
            string message;
            if (this._correctiveTaskService.Finish(id,currentAccount,out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }

        /// <summary>
        /// 整改审核
        /// </summary>
        /// <param name="id">整改ID集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Audit(List<int> id)
        {
            string message;
            if (this._correctiveTaskService.Audit(id, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }

        /// <summary>
        /// 整改审核不通过
        /// </summary>
        /// <param name="id">整改ID集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reject(List<int> id,string reason)
        {
            string message;
            if (this._correctiveTaskService.Reject(id, reason, currentAccount, out message))
            {
                return StandardJson();
            }
            else
            {
                return StandardJson(null, 0, message);
            }
        }
    }
}