using System.Diagnostics;
using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.SimpleInjector
{
    public class StatsDTimingInterceptor : BaseInterceptor<StatsDTiming>, IInterceptor
    {
        public StatsDTimingInterceptor(IVeStatsDClient client)
            : base(client)
        {
        }

        protected override void Invoke(IInvocation invocation, StatsDTiming attr)
        { 
            var watch = Stopwatch.StartNew();

            invocation.Proceed();

            Client.LogTiming(attr.Name, watch.ElapsedMilliseconds, attr.Tags);
        }
    }
}
