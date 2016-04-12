using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient
{
    public class StatsdConfig : IStatsdConfig
    {
        private const string DefaultHost = "localhost";
        private const int DefaultPort = 8125;

        private int _port;
        private string _host;

        public string Host
        {
            get
            {
                return string.IsNullOrEmpty(_host) ? DefaultHost : _host ;
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
                return _port == 0 ? DefaultPort : _port;
            }
            set
            {
                _port = value;
            }
        }

        public string AppName { get; set; }
    }
}