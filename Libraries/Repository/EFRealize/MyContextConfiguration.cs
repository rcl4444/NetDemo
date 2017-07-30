using Core.Configuration;
using Core.Infrastructure;
using MySql.Data.Entity;
using Repository.EFRealize.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EFRealize
{
    public class MyContextConfiguration : MySqlEFConfiguration
    {
        public MyContextConfiguration():base()
        {
            SetDatabaseInitializer(new MigrateDatabaseToLatestVersion<MyObjectContext, Configuration>());
        }
    }
}
