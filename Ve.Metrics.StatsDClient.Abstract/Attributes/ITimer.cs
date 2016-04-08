using System.Timers;

namespace Ve.Metrics.StatsDClient.Abstract.Attributes
{
    public interface ITimer
    {
        void Start();
        event ElapsedEventHandler Elapsed;
        int Interval { get; set; }
    }
}