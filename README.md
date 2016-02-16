Ve.Metrics.StatsDClient [![Build status](https://ci.appveyor.com/api/projects/status/n7qstecrnf0etli1?svg=true)](https://ci.appveyor.com/project/andyroyle/ve-metrics-statsdclient-csharp)
---

Provides a set of wrappers and utilities for logging to the VE metrics infrastructure.

Packages:

- [Ve.Metrics.StatsDClient](https://www.nuget.org/packages/Ve.Metrics.StatsDClient)
- [Ve.Metrics.StatsDClient.WebApi](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.WebApi)
- [Ve.Metrics.StatsDClient.SimpleInjector](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.SimpleInjector)
- [Ve.Metrics.StatsDClient.CastleWindsor](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.CastleWindsor)

```csharp
var statsd = new VeStatsDClient(
                 new StatsDConfig()
                 {
                   Host = "metrics-statsd.ve-ci.com",
                   Port = 8125,
                   AppName = "myapp",
                   Datacenter = "ci"
                 }));

statsd.LogCount("foo", new Dictionary<string, string>(){ { "bar", "baz" } });

```

####StatsDActionFilter

A HttpActionFilter designed to be used with Asp.Net WebApi. It logs the following info:

- response time
- response code
- controller name
- action name
- unhandled exceptions (note: only logs that an exception occurred, not the detail)

```csharp
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
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
}
```

####StatsDTiming (attribute):
Provides a timing attribute to wrap methods in with a LogTiming call.

Add the attribute to the method you want to time:

````csharp
public class MyProvider : IService {

    [StatsDTiming("dependencies.somedependency.get")]
    public object GetSomething()
    {
        return _fooService.GetSomething() // some external service (e.g. redis, sql server etc.)
    }
}
```

Register the interceptor for your container of choice. (Currently supported are SimpleInjector and Castle.Windsor)

```csharp
// SimpleInjector
container.InterceptWith<StatsDTimingInterceptor>(type => type == typeof(IService));

// Castle.Windsor
container.Register(Component.For<StatsDTimingInterceptor>());
container.Register(Component.For<IService>().ImplementedBy<MyProvider>().Interceptor<StatsDTimingInterceptor>());
```

Now every call to `IService.GetSomething()` will log timing data out to StatsD.

####Configuration:

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
