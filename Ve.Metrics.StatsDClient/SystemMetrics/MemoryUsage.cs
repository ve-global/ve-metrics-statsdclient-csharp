using System.Diagnostics;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class MemoryUsage : SystemMetric
    {
        private readonly PerformanceCounter _availableMemoryCounter;

        public override string Name => "MemoryUsage";

        public MemoryUsage()
        {
            _availableMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public override void Execute(IVeStatsDClient client)
        {
            var memoryUsageInMegaBytes = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024) / 1024;
            
            client.LogGauge("process.memoryusage", (int) memoryUsageInMegaBytes);
            client.LogGauge("system.memory.free", (int) _availableMemoryCounter.NextValue());
        }
    }
}
