using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SimpleInjector;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.SimpleInjector;
using Ve.Metrics.StatsDClient.Tests.TestClasses;

namespace Ve.Metrics.StatsDClient.Tests.SimpleInjector
{
    [TestFixture]
    public class SimpleInjectorInterfaceGenericsShould
    {
        protected Container Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected ITestGeneric<Foo> FooService;
        protected ITestGeneric<Bar> BarService;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new Container();

            Container.RegisterSingleton<IVeStatsDClient>(StatsdMock.Object);
            Container.RegisterSingleton<ITestGeneric<Foo>, TestGenerics>();
            Container.RegisterSingleton<ITestGeneric<Bar>, TestGenerics>();
            Container.InterceptWith<StatsDTimingInterceptor>(type => type == typeof(ITestGeneric<Foo>));
            Container.InterceptWith<StatsDTimingInterceptor>(type => type == typeof(ITestGeneric<Bar>));

            FooService = Container.GetInstance<ITestGeneric<Foo>>();
            BarService = Container.GetInstance<ITestGeneric<Bar>>();

            StatsdMock.Setup(x => x.LogTiming(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()));
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
