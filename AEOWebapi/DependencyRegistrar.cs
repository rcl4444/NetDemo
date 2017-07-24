using Core.Infrastructure.DependencyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Infrastructure;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace AEOWebapi
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(global::Autofac.ContainerBuilder builder, ITypeFinder typeFinder, MyConfig config)
        {
            //apicontrollers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}
