using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class OutlineClassMap : MyEntityTypeConfiguration<OutlineClass>
    {
        public OutlineClassMap()
        {
            this.ToTable("OutlineClass");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.OutlineClassName).HasColumnType("varchar").HasMaxLength(500);
            this.Property(bp => bp.CustomsID);
            this.HasRequired(bp=>bp.CustomsAuthentication).WithMany().HasForeignKey(bp=>bp.CustomsAuthenticationID);
            this.HasMany(bp => bp.Clauseses).WithRequired().HasForeignKey(bp=>bp.OutlineClassID);
        }
    }
}