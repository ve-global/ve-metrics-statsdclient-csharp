Ve.Metrics.StatsDClient [![Build status](https://ci.appveyor.com/api/projects/status/n7qstecrnf0etli1?svg=true)](https://ci.appveyor.com/project/andyroyle/ve-metrics-statsdclient-csharp)
---

Provides a set of wrappers and utilities for logging to the VE metrics infrastructure.

Packages:

- [Ve.Metrics.StatsDClient](https://www.nuget.org/packages/Ve.Metrics.StatsDClient)
- [Ve.Metrics.StatsDClient.WebApi](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.WebApi)
- [Ve.Metrics.StatsDClient.SimpleInjector](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.SimpleInjector)
- [Ve.Metrics.StatsDClient.CastleWindsor](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.CastleWindsor)
- [Ve.Metrics.StatsDClient.Unity](https://www.nuget.org/packages/Ve.Metrics.StatsDClient.Unity)

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
        config.Filters.Add(new StatsDActionFilter(
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

####Method Attributes:
Provides timing/counting attribute to wrap methods.

- `StatsDTiming`
- `StatsDCounting`

Add the attribute to the method you want:

````csharp
public class MyProvider : IService {

    [StatsDTiming("dependencies.somedependency.get")]
    public object GetSomething()
    {
        return _fooService.GetSomething() // some external service (e.g. redis, sql server etc.)
    }

    [StatsDCounting("dependencies.somedependency.do"]
    public void DoSomething()
    {
        _fooService.DoSomething();
    }
}
```

Register the interceptor for your container of choice. (Currently supported are SimpleInjector, Castle.Windsor and Unity)

```csharp
// SimpleInjector
container.RegisterSingleton<IService, MyService>();
container.InterceptWith<StatsDTimingInterceptor>(type => type == typeof(IService));
container.InterceptWith<StatsDCountingInterceptor(type => type == typeof(IService));

// Castle.Windsor
container.Register(Component.For<StatsDTimingInterceptor>());
container.Register(Component.For<IService>()
                       .ImplementedBy<MyProvider>()
                       .Interceptors<StatsDTimingInterceptor>()
                       .Interceptors<StatsDCountingInterceptor>());

// Unity
container.AddNewExtension<Interception>();
container.RegisterType<IService, MyService>(
    new Interceptor<InterfaceInterceptor>(),
    new InterceptionBehavior<StatsDTimingInterceptor(),
    new InterceptionBehavior<StatsDCountingInterceptor())
```

Now every call to `IService.GetSomething()` will log timing data and every call to `IService.DoSomething()` will log counts to statsd.


####System Metrics:

`SystemMetricsExecutor` provides a wrapper for periodically reporting system metrics to statsd. Currently supported:

- MemoryUsage
- ProcessorUsage

```csharp
var statsdClient = new VeStatsDClient(/*...*/);
var executor = new SystemMetricsExecutor(new List<SystemMetric>(){ new MemoryUsage(), new ProcessorUsage() }, statsdClient);
```

To add your own custom metrics simply create a class that inherits from `SystemMetric`:

```csharp
public class MyMetric : SystemMetric
{
    public string Name => "MyMetric";
    public void Execute(IVeStatsDClient client)
    {
        var myMetric = 1234;
        client.LogGauge("process.mymetric", myMetric);
    }
}
```

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
