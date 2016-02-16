using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Attributes;

namespace Ve.Metrics.StatsDClient.Tests.CastleWindsor
{
    [TestFixture]
    public class StatsDCountingInterceptorShould : WindsorInterceptorsTestBase<StatsDClient.CastleWindsor.StatsDCountingInterceptor, StatsDCounting>
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            StatsdMock.Setup(x => x.LogCount(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));
        }

        [Test]
        public void It_should_count_the_targeted_method_and_log_to_statsd()
        {
            Service.TrackedMethod();
            StatsdMock.Verify(x => x.LogCount("dependencies.fooservice.method", It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Test]
        public void It_should_not_fire_for_untracked_methods()
        {
            Service.UntrackedMethod();
            StatsdMock.Verify(x => x.LogCount("dependencies.fooservice.method", It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
    }
}
