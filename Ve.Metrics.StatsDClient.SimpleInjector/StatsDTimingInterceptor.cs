using System.Diagnostics;
using System.Linq;

namespace Ve.Metrics.StatsDClient.SimpleInjector
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
            var decoratedType = invocation.InvocationTarget.GetType();
            var timingAttr =
                decoratedType.GetCustomAttributes(typeof(StatsDTiming), true).FirstOrDefault() as StatsDTiming;

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
