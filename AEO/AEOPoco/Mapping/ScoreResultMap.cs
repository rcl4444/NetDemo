using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class ScoreResultMap : MyEntityTypeConfiguration<ScoreResult>
    {
        public ScoreResultMap()
        {
            this.ToTable("ScoreResult");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.Score);
            this.Property(bp => bp.ScoreTime);
            this.HasRequired(bp=>bp.ScoreTask).WithMany().HasForeignKey(bp=>bp.ScoreTaskID);
        }
    }
}