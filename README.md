Ve.Metrics.StatsDClient
---

Provides a set of wrappers and utilities for logging to the VE metrics infrastructure

StatsDInfluxWrapper(

This wraps the basic StatsD client in order to provide influxDB specific functionality (i.e. support for tagging).

```

var statsd = new StatsDInfluxWrapper();

statsd.LogCount("foo", new Dictionary<string, string>(){ { "bar", "baz" } });

```

StatsDActionFilter

A HttpActionFilter designed to be used with Asp.Net WebApi. It logs the following info:

- response time
- response code
- controller name
- action name
- unhandled exceptions (note: only logs that an exception occurred, not the detail)

```csharp
public static class WebApiConfig
```csharp
    public static void Register(HttpConfiguration config)
    
        config.Filters.Add(new StatsDLoggerFilter());
    {
         config.Filters.Add(new StatsdActionFilter(
	     new StatsdConfig()
	     {
	         Host = "metrics-statsd.ve-ci.com",
		 AppName = "myapp",
		 Datacenter = "ci",
		 Port = 8125
	     }));
    }
```

Configuration:

Relies on the following settings:

```csharp
new StatsDConfig()
{
  Host = "metrics-statsd.ve-ci.com", // host where the udp statsd listener is running
  Port = 8125, // optional, default is 8125
  AppName = "myapp", // name identifying this application
  DataCenter = "ci" // string identifying where this app is running (ci, preprod, pro-westeurope, pro-eastasia etc.)
}
```
