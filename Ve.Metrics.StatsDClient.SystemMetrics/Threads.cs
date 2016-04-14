using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class Threads : SystemMetric
    {
        public override string Name => "Threads";

        public override void Execute(IVeStatsDClient client)
        {
            var threads = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;
            client.LogGauge("process.threads", threads);
        }
    }
}
