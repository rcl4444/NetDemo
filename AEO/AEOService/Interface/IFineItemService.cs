using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IFineItemService : IBaseService<FineItem>
    {
        IQueryable GetFineItemList(int itemID, int CompanyID);

    }
}
