using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class CorrectiveTaskResultMap : MyEntityTypeConfiguration<CorrectiveTaskResult>
    {
        public CorrectiveTaskResultMap()
        {
            this.ToTable("CorrectiveTaskResult");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.IsCancel);
            this.Property(bp => bp.PhysicalFullPath).HasColumnType("varchar").HasMaxLength(200);
            this.Property(bp => bp.UploadTime);
            this.Property(bp => bp.ContentType);
            this.Property(bp => bp.FileName).HasColumnType("varchar").HasMaxLength(50);
            this.HasRequired(bp => bp.UploadPerson).WithMany().HasForeignKey(bp => bp.UploadPersonID).WillCascadeOnDelete(false);
            this.HasRequired(bp => bp.CorrectiveTask).WithMany().HasForeignKey(bp => bp.CorrectiveTaskID);
        }
    }
}
