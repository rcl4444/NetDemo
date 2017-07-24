using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class UserRoleMap : MyEntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            this.ToTable("UserRole");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.RoleID);
            this.HasRequired(bp=>bp.CustomerAccount).WithMany().HasForeignKey(bp=>bp.CustomerAccountID);
        }
    }
}
