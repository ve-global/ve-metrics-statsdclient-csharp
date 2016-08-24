using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Unity
{
    public abstract class UnityInterceptor<T> : InterceptorBase where T : StatsDBase
    {
        protected UnityInterceptor(IVeStatsDClient client)
            : base(client)
        {
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
            var arguments = invocation.Arguments.Cast<object>().ToArray();
            
            return GetAttributeInternal<T>(invocation.Target.GetType(),
                methodName,
                arguments);
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
    }
}
