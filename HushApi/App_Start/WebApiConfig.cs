using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HushApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = System.Web.Http.Tracing.TraceLevel.Debug;

            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            
        }
    }
}
