using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using System.Data.SqlClient;
using Core.Configuration;


namespace AEOService.Services
{
    public class FeedbackService : BaseService<Feedback>, IFeedbackService
    {
        private readonly MyConfig _myConfig;  
        public FeedbackService(IRepository<Feedback> selfRepository,
            MyConfig myConfig)
            : base(selfRepository)
        {
            this._myConfig = myConfig;
        }


        public bool SaveFeedback(string description, int id, out string message)
        {
            using (var tran = this.BeginTransaction())
            {
                try
                {
                    this.Add(new Feedback
                    {
                        Description = description,
                        CustomerAccountID = id,
                        CreateTime = DateTime.Now
                    });
                    tran.Commit();
                }
                catch (Exception)
                {
                    message = "操作失败，请联系客服";
                    tran.Rollback();
                    return false;
                }
            }
            message = "执行成功";
            return true;
        }
    }
}
