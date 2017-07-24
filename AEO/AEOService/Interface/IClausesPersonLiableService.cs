using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IClausesPersonLiableService : IBaseService<ClausesPersonLiable>
    {
        bool SetPersonLiable(int CompanyID, int AccountID, int ClausesID, out string message);
    }
}
