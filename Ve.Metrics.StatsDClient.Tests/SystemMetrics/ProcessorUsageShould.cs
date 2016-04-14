using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Shouldly;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.SystemMetrics;

namespace Ve.Metrics.StatsDClient.Tests.SystemMetrics
{
    [TestFixture]
    public class ProcessorUsageShould
    {
        private Mock<IVeStatsDClient> _statsd;
        private ProcessorUsage _metric;

        [SetUp]
        public void Setup()
        {
            _statsd = new Mock<IVeStatsDClient>();
            _statsd.Setup(x => x.LogGauge(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));
            _metric = new ProcessorUsage();

            _metric.Execute(_statsd.Object);
        }

        [Test]
        public void It_should_implement_the_name_property()
        {
            _metric.Name.ShouldBe("ProcessorUsage");
        }

        [Test]
        public void It_should_get_the_processor_time_and_log_it_to_statsd()
        {
            _statsd.Verify(x => x.LogGauge("process.processortime", It.IsAny<int>(), It.IsAny<Dictionary<string, string>>()));
        }
    }
}
