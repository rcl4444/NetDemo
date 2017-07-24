using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using AEOPoco.Other;

namespace AEOService.Services
{
    public class ScoreTaskService : BaseService<ScoreTask>, IScoreTaskService
    {
        protected readonly IRepository<FileSchedule> _fileScheduleRepository;
        protected readonly IScoreOperationNoteService _scoreOperationNoteService;
        private readonly IFileOperationNoteService _fileOperationNoteService;
        protected readonly IUserRoleService _userRoleService;
        protected readonly IFileResultService _fileResultService;
        protected readonly IFileScheduleService _fileScheduleService;
        protected readonly IAccountService _accountService;

        public ScoreTaskService(
            IRepository<ScoreTask> selfRepository,
            IUserRoleService userRoleService,
            IRepository<FileSchedule> fileScheduleRepository,
            IScoreOperationNoteService scoreOperationNoteService,
            IFileResultService fileResultService,
            IFileOperationNoteService fileOperationNoteService,
            IFileScheduleService fileScheduleService,
            IAccountService accountService) : base(selfRepository)
        {
            this._fileScheduleRepository = fileScheduleRepository;
            this._scoreOperationNoteService = scoreOperationNoteService;
            this._userRoleService = userRoleService;
            this._fileResultService = fileResultService;
            this._fileOperationNoteService = fileOperationNoteService;
            this._fileScheduleService = fileScheduleService;
            this._accountService = accountService;
        }

        public string GetScoreLevelDescription(ScoreLevel level)
        {
            string Description;
            switch (level)
            {
                case ScoreLevel.NotApplicable:
                    Description = "不适用(-)";
                    break;
                case ScoreLevel.Substandard:
                    Description = "不达标(-2)";
                    break;
                case ScoreLevel.PartiallyCompliant:
                    Description = "部分达标(-1)";
                    break;
                case ScoreLevel.ReachStandard:
                    Description = "达标(0)";
                    break;
                case ScoreLevel.Conform:
                    Description = "符合(2)";
                    break;
                case ScoreLevel.NotApplicableV2:
                    Description = "不适用(0)";
                    break;
                default:
                    Description = "其他";
                    break;
            }
            return Description;
        }

        public override ScoreTask GetByID(object id)
        {
            return this.Query.Where(o=>o.Id == (int)id).FirstOrDefault();
        }

        public IEnumerable<dynamic> ItemScoreSearch(CustomerAccount account)
        {
            IQueryable<ScoreTask> stquery;
            if (account.IsManager)
            {
                stquery = this.NoTrackingQuery;
            }
            else
            {
                var clausesPersonLiableFilter = this._accountService.GetChargeClausesID(account);
                stquery = this.NoTrackingQuery.Where(o=>o.ScorePersonID == account.Id || clausesPersonLiableFilter.Contains(o.Item.ClausesID));
            }
            var query = (from tb in 
                           (from scoreTask in stquery
                            join t1 in this._fileScheduleRepository.TableNoTracking on scoreTask.ItemID equals t1.FileRequire.FineItem.ItemID into temp
                            from fileSchedule in temp.DefaultIfEmpty()
                            join t2 in this._fileResultService.NoTrackingQuery on scoreTask.ItemID equals t2.FileRequire.FineItem.ItemID into temp2
                            from fileResult in temp2.DefaultIfEmpty()
                            where scoreTask.CustomerCompanyID == account.CustomerCompanyID
                            select new
                            {
                                scoreTask.Id,
                                scoreTask.Score,//评分结果
                                fileSchedule.FinishTime,//预计完成
                                fileResult.UploadTime,//实际上传
                                scoreTask.Item.ClausesID,//条ID
                                scoreTask.Item.Clauses.ClausesName,//条
                                scoreTask.Item.ItemName//项
                            })
                         group tb by new { tb.Id, tb.Score, tb.ClausesID, tb.ClausesName, tb.ItemName } into g
                         select new
                         {
                             g.Key.Id,
                             g.Key.Score,
                             g.Key.ClausesID,
                             g.Key.ClausesName,
                             g.Key.ItemName,
                             FinishTime = (DateTime?)g.Max(o => o.FinishTime),
                             UploadTime = (DateTime?)g.Max(o=>o.UploadTime)
                         }).ToList().Select(o => new {
                             o.Id,//评分项ID
                             Score = o.Score.HasValue? this.GetScoreLevelDescription(o.Score.Value):"", 
                             o.ClausesID,
                             o.ClausesName,//条名
                             o.ItemName,//项ID
                             FinishTime = o.FinishTime.HasValue? o.FinishTime.Value.ToString("yyyy-MM-dd"): "",//预计完成时间
                             UploadTime = o.UploadTime.HasValue ? o.UploadTime.Value.ToString("yyyy-MM-dd") : ""
                         });
            return query.OrderBy(o=>o.ClausesID);
        }

