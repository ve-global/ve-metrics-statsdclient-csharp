using System.Collections.Generic;

namespace Ve.Metrics.StatsDClient.Attributes
{
    public class StatsDCounting : StatsDBase
    {
        public int Count { get; set; }

        public StatsDCounting(string name, int count = 1, Dictionary<string, string> tags = null) 
            : base(name, tags)
        {
            Count = count;
        }
    }
}
