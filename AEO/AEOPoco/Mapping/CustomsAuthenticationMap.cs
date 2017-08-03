using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Domain
{
	public class CustomsAuthenticationMap : MyEntityTypeConfiguration<CustomsAuthentication>
	{
        public CustomsAuthenticationMap()
        {
            this.ToTable("CustomsAuthentication");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.TitleName).HasColumnType("varchar").HasMaxLength(500);
            this.Property(bp => bp.CustomsVersion).HasColumnType("varchar").HasMaxLength(20);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.CustomsID);
            this.HasMany(bp => bp.OutlineClasses).WithRequired().HasForeignKey(bp=>bp.CustomsAuthenticationID);
        }
	}
}

