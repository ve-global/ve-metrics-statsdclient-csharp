using System;
using System.Diagnostics;

namespace Ve.Metrics.StatsDClient
{
    public static class Logger
    {
        public static void Error(Exception e)
        {
            Trace.WriteLine(e);
        }
    }
}
