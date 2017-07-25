using AEOService.Interface;
using AEOService.Services;
using Autofac;
using Core;
using Core.Configuration;
using Core.Infrastructure;
using Core.Infrastructure.DependencyManagement;
using Repository;
using Repository.Interface;
using Service.Interface;
using System.Collections.Generic;
using System.Data.Entity;

namespace AEOWeb.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, MyConfig config)
        {
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<FormsAuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerCompanyService>().As<ICustomerCompanyService>().InstancePerLifetimeScope();
            builder.RegisterType<DeparementService>().As<IDeparementService>().InstancePerLifetimeScope();
            builder.RegisterType<FileRequireService>().As<IFileRequireService>().InstancePerLifetimeScope();
            builder.RegisterType<FileScheduleService>().As<IFileScheduleService>().InstancePerLifetimeScope();
            builder.RegisterType<ScoreTaskService>().As<IScoreTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<ClausesService>().As<IClausesService>().InstancePerLifetimeScope();
            builder.RegisterType<OutlineclassService>().As<IOutlineclassService>().InstancePerLifetimeScope();
            builder.RegisterType<FileResultService>().As<IFileResultService>().InstancePerLifetimeScope();
            builder.RegisterType<FileOperationNoteService>().As<IFileOperationNoteService>().InstancePerLifetimeScope();
            builder.RegisterType<ScoreOperationNoteService>().As<IScoreOperationNoteService>().InstancePerLifetimeScope();
            builder.RegisterType<ClausesPersonLiableService>().As<IClausesPersonLiableService>().InstancePerLifetimeScope();
            builder.RegisterType<ItemService>().As<IItemService>().InstancePerLifetimeScope();
            builder.RegisterType<FineItemService>().As<IFineItemService>().InstancePerLifetimeScope();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<CorrectiveTaskService>().As<ICorrectiveTaskService>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseInitializer>().As<IDataInitializer>().InstancePerLifetimeScope();
            builder.RegisterType<FeedbackService>().As<IFeedbackService>().InstancePerLifetimeScope();
            builder.RegisterType<PreviewTokenService>().As<IPreviewTokenService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }
}
