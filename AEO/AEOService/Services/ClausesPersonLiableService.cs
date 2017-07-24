using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using AEOService.Interface;

namespace AEOService.Services
{
    public class ClausesPersonLiableService: BaseService<ClausesPersonLiable>,IClausesPersonLiableService
    {
        private readonly IUserRoleService _userRoleService;

        public ClausesPersonLiableService(IUserRoleService userRoleService, 
            IRepository<ClausesPersonLiable> selfRepository)
            : base(selfRepository)
        {
            this._userRoleService = userRoleService;
        }

        public bool SetPersonLiable(int CompanyID, int AccountID, int ClausesID, out string message)
        {
            var PersonLiable = this.Query.Where(o => o.CustomerCompanyID == CompanyID && o.ClausesID == ClausesID).FirstOrDefault();
            using (var tran = this.BeginTransaction())
            {
                if (PersonLiable == null)
                {
                    var entity = new ClausesPersonLiable
                    {
                        CustomerCompanyID = CompanyID,
                        CustomerAccountID = AccountID,
                        ClausesID = ClausesID,
                        CreateTime = DateTime.Now
                    };
                    try
                    {
                        this.Add(entity);
                        _userRoleService.Update(AccountID, null, Role.Clause);
                        message = "设置成功";
                    }
                    catch (Exception ex)
                    {
                        message = "设置失败";
                        tran.Rollback();
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        _userRoleService.Update(AccountID, PersonLiable.CustomerAccountID, Role.Clause);
                        PersonLiable.CustomerAccountID = AccountID;
                        this.Update(PersonLiable);
                        message = "设置成功";

                    }
                    catch (Exception ex)
                    {
                        message = "设置失败";
                        tran.Rollback();
                        return false;
                    }
                }
                tran.Commit();
                return true;
            }
        }
    }
}
