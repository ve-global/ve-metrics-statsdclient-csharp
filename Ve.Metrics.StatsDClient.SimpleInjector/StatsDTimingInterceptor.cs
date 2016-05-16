using System.Diagnostics;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

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

            var name = attr.Name;
            var methodBase = invocation.GetConcreteMethod();
            var method = methodBase.Name;
            var target = invocation.InvocationTarget.GetType().Name;

            name = name.Replace("{type}", target.ToLowerInvariant());
            name = name.Replace("{method}", method.ToLowerInvariant());

            if (methodBase.IsGenericMethod)
            {
                var arguments = methodBase.GetGenericArguments();
                var generic = arguments[0].Name;

                name = name.Replace("{generic}", generic.ToLowerInvariant());
            }

            Client.LogTiming(name, watch.ElapsedMilliseconds, attr.Tags);
        }
    }
}
