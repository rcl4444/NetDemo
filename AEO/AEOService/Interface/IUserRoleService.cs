using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        void Update(int accountID, int? oldAccountID, Role type);
    }
}
