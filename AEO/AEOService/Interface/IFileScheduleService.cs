using AEOPoco.Domain;
using AEOPoco.Other;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IFileScheduleService : IBaseService<FileSchedule>
    {
        IEnumerable<dynamic> FileUploadRequireSearch(CustomerAccount account, int? status);

        IEnumerable<TaskSchedule> GetFileTaskStatus(int companyID,CustomerAccount account, bool isManager);

        bool SetChargeAndReviewer(int CompanyID, int? reviewerPersonID, int? chargePersonID, string finishTime, string FileRequireID, out string message);
    }
}
