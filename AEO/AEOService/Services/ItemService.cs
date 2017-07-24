using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Repository.Interface;

namespace AEOService.Services
{
    public class ItemService : BaseService<Item>, IItemService
    {
        protected readonly IRepository<ScoreTask> _scoreTaskRepository;
        protected readonly IRepository<FineItem> _fineItemRepository;
        protected readonly IRepository<FileSchedule> _fileScheduleRepository;
        protected readonly IRepository<FileRequire> _fileRequireRepository;
        protected readonly IRepository<Clauses> _clausesRepository;
        protected readonly IRepository<CustomerCompany> _customerCompanyRepository;
        protected readonly IRepository<ClausesPersonLiable>  _clausesPersonLiableRepository;

        public ItemService(IRepository<Item> itemRepository,
            IRepository<ScoreTask> scoreTaskRepository,
            IRepository<FineItem> fineItemRepository,
            IRepository<FileRequire> fileRequireRepository,
            IRepository<FileSchedule> fileScheduleRepository,
            IRepository<Clauses> clausesRepository,
            IRepository<CustomerCompany> customerCompanyRepository,
            IRepository<ClausesPersonLiable> clausesPersonLiableRepository
            )
            : base(itemRepository)
        {
            this._scoreTaskRepository = scoreTaskRepository;
            this._fineItemRepository = fineItemRepository;
            this._fileScheduleRepository = fileScheduleRepository;
            this._fileRequireRepository = fileRequireRepository;
            this._clausesRepository = clausesRepository;
            this._customerCompanyRepository = customerCompanyRepository;
            this._clausesPersonLiableRepository = clausesPersonLiableRepository;
        }



        public bool IsItemScore(int CompanyID)
        {
            var Company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == CompanyID).FirstOrDefault();
            var query = (from i in this.NoTrackingQuery.Where(o => o.Clauses.OutlineClass.CustomsAuthenticationID == Company.CustomsAuthenticationID)
                         join st in _scoreTaskRepository.TableNoTracking on i.Id equals st.ItemID into temp
                         from tp in temp.DefaultIfEmpty()
                         select new
                         {
                             i.Id,
                             i.ItemName,
                             tp.Score
                         }).ToList();
            foreach (var item in query)
            {
                if (!item.Score.HasValue)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetItemCount(int CompanyID)
        {
            var Company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == CompanyID).FirstOrDefault();
            return this.NoTrackingQuery.Where(o => o.Clauses.OutlineClass.CustomsAuthenticationID == Company.CustomsAuthenticationID).GroupBy(o=> o.Id).Count();
        }

