using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class MemoryUsage : SystemMetric
    {
        public override string Name => "MemoryUsage";

        public override void Execute(IVeStatsDClient client)
        {
            var memoryUsageInMegaBytes = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024) / 1024;
            client.LogGauge("process.memoryusage", (int) memoryUsageInMegaBytes);
        }
    }
}
