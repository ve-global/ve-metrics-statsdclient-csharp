using System;
using System.Collections.Generic;
using System.Linq;
using StatsdClient;

namespace Ve.Metrics.StatsDClient
{
    public class StatsDInfluxWrapper
    {
        private static string _systemTags;
        private readonly Statsd _statsd;

        public StatsDInfluxWrapper(IStatsdConfig config)
        {
            _systemTags = $"host={Environment.MachineName.ToLower()},datacenter={config.Datacenter}";
            _statsd = new Statsd(config.Host, config.Port, config.AppName);
        }

        public void LogCount(string name, Dictionary<string, string> tags = null)
        {
            _statsd.LogCount(BuildName(name, tags));
        }

        public void LogTiming(string name, long milliseconds, Dictionary<string, string> tags = null)
        {
            _statsd.LogTiming(BuildName(name, tags), milliseconds);
        }

        public void LogTiming(string name, int milliseconds, Dictionary<string, string> tags = null)
        {
            _statsd.LogTiming(BuildName(name, tags), milliseconds);
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
            if (tags == null)
            {
                tags = new Dictionary<string, string>();
            }

            return $"{name},{_systemTags},{string.Join(",", tags.Select(x => x.Key + '=' + x.Value))}";
        }
    }
}
