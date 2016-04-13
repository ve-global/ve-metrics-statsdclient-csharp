using System.Diagnostics;
using Castle.DynamicProxy;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.CastleWindsor
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
