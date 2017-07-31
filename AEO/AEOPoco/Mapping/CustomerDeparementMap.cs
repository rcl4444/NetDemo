using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class CustomerDeparementMap : MyEntityTypeConfiguration<CustomerDeparement>
    {
        public CustomerDeparementMap()
        {
            this.ToTable("CustomerDeparement");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.DeparementName).HasColumnType("varchar").HasMaxLength(50);
            this.Property(bp => bp.Description).HasColumnType("varchar").HasMaxLength(500);
            this.HasRequired(bp=>bp.CustomerCompany).WithMany().HasForeignKey(bp=>bp.CustomerCompanyID);
            this.HasMany(bp => bp.CustomerAccounts).WithOptional().HasForeignKey(bp => bp.CustomerDeparementID);
        }
    }
}