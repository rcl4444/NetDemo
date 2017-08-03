using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class PreviewTokenMap : MyEntityTypeConfiguration<PreviewToken>
    {
        public PreviewTokenMap()
        {
            this.ToTable("PreviewToken");
            this.HasKey(a => a.Id);
            this.Property(bp => bp.CreateTime);
            this.Property(bp => bp.Path).HasColumnType("varchar").HasMaxLength(200);
            this.Property(bp => bp.Token);
            this.Property(bp => bp.ContentType);
        }
    }
}
