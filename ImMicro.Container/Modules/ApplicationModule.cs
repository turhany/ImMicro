using Autofac;
using Exporty.Abstract;
using ImMicro.Cache.Abstract;
using ImMicro.Cache.Redis.Concrete;
using ImMicro.Lock.Abstract;
using ImMicro.Lock.Redis.Concrete;
using ImMicro.Validation.Abstract;
using ImMicro.Validation.FluentValidation.Concrete;

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