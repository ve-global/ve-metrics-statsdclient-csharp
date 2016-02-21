using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Shouldly;
using Ve.Metrics.StatsDClient.SystemMetrics;

namespace Ve.Metrics.StatsDClient.Tests.SystemMetrics
{
    [TestFixture]
    public class AspNetCountersShould
    {
        private Mock<IVeStatsDClient> _statsd;
        private AspNetCounters _metric;

        [SetUp]
        public void Setup()
        {
            _statsd = new Mock<IVeStatsDClient>();
            _statsd.Setup(x => x.LogGauge(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));
            _statsd.Setup(x => x.LogCount(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));
            _metric = new AspNetCounters();

            _metric.Execute(_statsd.Object);
        }

        [Test]
        public void It_should_implement_the_name_property()
        {
            _metric.Name.ShouldBe("AspNetCounters");
        }

        [Test]
        public void It_should_get_the_aspnet_counters_and_log_it_to_statsd()
        {
            _statsd.Verify(x => x.LogGauge(It.Is<string>((s) => s.StartsWith("aspnet")),It.IsAny<int>(), It.IsAny<Dictionary<string,string>>()));
            _statsd.Verify(x => x.LogCount(It.Is<string>((s) => s.StartsWith("aspnet")), It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));

        }
    }
}
