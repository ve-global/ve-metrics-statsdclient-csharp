using System;
using System.Collections.Generic;
using System.Timers;
using Moq;
using NUnit.Framework;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;
using Ve.Metrics.StatsDClient.SystemMetrics;

namespace Ve.Metrics.StatsDClient.Tests.SystemMetrics
{
    [TestFixture]
    public class SystemMetricsExecutorShould
    {
        private Mock<IVeStatsDClient> _statsd;
        private Mock<SystemMetric> _mockMetric;
        private Mock<ITimer> _timer;
        private SystemMetricsExecutor _executor;
        
        [SetUp]
        public void Setup()
        {
            _statsd = new Mock<IVeStatsDClient>();
            _mockMetric = new Mock<SystemMetric>();
            _mockMetric.Setup(x => x.Execute(It.IsAny<IVeStatsDClient>()));
            _timer = new Mock<ITimer>();
            _timer.Setup(x => x.Start());
            _executor = new SystemMetricsExecutor(new List<SystemMetric>() {_mockMetric.Object}, _statsd.Object, _timer.Object);
        }

        [Test]
        public void It_should_start_the_timer()
        {
            _timer.Verify(x => x.Start());
        }

        [Test]
        public void It_should_invoke_the_metrics_when_the_timeout_expires()
        {
            _timer.Raise(x => x.Elapsed += null, default(ElapsedEventArgs));
            _mockMetric.Verify(x => x.Execute(It.IsAny<IVeStatsDClient>()));
        }
    }
}
