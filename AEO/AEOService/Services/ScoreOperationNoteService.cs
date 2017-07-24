using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;

namespace AEOService.Services
{
    public class ScoreOperationNoteService : BaseService<ScoreOperationNote>, IScoreOperationNoteService
    {
        public ScoreOperationNoteService(IRepository<ScoreOperationNote> selfNoteRepository) : base(selfNoteRepository)
        {

        }
    }
}