        public IQueryable GetItemList(int CompanyID, int AccountID, bool IsManager)
        {
            var Company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == CompanyID).FirstOrDefault();
            if (IsManager)
            {
                var query =
                (from tp in
                     (
                      from c in _clausesRepository.TableNoTracking.Where(o => o.OutlineClass.CustomsAuthenticationID == Company.CustomsAuthenticationID)
                      join i in this.NoTrackingQuery on c.Id equals i.ClausesID
                      join o in _fineItemRepository.TableNoTracking on i.Id equals o.ItemID
                      join s in _scoreTaskRepository.TableNoTracking.Where(o => o.CustomerCompanyID.Equals(CompanyID)) on i.Id equals s.ItemID into temp
                      from tp in temp.DefaultIfEmpty()
                      join sr in _fileRequireRepository.TableNoTracking on o.Id equals sr.FineItemID into temp4
                      from sr in temp4.DefaultIfEmpty()
                      join fs in _fileScheduleRepository.TableNoTracking on sr.Id equals fs.Id into temp2
                      from tp2 in temp2.DefaultIfEmpty()
                      join cpl in _clausesPersonLiableRepository.TableNoTracking on c.Id equals cpl.ClausesID into temp3
                      from tp3 in temp3.DefaultIfEmpty()
                      select new
                      {
                          clausesID = c.Id,
                          c.ClausesName,
                          itemId = i.Id,
                          i.ItemName,
                          fineItemID = o.Id,
                          o.FineItemName,
                          //ScorePersonID = (int?)tp.ScorePersonID,
                          tp.ScorePerson.PersonName,
                          FinishTime = (DateTime?)tp2.FinishTime
                      })
                 group tp by new { tp.clausesID, tp.ClausesName, tp.itemId, tp.ItemName, tp.fineItemID, tp.FineItemName, tp.PersonName } into j
                 select new
                 {
                     j.Key.clausesID, //类ID
                     j.Key.ClausesName, //类名称
                     j.Key.itemId, // 项ID
                     j.Key.ItemName, //项名称
                     j.Key.fineItemID, //细项ID
                     j.Key.FineItemName, //细项名称
                     //j.Key.ScorePersonID, //评分人ID
                     j.Key.PersonName, //评分人
                     //TaskSum = j.Select(o => o.fineItemID).Distinct().Count(), //任务数
                     FinishTime = j.Max(o => o.FinishTime)
                 })
                 .ToList().OrderBy(o => o.clausesID).ThenBy(o=>o.itemId).Select(o => new
                 {
                     o.clausesID, //类ID
                     o.ClausesName, //类名称
                     o.itemId, // 项ID
                     o.ItemName, //项名称
                     o.fineItemID, //细项ID
                     o.FineItemName, //细项名称
                     //j.Key.ScorePersonID, //评分人ID
                     o.PersonName, //评分人
                     //TaskSum = j.Select(o => o.fineItemID).Distinct().Count(), //任务数
                     FinishTime = !o.FinishTime.HasValue ? "" : o.FinishTime.Value.ToString("yyyy-MM-dd")
                 }).AsQueryable();
                return query;
            }
            else
            {
                var query =
                (from tp in
                     (
                      from c in _clausesRepository.TableNoTracking.Where(o => o.OutlineClass.CustomsAuthenticationID == Company.CustomsAuthenticationID)
                      join i in this.NoTrackingQuery on c.Id equals i.ClausesID
                      join o in _fineItemRepository.TableNoTracking on i.Id equals o.ItemID
                      join s in _scoreTaskRepository.TableNoTracking.Where(o => o.CustomerCompanyID.Equals(CompanyID)) on i.Id equals s.ItemID into temp
                      from tp in temp.DefaultIfEmpty()
                      join sr in _fileRequireRepository.TableNoTracking on o.Id equals sr.FineItemID
                      join fs in _fileScheduleRepository.TableNoTracking on sr.Id equals fs.Id into temp2
                      from tp2 in temp2.DefaultIfEmpty()
                      join cpl in _clausesPersonLiableRepository.TableNoTracking.Where(o => o.CustomerAccountID == AccountID) on c.Id equals cpl.ClausesID
                      select new
                      {
                          clausesID = c.Id,
                          c.ClausesName,
                          itemId = i.Id,
                          i.ItemName,
                          fineItemID = o.Id,
                          o.FineItemName,
                          //ScorePersonID = (int?)tp.ScorePersonID,
                          tp.ScorePerson.PersonName,
                          FinishTime = (DateTime?)tp2.FinishTime
                      })
                 group tp by new { tp.clausesID, tp.ClausesName, tp.itemId, tp.ItemName, tp.fineItemID, tp.FineItemName, tp.PersonName } into j
                 select new
                 {
                     j.Key.clausesID, //类ID
                     j.Key.ClausesName, //类名称
                     j.Key.itemId, // 项ID
                     j.Key.ItemName, //项名称
                     j.Key.fineItemID, //细项ID
                     j.Key.FineItemName, //细项名称
                     //j.Key.ScorePersonID, //评分人ID
                     j.Key.PersonName, //评分人
                     //TaskSum = j.Select(o => o.fineItemID).Distinct().Count(), //任务数
                     FinishTime = j.Max(o => o.FinishTime)
                 })
                 .ToList().OrderBy(o => o.clausesID).ThenBy(o => o.itemId).Select(o => new
                 {
                     o.clausesID, //类ID
                     o.ClausesName, //类名称
                     o.itemId, // 项ID
                     o.ItemName, //项名称
                     o.fineItemID, //细项ID
                     o.FineItemName, //细项名称
                     //j.Key.ScorePersonID, //评分人ID
                     o.PersonName, //评分人
                     //TaskSum = j.Select(o => o.fineItemID).Distinct().Count(), //任务数
                     FinishTime = !o.FinishTime.HasValue ? "" : o.FinishTime.Value.ToString("yyyy-MM-dd")
                 }).AsQueryable();
                return query;
            }
            
            
        }

    }
}
