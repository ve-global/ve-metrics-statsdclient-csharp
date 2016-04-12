using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Ve.Metrics.StatsDClient.SystemMetrics
{
    public class AspNetCounters : SystemMetric
    {
        private readonly PerformanceCounter[] _counters;
        private readonly string[] _normalizedNames;

        public override string Name => "AspNetCounters";

        public AspNetCounters()
        {
            //as specified by https://msdn.microsoft.com/en-us/library/fxk122b4.aspx
            if (PerformanceCounterCategory.Exists("ASP.NET Applications"))
            {
                //There is no search method for category
                var categories = PerformanceCounterCategory.GetCategories();
                foreach (var category in categories)
                {
                    if(category.CategoryName == "ASP.NET Applications")
                    {
                        _counters = category.GetCounters("__TOTAL__");
                        _normalizedNames = new string[_counters.Length];
                        for(int i = 0; i < _counters.Length; i++)
                        {
                            _normalizedNames[i] = NormalizeName(_counters[i].CounterName);
                        }
                    }
                }
            }
        }

        public override void Execute(IVeStatsDClient client)
        {
            for (int i = 0; i < _counters.Length; i++)
            {
                var counter = _counters[i];
                switch (counter.CounterType)
                {
                    case PerformanceCounterType.NumberOfItems32:
                        client.LogCount(_normalizedNames[i], (int)_counters[i].NextValue());
                        break;
                    case PerformanceCounterType.RateOfCountsPerSecond32:
                        client.LogGauge(_normalizedNames[i], (int)Math.Round(_counters[i].NextValue()));
                        break;
                    
                    case PerformanceCounterType.RawFraction:
                        //TODO: Fix Raw fraction to verify base before to avoid exeption
                        try
                        {
                            client.LogGauge(
                                _normalizedNames[i], 
                                (int)Math.Round((_counters[i].NextValue() * 100))
                                );
                        }
                        catch (DivideByZeroException)
                        {
                        }
                        break;
                    case PerformanceCounterType.RawBase:
                        break;
                    case PerformanceCounterType.CountPerTimeInterval32:
                    case PerformanceCounterType.AverageBase:
                    case PerformanceCounterType.AverageCount64:
                    case PerformanceCounterType.SampleFraction:
                    case PerformanceCounterType.SampleCounter:
                    case PerformanceCounterType.SampleBase:
                    case PerformanceCounterType.AverageTimer32:
                    case PerformanceCounterType.CounterTimer:
                    case PerformanceCounterType.CounterTimerInverse:
                    case PerformanceCounterType.Timer100Ns:
                    case PerformanceCounterType.Timer100NsInverse:
                    case PerformanceCounterType.ElapsedTime:
                    case PerformanceCounterType.CounterMultiTimer:
                    case PerformanceCounterType.CounterMultiTimerInverse:
                    case PerformanceCounterType.CounterMultiTimer100Ns:
                    case PerformanceCounterType.CounterMultiTimer100NsInverse:
                    case PerformanceCounterType.CounterMultiBase:
                    case PerformanceCounterType.CounterDelta32:
                    case PerformanceCounterType.CounterDelta64:
                    case PerformanceCounterType.NumberOfItems64:
                    case PerformanceCounterType.NumberOfItemsHEX32:
                    case PerformanceCounterType.NumberOfItemsHEX64:
                    case PerformanceCounterType.RateOfCountsPerSecond64:
                    case PerformanceCounterType.CountPerTimeInterval64:
                    default:
                        Trace.TraceWarning("Not supported performance counter type");
                        break;
                };
            }
        }

        //Hack to put names in acceptable format
        private string NormalizeName(string name)
        {
            name = name.Trim();
            name = Regex.Replace(name, @"([\s%(/]+)", ".");
            name.Replace(")", "");
            name = name.ToLowerInvariant();
            return string.Format("aspnet.{0}",name);
        }
    }
}
