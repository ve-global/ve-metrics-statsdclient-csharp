using System.Collections.Generic;

namespace Ve.Metrics.StatsDClient.Abstract
{
    public interface IVeStatsDClient
    {
        void LogCount(string name);
        void LogCount(string name, Dictionary<string, string> tags);
        void LogCount(string name, int count, Dictionary<string, string> tags = null);
        void LogTiming(string name, long milliseconds, Dictionary<string, string> tags = null);
        void LogTiming(string name, int milliseconds, Dictionary<string, string> tags = null);
        ITimingToken LogTiming(string name);
        void LogGauge(string name, int value, Dictionary<string, string> tags = null);
        void LogCalendargram(string name, int value, string period, Dictionary<string, string> tags = null);
        void LogCalendargram(string name, string value, string period, Dictionary<string, string> tags = null);
        void LogRaw(string name, int value, string period, long? epoch = null, Dictionary<string, string> tags = null);
    }
}