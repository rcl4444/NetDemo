namespace Repository.EFRealize.Migrations
{
    using Core.Infrastructure;
    using Repository.Interface;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<MyObjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MyObjectContext context)
        {
            EngineContext.Current.Resolve<IDataInitializer>().Seed(context);
        }
    }
}
