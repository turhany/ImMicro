using Autofac;
using Exporty.Abstract;
using ImMicro.Common.Cache.Abstract;
using ImMicro.Common.Cache.Concrete;
using ImMicro.Common.Lock.Abstract;
using ImMicro.Common.Lock.Concrete;
using ImMicro.Common.Validation.Abstract;
using ImMicro.Common.Validation.Concrete;

namespace ImMicro.Container.Modules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RedisCacheService>().As<ICacheService>().InstancePerLifetimeScope();
            builder.RegisterType<RedisLockService>().As<ILockService>().InstancePerLifetimeScope(); 
            builder.RegisterType<ValidationService>().As<IValidationService>().InstancePerLifetimeScope(); 
            builder.RegisterType<Exporty.Concrete.Exporty>().As<IExporty>().InstancePerLifetimeScope();
            
            base.Load(builder);
        }
    }
}