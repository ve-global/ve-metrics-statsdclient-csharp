using System;
using System.Diagnostics;

namespace Ve.Metrics.StatsDClient
{
    public sealed class TimingToken : IDisposable
    {
        private readonly StatsDInfluxWrapper _client;
        private readonly string _name;
        private readonly Stopwatch _stopwatch;

        internal TimingToken(StatsDInfluxWrapper client, string name)
        {
            _stopwatch = Stopwatch.StartNew();
            _client = client;
            _name = name;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _client.LogTiming(_name, _stopwatch.ElapsedMilliseconds);
        }
    }
}
