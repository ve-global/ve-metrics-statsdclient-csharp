using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                var generic = new List<string>();
                var arguments = methodBase.GetGenericArguments();

                while (arguments.Any())
                {
                    var argument = arguments.First();
                    generic.Add(argument.Name.ToLowerInvariant());
                    arguments = argument.GetGenericArguments();
                }

                name = name.Replace("{generic}", string.Join("-", generic));
            }

            Client.LogTiming(name, watch.ElapsedMilliseconds, attr.Tags);
        }
    }
}
