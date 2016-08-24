using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.SimpleInjector
{
    public abstract class SimpleInjectorInterceptor<T> : InterceptorBase where T : StatsDBase
    {
        protected SimpleInjectorInterceptor(IVeStatsDClient client)
            : base(client)
        {
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
            var arguments = invocation.Arguments;

            return GetAttributeInternal<T>(
                invocation.InvocationTarget.GetType(), 
                methodName, 
                arguments);
        }
    }
}
