using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class CustomerAccountMap : MyEntityTypeConfiguration<CustomerAccount>
    {
        public CustomerAccountMap()
        {
            this.ToTable("CustomerAccount");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.AccountName).HasColumnType("varchar").HasMaxLength(50);
            this.Property(bp => bp.PassWord).HasColumnType("varchar").HasMaxLength(50);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.HasChange);
            this.HasMany(bp=>bp.UserRoles).WithRequired().HasForeignKey(o=>o.CustomerAccountID);
            this.HasOptional(bp => bp.CustomerDeparement).WithMany().HasForeignKey(bp => bp.CustomerDeparementID);
            this.HasRequired(bp => bp.CustomerCompany).WithMany().HasForeignKey(bp => bp.CustomerCompanyID);
        }
    }
}