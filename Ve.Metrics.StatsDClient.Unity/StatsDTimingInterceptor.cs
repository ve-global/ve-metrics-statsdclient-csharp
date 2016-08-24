using System;
using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Unity
{
    public class StatsDTimingInterceptor : UnityInterceptor<StatsDTiming>, IInterceptionBehavior
    {
        public StatsDTimingInterceptor(IVeStatsDClient client)
            : base(client)
        {
        }
        
        protected override IMethodReturn Invoke(Func<IMethodReturn> invocation, StatsDTiming attr)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = invocation();

            stopwatch.Stop();

            Client.LogTiming(attr.Name, stopwatch.ElapsedMilliseconds, attr.Tags);

            return result;
        }
    }
}
