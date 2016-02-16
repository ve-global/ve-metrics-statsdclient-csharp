using System;
using System.Collections.Generic;

namespace Ve.Metrics.StatsDClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class StatsDBase : Attribute
    {
        public string Name { get; set; }
        public Dictionary<string, string> Tags { get; set; }

        protected StatsDBase(string name, Dictionary<string, string> tags = null)
        {
            Name = name;
            Tags = tags;
        }
    }
}
