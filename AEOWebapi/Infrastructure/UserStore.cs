using AEOPoco.Domain;
using AEOService.Interface;
using Core.Infrastructure;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AEOWebapi.Controllers.Infrastructure
{
    public class UserStore : IUserStore<WebapiUser, int>, 
        IUserPasswordStore<WebapiUser, int>
    {
        protected IAccountService _AccountService { get; set; }

        public UserStore()
        {
            this._AccountService = EngineContext.Current.Resolve<IAccountService>();
        }

        public Task CreateAsync(WebapiUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(WebapiUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public Task<WebapiUser> FindByIdAsync(int userId)
        {
            var account = this._AccountService.GetByID(userId);
            if (account == null)
            {
                return Task.FromResult<WebapiUser>(null);
            }
            return Task.FromResult(new WebapiUser() {
                Id = account.Id,
                UserName = account.AccountName
            });
        }

        public Task<WebapiUser> FindByNameAsync(string userName)
        {
            var account = this._AccountService.GetByName(userName);
            if (account == null)
            {
                return Task.FromResult<WebapiUser>(null);
            }
            return Task.FromResult(new WebapiUser()
            {
                Id = account.Id,
                UserName = account.AccountName
            });
        }

        public Task UpdateAsync(WebapiUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(WebapiUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(WebapiUser user)
        {
            var account = this._AccountService.GetByName(user.UserName);
            if (account == null)
            {
                return Task.FromResult(string.Empty);
            }
            return Task.FromResult(account.PassWord);
        }

        public Task<bool> HasPasswordAsync(WebapiUser user)
        {
            return Task.FromResult(true);
        }
    }
}
