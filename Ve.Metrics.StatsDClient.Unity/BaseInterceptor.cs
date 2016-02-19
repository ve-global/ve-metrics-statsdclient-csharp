using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;
using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.Unity
{
    public abstract class BaseInterceptor<T> where T : StatsDBase
    {
        protected readonly IVeStatsDClient Client;

        protected BaseInterceptor(IVeStatsDClient client)
        {
            Client = client;
        }

        public bool WillExecute => true;

        public IMethodReturn Invoke(IMethodInvocation invocation,
            GetNextInterceptionBehaviorDelegate getNext)
        {
            var attr = GetAttribute(invocation);

            if (attr == null)
            {
                return getNext()(invocation, getNext);
            }

            return Invoke(() => getNext()(invocation, getNext), attr);
        }

        protected abstract IMethodReturn Invoke(Func<IMethodReturn> invocation, T attr);

        protected T GetAttribute(IMethodInvocation invocation)
        {
            var methodName = invocation.MethodBase.Name;
            return invocation.Target.GetType()
                    .GetMethod(methodName)
                    .GetCustomAttributes(typeof(T), true)
                    .FirstOrDefault() as T;

        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}
