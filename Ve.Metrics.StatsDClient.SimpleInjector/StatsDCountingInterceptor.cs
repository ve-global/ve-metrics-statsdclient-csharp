using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.SimpleInjector
{
    public class StatsDCountingInterceptor : BaseInterceptor<StatsDCounting>, IInterceptor
    {
        private readonly IVeStatsDClient _statsd;

        public StatsDCountingInterceptor(IVeStatsDClient statsd)
        {
            _statsd = statsd;
        }
        
        protected override void Invoke(IInvocation invocation, StatsDCounting attr)
        {
            invocation.Proceed();
            _statsd.LogCount(attr.Name, attr.Count, attr.Tags);
        }
    }
}
