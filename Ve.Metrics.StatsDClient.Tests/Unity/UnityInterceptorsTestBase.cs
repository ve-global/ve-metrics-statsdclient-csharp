using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;
using Ve.Metrics.StatsDClient.Tests.TestClasses;
using Ve.Metrics.StatsDClient.Unity;

namespace Ve.Metrics.StatsDClient.Tests.Unity
{
    public class UnityInterceptorsTestBase<T1 ,T2> where T1: UnityInterceptor<T2>, IInterceptionBehavior where T2: StatsDBase
    {
        protected IUnityContainer Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected IFooService Service;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new UnityContainer();

            Container.RegisterInstance(StatsdMock.Object);
            Container.AddNewExtension<Interception>();
            Container.RegisterType<IFooService, FooService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<T1>()
                );

            Service = Container.Resolve<IFooService>();
        }
    }
}