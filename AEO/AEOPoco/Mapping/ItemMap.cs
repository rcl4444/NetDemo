using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class ItemMap : MyEntityTypeConfiguration<Item>
    {
        public ItemMap()
        {
            this.ToTable("Item");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.ItemName);
            this.Property(bp => bp.CustomsID);
            this.Property(bp => bp.IsImportant);
            this.HasRequired(bp=>bp.Clauses).WithMany().HasForeignKey(bp=>bp.ClausesID);
            this.HasMany(bp=>bp.FineItems).WithRequired().HasForeignKey(bp=>bp.ItemID);
            this.HasMany(bp => bp.ScoreConfigure).WithOptional().HasForeignKey(bp => bp.ItemID);
        }
    }
}