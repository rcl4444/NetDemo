using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOPoco.Domain;
using Core;
using Repository.Interface;
using System.Linq.Expressions;
using AEOPoco.Other;
using System.Data.SqlClient;

namespace AEOService.Services
{
    public class FileScheduleService : BaseService<FileSchedule>,IFileScheduleService
    {
        protected readonly IUserRoleService _userRoleService;
        protected readonly IFileResultService _fileResultService;
        protected readonly IAccountService _accountService;
        protected readonly IRepository<FileRequire> _fileRequireRepository;
        protected readonly IRepository<FileResult> _fileResultRepository;
        protected readonly IRepository<ScoreTask> _scoreTaskRepository;
        protected readonly IRepository<ClausesPersonLiable> _clausesPersonLiableRepository;
        protected readonly IRepository<Item> _itemRequireRepository;
        protected readonly IRepository<FineItem> _fineItemRepository;

        public FileScheduleService(
            IRepository<FileSchedule> selfRepository,
            IUserRoleService userRoleService,
            IRepository<FileRequire> fileRequireRepository,
            IRepository<FineItem> fineItemRepository,
            IRepository<Item> itemRequireRepository,
            IRepository<FileResult> fileResultRepository,
            IAccountService accountService,
            IRepository<ScoreTask> scoreTaskRepository,
            IRepository<ClausesPersonLiable> clausesPersonLiableRepository,
            IFileResultService fileResultService)
            : base(selfRepository)
        {
            this._userRoleService = userRoleService;
            this._fileRequireRepository = fileRequireRepository;
            this._fineItemRepository = fineItemRepository;
            this._itemRequireRepository = itemRequireRepository;
            this._fileResultRepository = fileResultRepository;
            this._accountService = accountService;
            this._scoreTaskRepository = scoreTaskRepository;
            this._clausesPersonLiableRepository = clausesPersonLiableRepository;
            this._fileResultService = fileResultService;
        }

