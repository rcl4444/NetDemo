using AEOPoco.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AEOWebapi.Controllers.Infrastructure
{
    public class WebapiUser : CustomerAccount, IUser<int>
    {
        public string UserName
        {
            get
            {
                return this.AccountName;
            }
            set
            {
                this.AccountName = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<WebapiUser, int> manager, string authenticationType)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }
}
