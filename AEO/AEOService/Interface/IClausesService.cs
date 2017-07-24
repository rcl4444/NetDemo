using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using AEOPoco.Domain;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using AEOPoco.Other;

namespace AEOService.Interface
{
    public interface IClausesService:IBaseService<Clauses>
    {
        IQueryable ClausesSearch(int CompanyID);

        IQueryable GetTaskList(int CompanyID, int ClausesID);

        IEnumerable<TaskItemConut> GetTaskConut(int CompanyID, int AccountID, bool IsManger);
    }
}
