using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Shouldly;
using StatsdClient;

namespace Ve.Metrics.StatsDClient.Tests
{
    [TestFixture]
    public class VeStatsDClientShould
    {
        private static Mock<IStatsd> _statsd;
        private static VeStatsDClient _client;

        [SetUp]
        public void Setup()
        {
            _statsd = new Mock<IStatsd>();
            _statsd.Setup(x => x.LogCount(It.IsAny<string>(), It.IsAny<int>()));
            _statsd.Setup(x => x.LogTiming(It.IsAny<string>(), It.IsAny<long>()));
            _client = new VeStatsDClient(_statsd.Object, "foo");
        }

        [Test]
        public void It_should_not_allow_datacenter_to_be_empty()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                var client =
                    new VeStatsDClient(new StatsdConfig()
                    {
                        AppName = "foo"
                    });
            });
        }

        [Test]
        public void It_should_not_allow_appname_to_be_empty()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                var client =
                    new VeStatsDClient(new StatsdConfig()
                    {
                        Datacenter = "bar"
                    });
            });
        }

        [Test]
        public void It_should_include_system_tags_before_sending_metrics()
        {
            _client.LogCount("foo.bar");
            _statsd.Verify(x => x.LogCount(It.IsRegex("foo\\.bar\\,host\\=([A-Za-z0-9-]+)\\,datacenter\\=foo"), It.IsAny<int>()));
        }

        [Test]
        public void It_should_include_custom_tags_before_sending_metrics()
        {
            var client = new VeStatsDClient(_statsd.Object, "foo", new Dictionary<string, string> { { "foo", "bar" } });
            client.LogCount("foo.bar");
            _statsd.Verify(x => x.LogCount(It.IsRegex("foo\\.bar\\,host\\=([A-Za-z0-9-]+)\\,datacenter\\=foo\\,foo\\=bar"), It.IsAny<int>()));
        }

        [Test]
        public void It_should_include_runtime_tags_before_sending_metrics()
        {
            _client.LogCount("foo.bar", new Dictionary<string, string>() { { "foo", "bar" } });
            _statsd.Verify(x => x.LogCount(It.IsRegex("foo\\.bar\\,host\\=([A-Za-z0-9-]+)\\,datacenter\\=foo,foo\\=bar"), It.IsAny<int>()));
        }

        [Test]
        public void It_should_pass_the_tags_supplied_when_using_the_timing_token()
        {
            var token = _client.LogTiming("foo.bar", new Dictionary<string, string> { { "foo", "bar" } });
            token.Dispose();
            _statsd.Verify(x => x.LogTiming(It.IsRegex("foo\\.bar\\,host\\=([A-Za-z0-9-]+)\\,datacenter\\=foo,foo\\=bar"), It.IsAny<long>()));
        }

        [Test]
        public void It_should_catch_runtime_errors_from_logcount()
        {
            _statsd.Setup(x => x.LogCount(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception("ohes noes, something broke"));

            _client.LogCount("foo.bar");
        }

        [Test]
        public void It_should_catch_runtime_errors_from_loggauge()
        {
            _statsd.Setup(x => x.LogGauge(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception("ohes noes, something broke"));

            _client.LogCount("foo.bar");
        }

        [Test]
        public void It_should_catch_runtime_errors_from_lograw()
        {
            _statsd.Setup(x => x.LogRaw(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<long?>()))
                .Throws(new Exception("ohes noes, something broke"));

            _client.LogCount("foo.bar");
        }

        [Test]
        public void It_should_catch_runtime_errors_from_logcalendargram()
        {
            _statsd.Setup(x => x.LogCalendargram(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("ohes noes, something broke"));

            _client.LogCount("foo.bar");
        }

        [Test]
        public void It_should_catch_runtime_errors_from_logset()
        {
            _statsd.Setup(x => x.LogSet(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception("ohes noes, something broke"));

            _client.LogCount("foo.bar");
        }
    }
}
