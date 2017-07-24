using AEOPoco.Domain;
using AEOPoco.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IFileResultService : IBaseService<FileResult>
    {
        /// <summary>
        /// 获取有效的记录
        /// </summary>
        /// <param name="itemid"></param>
        /// <returns></returns>
        IEnumerable<FileRequireResult> GetEffectiveRecords(int itemid, int companyId);

        FileRequireResult GetEffectiveRecord(int fileRequireID, int companyId);

        IEnumerable<FileRequireStatus> GetFileRequireStatus(IEnumerable<int> fileRequireIDs);

        bool SetCancel(int fileResultID, CustomerAccount currentAccount, out string message);

        bool SetApplyAudit(int fileResultID, CustomerAccount currentAccount, out string message);

        bool SetAudit(int fileResultID, CustomerAccount currentAccount, out string message);

        bool SetReject(int fileResultID,string rejectReason, CustomerAccount currentAccount, out string message);

        IEnumerable<FileSituation> GetFileSituation(int companyId);

        Stream PackFile(int companyId,string physicalBasePath);
    }
}
