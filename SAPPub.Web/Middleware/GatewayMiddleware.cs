using Microsoft.AspNetCore.Http;

namespace SAPPub.Web.Middleware
{
    public class GatewayMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<GatewayMiddleware> _logger;
        private List<string> _restrictedUrls = new List<string>
        {
            "/school",
            "/search"
        };

        public GatewayMiddleware(RequestDelegate next, ILogger<GatewayMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Path.StartsWithSegments("/gateway"))
            {
                // Custom logic for gateway requests
                await _next(httpContext);
            }
            else
            {
                foreach (var url in _restrictedUrls)
                {
                    if (httpContext.Request.Path.StartsWithSegments(url))
                    {
                        // Call the next middleware in the pipeline
                        if (httpContext.Request.Cookies.ContainsKey("GatewayCookie") && !string.IsNullOrWhiteSpace(httpContext.Request.Cookies["GatewayCookie"]))
                        {

                            var cookieIDVal = Guid.Parse(httpContext.Request.Cookies["GatewayCookie"]);
                        }

                        httpContext.Response.Redirect("/Gateway/");
                        return;
                    }
                }

            }

            await _next(httpContext);
        }



    }

    public static class GatewayMiddlewareExtensions
    {
        public static IApplicationBuilder UseGateway(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GatewayMiddleware>();
        }
    }
}
