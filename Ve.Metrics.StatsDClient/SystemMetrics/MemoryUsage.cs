using System.Diagnostics;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.SystemMetrics;

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
