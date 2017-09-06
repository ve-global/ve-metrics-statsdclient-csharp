using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient.Mvc
{
    public class StatsDActionFilter : ActionFilterAttribute
    {
        private readonly IVeStatsDClient _statsd;
        private const string StopwatchKey = "Statsd_Stopwatch";

        public StatsDActionFilter(IVeStatsDClient statsd)
        {
            _statsd = statsd;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Items.Contains(StopwatchKey))
            {
                filterContext.HttpContext.Items.Add(StopwatchKey, Stopwatch.StartNew());
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var routeData = GetRouteData(filterContext.HttpContext);

            if (filterContext.HttpContext.Items.Contains(StopwatchKey))
            {
                var stopwatch = (Stopwatch)filterContext.HttpContext.Items[StopwatchKey];
                stopwatch.Stop();
                _statsd.LogTiming("responses", stopwatch.ElapsedMilliseconds, routeData);
            }

            _statsd.LogCount("request", routeData);

            if (filterContext.Exception != null)
            {
                _statsd.LogCount("exceptions", GetRouteData(filterContext.HttpContext));
            }

            base.OnResultExecuted(filterContext);
        }

        private Dictionary<string, string> GetRouteData(HttpContextBase context)
        {
            object controller;
            object action;
            context.Request.RequestContext.RouteData.Values.TryGetValue("controller", out controller);
            context.Request.RequestContext.RouteData.Values.TryGetValue("action", out action);

            var ctr = string.IsNullOrEmpty(controller?.ToString())
                ? "none"
                : controller.ToString().ToLower();
            var act = string.IsNullOrEmpty(action?.ToString())
                ? "none"
                : action.ToString().ToLower();

            return new Dictionary<string, string>()
            {
                { "code", context.Response.StatusCode.ToString() },
                { "controller", ctr },
                { "action", act },
                { "userAgent", context.Request.UserAgent}
            };
        }
    }
}
