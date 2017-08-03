using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class FineItemMap : MyEntityTypeConfiguration<FineItem>
    {
        public FineItemMap()
        {
            this.ToTable("FineItem");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp=>bp.FineItemName).HasColumnType("varchar").HasMaxLength(1000);
            this.Property(bp => bp.CustomsID);
            this.HasRequired(bp=>bp.Item).WithMany().HasForeignKey(bp=>bp.ItemID);
            this.HasMany(bp => bp.FileRequires).WithOptional().HasForeignKey(bp => bp.FineItemID);
        }
    }
}