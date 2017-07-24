using AEOPoco.Domain;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOService.Interface;
using Service.Interface;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace AEOService.Services
{
    public class DeparementService : BaseService<CustomerDeparement>, IDeparementService
    {
        private readonly ICustomerCompanyService _CustomerCompanyService;
        private readonly IRepository<CustomerAccount> _CustomerAccountRepository;

        public DeparementService(IRepository<CustomerDeparement> customerDeparementRepository,
            ICustomerCompanyService CustomerCompanyService)
            : base(customerDeparementRepository)
        {
            this._CustomerCompanyService = CustomerCompanyService;
        }

        public bool AddDeparement(int companyid, string DeparementName, string Description, out string message)
        {
            var Deparement = new CustomerDeparement
            {
                CustomerCompanyID = companyid,
                DeparementName = DeparementName,
                Description = Description,
                CreateTime = DateTime.Now
            };
            try
            {
                this.Add(Deparement);
                message = "添加成功";
                return true;
            }
            catch (Exception)
            {
                message = "添加成功";
                return false;
            }
        }

        public bool DelDeparement(int companyid, int deparementid, out string message)
        {
            var Deparement = this.Query.Where(o => o.CustomerCompanyID.Equals(companyid) && o.Id.Equals(deparementid)).FirstOrDefault();
            if (Deparement == null)
            {
                message = "部门不能为空";
                return false;
            }
            else
            {
                try
                {
                    var i = this.ExecuteSqlCommand(@"
                                           update CustomerAccount set CustomerDeparementID = null where id in (select id from CustomerAccount where CustomerDeparementID = @deparementid);
                                           delete CustomerDeparement where id = @deparementid;",
                        new SqlParameter { ParameterName = "@deparementid", Value = deparementid });
                    message = "删除成功";
                    return true;
                }
                catch (Exception ex)
                {
                    message = "删除失败";
                    return false;
                }
            }
        }

        public bool UpdateDeparement(int companyid, int deparementid, string DeparementName,string Description, out string message)
        {
            var Deparement = this.Query.Where(o => o.CustomerCompanyID.Equals(companyid) && o.Id.Equals(deparementid)).FirstOrDefault();
            Deparement.DeparementName = DeparementName;
            Deparement.Description = Description;
            try
            {
                this.Update(Deparement);
                message = "更新成功";
                return true;
            }
            catch (Exception ex)
            {
                message = "更新失败";
                return false;
            }
        }

        public IQueryable GetDeparementList(int companyid)
        {
            var query = (from d in this.NoTrackingQuery.Where(o => o.CustomerCompanyID == companyid)
                         select new 
                         {
                            d.Id, //部门ID
                            d.DeparementName, //部门名称
                         });
            return query;
        }
    }
}
