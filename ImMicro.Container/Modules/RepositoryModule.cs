using System.Reflection;
using Autofac;
using ImMicro.Data.BaseRepositories;
using ImMicro.Data.EntityFramework.Repositories;
using ImMicro.Data.Repositories.Abstract;
using Module = Autofac.Module;

namespace ImMicro.Container.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfGenericRepository<>))
                .As(typeof(IGenericRepository<>))
                .InstancePerLifetimeScope(); 
            
            
            TypeInfo assemblyType = typeof(ISampleRepository).GetTypeInfo();
            
            builder.RegisterAssemblyTypes(assemblyType.Assembly)
                .Where(x => typeof(IRepository).IsAssignableFrom(x))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}