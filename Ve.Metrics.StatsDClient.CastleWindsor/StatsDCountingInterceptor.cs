using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

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

            Client.LogCount(name, attr.Count, attr.Tags);
        }
    }
}
