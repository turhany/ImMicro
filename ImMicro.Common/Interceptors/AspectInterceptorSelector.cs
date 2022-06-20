using Castle.DynamicProxy;
using System;
using System.Linq;
using System.Reflection;

namespace ImMicro.Common.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes = type.GetMethods()?.Where(p => p.Name == method.Name).FirstOrDefault().GetCustomAttributes<MethodInterceptionBaseAttribute>(true);

            if (methodAttributes != null)
            {
                classAttributes.AddRange(methodAttributes);
            }

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}