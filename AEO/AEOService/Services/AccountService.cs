using AEOPoco.Domain;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEOService.Interface;
using Service.Interface;
using System.Linq.Expressions;
using Core;
using System.Data.SqlClient;

namespace AEOService.Services
{
    public class AccountService : BaseService<CustomerAccount>, IAccountService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<ClausesPersonLiable> _clausesPersonLiableRepository;
  
        public AccountService(IRepository<CustomerAccount> customerAccountRepository,
            IRepository<ClausesPersonLiable> clausesPersonLiableRepository,
            IEncryptionService encryptionService):base(customerAccountRepository)
        {
            this._encryptionService = encryptionService;
            this._clausesPersonLiableRepository = clausesPersonLiableRepository;
        }

        public bool CustomerCompanyAdminAccountLogin(int companyID, string userName, string passWord, out CustomerAccount currentUser, out string message)
        {
            CustomerAccount accountUser = this.GetByName(companyID, userName);
            currentUser = null;
            if (accountUser == null)
            {
                message = "用户信息不存在";
                return false;
            }
            else if (!accountUser.PassWord.Equals(GetEncryptStr(passWord)))
            {
                message = "密码错误";
                return false;
            }
            else if (!isManager(accountUser))
            {
                message = "不是管理员，请联系你的上级";
                var PersonLiable = _clausesPersonLiableRepository.TableNoTracking.Where(o => o.CustomerAccountID == accountUser.Id && o.CustomerCompanyID == companyID).FirstOrDefault();
                if (PersonLiable == null)
                {
                    message = "不是条管理员，请联系你的上级";
                    return false;
                }
                else
                {
                    currentUser = accountUser;
                    message = "登陆成功";
                    return true;
                }
            }
            else
            {
                currentUser = accountUser;
                message = "登录成功";
                return true;
            }
        }

        public bool CustomerCompanyGeneralAccountLogin(int companyID, string userName, string passWord, out CustomerAccount currentUser, out string message)
        {
            CustomerAccount accountUser = this.GetByName(companyID, userName);
            currentUser = null;
            if (accountUser == null)
            {
                message = "用户信息不存在";
                return false;
            }
            else if (!accountUser.PassWord.Equals(GetEncryptStr(passWord)))
            {
                message = "密码错误";
                return false;
            }
            else
            {
                currentUser = accountUser;
                message = "登录成功";
                return true;
            }
        }

        public CustomerAccount GetByName(int companyID, string username)
        {
            return this.Query.Where(o => o.CustomerCompanyID == companyID && o.AccountName == username).FirstOrDefault();
        }

        public string GetEncryptStr(string passWord)
        {
            return _encryptionService.CreatePasswordDefault(passWord);
        }

