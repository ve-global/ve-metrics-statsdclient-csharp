using System.Linq;
using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.SimpleInjector
{
    public abstract class BaseInterceptor<T> where T : StatsDBase
    {
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
    }
}
