using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class FileRequireMap : MyEntityTypeConfiguration<FileRequire>
    {
        public FileRequireMap()
        {
            this.ToTable("FileRequire");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.Description).HasColumnType("varchar").HasMaxLength(500);
            this.Property(bp => bp.SuggestFileName).HasColumnType("varchar").HasMaxLength(200);
            this.Property(bp => bp.CustomsID);
            this.HasRequired(bp=>bp.FineItem).WithMany().HasForeignKey(bp=>bp.FineItemID);
            this.HasRequired(bp=>bp.CustomerCompany).WithMany().HasForeignKey(bp=>bp.CustomerCompanyID);
            this.HasOptional(bp => bp.FileSchedule).WithRequired(m=>m.FileRequire);////与文件排程表一对一映射
        }
    }
}