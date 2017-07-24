using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IOutlineclassService : IBaseService<OutlineClass>
    {
        IQueryable<dynamic> OutlineclassSerach(int companyid);

        NPOI.SS.UserModel.IWorkbook GetExportData(int companyid, NPOI.SS.UserModel.IWorkbook book, string[] CellArray, string templetFileName, bool IsSenior);
    }
}