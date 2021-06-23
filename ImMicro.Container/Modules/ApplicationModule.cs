using Autofac;
using ImMicro.Common.Cache.Abstract;
using ImMicro.Common.Cache.Concrete;
using ImMicro.Common.Lock.Abstract;
using ImMicro.Common.Lock.Concrete;

namespace ImMicro.Container.Modules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RedisCacheService>().As<ICacheService>().InstancePerLifetimeScope();
            builder.RegisterType<RedisLockService>().As<ILockService>().InstancePerLifetimeScope(); 
            
            base.Load(builder);
        }
    }
}