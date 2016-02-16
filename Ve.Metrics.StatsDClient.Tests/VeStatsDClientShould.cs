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
            _statsd.Verify(x => x.LogCount(It.IsRegex("foo\\.bar\\,host\\=(.*)\\,datacenter\\=foo"), It.IsAny<int>()));
        }

        [Test]
        public void It_should_include_runtime_tags_before_sending_metrics()
        {
            _client.LogCount("foo.bar", new Dictionary<string,string>() { {"foo", "bar"} });
            _statsd.Verify(x => x.LogCount(It.IsRegex("foo\\.bar\\,host\\=(.*)\\,datacenter\\=foo,foo\\=bar"), It.IsAny<int>()));
        }
    }
}
