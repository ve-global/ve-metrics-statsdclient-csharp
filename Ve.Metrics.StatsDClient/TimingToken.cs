using System.Diagnostics;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient
{
    public sealed class TimingToken : ITimingToken
    {
        private readonly IVeStatsDClient _client;
        private readonly string _name;
        private readonly Stopwatch _stopwatch;

        public TimingToken(IVeStatsDClient client, string name)
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
