using AEOPoco.Domain;
using AEOPoco.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface ICorrectiveTaskService : IBaseService<CorrectiveTask>
    {
        IEnumerable<dynamic> GetDataByAccount(CustomerAccount currAccount);

        bool Add(CustomerAccount currAccount,int clausesID, int auditorID, int chargePersonID, DateTime finishTime, string content, out string message);

        bool Update(CustomerAccount currAccount, int correctiveTaskID, int clausesID, int auditorID, int chargePersonID, DateTime finishTime, string content, out string message);

        bool Delete(CustomerAccount currAccount,List<int> correctiveTaskIds,out string message);

        bool Finish(List<int> ids, CustomerAccount currAccount,out string message);

        bool Audit(List<int> ids, CustomerAccount currAccount, out string message);

        bool Reject(List<int> ids, string reason, CustomerAccount currAccount, out string message);

        bool UploadFile(int correctiveTaskID, CustomerAccount currentAccount, string uploadFileName, string contentType, Action<string, string> fileSave, out string message);

        IQueryable<CorrectiveTaskResult> GetCorrectiveTaskResult(int correctiveTaskID);

        IQueryable<CorrectiveTaskOperationNote> GetOperationNote(int correctiveTaskID);

        bool FileCancel(int correctiveTaskResultID, CustomerAccount account, out string message);

        CorrectiveTaskResult GetUploadFile(int correctiveTaskResultID);

        IEnumerable<TaskSchedule> GetCorrectiveSchedule(CustomerAccount account);
    }
}
