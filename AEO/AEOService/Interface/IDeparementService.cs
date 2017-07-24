using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IDeparementService : IBaseService<CustomerDeparement>
    {
        bool AddDeparement(int companyid, string deparementname, string description, out string message);

        bool DelDeparement(int companyid, int deparementid, out string message);

        bool UpdateDeparement(int companyid, int deparementid, string deparementname, string description, out string message);

        IQueryable GetDeparementList(int companyid);
    }
}
