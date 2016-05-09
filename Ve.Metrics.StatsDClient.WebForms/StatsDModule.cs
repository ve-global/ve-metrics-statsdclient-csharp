using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using Ve.Metrics.StatsDClient.Abstract;

namespace Ve.Metrics.StatsDClient.WebForms
{
    public class StatsDModule : IHttpModule
    {
        public static IVeStatsDClient Statsd;
        private const string StopwatchKey = "Statsd_Stopwatch";
        private const int DEFAULT_RESPONSE_STATUS_CODE = 500;

        public void Init(HttpApplication application)
        {
            application.BeginRequest += Application_BeginRequest;
            application.EndRequest += Application_EndRequest;
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            var application = (HttpApplication)source;
            var context = application.Context;
            var filePath = context.Request.FilePath;
            var fileExtension = VirtualPathUtility.GetExtension(filePath);

            if (fileExtension != null && fileExtension.Equals(".aspx"))
            {
                application.Context.Items[StopwatchKey] = Stopwatch.StartNew();
            }
        }

        private void Application_EndRequest(Object source, EventArgs e)
        {
            if (Statsd != null)
            {
                var application = (HttpApplication)source;
                var context = application.Context;
                var filePath = context.Request.FilePath;
                var fileExtension = VirtualPathUtility.GetExtension(filePath);

                if (fileExtension != null && fileExtension.Equals(".aspx"))
                {
                    var stopwatch = (Stopwatch)context.Items[StopwatchKey];
                    stopwatch.Stop();

                    var hasException = false;
                    if (context.Error != null)
                    {
                        hasException = true;
                        Statsd.LogCount("exceptions", GetRouteData(context, hasException));
                    }

                    Statsd.LogCount("request", GetRouteData(context, hasException));
                    Statsd.LogTiming("responses", stopwatch.ElapsedMilliseconds, GetRouteData(context, hasException));
                }
            }
        }

        private Dictionary<string, string> GetRouteData(HttpContext actionContext, bool hasException = false)
        {
            var filePath = actionContext.Request.FilePath;
            var fileExtension = VirtualPathUtility.GetExtension(filePath);

            var action = actionContext.Request.HttpMethod.ToLower();
            var controller = filePath.Replace(fileExtension, "").ToLower();
            var statusCode = !hasException ? GetStatusCode(actionContext).ToString() : DEFAULT_RESPONSE_STATUS_CODE.ToString();

            return new Dictionary<string, string>()
            {
                { "code", statusCode },
                { "controller", controller },
                { "action", action }
            };
        }

        private static int GetStatusCode(HttpContext actionExecutedContext)
        {
            return actionExecutedContext.Response.StatusCode;
        }

        public void Dispose() { }
    }
}
