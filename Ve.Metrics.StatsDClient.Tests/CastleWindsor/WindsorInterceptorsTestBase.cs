using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;
using Ve.Metrics.StatsDClient.CastleWindsor;

namespace Ve.Metrics.StatsDClient.Tests.CastleWindsor
{
    public class WindsorInterceptorsTestBase<T1 ,T2> where T1: BaseInterceptor<T2>, IInterceptor where T2: StatsDBase
    {
        protected WindsorContainer Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected IFooService Service;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new WindsorContainer();

            Container.Register(Component.For<IVeStatsDClient>().Instance(StatsdMock.Object));
            Container.Register(Component.For<T1>());
            Container.Register(Component.For<IFooService>().ImplementedBy<FooService>().Interceptors<T1>());

            Service = Container.Resolve<IFooService>();
        }
    }
}