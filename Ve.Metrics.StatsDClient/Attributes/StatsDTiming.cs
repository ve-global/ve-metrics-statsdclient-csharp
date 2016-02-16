using System.Collections.Generic;

namespace Ve.Metrics.StatsDClient.Attributes
{
    public class StatsDTiming : StatsDBase
    {
        public StatsDTiming(string name, Dictionary<string, string> tags = null) : base(name, tags)
        {}
    }
}
