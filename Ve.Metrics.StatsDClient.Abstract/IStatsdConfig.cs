namespace Ve.Metrics.StatsDClient.Abstract
{
    public interface IStatsdConfig
    {
        string Host { get; }
        string Datacenter { get; }
        int Port { get; }
        string AppName { get; }
    }
}