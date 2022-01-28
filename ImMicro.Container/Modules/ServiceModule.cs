using System.Reflection;
using Autofac;
using ImMicro.Business.RequestLog.Concrete; 
using ImMicro.Common.Data.Abstract;
using Module = Autofac.Module;

namespace ImMicro.Container.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            TypeInfo assemblyType = typeof(RequestLogService).GetTypeInfo();

            builder.RegisterAssemblyTypes(assemblyType.Assembly)
                .Where(x => typeof(IService).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}