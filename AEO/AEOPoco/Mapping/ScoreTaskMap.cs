using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class ScoreTaskMap : MyEntityTypeConfiguration<ScoreTask>
    {
        public ScoreTaskMap()
        {
            this.ToTable("ScoreTask");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.ScoreTime);
            this.Property(bp => bp.Score);
            this.HasRequired(bp=>bp.CustomerCompany).WithMany().HasForeignKey(bp=>bp.CustomerCompanyID);
            this.HasRequired(bp => bp.ScorePerson).WithMany().HasForeignKey(bp => bp.ScorePersonID).WillCascadeOnDelete(false);
            this.HasRequired(bp=>bp.Item).WithMany().HasForeignKey(bp=>bp.ItemID);
        }
    }
}