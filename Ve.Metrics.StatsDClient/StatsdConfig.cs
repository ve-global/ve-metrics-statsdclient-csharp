namespace Ve.Metrics.StatsDClient
{
    public interface IStatsdConfig
    {
        string Host { get; }
        string Datacenter { get; }
        int Port { get; }
        string AppName { get; }
    }

    public class StatsdConfig : IStatsdConfig
    {
        public string Host { get; set; }
        public string Datacenter { get; set; }
        public int Port { get; set; }
        public string AppName { get; set; }
    }
}