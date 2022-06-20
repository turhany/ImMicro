using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using ImMicro.Business.RequestLog.Concrete;  
using ImMicro.Common.Interceptors;
using Module = Autofac.Module;

namespace ImMicro.Container.Modules
{
    public class AspectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            TypeInfo assemblyType = typeof(RequestLogService).GetTypeInfo();

            builder.RegisterAssemblyTypes(assemblyType.Assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance().InstancePerDependency();

            base.Load(builder);
        }
    }
}