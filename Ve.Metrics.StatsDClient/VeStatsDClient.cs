using System;
using System.Collections.Generic;
using System.Linq;
using StatsdClient;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient
{
    public class VeStatsDClient : IVeStatsDClient
    {
        private static string _systemTags;
        private readonly IStatsd _statsd;

        public VeStatsDClient(IStatsdConfig config) : this(new Statsd(config.Host, config.Port, config.AppName), config.Datacenter, config.CustomTags)
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

        internal VeStatsDClient(IStatsd statsd, string datacenter, Dictionary<string, string> customTags = null)
        {
            _statsd = statsd;
            _systemTags = $"host={Environment.MachineName.ToLower()},datacenter={datacenter}";
            if (customTags != null)
            {
                _systemTags = $"{_systemTags},{string.Join(",", customTags.Select(x => x.Key + '=' + x.Value))}";
            }
        }

        private static void RunSafe(Action thing)
        {
            try
            {
                thing();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
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
            RunSafe(
                () => _statsd.LogCount(BuildName(name, tags), count));
        }

        public void LogTiming(string name, long milliseconds, Dictionary<string, string> tags = null)
        {
            RunSafe(() =>
                _statsd.LogTiming(BuildName(name, tags), milliseconds));
        }

        public void LogTiming(string name, int milliseconds, Dictionary<string, string> tags = null)
        {
            RunSafe(() =>
                _statsd.LogTiming(BuildName(name, tags), milliseconds));
        }

        public ITimingToken LogTiming(string name)
        {
            return new TimingToken(this, name);
        }

        public ITimingToken LogTiming(string name, Dictionary<string, string> tags)
        {
            return new TimingToken(this, name, tags);
        }

        public void LogGauge(string name, int value, Dictionary<string, string> tags = null)
        {
            RunSafe(() =>
                _statsd.LogGauge(BuildName(name, tags), value));
        }

        public void LogCalendargram(string name, int value, string period, Dictionary<string, string> tags = null)
        {
            RunSafe(() =>
                _statsd.LogCalendargram(BuildName(name, tags), value, period));
        }

        public void LogCalendargram(string name, string value, string period, Dictionary<string, string> tags = null)
        {
            RunSafe(() =>
                _statsd.LogCalendargram(BuildName(name, tags), value, period));
        }

        public void LogRaw(string name, int value, string period, long? epoch = null, Dictionary<string, string> tags = null)
        {
            RunSafe(() =>
                _statsd.LogRaw(BuildName(name, tags), value, epoch));
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
