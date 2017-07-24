using AEOPoco.Domain;
using Repository.EFRealize.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOPoco.Mapping
{
    public class ClausesPersonLiableMap : MyEntityTypeConfiguration<ClausesPersonLiable>
    {
        public ClausesPersonLiableMap()
        {
            this.ToTable("ClausesPersonLiable");
            this.HasKey(a => a.Id);
            this.HasRequired(bp => bp.Clauses).WithMany().HasForeignKey(bp=>bp.ClausesID);
            this.HasRequired(bp => bp.CustomerAccount).WithMany().HasForeignKey(bp => bp.CustomerAccountID);
            this.HasRequired(bp => bp.CustomerCompany).WithMany().HasForeignKey(bp => bp.CustomerCompanyID).WillCascadeOnDelete(false);
        }
    }
}
