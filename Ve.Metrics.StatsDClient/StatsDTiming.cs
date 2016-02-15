using System;
using System.Collections.Generic;

namespace Ve.Metrics.StatsDClient
{
    [AttributeUsage(AttributeTargets.Method)]
    public class StatsDTiming : Attribute
    {
        public string Name { get; set; }
        public Dictionary<string,string> Tags { get; set; } 

        public StatsDTiming(string name)
        {
            Name = name;
        }
    }
}
