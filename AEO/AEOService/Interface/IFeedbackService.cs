using AEOPoco.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEOService.Interface
{
    public interface IFeedbackService : IBaseService<Feedback>
    {
        bool SaveFeedback(string description, int id, out string message);
    }
}
