using Core.Infrastructure;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EFRealize.Initializers
{
    public class CreateDatabase : CreateDatabaseIfNotExists<MyObjectContext>
    {
        protected override void Seed(MyObjectContext context)
        {
            EngineContext.Current.Resolve<IDataInitializer>().Seed(context);
        }
    }
}
