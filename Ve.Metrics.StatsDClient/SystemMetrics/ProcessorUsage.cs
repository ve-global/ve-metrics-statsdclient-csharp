using System.Diagnostics;
using Ve.Metrics.StatsDClient.Abstract;
using Ve.Metrics.StatsDClient.Abstract.SystemMetrics;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class ProcessorUsage : SystemMetric
    {
        public override string Name => "ProcessorUsage";

        public override void Execute(IVeStatsDClient client)
        {
            var processorTime = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime.Milliseconds;
            client.LogGauge("process.processortime", processorTime);
        }
    }
}
