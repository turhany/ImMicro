using System.Reflection;
using Autofac;
using ImMicro.Common.Data.Abstract;
using ImMicro.Data.Repositories.Concrete;
using Module = Autofac.Module;

namespace ImMicro.Container.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            TypeInfo assemblyType = typeof(RequestLogRepository).GetTypeInfo();

            builder.RegisterAssemblyTypes(assemblyType.Assembly)
                .Where(x => typeof(IRepository).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}