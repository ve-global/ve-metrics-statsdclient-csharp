using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Unity
{
    public class StatsDCountingInterceptor : BaseInterceptor<StatsDCounting>, IInterceptionBehavior
    {
        public StatsDCountingInterceptor(IVeStatsDClient client)
            : base(client)
        {
        }
        
        protected override IMethodReturn Invoke(Func<IMethodReturn> invocation, StatsDCounting attr)
        {
            var result = invocation();
            Client.LogCount(attr.Name, attr.Count, attr.Tags);

            return result;
        }
    }
}
