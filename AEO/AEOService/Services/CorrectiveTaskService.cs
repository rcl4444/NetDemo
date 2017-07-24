using AEOPoco.Domain;
using AEOService.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Other;

namespace AEOService.Services
{
    public class CorrectiveTaskService: BaseService<CorrectiveTask>,ICorrectiveTaskService
    {
        protected readonly IAccountService _accountService;
        protected readonly IRepository<CorrectiveTaskOperationNote> _correctiveTaskOperationNoteRepository;
        protected readonly IRepository<CorrectiveTaskResult> _correctiveTaskResultRepository;
        protected readonly IRepository<ClausesPersonLiable> _clausesPersonLiableRepository;

        public CorrectiveTaskService(
            IRepository<CorrectiveTask> selfRepository,
            IAccountService accountService,
            IRepository<CorrectiveTaskOperationNote> correctiveTaskOperationNoteRepository,
            IRepository<CorrectiveTaskResult> correctiveTaskResultRepository,
            IRepository<ClausesPersonLiable> clausesPersonLiableRepository): base(selfRepository)
        {
            this._accountService = accountService;
            this._correctiveTaskOperationNoteRepository = correctiveTaskOperationNoteRepository;
            this._correctiveTaskResultRepository = correctiveTaskResultRepository;
            this._clausesPersonLiableRepository = clausesPersonLiableRepository;
        }

        public IEnumerable<dynamic> GetDataByAccount(CustomerAccount currAccount)
        {
            List<int> clausesIDs = new List<int>();
            var query = (from ct in this.NoTrackingQuery where ct.CompanyID == currAccount.CustomerCompanyID && !ct.IsDelete select ct);
            if (!currAccount.IsManager)
            {
                clausesIDs = this._accountService.GetChargeClausesID(currAccount).ToList();
                query = query.Where(o => o.ChargePersonID == currAccount.Id || o.CreaterID == currAccount.Id || o.AuditorID == currAccount.Id || clausesIDs.Contains(o.CorrectiveObjectID));
            }
            return query.Select(o => new {
                o.Id,
                o.CorrectiveObjectID,
                o.CorrectiveObject.ClausesName,//条
                o.CorrectiveContent,//整改内容
                o.Status,//状态
                FinishTime = o.FinishTime,//预计完成时间
                o.ChargePersonID,
                ChargePerson = o.ChargePerson.PersonName,//主办人
                o.AuditorID,
                AuditPerson = o.Auditor.PersonName,//审核人
                o.CreaterID,
                CreatePerson = o.CreatePerson.PersonName//创建人
            }).ToList().Select(o => new {
                o.Id,
                o.ClausesName,//条
                o.CorrectiveContent,//整改内容
                o.Status,//状态
                FinishTime = o.FinishTime.ToString("yyyy-MM-dd"),//预计完成时间
                o.ChargePerson,//主办人
                o.AuditorID,
                o.AuditPerson,//审核人
                o.CreatePerson,//创建人
                ShowEdit = (o.Status == CorrectiveTaskStatus.Create || o.Status == CorrectiveTaskStatus.Reject) && (currAccount.IsManager || o.CreaterID == currAccount.Id|| clausesIDs.Contains(o.CorrectiveObjectID)),//编辑
                ShowFinish = (o.Status == CorrectiveTaskStatus.Create || o.Status == CorrectiveTaskStatus.Reject) && (o.ChargePersonID == currAccount.Id),//完成
                ShowUpload = (o.Status == CorrectiveTaskStatus.Create || o.Status == CorrectiveTaskStatus.Reject) && (o.ChargePersonID == currAccount.Id),//上传
                ShowDelete = (o.Status == CorrectiveTaskStatus.Create || o.Status == CorrectiveTaskStatus.Reject) && (currAccount.IsManager || o.CreaterID == currAccount.Id || clausesIDs.Contains(o.CorrectiveObjectID)),//删除
                ShowAudit = (o.Status == CorrectiveTaskStatus.Finish) && (o.AuditorID == currAccount.Id),//审核
                ShowReject = (o.Status == CorrectiveTaskStatus.Finish) && (o.AuditorID == currAccount.Id)//审核不通过
            });
        }

