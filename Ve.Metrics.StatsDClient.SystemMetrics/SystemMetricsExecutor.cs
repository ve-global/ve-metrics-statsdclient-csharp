using System.Collections.Generic;
using System.Timers;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class SystemMetricsExecutor
    {
        private readonly IList<SystemMetric> _metrics;
        private readonly IVeStatsDClient _client;
        private static ITimer _timer;

        public int Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        public SystemMetricsExecutor(IList<SystemMetric> metrics, IVeStatsDClient client)
            : this(metrics, client, new TimerWrapper())
        {
            Interval = 10000;
        }
        
        internal SystemMetricsExecutor(IList<SystemMetric> metrics, IVeStatsDClient client, ITimer timer)
        {
            _metrics = metrics;
            _client = client;
            _timer = timer;

            _timer.Elapsed += Invoke;
            _timer.Start();
        }

        private void Invoke(object sender, ElapsedEventArgs e)
        {
            foreach (var metric in _metrics)
            {
                metric.Execute(_client);
            }
        }
    }
}
