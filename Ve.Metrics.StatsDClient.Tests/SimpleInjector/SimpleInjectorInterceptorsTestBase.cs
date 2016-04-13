using Moq;
using NUnit.Framework;
using SimpleInjector;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;
using Ve.Metrics.StatsDClient.SimpleInjector;

namespace Ve.Metrics.StatsDClient.Tests.SimpleInjector
{
    public class SimpleInjectorInterceptorsTestBase<T1 ,T2> where T1: BaseInterceptor<T2>, IInterceptor where T2: StatsDBase
    {
        protected Container Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected IFooService Service;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new Container();

            Container.RegisterSingleton<IVeStatsDClient>(StatsdMock.Object);
            Container.RegisterSingleton<IFooService, FooService>();
            Container.InterceptWith<T1>(type => type == typeof(IFooService));

            Service = Container.GetInstance<IFooService>();
        }
    }
}