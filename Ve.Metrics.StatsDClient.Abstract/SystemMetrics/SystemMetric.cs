namespace Ve.Metrics.StatsDClient.Abstract.SystemMetrics
{
    public abstract class SystemMetric
    {
        public abstract string Name { get; }
        public abstract void Execute(IVeStatsDClient client);
    }
}