        public bool isManager(CustomerAccount currentUser)
        {
            if (currentUser.IsManager)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DelAccount(int companyID, int userid, out string message)
        {
            try
            {
                var i = this.ExecuteSqlCommand(@"    
    delete from OperationNote where OperationerID = @id;
    delete from OperationNote where ScoreTaskID in (select id from ScoreTask where ScorePersonID =@id);
    delete from ScoreResult where ScoreTaskID in (select id from ScoreTask where ScorePersonID =@id);
    delete from ScoreTask where id in (select id from ScoreTask where ScorePersonID =@id);
    delete from FileSchedule where ChargePersonID = @id or ReviewerID = @id;
    delete from FileResult where UploadPersonID = @id ;
    delete from CustomerAccount where id = @id;
    delete from UserRole where CustomerAccountID=@id", new SqlParameter { ParameterName = "@id", Value = userid });
                message = "删除成功";
                return true;
            }
            catch (Exception)
            {
                message = "删除失败";
                return false;
                
            }
        }

        public IQueryable SearchAccout(int CompanyID, string PersonName, string AccountName, int? DeparementID, int id)
        {
            Expression<Func<CustomerAccount, bool>> filter = (o => o.CustomerCompanyID.Equals(CompanyID) && o.IsManager.Equals(false) && o.Id != id);
            if (!string.IsNullOrEmpty(PersonName))
            {
                Expression<Func<CustomerAccount, bool>> childWhere = (o => o.PersonName.Equals(PersonName));
                filter = filter.AndAlso(childWhere);
            }
            if (!string.IsNullOrEmpty(AccountName))
            {
                Expression<Func<CustomerAccount, bool>> childWhere = (o => o.AccountName.Equals(AccountName));
                filter = filter.AndAlso(childWhere);
            }
            if (DeparementID.HasValue)
            {
                Expression<Func<CustomerAccount, bool>> childWhere = (o => o.CustomerDeparementID == DeparementID );
                filter = filter.AndAlso(childWhere);
            }

            var query = (from t in this.NoTrackingQuery.Where(filter)
                     select new
                     {
                         t.Id,
                         t.PersonName,
                         t.AccountName,
                         CustomerDeparementID = (int?)t.CustomerDeparement.Id,
                         t.CustomerDeparement.DeparementName
                     });
            return query;
        }

        public IQueryable GetAccoutList(int companyid)
        {
            var list = (from t in this.NoTrackingQuery.Where(o => o.CustomerCompanyID.Equals(companyid))
                        select new
                        {
                            t.Id,
                            t.PersonName,
                            t.AccountName,
                            CustomerDeparementID = (int?)t.CustomerDeparement.Id,
                            t.CustomerDeparement.DeparementName
                        });
            return list;
        }

        public bool InsertAccount(int? DeparementID, string PersonName, string AccountName, string Pwd,int CompanyID, out string message)
        {
            if (string.IsNullOrEmpty(PersonName))
            {
                message = "真实姓名不能为空";
                return false;
            }
            var u = this.Query.Where(o => o.AccountName == AccountName && o.CustomerCompanyID == CompanyID).FirstOrDefault();
            if (u != null)
            {
                message = "用户名重复，请重新输入";
                return false;
            }
            if (string.IsNullOrEmpty(AccountName))
            {
                message = "用户名不能为空";
                return false;
            }
            if (string.IsNullOrEmpty(Pwd))
            {
                message = "密码不能为空";
                return false;
            }
            try
            {
                var user = new CustomerAccount
                {
                    CustomerCompanyID = CompanyID,
                    CustomerDeparementID = DeparementID,
                    PersonName = PersonName,
                    AccountName = AccountName,
                    PassWord = _encryptionService.CreatePasswordDefault(Pwd),
                    CreateTime = DateTime.Now
                };
                this.Add(user);
                message = "新增成功";
                return true;
            }
            catch (Exception ex)
            {
                message = "新增失败";
                return false;
            }
        }

        public bool UpdateAccount(int? DeparementID, string PersonName, string AccountName,string Pwd ,int companyID, int userid, out string message)
        {
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    var u = this.Query.Where(o =>o.Id!= userid && o.AccountName == AccountName && o.CustomerCompanyID == companyID).FirstOrDefault();
                    if (u != null)
                    {
                        message = "用户名重复，请重新输入";
                        return false;
                    }
                    u = this.Query.Where(o => o.CustomerCompanyID.Equals(companyID) && o.Id.Equals(userid)).FirstOrDefault();
                    u.CustomerDeparementID = DeparementID;
                    u.PersonName = PersonName;
                    u.AccountName = AccountName;
                    string pwdStr = this._encryptionService.CreatePasswordDefault(Pwd);
                    if (!u.PassWord.Equals(pwdStr))
                    {
                        u.PassWord = pwdStr;
                        u.HasChange = false;
                    }
                    this.Update(u);
                    message = "更新成功";
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    message = "更新失败";
                    return false;
                }
            }
        }

        public IQueryable<int> GetChargeClausesID(CustomerAccount account)
        {
            return (from cp in this._clausesPersonLiableRepository.TableNoTracking
                    where cp.CustomerAccountID == account.Id && cp.CustomerCompanyID == account.CustomerCompanyID
                    select cp.ClausesID);
        }

        public bool ChangePassWord(CustomerAccount account, string oldpwd, string pwd, string pwds,bool? hasChange, out string message)
        {
            if (!pwd.Equals(pwds))
            {
                message = "前后密码不一致";
                return false;
            }
            string pwdStr = GetEncryptStr(oldpwd);
            if (!account.PassWord.Equals(pwdStr))
            {
                message = "原密码错误";
                return false;
            }
            account.PassWord = GetEncryptStr(pwd);
            if (hasChange.HasValue)
            {
                account.HasChange = hasChange.Value;
            }
            this.Update(account);
            message = "";
            return true;
        }
    }
}
