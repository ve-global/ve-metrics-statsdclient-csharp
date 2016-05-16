using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.CastleWindsor
{
    public abstract class BaseInterceptor<T> where T: StatsDBase
    {
        protected IVeStatsDClient Client { get; set; }

        protected BaseInterceptor(IVeStatsDClient client)
        {
            Client = client;
        }
        
        public void Intercept(IInvocation invocation)
        {
            var attr = GetAttribute(invocation);

            if (attr == null)
            {
                invocation.Proceed();
                return;
            }

            Invoke(invocation, attr);
        }

        protected abstract void Invoke(IInvocation invocation, T attr);

        protected T GetAttribute(IInvocation invocation)
        {
            var methodName = invocation.GetConcreteMethod().Name;
            return invocation.InvocationTarget.GetType()
                    .GetMethod(methodName)
                    .GetCustomAttributes(typeof(T), true)
                    .FirstOrDefault() as T;
        }

        protected static string GetGenericName(string name, System.Reflection.MethodBase methodBase)
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
            return name;
        }
    }
}
