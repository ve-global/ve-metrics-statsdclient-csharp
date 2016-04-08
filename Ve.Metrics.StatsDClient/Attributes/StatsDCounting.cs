using Ve.Metrics.StatsDClient.Abstract.Attributes;

namespace Ve.Metrics.StatsDClient.Attributes
{
    public class StatsDCounting : StatsDBase
    {
        public int Count { get; set; }

        public StatsDCounting(string name, int count = 1) 
            : base(name)
        {
            Count = count;
        }
    }
}
