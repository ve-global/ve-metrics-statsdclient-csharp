using System;
using System.Collections.Generic;
using System.Linq;
using StatsdClient;

namespace Ve.Metrics.StatsDClient
{
    public interface IVeStatsDClient
    {
        void LogCount(string name, Dictionary<string, string> tags = null);
        void LogCount(string name, int count = 1, Dictionary<string, string> tags = null);
        void LogTiming(string name, long milliseconds, Dictionary<string, string> tags = null);
        void LogTiming(string name, int milliseconds, Dictionary<string, string> tags = null);
        TimingToken LogTiming(string name);
        void LogGauge(string name, int value, Dictionary<string, string> tags = null);
        void LogCalendargram(string name, int value, string period, Dictionary<string, string> tags = null);
        void LogCalendargram(string name, string value, string period, Dictionary<string, string> tags = null);
        void LogRaw(string name, int value, string period, long? epoch = null, Dictionary<string, string> tags = null);
    }

    public class VeStatsDClient : IVeStatsDClient
    {
        private static string _systemTags;
        private readonly IStatsd _statsd;

        public VeStatsDClient(IStatsdConfig config) : this(new Statsd(config.Host, config.Port, config.AppName), config.Datacenter)
        {
            if (string.IsNullOrEmpty(config.Datacenter))
            {
                throw new ArgumentNullException("datacenter", "statsd datacenter cannot be empty");
            }

            if (string.IsNullOrEmpty(config.AppName))
            {
                throw new ArgumentNullException("appName", "statsd appName cannot be empty");
            }
        }

        internal VeStatsDClient(IStatsd statsd, string datacenter)
        {
            _statsd = statsd;
            _systemTags = $"host={Environment.MachineName.ToLower()},datacenter={datacenter}";
        }

        public void LogCount(string name)
        {
            LogCount(name, 1);
        }

        public void LogCount(string name, Dictionary<string, string> tags)
        {
            LogCount(name, 1, tags);
        }

        public void LogCount(string name, int count, Dictionary<string, string> tags = null)
        {
            _statsd.LogCount(BuildName(name, tags), count);
        }

        public void LogTiming(string name, long milliseconds, Dictionary<string, string> tags = null)
        {
            _statsd.LogTiming(BuildName(name, tags), milliseconds);
        }

        public void LogTiming(string name, int milliseconds, Dictionary<string, string> tags = null)
        {
            _statsd.LogTiming(BuildName(name, tags), milliseconds);
        }

        public TimingToken LogTiming(string name)
        {
            return new TimingToken(this, name);
        }

        public void LogGauge(string name, int value, Dictionary<string, string> tags = null)
        {
            _statsd.LogGauge(BuildName(name, tags), value);
        }

        public void LogCalendargram(string name, int value, string period, Dictionary<string, string> tags = null)
        {
            _statsd.LogCalendargram(BuildName(name, tags), value, period);
        }

        public void LogCalendargram(string name, string value, string period, Dictionary<string, string> tags = null)
        {
            _statsd.LogCalendargram(BuildName(name, tags), value, period);
        }

        public void LogRaw(string name, int value, string period, long? epoch = null, Dictionary<string, string> tags = null)
        {
            _statsd.LogRaw(BuildName(name, tags), value, epoch);
        }

        private static string BuildName(string name, Dictionary<string, string> tags)
        {
            var prefix = $"{name},{_systemTags}";
            return tags == null
                ? prefix
                : $"{prefix},{string.Join(",", tags.Select(x => x.Key + '=' + x.Value))}";
        }
    }
}
