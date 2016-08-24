using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Tests.TestClasses;

namespace Ve.Metrics.StatsDClient.Tests.Unity
{
    [TestFixture]
    public class UnityOverloadedMethodShould
    {
        protected IUnityContainer Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected IOverloadedMethodsClass Service;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new UnityContainer();

            Container.RegisterInstance(StatsdMock.Object);
            Container.AddNewExtension<Interception>();
            Container.RegisterType<IOverloadedMethodsClass, OverloadedMethodsClass>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<StatsDClient.Unity.StatsDTimingInterceptor>()
                );
            
            Service = Container.Resolve<IOverloadedMethodsClass>();
        }

        [Test]
        public void It_should_time_the_generic_foo_method()
        {
            Service.Do(new Foo());
            StatsdMock.Verify(x => x.LogTiming("dependencies.foo.test", It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Test]
        public void It_should_time_the_generic_bar_method()
        {
            Service.Do(new Bar());
            StatsdMock.Verify(x => x.LogTiming("dependencies.bar.test", It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }
    }
}
