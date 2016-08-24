using Moq;
using NUnit.Framework;
using Shouldly;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;
using Ve.Metrics.StatsDClient.Tests.TestClasses;

namespace Ve.Metrics.StatsDClient.Tests
{
    [TestFixture]
    public class InterceptorBaseShould
    {
        private FakeInterceptor<StatsDTiming> _interceptor;
        private Mock<IVeStatsDClient> _statsDClient;

        [SetUp]
        public void Setup()
        {
            _statsDClient = new Mock<IVeStatsDClient>();
            _interceptor = new FakeInterceptor<StatsDTiming>(_statsDClient.Object);
        }

        [Test]
        public void It_should_use_method_params_to_match_overloaded_methods()
        {
            var result = _interceptor.GetAttribute(typeof(OverloadedMethodsClass), "Do", new object[] { new Foo() });
            result.ShouldBeOfType(typeof(StatsDTiming));
        }

        [Test]
        public void It_should_return_null_if_method_is_not_matched()
        {
            var result = _interceptor.GetAttribute(typeof(OverloadedMethodsClass), "Blarg", new object[] { new Foo() });
            result.ShouldBeNull();
        }

        [Test]
        public void It_should_not_match_params_when_there_are_no_overloads()
        {
            var result = _interceptor.GetAttribute(typeof(OverloadedMethodsClass), "NonOverloadedMethod", new object[] { new Bar() });
            result.ShouldBeOfType(typeof(StatsDTiming));
        }
    }


}
