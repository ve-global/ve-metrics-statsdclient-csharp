using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.CastleWindsor;
using Ve.Metrics.StatsDClient.Tests.TestClasses;

namespace Ve.Metrics.StatsDClient.Tests.CastleWindsor
{
    [TestFixture]
    public class CastleWindsorInterfaceGenericsShould
    {
        protected WindsorContainer Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected ITestGeneric<Foo> FooService;
        protected ITestGeneric<Bar> BarService;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new WindsorContainer();

            Container.Register(Component.For<IVeStatsDClient>().Instance(StatsdMock.Object));
            Container.Register(Component.For<StatsDTimingInterceptor>());
            Container.Register(Component.For<ITestGeneric<Foo>, ITestGeneric<Bar>>()
                .ImplementedBy<TestGenerics>()
                .Interceptors<StatsDTimingInterceptor>());
            
            FooService = Container.Resolve<ITestGeneric<Foo>>();
            BarService = Container.Resolve<ITestGeneric<Bar>>();
        }

        [Test]
        public void It_should_time_the_generic_foo_method()
        {
            FooService.Do(new Foo());
            StatsdMock.Verify(x => x.LogTiming("dependencies.foo.test", It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Test]
        public void It_should_time_the_generic_bar_method()
        {
            BarService.Do(new Bar());
            StatsdMock.Verify(x => x.LogTiming("dependencies.bar.test", It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }
    }
}
