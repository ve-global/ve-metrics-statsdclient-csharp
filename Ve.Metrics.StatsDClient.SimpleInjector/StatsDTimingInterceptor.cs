using System.Diagnostics;
using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.SimpleInjector
{
    public class StatsDTimingInterceptor : BaseInterceptor<StatsDTiming>, IInterceptor
    {
        private readonly IVeStatsDClient _statsd;

        public StatsDTimingInterceptor(IVeStatsDClient statsd)
        {
            _statsd = statsd;
        }

        protected override void Invoke(IInvocation invocation, StatsDTiming attr)
        { 
            var watch = Stopwatch.StartNew();

            invocation.Proceed();

            _statsd.LogTiming(attr.Name, watch.ElapsedMilliseconds, attr.Tags);
        }
    }
}
