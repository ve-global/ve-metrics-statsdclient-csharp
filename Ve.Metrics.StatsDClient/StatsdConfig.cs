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
        private const string DEFAULT_HOST = "localhost";
        private const int DEFAULT_PORT = 8125;

        private int _port;
        private string _host;

        public string Host
        {
            get
            {
                return string.IsNullOrEmpty(_host) ? DEFAULT_HOST : _host ;
            }
            set
            {
                _host = value;
            }
        }

        public string Datacenter { get; set; }

        public int Port
        {
            get
            {
                return _port == 0 ? DEFAULT_PORT : _port;
            }
            set
            {
                _port = value;
            }
        }

        public string AppName { get; set; }
    }
}