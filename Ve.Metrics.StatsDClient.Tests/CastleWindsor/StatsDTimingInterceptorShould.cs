using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Tests.CastleWindsor
{
    [TestFixture]
    public class StatsDTimingInterceptorShould : WindsorInterceptorsTestBase<StatsDClient.CastleWindsor.StatsDTimingInterceptor, StatsDTiming>
    {
        [SetUp]
        public new void Setup()
        {
            base.Setup();
            StatsdMock.Setup(x => x.LogTiming(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()));
        }

        [Test]
        public void It_should_time_the_targeted_method_and_log_to_statsd()
        {
            Service.TrackedMethod();
            StatsdMock.Verify(x => x.LogTiming("dependencies.fooservice.method", It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()), Times.Once);
        }

        [Test]
        public void It_should_not_fire_for_untracked_methods()
        {
            Service.UntrackedMethod();
            StatsdMock.Verify(x => x.LogTiming("dependencies.fooservice.method", It.IsAny<long>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
        }
    }
}
