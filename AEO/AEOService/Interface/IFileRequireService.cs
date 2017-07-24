using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using AEOPoco.Domain;
using System.Linq.Expressions;
using AEOPoco.Other;

namespace AEOService.Interface
{
    public interface IFileRequireService : IBaseService<FileRequire>
    {
        IEnumerable<FileAuditResult> FileAuditSearch(int companyID, int reviewerID,bool isManager);

        bool DeleteFileRequire(int companyID, int FileRequireID, out string message);

        bool AddFileRequire(int companyID, int FindItemID, string SuggestFileName, string Description, out string message);

        bool UpdateFileRequire(int companyID, int FileRequireID, string SuggestFileName, string Description, out string message);

        bool SortFileRequire(int compayID, int oldFileRequireID, int newFileRequire, out string message);

        bool UploadFile(int fileRequireID, CustomerAccount currentAccount, string uploadFileName, string contentType, Func<string, string, IFileManager.FileResult> fileSave, out string message);
    }
}