        public bool Add(CustomerAccount currAccount, int clausesID, int auditorID, int chargePersonID, DateTime finishTime, string content, out string message)
        {
            var obj = new CorrectiveTask()
            {
                CreateTime = DateTime.Now,
                CorrectiveObjectID = clausesID,
                Status = CorrectiveTaskStatus.Create,
                CreaterID = currAccount.Id,
                CompanyID = currAccount.CustomerCompanyID,
                CorrectiveContent = content,
                FinishTime = finishTime,
                ChargePersonID = chargePersonID,
                AuditorID = auditorID
            };
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    this.Add(obj);
                    this._correctiveTaskOperationNoteRepository.Insert(new CorrectiveTaskOperationNote()
                    {
                        CorrectiveTask = obj,
                        OperationerID = currAccount.Id,
                        CreateTime = DateTime.Now,
                        Description = string.Format("{0}新增整改", currAccount.PersonName)
                    });
                    tran.Commit();
                    message = "";
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }

        public bool Update(CustomerAccount currAccount, int correctiveTaskID, int clausesID, int auditorID, int chargePersonID, DateTime finishTime, string content, out string message)
        {
            var oldobj = this.GetByID(correctiveTaskID);
            if (oldobj == null|| oldobj.IsDelete)
            {
                message = "数据不存在";
                return false;
            }
            if (!currAccount.IsManager && oldobj.CreaterID != currAccount.Id)
            {
                message = "除了创建者和系统管理员不能做修改";
                return false;
            }
            if (oldobj.Status == CorrectiveTaskStatus.Create || oldobj.Status == CorrectiveTaskStatus.Reject)
            {
                oldobj.CorrectiveObjectID = clausesID;
                oldobj.CorrectiveContent = content;
                oldobj.FinishTime = finishTime;
                oldobj.ChargePersonID = chargePersonID;
                oldobj.AuditorID = auditorID;
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        this.Update(oldobj);
                        this._correctiveTaskOperationNoteRepository.Insert(new CorrectiveTaskOperationNote()
                        {
                            CorrectiveTask = oldobj,
                            OperationerID = currAccount.Id,
                            CreateTime = DateTime.Now,
                            Description = string.Format("{0}修改整改", currAccount.PersonName)
                        });
                        tran.Commit();
                        message = "";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        return false;
                    }
                }
            }
            else
            {
                message = "处于已完成状态后不能做修改";
                return false;
            }
        }

        public bool Delete(CustomerAccount currAccount, List<int> correctiveTaskIds,out string message)
        {
            var oldobjs = this.Query.Where(o=>correctiveTaskIds.Contains(o.Id) && !o.IsDelete).ToList();
            if (oldobjs.Count != correctiveTaskIds.Count)
            {
                message = string.Format("id:{0}不存在", string.Join(",", correctiveTaskIds.Where(o => !oldobjs.Select(oi => oi.Id).Contains(o))));
                return false;
            }
            var unpowerIds = oldobjs.Where(o=>o.CreaterID != currAccount.Id).Select(o=>o.Id);
            if (!currAccount.IsManager && unpowerIds.Count()>0)
            {
                message = "您无删除权限";
                return false;
            }
            unpowerIds = oldobjs.Where(o => o.Status != CorrectiveTaskStatus.Create && o.Status != CorrectiveTaskStatus.Reject).Select(o=>o.Id);
            if (unpowerIds.Count() == 0)
            {
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        foreach (var oldobj in oldobjs)
                        {
                            oldobj.IsDelete = true;
                            this.Update(oldobj);
                            this._correctiveTaskOperationNoteRepository.Insert(new CorrectiveTaskOperationNote()
                            {
                                CorrectiveTask = oldobj,
                                OperationerID = currAccount.Id,
                                CreateTime = DateTime.Now,
                                Description = string.Format("{0}删除整改", currAccount.PersonName)
                            });
                        }
                        tran.Commit();
                        message = "";
                        return true;
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        return false;
                    }
                }
            }
            else
            {
                message = "您无删除权限";
                return false;
            }
        }

        private string GetFileStatusDescription(CorrectiveTaskStatus cts)
        {
            string description;
            switch (cts)
            {
                case CorrectiveTaskStatus.Create:
                    description = "创建";
                    break;
                case CorrectiveTaskStatus.Finish:
                    description = "完成";
                    break;
                case CorrectiveTaskStatus.Audit:
                    description = "审核";
                    break;
                case CorrectiveTaskStatus.Reject:
                    description = "审核不通过";
                    break;
                default:
                    description = "其他";
                    break;
            }
            return description;
        }

        private bool StatusChange(List<int> ids,
            CustomerAccount currAccount,
            List<CorrectiveTaskStatus> allowStatus,
            Action<CorrectiveTask> editAction,
            Func<CorrectiveTask,CustomerAccount,string> appendValidate,
            string logformat,
            out string message)
        {
            if (ids == null || ids.Count == 0)
            {
                message = "请选择";
                return false;
            }
            IEnumerable<CorrectiveTask> query = this.Query.Where(o => ids.Contains(o.Id) && o.CompanyID == currAccount.CustomerCompanyID && !o.IsDelete);
            var list = query.ToList();
            if (ids.Count != list.Count)
            {
                message = string.Format("id:{0}不存在", string.Join(",", ids.Where(o => !list.Select(oi => oi.Id).Contains(o))));
                return false;
            }
            var errorStatusli = list.Where(o => !allowStatus.Contains(o.Status));
            if (errorStatusli.Count() > 0)
            {
                message = "状态不符";
                //message = string.Format("id:{0}状态不是{1}", string.Join(",", errorStatusli.Select(o => o.Id)), string.Join(",", allowStatus.Select(o => GetFileStatusDescription(o))));
                return false;
            }
            if (appendValidate != null)
            {
                foreach (var item in list)
                {
                    message = appendValidate(item,currAccount);
                    if (!string.IsNullOrEmpty(message))
                    {
                        return false;
                    }
                }
            }
            list.ForEach(editAction);
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    this.Update(list);
                    this._correctiveTaskOperationNoteRepository.Insert(list.Select(o => new CorrectiveTaskOperationNote()
                    {
                        CorrectiveTask = o,
                        OperationerID = currAccount.Id,
                        CreateTime = DateTime.Now,
                        Description = string.Format(logformat, currAccount.PersonName)
                    }));
                    tran.Commit();
                    message = "成功执行";
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    tran.Rollback();
                    return false;
                }
            }
        }

        public bool Finish(List<int> ids, CustomerAccount currAccount, out string message)
        {
            return StatusChange(ids,
                currAccount,
                new List<CorrectiveTaskStatus> { CorrectiveTaskStatus.Reject,CorrectiveTaskStatus.Create },
                (o => { o.Status = CorrectiveTaskStatus.Finish; }),
                ((o, c) =>
                {
                    if (o.ChargePersonID == c.Id)
                    {
                        return "";
                    }
                    else
                    {
                        return "只有整改主办人才能进行完成操作";
                    }
                }),
                "{0}进行完成操作",
                out message);
        }

        public bool Audit(List<int> ids, CustomerAccount currAccount, out string message)
        {
            return StatusChange(ids,
                currAccount,
                new List<CorrectiveTaskStatus> { CorrectiveTaskStatus.Finish },
                (o => { o.Status = CorrectiveTaskStatus.Audit;o.CompleteTime = DateTime.Now; }),
                ((o, c) =>
                {
                    if (o.AuditorID == c.Id)
                    {
                        return "";
                    }
                    else
                    {
                        return "只有整改审核人才能进行审核操作";
                    }
                }),
                "{0}进行审核操作",
                out message);
        }

        public bool Reject(List<int> ids, string reason, CustomerAccount currAccount, out string message)
        {
            return StatusChange(ids,
                currAccount,
                new List<CorrectiveTaskStatus> { CorrectiveTaskStatus.Finish },
                (o => { o.Status = CorrectiveTaskStatus.Reject; }),
                ((o, c) =>
                {
                    if (o.AuditorID == c.Id)
                    {
                        return "";
                    }
                    else
                    {
                        return "只有整改审核人才能进行审核不通过操作";
                    }
                }),
                "{0}进行审核不通过操作,原因:"+reason,
                out message);
        }

        public bool UploadFile(int correctiveTaskID, CustomerAccount currentAccount, string uploadFileName, string contentType, Action<string, string> fileSave, out string message)
        {
            var correctiveTask = this.GetByID(correctiveTaskID);
            if (correctiveTask == null || correctiveTask.IsDelete)
            {
                message = "不存在该整改信息";
                return false;
            }
            if (correctiveTask.ChargePersonID != currentAccount.Id)
            {
                message = "除了主办人不能上传文件";
                return false;
            }
            string physicalFileName = string.Format("{0}{1}", Guid.NewGuid(), System.IO.Path.GetExtension(uploadFileName));
            string physicalContents = string.Format("/corrective/{0}/{1}/", currentAccount.CustomerCompany.CompanyName, correctiveTask.CorrectiveObject.Id);
            fileSave(physicalContents, physicalFileName);
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    this._correctiveTaskResultRepository.Insert(new CorrectiveTaskResult() 
                    {
                        CorrectiveTask = correctiveTask,
                        FileName = uploadFileName,
                        PhysicalFullPath = physicalContents + physicalFileName,
                        UploadTime = DateTime.Now,
                        UploadPerson = currentAccount,
                        ContentType = contentType,
                        CreateTime = DateTime.Now,
                        IsCancel = false
                    });
                    this._correctiveTaskOperationNoteRepository.Insert(new CorrectiveTaskOperationNote()
                    {
                        CorrectiveTask = correctiveTask,
                        Operationer = currentAccount,
                        Description = string.Format("上传:{0}", uploadFileName),
                        CreateTime = DateTime.Now
                    });
                    tran.Commit();
                    message = "";
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    tran.Rollback();
                    return false;
                }
            }
        }

        public IQueryable<CorrectiveTaskResult> GetCorrectiveTaskResult(int correctiveTaskID)
        {
            return this._correctiveTaskResultRepository.TableNoTracking.Where(o => o.CorrectiveTaskID == correctiveTaskID && !o.IsCancel);
        }

        public IQueryable<CorrectiveTaskOperationNote> GetOperationNote(int correctiveTaskID)
        {
            return this._correctiveTaskOperationNoteRepository.TableNoTracking.Where(o => o.CorrectiveTaskID == correctiveTaskID);
        }

        public bool FileCancel(int correctiveTaskResultID, CustomerAccount account,out string message)
        {
            var result = this._correctiveTaskResultRepository.GetById(correctiveTaskResultID);
            if (result == null || result.IsCancel)
            {
                message = "该文件上传不存在";
                return false;
            }
            if (result.CorrectiveTask.ChargePersonID != account.Id )
            {
                message = "除了主办人不能作废文件";
                return false;
            }
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    result.IsCancel = true;
                    this._correctiveTaskResultRepository.Update(result);
                    this._correctiveTaskOperationNoteRepository.Insert(new CorrectiveTaskOperationNote()
                    {
                        CorrectiveTask = result.CorrectiveTask,
                        OperationerID = account.Id,
                        CreateTime = DateTime.Now,
                        Description = string.Format("作废:{0}", result.FileName)
                    });
                    tran.Commit();
                    message = "";
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }

        public CorrectiveTaskResult GetUploadFile(int correctiveTaskResultID)
        {
            var result = this._correctiveTaskResultRepository.GetById(correctiveTaskResultID);
            return (result == null|| result.IsCancel)? null :result;
        }

        public IEnumerable<TaskSchedule> GetCorrectiveSchedule(CustomerAccount account)
        {
            var query = (from ct in this.NoTrackingQuery
                         join cplr in _clausesPersonLiableRepository.TableNoTracking on ct.CorrectiveObjectID equals cplr.ClausesID into temp
                         from clausesPerson in temp.DefaultIfEmpty()
                         where ct.CompanyID == account.CustomerCompanyID && !ct.IsDelete
                         select new
                         {
                             ct.Id,
                             ct.CorrectiveObjectID,
                             ct.CorrectiveObject.ClausesName,
                             ct.CorrectiveContent,
                             ct.FinishTime,
                             ct.CompleteTime,
                             ct.CreaterID,
                             ChargePerson = ct.ChargePerson.PersonName,
                             ct.ChargePersonID,
                             ct.AuditorID,
                             ClausesChargePerson = clausesPerson.CustomerAccount.PersonName,
                         });
            if (!account.IsManager)
            {
                var clausesFilter = this._accountService.GetChargeClausesID(account);
                query = query.Where(o => o.CreaterID == account.Id || o.ChargePersonID == account.Id || o.AuditorID == account.Id || clausesFilter.Contains(o.CorrectiveObjectID));
            }
            return query.Select(o => new TaskSchedule()
            {
                FileRequireID = o.Id,
                ClausesId = o.CorrectiveObjectID,
                ClausesName = o.ClausesName,
                ClausesChargePerson = o.ClausesChargePerson,
                ItemName = o.CorrectiveContent,
                FinishTime = o.FinishTime,
                RealFinishTime = !o.CompleteTime.HasValue ? (DateTime?)null : o.CompleteTime.Value,
                ChargePerson = o.ChargePerson,
                Type = ScheduleType.CorrectiveTask
            });
        }
    }
}
