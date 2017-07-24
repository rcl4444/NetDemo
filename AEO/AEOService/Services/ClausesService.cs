using AEOPoco.Domain;
using AEOPoco.Other;
using AEOService.Interface;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Services
{
    public class ClausesService : BaseService<Clauses>, IClausesService
    {
        protected readonly IRepository<ClausesPersonLiable> _clausesPersonLiableRepository;
        protected readonly IRepository<CustomerCompany> _customerCompanyRepository;
        protected readonly IRepository<CustomerAccount> _customerAccountRepository;
        protected readonly IRepository<Item> _itemRepository;
        protected readonly IRepository<FineItem> _fineItemRepository;
        protected readonly IRepository<FileRequire> _fileRequireRepository;
        protected readonly IRepository<OutlineClass> _outlineClassRepository;
        protected readonly IRepository<ScoreTask> _scoreTaskRepository;


        public ClausesService(IRepository<Clauses> clausesRepository,
            IRepository<ClausesPersonLiable> clausesPersonLiableRepository,
            IRepository<CustomerCompany> customerCompanyRepository,
            IRepository<CustomerAccount> customerAccountRepository,
            IRepository<Item> itemRepository,
            IRepository<FineItem> fineItemRepository,
            IRepository<FileRequire> fileRequireRepository,
            IRepository<OutlineClass> outlineClassRepository,
            IRepository<ScoreTask> scoreTaskRepository)
            : base(clausesRepository)
        {
            this._clausesPersonLiableRepository = clausesPersonLiableRepository;
            this._customerCompanyRepository = customerCompanyRepository;
            this._customerAccountRepository = customerAccountRepository;
            this._itemRepository = itemRepository;
            this._fineItemRepository = fineItemRepository;
            this._fileRequireRepository = fileRequireRepository;
            this._outlineClassRepository = outlineClassRepository;
            this._scoreTaskRepository = scoreTaskRepository;
        }

        public IQueryable ClausesSearch(int CompanyID)
        {
            var company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == CompanyID).FirstOrDefault();


            var query = (from o in _outlineClassRepository.TableNoTracking.Where(o => o.CustomsAuthenticationID == company.CustomsAuthenticationID)
                         join c in this.NoTrackingQuery on o.Id equals c.OutlineClassID
                         join cp in _clausesPersonLiableRepository.TableNoTracking on c.Id equals cp.ClausesID into temp
                         from c1 in temp.DefaultIfEmpty()
                         select new
                         {
                             outlineClassID =  o.Id,
                             o.OutlineClassName,
                             c.Id, //类ID
                             c.ClausesName, //类
                             PersonID = (int?)c1.CustomerAccountID, //主办人ID
                             c1.CustomerAccount.PersonName //主办人
                         });

            var result = (from q in query
                          group q by new
                          {
                              q.outlineClassID,
                              q.OutlineClassName
                          }
                              into g
                              select new
                              {
                                  g.Key.OutlineClassName,
                                  item = g.Select(o => new
                                  {
                                      o.Id,
                                      o.ClausesName,
                                      o.PersonID,
                                      o.PersonName
                                  })
                              }
                          );
            return result;
        }

        public IQueryable GetTaskList(int CompanyID, int ClausesID)
        {
            var company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == CompanyID).FirstOrDefault();
            //var query = (from c in _selfRepository.TableNoTracking.Where(o => o.Id.Equals(ClausesID) && o.OutlineClass.CustomsAuthenticationID == company.CustomsAuthenticationID)
            //             join cp in _clausesPersonLiableRepository.TableNoTracking.Where(o => o.CustomerCompanyID == company.Id) on c.Id equals cp.ClausesID into temp
            //             from c1 in temp.DefaultIfEmpty()
            //             select new
            //             {
            //                 clausesID = c.Id, //条ID
            //                 c.ClausesName, //条名称
            //                 clausesPersonID = (int?)c1.CustomerAccount.Id, //主办人
            //                 Item = c.Items.Distinct().Select(i => new
            //                 {
            //                     ItemID = i.Id,  //项ID
            //                     i.ItemName,//项名称

            //                     FineItem = i.FineItems.Select(f => new
            //                     {
            //                         FineItemID = f.Id, //细项ID
            //                         f.FineItemName, //细项名称
            //                         FileRequire = f.FileRequires.Select(ff => new
            //                         {
            //                             FileRequireID = ff.Id, //文件名ID
            //                             ff.Description, //文件描述
            //                             ff.SuggestFileName //建议文件名

            //                         })
            //                     })
            //                 })
            //             });

            var query = (from c in this.NoTrackingQuery.Where(o => o.Id.Equals(ClausesID) && o.OutlineClass.CustomsAuthenticationID == company.CustomsAuthenticationID)
                         join cp in _clausesPersonLiableRepository.TableNoTracking.Where(o => o.CustomerCompanyID == company.Id) on c.Id equals cp.ClausesID into temp
                         from c1 in temp.DefaultIfEmpty()
                         join i in _itemRepository.TableNoTracking on c.Id equals i.ClausesID
                         join s in _scoreTaskRepository.TableNoTracking on i.Id equals s.ItemID into temp1
                         from tp in temp1.DefaultIfEmpty()
                         //join fi in _fineItemRepository.TableNoTracking on i.Id equals fi.ItemID
                         //join fr in _fileRequireRepository.TableNoTracking on fi.Id equals fr.FineItemID

                         select new
                         {
                             clausesID = c.Id, //条ID
                             c.ClausesName, //条名称
                             clausesPersonID = (int?)c1.CustomerAccount.Id, //主办人
                             ItemID = i.Id, //项ID
                             i.ItemName, //项名称
                             //FineItemID = fi.Id, //细项ID
                             //fi.FineItemName, //细项名称
                             ScorePersonID = (int?)tp.ScorePersonID, //评分人ID
                             ScorePersonName = tp.ScorePerson.PersonName, //评分人
                             //FileRequireID = fr.Id, //文件名ID
                             //fr.Description,  //文件描述
                             //fr.SuggestFileName//建议文件名
                             FineItem = i.FineItems.Select(f => new
                             {
                                 FineItemID = f.Id, //细项ID
                                 f.FineItemName, //细项名称
                                 FileRequire = f.FileRequires.Select(ff => new
                                 {
                                     FileRequireID = ff.Id, //文件名ID
                                     ff.Description, //文件描述
                                     ff.SuggestFileName //建议文件名

                                 })
                             })
                         });
            var result = (from q in query
                          group q by new
                          {
                              q.clausesID,
                              q.ItemID,
                              q.ItemName,
                              //q.FineItemID,
                              //q.FileRequireID,
                              q.clausesPersonID,
                              q.ScorePersonID,
                              q.ScorePersonName
                              //q.ClausesName
                          }
                              into g
                              select new
                              {
                                  g.Key.clausesID, //条ID
                                  //g.Key.ClausesName, //条名称
                                  g.Key.clausesPersonID,
                                  Item = g.Select(o => new
                                  {
                                      //g.Key.ItemID,
                                      //g.Key.ItemName,
                                      o.ItemID,
                                      o.ItemName,
                                      o.ScorePersonID,
                                      o.ScorePersonName,
                                      o.FineItem
                                      //FineItem = g.Select(f => new
                                      //{
                                      //    f.FineItemID,
                                      //    f.FineItemName,
                                      //    FileRequire = g.Select(ff => new
                                      //    {
                                      //        ff.FileRequireID,
                                      //        ff.Description,
                                      //        ff.SuggestFileName
                                      //    })
                                      //})
                                  })
                              });
            return result;
        }

        public IEnumerable<TaskItemConut> GetTaskConut(int CompanyID, int AccountID, bool IsManger)
        {
            if (IsManger)
            {
                return this.SqlQuery<TaskItemConut>(@"select oc.OutlineClassName,
c.Id ClausesID,c.ClausesName,
COUNT(DISTINCT i.Id) ItemCount,
COUNT(DISTINCT fi.Id) FineItemCount,
COUNT(fr.Id) FileRequireCount
from OutlineClass oc 
inner join Clauses c on oc.Id=c.OutlineClassID
inner join Item i on c.Id =i.ClausesID
inner join FineItem fi on i.Id = fi.ItemID
left join  FileRequire fr on fi.Id = fr.FineItemID 
where fr.CustomerCompanyID = @id 
group by c.Id,c.ClausesName,oc.OutlineClassName,oc.Id
order by oc.Id",
                         new SqlParameter { ParameterName = "@id", Value = CompanyID });
            }
            else
            {
                return this.SqlQuery<TaskItemConut>(@"select oc.OutlineClassName,
c.Id ClausesID,c.ClausesName,
COUNT(DISTINCT i.Id) ItemCount,
COUNT(DISTINCT fi.Id) FineItemCount,
COUNT(fr.Id) FileRequireCount
from OutlineClass oc 
inner join Clauses c on oc.Id=c.OutlineClassID
inner join Item i on c.Id =i.ClausesID
inner join FineItem fi on i.Id = fi.ItemID
left join  FileRequire fr on fi.Id = fr.FineItemID 
left join ClausesPersonLiable cp on c.Id = cp.ClausesID
where fr.CustomerCompanyID = @id and cp.CustomerAccountID=@AccountID
group by c.Id,c.ClausesName,oc.OutlineClassName,oc.Id
order by oc.Id",
                         new SqlParameter { ParameterName = "@id", Value = CompanyID }, new SqlParameter { ParameterName = "@AccountID", Value = AccountID });
            }
            
        }
    }
}
