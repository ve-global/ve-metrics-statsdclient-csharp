using System.Collections.Generic;
using System.Diagnostics;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient
{
    public sealed class TimingToken : ITimingToken
    {
        private readonly IVeStatsDClient _client;
        private readonly string _name;
        private readonly Stopwatch _stopwatch;
        private Dictionary<string, string> _tags;

        public TimingToken(IVeStatsDClient client, string name, Dictionary<string,string> tags = null)
        {
            _stopwatch = Stopwatch.StartNew();
            _client = client;
            _name = name;
            _tags = tags;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _client.LogTiming(_name, _stopwatch.ElapsedMilliseconds, _tags);
        }
    }
}
