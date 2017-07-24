using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class CorrectiveTaskMap : MyEntityTypeConfiguration<CorrectiveTask>
    {
        public CorrectiveTaskMap()
        {
            this.ToTable("CorrectiveTask");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp=>bp.FinishTime);
            this.Property(bp=>bp.Status);
            this.Property(bp=>bp.CorrectiveContent);
            this.Property(bp => bp.IsDelete);
            this.Property(bp => bp.CompleteTime);
            this.HasRequired(bp => bp.CustomerCompany).WithMany().HasForeignKey(bp => bp.CompanyID);
            this.HasRequired(bp=>bp.CorrectiveObject).WithMany().HasForeignKey(bp=>bp.CorrectiveObjectID);
            this.HasRequired(bp => bp.ChargePerson).WithMany().HasForeignKey(bp => bp.ChargePersonID).WillCascadeOnDelete(false);
            this.HasRequired(bp => bp.Auditor).WithMany().HasForeignKey(bp => bp.AuditorID).WillCascadeOnDelete(false);
            this.HasRequired(bp => bp.CreatePerson).WithMany().HasForeignKey(bp => bp.CreaterID).WillCascadeOnDelete(false);
            this.HasMany(bp => bp.CorrectiveTaskResults).WithRequired().HasForeignKey(bp => bp.CorrectiveTaskID);
        }
    }
}
