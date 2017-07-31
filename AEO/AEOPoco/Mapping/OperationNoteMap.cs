using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class BaseOperationNoteMap : MyEntityTypeConfiguration<BaseOperationNote>
    {
        public BaseOperationNoteMap()
        {
            this.ToTable("OperationNote");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.Description).HasColumnType("varchar").HasMaxLength(500);
            this.HasRequired(bp => bp.Operationer).WithMany().HasForeignKey(bp => bp.OperationerID);
            this.Map<FileOperationNote>(l =>
            { 
                l.Requires("Type").HasValue((int)OperationNoteType.FileUpload); 
            });
            this.Map<ScoreOperationNote>(l =>
            { 
                l.Requires("Type").HasValue((int)OperationNoteType.ItemScore);
            });
            this.Map<CorrectiveTaskOperationNote>(l =>
            {
                l.Requires("Type").HasValue((int)OperationNoteType.CorrectiveTask);
            });
        }
    }

    public class FileOperationNoteMap : MyEntityTypeConfiguration<FileOperationNote>
    {
        public FileOperationNoteMap()
        {
            this.HasRequired(bp => bp.FileRequire).WithMany().HasForeignKey(bp => bp.FileRequireID).WillCascadeOnDelete(false);
        }
    }

    public class ScoreOperationNoteMap : MyEntityTypeConfiguration<ScoreOperationNote>
    {
        public ScoreOperationNoteMap()
        {
            this.HasRequired(bp => bp.ScoreTask).WithMany().HasForeignKey(bp => bp.ScoreTaskID).WillCascadeOnDelete(false);
            this.Property(bp => bp.Score).HasColumnName("Status");
        }
    }

    public class CorrectiveTaskOperationNoteMap : MyEntityTypeConfiguration<CorrectiveTaskOperationNote>
    {
        public CorrectiveTaskOperationNoteMap()
        {
            this.HasRequired(bp => bp.CorrectiveTask).WithMany().HasForeignKey(bp => bp.CorrectiveTaskID).WillCascadeOnDelete(false);
        }
    }
}