using AEOPoco.Domain;
using AEOPoco.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IScoreTaskService : IBaseService<ScoreTask>
    {
        IEnumerable<dynamic> ItemScoreSearch(CustomerAccount account);

        bool Score(ScoreRequire score, CustomerAccount currentAccount, out string message);

        bool SetScoerPerosn(List<int> itemIDs, int scorePersonID, int CompanyID, out string message);

        string GetScoreLevelDescription(ScoreLevel level);
    }
}
