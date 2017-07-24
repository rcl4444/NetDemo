using Core.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EFRealize
{
    public class MyDbContextFactory : IDbContextFactory<MyObjectContext>
    {
        private readonly string _dataConnectionString;

        public MyDbContextFactory(string dataConnectionString)
        {
            this._dataConnectionString = dataConnectionString;
        }

        public MyObjectContext Create()
        {
            return new MyObjectContext(this._dataConnectionString);
        }
    }
}
