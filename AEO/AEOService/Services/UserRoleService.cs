using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using System.Data.SqlClient;

namespace AEOService.Services
{
    public class UserRoleService : BaseService<UserRole>, IUserRoleService
    {
        public UserRoleService(IRepository<UserRole> selfRepository):base(selfRepository)
        {

        }

        public void Update(int accountID, int? oldAccountID, Role type)
        {
            var sqlParams = new List<SqlParameter>();
            string sql = @"
 if  exists (select * from UserRole where RoleID=@type and CustomerAccountID=@accountID)
select null
else
	insert into UserRole(CustomerAccountID,RoleID,CreateTime) 
	select @accountID,@type,SYSDATETIME() ";
            
            if (oldAccountID.HasValue)
            {
                
                sql = "delete UserRole where RoleID = @type and CustomerAccountID = @oldAccountID;" + sql;
                sqlParams.Add(new SqlParameter("@oldAccountID", oldAccountID));
            }

            sqlParams.Add(new SqlParameter("@accountID", accountID));
            sqlParams.Add(new SqlParameter("@type", type));
            this.ExecuteSqlCommand(sql, sqlParams.ToArray());
        }
    }
}
