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
        private int _port;
        public string Host { get; set; }
        public string Datacenter { get; set; }
        public int Port { get { return _port == 0 ? 8125 : _port; } set { _port = value; } }
        public string AppName { get; set; }
    }
}