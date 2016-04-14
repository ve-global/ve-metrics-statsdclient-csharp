using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public abstract class SystemMetric
    {
        public abstract string Name { get; }
        public abstract void Execute(IVeStatsDClient client);
    }
}