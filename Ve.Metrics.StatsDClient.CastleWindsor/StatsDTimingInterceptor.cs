using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;

namespace Ve.Metrics.StatsDClient.CastleWindsor
{
    public class StatsDTimingInterceptor : IInterceptor
    {
        private readonly IVeStatsDClient _statsd;

        public StatsDTimingInterceptor(IVeStatsDClient statsd)
        {
            _statsd = statsd;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.GetConcreteMethod().Name;
            var timingAttr =
                invocation.InvocationTarget.GetType()
                    .GetMethod(methodName)
                    .GetCustomAttributes(typeof(StatsDTiming), true)
                    .FirstOrDefault() as StatsDTiming;

            if (timingAttr == null)
            {
                invocation.Proceed();
                return;
            }

            var watch = Stopwatch.StartNew();

            // Calls the decorated instance.
            invocation.Proceed();

            _statsd.LogTiming(timingAttr.Name, watch.ElapsedMilliseconds, timingAttr.Tags);
        }
    }
}
