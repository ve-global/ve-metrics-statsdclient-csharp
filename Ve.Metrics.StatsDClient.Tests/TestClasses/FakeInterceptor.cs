using System;
using System.Reflection;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Tests.TestClasses
{
    public class FakeInterceptor<T> : InterceptorBase where T : StatsDBase
    {
        public FakeInterceptor(IVeStatsDClient client) : base(client)
        {
        }

        public T GetAttribute(Type type, string methodName, object[] args)
        {
            return GetAttributeInternal<T>(
                type,
                methodName,
                args);
        }
    }
}
