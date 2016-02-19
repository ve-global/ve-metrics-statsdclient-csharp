using System.Diagnostics;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class ProcessorUsage : SystemMetric
    {
        private readonly PerformanceCounter _cpuCounter;
        public override string Name => "ProcessorUsage";

        public ProcessorUsage()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public override void Execute(IVeStatsDClient client)
        {
            var processorTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.Milliseconds;

            client.LogGauge("system.processor.time", (int) _cpuCounter.NextValue());
            client.LogGauge("process.processortime", processorTime);
        }
    }
}
