using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient.WebApi
{
    public class StatsDActionFilter : ActionFilterAttribute
    {
        private readonly IVeStatsDClient _statsd;
        private const string StopwatchKey = "Statsd_Stopwatch";
        private const int DEFAULT_RESPONSE_STATUS_CODE = 500;

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
            if (!actionContext.Request.Properties.ContainsKey(StopwatchKey))
            {
                actionContext.Request.Properties.Add(StopwatchKey, Stopwatch.StartNew());
            }

            base.OnActionExecuting(actionContext);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var hasException = actionExecutedContext.Exception != null;
            var routeData = GetRouteData(actionExecutedContext.ActionContext, hasException);

            if (actionExecutedContext.Request.Properties.ContainsKey(StopwatchKey))
            {
                var stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[StopwatchKey];
                stopwatch.Stop();
                _statsd.LogTiming("responses", stopwatch.ElapsedMilliseconds, GetRouteData(actionExecutedContext.ActionContext, hasException));
            }

            if (hasException)
            {
                _statsd.LogCount("exceptions", routeData);
            }

            _statsd.LogCount("request", routeData);
            
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        private Dictionary<string, string> GetRouteData(HttpActionContext actionContext, bool hasException = false)
        {
            var action = actionContext?.ActionDescriptor?.ActionName?.ToLower() ?? "none";
            var controller = actionContext?.ControllerContext?.ControllerDescriptor?.ControllerName?.ToLower() ?? "none";
            var statusCode = !hasException ? GetStatusCode(actionContext).ToString() : DEFAULT_RESPONSE_STATUS_CODE.ToString();

            return new Dictionary<string, string>()
            {
                { "code", statusCode },
                { "controller", controller },
                { "action", action }
            };
        }

        private static int GetStatusCode(HttpActionContext actionExecutedContext)
        {
            return actionExecutedContext.Response != null
                ? (int)actionExecutedContext.Response.StatusCode
                : 0;
        }
    }
}