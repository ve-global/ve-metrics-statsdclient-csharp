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
    public class SimpleInjectorOverloadedMethodShould
    {
        protected Container Container;
        protected Mock<IVeStatsDClient> StatsdMock;
        protected IOverloadedMethodsClass Service;

        [SetUp]
        protected void Setup()
        {
            StatsdMock = new Mock<IVeStatsDClient>();
            Container = new Container();

            Container.RegisterSingleton<IVeStatsDClient>(StatsdMock.Object);
            Container.RegisterSingleton<IOverloadedMethodsClass, OverloadedMethodsClass>();
            Container.InterceptWith<StatsDTimingInterceptor>(type => type == typeof(IOverloadedMethodsClass));

            Service = Container.GetInstance<IOverloadedMethodsClass>();

            StatsdMock.Setup(x => x.LogTiming(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()));
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