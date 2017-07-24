using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class ItemScoreConfigureMap : MyEntityTypeConfiguration<ItemScoreConfigure>
    {
        public ItemScoreConfigureMap()
        {
            this.ToTable("ItemScoreConfigure");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.ScoreValue);
            this.HasRequired(bp=>bp.Item).WithMany().HasForeignKey(bp=>bp.ItemID).WillCascadeOnDelete(false);
        }
    }
}
