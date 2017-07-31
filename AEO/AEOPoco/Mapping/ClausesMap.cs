using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class ClausesMap : MyEntityTypeConfiguration<Clauses>
    {
        public ClausesMap()
        {
            this.ToTable("Clauses");
            this.HasKey(a => a.Id);
            this.Property(bp=>bp.ClausesName).HasColumnType("varchar").HasMaxLength(50);
            this.Property(bp => bp.CustomsID);
            this.HasMany(bp => bp.Items).WithRequired().HasForeignKey(bp => bp.ClausesID);
            this.HasRequired(bp=>bp.OutlineClass).WithMany().HasForeignKey(bp=>bp.OutlineClassID);
        }
    }
}