        public bool SetChargeAndReviewer(int CompanyID, int? reviewerPersonID, int? chargePersonID, string finishTime, string FileRequireIDs, out string message)
        {
            if (!chargePersonID.HasValue)
            {
                message = "主办人不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(finishTime))
            {
                message = "预计完成时间不能为空";
                return false;
            }
            using (var tran = this.BeginTransaction())
            {
                var listID = FileRequireIDs.Split(',');

                for (int i = 0; i < listID.Count(); i++)
                {
                    int id = Convert.ToInt32(listID[i]);
                    var fileRequire = this._fileRequireRepository.TableNoTracking.Where(o => o.Id.Equals(id)).FirstOrDefault();
                    if (fileRequire == null)
                    {
                        message = "文件要求不能为空";
                        return false;
                    }
                    else
                    {
                        var entity = this.Query.
                        Where(o => o.FileRequire.Id == id &&
                            o.FileRequire.CustomerCompanyID == CompanyID).FirstOrDefault();
                        if (entity == null)
                        {
                            try
                            {
                                var FileSchedule = new FileSchedule
                                {
                                    Id = id,
                                    ChargePersonID = chargePersonID.Value,
                                    AuditorID = reviewerPersonID,
                                    FinishTime = Convert.ToDateTime(finishTime),
                                    CreateTime = DateTime.Now
                                };
                                _userRoleService.Update(chargePersonID.Value, null, Role.Charge);
                                if (reviewerPersonID.HasValue)
                                {
                                    _userRoleService.Update(reviewerPersonID.Value, null, Role.Reviewer);
                                }
                                this.Add(FileSchedule);
                                message = "设置成功";
                            }
                            catch (Exception ex)
                            {
                                message = "设置失败";
                                return false;
                            }
                        }
                        else
                        {
                            try
                            {
                                _userRoleService.Update(chargePersonID.Value, entity.ChargePersonID, Role.Charge);
                                if (reviewerPersonID.HasValue)
                                {
                                    _userRoleService.Update(reviewerPersonID.Value, entity.AuditorID, Role.Reviewer);
                                }
                                entity.FinishTime = Convert.ToDateTime(finishTime);
                                entity.ChargePersonID = chargePersonID.Value;
                                entity.AuditorID = reviewerPersonID;
                                this.Update(entity);
                                message = "设置成功";
                            }
                            catch (Exception ex)
                            {
                                message = "设置失败";
                                return true;
                            }
                        }
                    }
                }
                message = "设置成功";
                tran.Commit();
                return true;
            }
        }

        public IEnumerable<dynamic> FileUploadRequireSearch(CustomerAccount account,int? status)
        {
            var query = (from fs in this.NoTrackingQuery
                         where fs.FileRequire.CustomerCompanyID == account.CustomerCompanyID && fs.ChargePersonID == account.Id
                         select new
                         {
                             Id = fs.FileRequire.Id,
                             FinishTime = fs.FinishTime,//预计完成
                             ClausesID = fs.FileRequire.FineItem.Item.ClausesID,
                             ClausesName = fs.FileRequire.FineItem.Item.Clauses.ClausesName,//条
                             ItemID = fs.FileRequire.FineItem.ItemID,
                             ItemName = fs.FileRequire.FineItem.Item.ItemName,//项
                             FineItemID = fs.FileRequire.FineItemID,
                             FineItemName = fs.FileRequire.FineItem.FineItemName,//细项内容
                             ChargePersonID = fs.ChargePersonID,
                             PersonName = fs.ChargePerson.PersonName,//主办人
                             SuggestFileName = fs.FileRequire.SuggestFileName,//建议文件名
                         });
            var statusQuery = (from t in this._fileResultService.Query
                               where t.FileRequire.CustomerCompanyID == account.CustomerCompanyID && t.Status != FileStatus.Cancel
                               group t by t.FileRequireID into g
                               select new
                               {
                                   FileRequireID = g.Key,
                                   Status = g.FirstOrDefault(o => o.Id == g.Max(oi => oi.Id)).Status
                               });
            var result = (from fs in query
                     join sq in statusQuery on fs.Id equals sq.FileRequireID into temp
                     from sq in temp.DefaultIfEmpty()
                     select new {
                         fs.Id,
                         fs.FinishTime,
                         fs.ClausesID,
                         fs.ClausesName,
                         fs.ItemID,
                         fs.ItemName,
                         fs.FineItemID,
                         fs.FineItemName,
                         fs.ChargePersonID,
                         fs.PersonName,
                         fs.SuggestFileName,
                         Status = (FileStatus?)sq.Status
                     }
             );
            if (status.HasValue)
            {
                FileStatus? fileStatus = (FileStatus?)(status == 200 ? null : status);
                result = result.Where(o => o.Status == fileStatus);
            }
            result = result.OrderBy(o=>o.ClausesID).ThenBy(o=>o.ItemID).ThenBy(o=>o.FineItemID).ThenBy(o=>o.Id);
            return result.Select(o => new
            {
                o.Id,
                o.FinishTime,//预计完成
                o.ClausesName,//条
                o.ItemName,//项
                o.FineItemName,//细项内容
                o.PersonName,//主办人
                o.SuggestFileName,//文件
                o.Status
            }).ToList().Select(o => new
            {
                o.Id,
                FinishTime = o.FinishTime.HasValue==true?o.FinishTime.Value.ToString("yyyy-MM-dd"):"",//预计完成
                o.ClausesName,//条
                o.ItemName,//项
                o.FineItemName,//细项内容
                o.PersonName,//主办人
                o.SuggestFileName,//文件
                Status = o.Status.HasValue ? (int)o.Status:200
            });
        }

        public IEnumerable<TaskSchedule> GetFileTaskStatus(int companyID, CustomerAccount account, bool isManager)
        {
            List<TaskSchedule> result = new List<TaskSchedule>();
            var query = (from fs in this.NoTrackingQuery
                         join fr in this._fileRequireRepository.TableNoTracking on fs.FileRequire equals fr
                         join fi in this._fineItemRepository.TableNoTracking on fr.FineItem equals fi
                         join i in this._itemRequireRepository.TableNoTracking on fi.Item equals i
                         join cpl in this._clausesPersonLiableRepository.TableNoTracking on i.ClausesID equals cpl.ClausesID into temp
                         from clausesPerson in temp.DefaultIfEmpty()
                         where fs.FileRequire.CustomerCompanyID == companyID && fs.FinishTime.HasValue
                         select new
                         {
                             ClausesId = i.ClausesID,
                             ClausesName = i.Clauses.ClausesName,
                             ItemId = fi.ItemID,
                             ItemName = i.ItemName,
                             FineItemId = fr.FineItemID,
                             FineItemName = fi.FineItemName,
                             FileRequireID = fr.Id,
                             SuggestFileName = fr.SuggestFileName,
                             FinishTime = fs.FinishTime,
                             ReviewerID = fs.AuditorID,
                             fs.ChargePersonID,
                             ChargePerson = fs.ChargePerson.PersonName,
                             ClausesPerson = clausesPerson.CustomerAccount.PersonName
                         });
            if (!isManager)
            {
                var clausesPersonLiableFilter = this._accountService.GetChargeClausesID(account);
                var scoreTaskFilter = (from st in _scoreTaskRepository.TableNoTracking
                                       where st.ScorePersonID == account.Id && st.CustomerCompanyID == companyID
                                       select st.ItemID);
                query = query.Where(o=>(
                (clausesPersonLiableFilter.Contains(o.ClausesId) 
                || scoreTaskFilter.Contains(o.ItemId) 
                || o.ReviewerID == account.Id
                || o.ChargePersonID == account.Id)));
            }
            var fileRequireIDs = query.Select(o => o.FileRequireID).ToList();
            IEnumerable<FileRequireStatus> fileRequireStatuses = new List<FileRequireStatus>();
            if (fileRequireIDs.Count>0)
            {
                fileRequireStatuses = this._fileResultService.GetFileRequireStatus(fileRequireIDs);
            }
            query.ToList().ForEach(o => {
                var fileRequireStatus = fileRequireStatuses.FirstOrDefault(oi => oi.FileRequireId == o.FileRequireID);
                result.Add(new TaskSchedule() {
                    ClausesId = o.ClausesId,
                    ClausesName = o.ClausesName,
                    ItemId = o.ItemId,
                    ItemName = o.ItemName,
                    FineItemId = o.FineItemId,
                    FineItemName = o.FineItemName,
                    FileRequireID = o.FileRequireID,
                    SuggestFileName = o.SuggestFileName,
                    FinishTime = o.FinishTime.HasValue==true?o.FinishTime:null,
                    RealFinishTime = fileRequireStatus == null ? (DateTime?)null : fileRequireStatus.Status == FileStatus.Audit ? fileRequireStatus.AuditTime :(DateTime?)null,
                    Type = ScheduleType.FileRequire,
                    ClausesChargePerson = o.ClausesPerson,
                    ChargePerson = o.ChargePerson
                });
            });
            return result;
        }
    }
}
