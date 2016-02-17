using System.Timers;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public interface ITimer
    {
        void Start();
        event ElapsedEventHandler Elapsed;
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