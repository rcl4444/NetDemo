using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class FeedbackMap : MyEntityTypeConfiguration<Feedback>
    {
        public FeedbackMap()
        {
            this.ToTable("Feedback");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.Description).HasColumnType("varchar").HasMaxLength(500);
            this.HasRequired(bp => bp.CustomerAccount).WithMany().HasForeignKey(bp => bp.CustomerAccountID);
        }
    }
}
