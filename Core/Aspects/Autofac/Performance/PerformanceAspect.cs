using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        private Stopwatch _stopwatch;

        public PerformanceAspect(int interval)  //interval: [PerformanceAspect(int)] kodundaki int kısmı. Bu süreyi geçerse beni ayıktır diyor. 
            //Interval: müddet
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();
            //stopwatch MemoryManager gibi ya da HttpContextAccessor classları gibi bir şey. 
        }

        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }

        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            //debug.WriteLine : hata ayıklama yerlerine yazılan bir şey. oraya kodun çalışma süresi yazılacak. tabi bunun için ona verilen müddeti geçmesi gerekli.
            _stopwatch.Reset();
        }
    }
}
