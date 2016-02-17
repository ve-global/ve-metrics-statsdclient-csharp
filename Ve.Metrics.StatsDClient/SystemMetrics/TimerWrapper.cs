using System.Timers;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public interface ITimer
    {
        void Start();
        event ElapsedEventHandler Elapsed;
        int Interval { get; set; }
    }

    public class TimerWrapper : ITimer
    {
        private readonly Timer _timer;

        public TimerWrapper(int interval = 10000)
        {
            _timer = new Timer(interval)
            {
                AutoReset = true,
                Enabled = true
            };
        }

        public int Interval
        {
            get { return (int) _timer.Interval;  }
            set { _timer.Interval = value; }
        }

        public void Start()
        {
            _timer.Start();
        }

        public event ElapsedEventHandler Elapsed
        {
            add
            {
                _timer.Elapsed += value;
            }
            remove
            {
                _timer.Elapsed -= value;
            }
        }
    }
}