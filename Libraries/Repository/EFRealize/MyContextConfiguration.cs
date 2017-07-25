using Core.Configuration;
using Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EFRealize
{
    public class MyContextConfiguration : DbConfiguration
    {
        public MyContextConfiguration()
        {
            SetContextFactory(() => new MyDbContextFactory(EngineContext.Current.Resolve<MyConfig>().DataConnectionString).Create());
            SetDefaultConnectionFactory(new MySql.Data.Entity.MySqlConnectionFactory());
            SetProviderFactory("MySql.Data.MySqlClient", new MySql.Data.MySqlClient.MySqlClientFactory());
        }
    }
}
