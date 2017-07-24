using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class FileScheduleMap : MyEntityTypeConfiguration<FileSchedule>
    {
        public FileScheduleMap()
        {
            this.ToTable("FileSchedule");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp=>bp.FinishTime);
            this.HasRequired(bp=>bp.ChargePerson).WithMany().HasForeignKey(bp=>bp.ChargePersonID);
            this.HasOptional(bp=>bp.Auditor).WithMany().HasForeignKey(bp=>bp.AuditorID);
            this.HasRequired(m => m.FileRequire).WithOptional(n => n.FileSchedule);//与文件要求表一对一映射
        }
    }
}