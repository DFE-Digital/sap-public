using Microsoft.AspNetCore.Http;
using SAPPub.Core.Interfaces.Services.Gateway;

namespace SAPPub.Web.Middleware
{
    public class GatewayMiddleware
    {
        private IGatewayUserService _userService;
        private readonly RequestDelegate _next;
        private ILogger<GatewayMiddleware> _logger;
        private IGatewaySettingsService _gatewaySettingsService;
        private readonly static List<string> _restrictedUrls = new List<string>
        {
            "/school",
            "/search"
        };
        private readonly static List<string> _unrestrictedUrls = new List<string>
        {
            "/gateway/closed",
            "/gateway/sessionended"
        };

        public GatewayMiddleware(RequestDelegate next, 
            ILogger<GatewayMiddleware> logger, 
            IGatewayUserService userService, 
            IGatewaySettingsService gatewaySettingsService)
        {
            _next = next;
            _logger = logger;
            _userService = userService;
            _gatewaySettingsService = gatewaySettingsService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (UrlInUnRestrictedList(httpContext.Request.Path))
            {
                await _next(httpContext);
                return;
            }

            var shouldGatewayBeLive = await _gatewaySettingsService.IsServiceLive();
            if (shouldGatewayBeLive == false)
            {
                httpContext.Response.Redirect("/gateway/closed");
                return;
            }

            var gatewayCookieValue = GetGatewayCookieValue(httpContext);

            // If user has cookie, and is logged in, let them go wherever they like (unless it's gateway, in which return to home)
            if (httpContext.Request.Path.StartsWithSegments("/gateway"))
            {
                if (Guid.TryParse(gatewayCookieValue, out Guid userId))
                {
                    var user = await _userService.GetById(userId);
                    if (user != null)
                    {
                        // User's time is up
                        if (await _userService.IsUserExpiredAsync(user.Id))
                        {
                            httpContext.Response.Redirect("/Gateway/SessionEnded");
                            return;
                        }

                        // If user has just registered, they still should be able to get to the gateway/complete page
                        if (httpContext.Request.Path.StartsWithSegments("/gateway/complete"))
                        {
                            await _next(httpContext);
                            return;
                        }

                        httpContext.Response.Redirect("/");
                        return;
                    }
                }
                // No cookie, no user, but entering gateway. Let through
                // OR
                // User has cookie, but isn't legit user
                await _next(httpContext);
                return;
            }
            else if (UrlInRestrictedList(httpContext.Request.Path))
            {
                if (Guid.TryParse(gatewayCookieValue, out Guid userId))
                {
                    var user = await _userService.GetById(userId);
                    if (user != null)
                    {
                        // User's time is up
                        if (await _userService.IsUserExpiredAsync(user.Id))
                        {
                            httpContext.Response.Redirect("/Gateway/SessionEnded");
                            return;
                        }

                        await _next(httpContext);
                        return;
                    }
                }
                httpContext.Response.Redirect("/Gateway/Error");
                return;
            }
            else
            {
                await _next(httpContext);
                return;
            }
        }


        public static string? GetGatewayCookieValue(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue("gateway", out var cookieValue))
            {
                return cookieValue;
            }
            return null;
        }

        public static bool UrlInRestrictedList(string url)
        {
            return _restrictedUrls.Any(restrictedUrl => url.StartsWith(restrictedUrl, StringComparison.OrdinalIgnoreCase));
        }

        public static bool UrlInUnRestrictedList(string url)
        {
            return _unrestrictedUrls.Any(unrestrictedUrl => url.StartsWith(unrestrictedUrl, StringComparison.OrdinalIgnoreCase));
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
