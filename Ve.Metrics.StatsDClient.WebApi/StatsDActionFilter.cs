using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Ve.Metrics.StatsDClient.WebApi
{
    public class StatsDActionFilter : ActionFilterAttribute
    {
        private readonly StatsDInfluxWrapper _statsd;
        private const string StopwatchKey = "Statsd_Stopwatch";

        public StatsDActionFilter(IStatsdConfig config)
        {
            _statsd = new StatsDInfluxWrapper(config);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.Properties.Add(StopwatchKey, Stopwatch.StartNew());
            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[StopwatchKey];
            stopwatch.Stop();

            _statsd.LogCount("request", GetRouteData(actionExecutedContext.ActionContext));
            _statsd.LogTiming("responses", stopwatch.ElapsedMilliseconds, GetRouteData(actionExecutedContext.ActionContext));

            if (actionExecutedContext.Exception != null)
            {
                _statsd.LogCount("exceptions", GetRouteData(actionExecutedContext.ActionContext));
            }

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        private Dictionary<string, string> GetRouteData(HttpActionContext actionContext)
        {
            object controller;
            object action;
            actionContext.ControllerContext.RouteData.Values.TryGetValue("controller", out controller);
            actionContext.ControllerContext.RouteData.Values.TryGetValue("action", out action);

            return new Dictionary<string, string>()
            {
                { "code", GetStatusCode(actionContext).ToString() },
                { "controller", controller?.ToString().ToLower() },
                { "action", action?.ToString().ToLower() }
            };
        }

        private static int GetStatusCode(HttpActionContext actionExecutedContext)
        {
            return actionExecutedContext.Response != null
                ? (int) actionExecutedContext.Response.StatusCode
                : 0;
        }
    }
}
