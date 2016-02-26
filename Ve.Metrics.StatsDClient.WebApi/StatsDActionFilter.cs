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
        private readonly IVeStatsDClient _statsd;
        private const string StopwatchKey = "Statsd_Stopwatch";

        public StatsDActionFilter(IStatsdConfig config)
        {
            _statsd = new VeStatsDClient(config);
        }

        public StatsDActionFilter(IVeStatsDClient client)
        {
            _statsd = client;
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
            var action = actionContext?.ActionDescriptor?.ActionName?.ToLower() ?? "none";
            var controller = actionContext?.ControllerContext?.ControllerDescriptor?.ControllerName?.ToLower() ?? "none";

            var ctr = string.IsNullOrEmpty(controller)
                ? "none"
                : controller.ToLower();
            var act = string.IsNullOrEmpty(action)
                ? "none"
                : action.ToLower();

            return new Dictionary<string, string>()
            {
                { "code", GetStatusCode(actionContext).ToString() },
                { "controller", ctr },
                { "action", act }
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
