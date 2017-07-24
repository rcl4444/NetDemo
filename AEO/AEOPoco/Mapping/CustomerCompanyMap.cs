using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class CustomerCompanyMap : MyEntityTypeConfiguration<CustomerCompany>
    {
        public CustomerCompanyMap()
        {
            this.ToTable("CustomerCompany");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp=>bp.CompanyName);
            this.Property(bp=>bp.UniqueFlag);
            this.Property(bp => bp.ExportSetting);
            this.HasOptional(bp=>bp.CustomsAuthentication).WithMany().HasForeignKey(bp=>bp.CustomsAuthenticationID);
            this.HasMany(bp=>bp.CustomerDeparements).WithRequired().HasForeignKey(bp=>bp.CustomerCompanyID);
            this.HasMany(bp => bp.CustomerAccounts).WithRequired().HasForeignKey(bp => bp.CustomerCompanyID);
        }
    }
}