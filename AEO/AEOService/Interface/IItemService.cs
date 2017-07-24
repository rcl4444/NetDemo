using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IItemService : IBaseService<Item>
    {
        IQueryable GetItemList(int CompanyID, int AccountID, bool IsManager);

        int GetItemCount(int CompanyID);

        bool IsItemScore(int CompanyID);
    }
}
