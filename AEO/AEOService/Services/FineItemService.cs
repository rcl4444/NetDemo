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
    public class FineItemService : BaseService<FineItem>, IFineItemService
    {
        protected readonly IRepository<CustomerCompany> _customerCompanyRepository;

        public FineItemService(IRepository<FineItem> fineItemRepository,
            IRepository<CustomerCompany> customerCompanyRepository)
            : base(fineItemRepository)
        {
            this._customerCompanyRepository = customerCompanyRepository;
        }

        public IQueryable GetFineItemList(int itemID, int CompanyID)
        {
            var company = _customerCompanyRepository.TableNoTracking.Where(o => o.Id == CompanyID).FirstOrDefault();
            var query = (from o in this.NoTrackingQuery.Where(o => o.Id == itemID && o.Item.Clauses.OutlineClass.CustomsAuthenticationID == company.CustomsAuthenticationID)
                         select new
                         {
                             FineItemID = o.Id,   //细项ID
                             o.FineItemName, //细项名称
                             FileRequires = o.FileRequires.Select(f => new
                             {
                                 fileRequireID = f.Id, //文件ID
                                 f.Description,        //文件描述
                                 f.SuggestFileName,   //建议文件名
                                 FinishTime = (DateTime?)f.FileSchedule.FinishTime,  //预计完成时间
                                 ReviewerPersonID = (int?)f.FileSchedule.Auditor.Id,       //审核人ID
                                 ReviewerPersonName = f.FileSchedule.Auditor.PersonName,  //审核人
                                 ChargePersonID = (int?)f.FileSchedule.ChargePerson.Id,     //主办人ID
                                 ChargePersonName = f.FileSchedule.ChargePerson.PersonName //主办人
                             })
                         }).ToList().Select(c => new
                         {
                             c.FineItemID,
                             c.FineItemName,
                             FileRequires = c.FileRequires.Select(f => new
                             {
                                 f.fileRequireID, //文件ID
                                 f.Description,        //文件描述
                                 f.SuggestFileName,   //建议文件名
                                 FinishTime = !f.FinishTime.HasValue ? "" : f.FinishTime.Value.ToString("yyyy-MM-dd"),  //预计完成时间
                                 f.ReviewerPersonID,       //审核人ID
                                 f.ReviewerPersonName,  //审核人
                                 f.ChargePersonID,     //主办人ID
                                 f.ChargePersonName //主办人 
                             })
                         }).AsQueryable();
            return query;
        }
    }
}
