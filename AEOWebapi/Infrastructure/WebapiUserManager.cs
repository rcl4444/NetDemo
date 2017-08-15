using Core.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOWebapi.Controllers.Infrastructure
{
    public class WebapiUserManager : UserManager<WebapiUser, int>
    {
        public WebapiUserManager(IUserStore<WebapiUser, int> store) : base(store){ }

        public static WebapiUserManager Create(IdentityFactoryOptions<WebapiUserManager> options, IOwinContext context)
        {
            var manager = new WebapiUserManager(new UserStore());
            // 配置用户名的验证逻辑
            manager.UserValidator = new UserValidator<WebapiUser,int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            //// 配置密码的验证逻辑
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<WebapiUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            manager.PasswordHasher = new PasswordHasher();
            return manager;
        }
    }

    public class PasswordHasher : IPasswordHasher
    {
        protected IEncryptionService _encryptionService;
        public PasswordHasher()
        {
            _encryptionService = EngineContext.Current.Resolve<IEncryptionService>();
        }

        public string HashPassword(string password)
        {
            return _encryptionService.CreatePasswordDefault(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (this.HashPassword(providedPassword).Equals(hashedPassword))
            {
                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }
    }
}