        public bool Score(ScoreRequire score, CustomerAccount currentAccount, out string message)
        {
            var scoreTask = this.GetByID(score.ScoreTaskID);
            if (scoreTask != null)
            {
                var fileResults = this._fileResultService.GetEffectiveRecords(scoreTask.ItemID, currentAccount.CustomerCompanyID);
                if (fileResults.Where(o => o.Status != FileStatus.Audit).Count() > 0)
                {
                    message = "只有所有文件审核完毕才可评分";
                    return false;
                }
                if (scoreTask.ScorePersonID != currentAccount.Id)
                {
                    message = "只有评分人才可进行评分";
                    return false;
                }
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        var dt = DateTime.Now;
                        StringBuilder sb = new StringBuilder();
                        scoreTask.Score = score.Score;
                        scoreTask.ScoreTime = dt;
                        this.Update(scoreTask);
                        if (score.Score == ScoreLevel.PartiallyCompliant || score.Score == ScoreLevel.Substandard)
                        {
                            var fileResultIDs = score.Argument.Select(oi => oi.FileResultID).ToList();
                            var rejectFileResults = this._fileResultService.Query.Where(o => fileResultIDs.Contains(o.Id));
                            foreach (var item in rejectFileResults)
                            {
                                item.Status = FileStatus.Reject;
                                this._fileResultService.Update(item);
                                string cause = score.Argument.First(o=>o.FileResultID==item.Id).Cause;
                                this._fileOperationNoteService.Add(new FileOperationNote()
                                {
                                    FileRequire = item.FileRequire,
                                    Operationer = currentAccount,
                                    Description = string.Format("评分:{0},原因:{1}", GetScoreLevelDescription(score.Score),cause),
                                    CreateTime = DateTime.Now
                                });
                                sb.AppendLine(string.Format("{0}:{1}", item.FileRequire.SuggestFileName, cause));
                                var fileSchedule = _fileScheduleService.Query.Where(o => o.Id == item.FileRequireID).FirstOrDefault();
                                fileSchedule.FinishTime = null;
                                this._fileScheduleService.Update(fileSchedule);
                            }
                        }
                        this._scoreOperationNoteService.Add(new ScoreOperationNote()
                        {
                            ScoreTask = scoreTask,
                            Operationer = currentAccount,
                            Score = score.Score,
                            Description = sb.ToString(),
                            CreateTime = dt
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
            else
            {
                message = "该数据不存在";
            }
            return false;
        }

        public bool Score(int scoreTaskID, ScoreLevel level, CustomerAccount currentAccount, out string message)
        {
            var scoreTask = this.GetByID(scoreTaskID);
            if (scoreTask != null)
            {
                var fileResults = this._fileResultService.GetEffectiveRecords(scoreTask.ItemID, currentAccount.CustomerCompanyID);
                if (fileResults.Where(o=>o.Status != FileStatus.Audit).Count()>0)
                {
                    message = "只有所有文件审核完毕才可评分";
                    return false;
                }
                if (scoreTask.ScorePersonID != currentAccount.Id)
                {
                    message = "只有评分人才可进行评分";
                    return false;
                }
                using (var tran = this.BeginTransaction())
                {
                    try
                    {
                        var dt = DateTime.Now;
                        scoreTask.Score = level;
                        scoreTask.ScoreTime = dt;
                        this.Update(scoreTask);
                        this._scoreOperationNoteService.Add(new ScoreOperationNote() {
                            ScoreTask = scoreTask,
                            Operationer = currentAccount,
                            Score = level,
                            Description = GetScoreLevelDescription(level),
                            CreateTime = dt
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
            else
            {
                message = "该数据不存在";
            }
            return false;
        }

        public bool SetScoerPerosn(List<int> itemIDs, int scorePersonID, int CompanyID, out string message)
        {
            if (itemIDs.Count() > 0)
            {
                using (var tran = this.BeginTransaction())
                {
                    for (int i = 0; i < itemIDs.Count(); i++)
                    {
                        int ID = Convert.ToInt32(itemIDs[i]);
                        var scoreTask = this.Query.Where(o => o.ItemID == ID && o.CustomerCompanyID == CompanyID).FirstOrDefault();
                        if (scoreTask == null)
                        {
                            try
                            {
                                var entity = new ScoreTask
                                {
                                    ItemID = ID,
                                    ScorePersonID = scorePersonID,
                                    CustomerCompanyID = CompanyID,
                                    CreateTime = DateTime.Now
                                };
                                this._userRoleService.Update(scorePersonID, null, Role.Score);
                                this.Add(entity);
                                message = "设置成功";
                            }
                            catch (Exception)
                            {
                                tran.Rollback();
                                message = "设置失败";
                                return false;
                            }
                        }
                        else
                        {
                            try
                            {
                                this._userRoleService.Update(scorePersonID, scoreTask.ScorePersonID, Role.Score);
                                scoreTask.ItemID = ID;
                                scoreTask.ScorePersonID = scorePersonID;
                                this.Update(scoreTask);
                                message = "设置成功";
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                message = "设置失败";
                                return false;
                            }
                        }
                    }
                    message = "设置完成";
                    tran.Commit();
                    return true;
                }
            }
            else
            {
                message = "请选择ID";
                return false;
            }
        }
    }
}
