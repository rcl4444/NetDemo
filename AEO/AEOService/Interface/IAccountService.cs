using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IAccountService: IBaseService<CustomerAccount>
    {
        string GetEncryptStr(string passWord);

        /// <summary>
        /// 公司管理用户登录
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="currentUser">返回登录用户信息</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        bool CustomerCompanyAdminAccountLogin(int companyID,string userName, string passWord,out CustomerAccount currentUser, out string message);

        /// <summary>
        /// 公司普通用户登录
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="currentUser">返回登录用户信息</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        bool CustomerCompanyGeneralAccountLogin(int companyID, string userName, string passWord, out CustomerAccount currentUser, out string message);

        CustomerAccount GetByName(string username);

        CustomerAccount GetByName(int companyID, string username);

        bool DelAccount(int companyID, int userid, out string message);

        bool UpdateAccount(int? DeparementID, string PersonName, string AccountName, string Pwd, int CompanyID, int userid, out string message);

        bool InsertAccount(int? DeparementID, string PersonName, string AccountName, string Pwd, int CompanyID, out string message);

        IQueryable SearchAccout(int CompanyID, string PersonName, string AccountName, int? DeparementID, int id);

        IQueryable GetAccoutList(int companyid);

        IQueryable<int> GetChargeClausesID(CustomerAccount account);

        bool ChangePassWord(CustomerAccount account, string oldpwd, string pwd, string pwds, bool? hasChange, out string message);
    }
}
