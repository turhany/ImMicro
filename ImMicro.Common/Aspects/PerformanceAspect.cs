using Castle.DynamicProxy;
using ImMicro.Common.Interceptors;
using ImMicro.Common.IoC;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace ImMicro.Common.Aspects
{
    public class PerformanceAspect : MethodInterception
    {
        private readonly int _interval;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Performance Aspect
        /// </summary>
        /// <param name="interval">Second</param>
        public PerformanceAspect(int interval)
        {
            _interval = interval;
            _stopwatch = GlobalServiceProvider.ServiceProvider.GetService<Stopwatch>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                Debug.WriteLine($"Performance: {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name} --> {_stopwatch.Elapsed.TotalSeconds} seconds");
            }

            _stopwatch.Reset();
        }
    }
}
