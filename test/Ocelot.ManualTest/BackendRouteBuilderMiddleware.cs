using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Ocelot.Configuration.File;
using Ocelot.Middleware;

namespace Ocelot.ManualTest
{
    public class BackendRouteBuilderMiddleware
    {
        static List<BackendRoute> routes = new List<BackendRoute>()
        {
            new BackendRoute
            {
                Host = "localhost",
                Port = 5000,
                Scheme = "http",
                BackendPathAndMethods = new List<BackendPathAndMethod>
                {
                    new BackendPathAndMethod{Method = HttpMethod.Get,Path = "/api/values",MapKey = "api1"},
                    new BackendPathAndMethod{Method = HttpMethod.Post,Path = "/api/values/pp",MapKey = "api2"},
                    new BackendPathAndMethod{Method = HttpMethod.Get,Path = "/api/values/get1",MapKey = "api3"},
                },
            },
        };

        public static async Task Invoke(DownstreamContext context, Func<Task> next)
        {
            var x_action = context.HttpContext.Request.Headers["x-action"];
            if (x_action != StringValues.Empty)
            {
                foreach (var backendRoute in routes)
                {
                    var pathinfo = backendRoute.BackendPathAndMethods.FirstOrDefault(x => x.MapKey == x_action.ToString());
                    if (pathinfo != null)
                    {
                        if (backendRoute.Port != null)
                        {
                            context.DownstreamRequest.Port = backendRoute.Port.Value;
                        }

                        if (!string.IsNullOrEmpty(backendRoute.Scheme))
                        {
                            context.DownstreamRequest.Scheme = backendRoute.Scheme;
                        }

                        if (!string.IsNullOrEmpty(backendRoute.Host))
                        {
                            context.DownstreamRequest.Host = backendRoute.Host;
                        }
                        context.DownstreamRequest.AbsolutePath = pathinfo.Path;
                        context.DownstreamRequest.Method = pathinfo.Method.ToString();
                    }
                }
            }
            await next.Invoke();
        }
    }
}
