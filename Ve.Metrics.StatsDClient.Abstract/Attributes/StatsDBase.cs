using System;
using System.Collections.Generic;

namespace Ve.Metrics.StatsDClient.Abstract.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class StatsDBase : Attribute
    {
        public string Name { get; set; }
        public Dictionary<string, string> Tags { get; set; }

        protected StatsDBase(string name)
        {
            Name = name;
        }
    }
}
