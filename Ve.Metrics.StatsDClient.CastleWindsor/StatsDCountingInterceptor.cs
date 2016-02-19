using Castle.DynamicProxy;
using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.CastleWindsor
{
    public class StatsDCountingInterceptor : BaseInterceptor<StatsDCounting>, IInterceptor
    {
        public StatsDCountingInterceptor(IVeStatsDClient client)
            : base(client)
        {
        }
        
        protected override void Invoke(IInvocation invocation, StatsDCounting attr)
        {
            invocation.Proceed();
            Client.LogCount(attr.Name, attr.Count, attr.Tags);
        }
    }
}
