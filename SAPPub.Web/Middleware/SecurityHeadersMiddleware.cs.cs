using SAPPub.Web.Helpers;

namespace SAPPub.Web.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate a per-request nonce and make it available to views
            var nonce = CSPHelper.RandomCharacters;
            context.Items["ScriptNonce"] = nonce;

            var env = context.RequestServices.GetService(typeof(IHostEnvironment)) as IHostEnvironment;

            // Build CSP using the existing concatenation style
            // Additions:
            //  - img-src allows OpenStreetMap tiles
            //  - style-src allows unpkg (Leaflet CSS)
            //  - script-src retains nonce and allows unpkg
            //  - connect-src in Development allows localhost & websockets for Browser Link
            var csp =
                  "default-src 'self';"
                + "base-uri 'self';"
                + "frame-ancestors 'self';"
                + "img-src 'self' data: https://*.tile.openstreetmap.org https://www.googletagmanager.com;"
                + "style-src 'self' 'unsafe-inline' https://unpkg.com;"
                + "font-src 'self' data:;"
                + $"script-src 'self' 'nonce-{nonce}' https://unpkg.com https://*.googletagmanager.com https://*.clarity.ms;"
                + "connect-src 'self' "
                    + "*.google-analytics.com "
                    + "https://*.googletagmanager.com "
                    + "*.analytics.google.com "
                    //+ "https://www.compare-school-performance.service.gov.uk "
                    + "https://api.postcodes.io "
                    + "https://*.doubleclick.net "
                    + "https://*.clarity.ms "
                    + "https://c.bing.com "
                    + "https://*.applicationinsights.azure.com/ "
                    + "https://*.visualstudio.com/;";

            // In Development, allow Browser Link / local dev tools over HTTP/HTTPS and WS/WSS
            if (env?.IsDevelopment() == true)
            {
                csp =
                      "default-src 'self';"
                    + "base-uri 'self';"
                    + "frame-ancestors 'self';"
                    + "img-src 'self' data: https://*.tile.openstreetmap.org https://www.googletagmanager.com;"
                    + "style-src 'self' 'unsafe-inline' https://unpkg.com;"
                    + "font-src 'self' data:;"
                    + $"script-src 'self' 'nonce-{nonce}' https://unpkg.com https://*.googletagmanager.com https://*.clarity.ms;"
                    + "connect-src 'self' "
                        + "*.google-analytics.com "
                        + "https://*.googletagmanager.com "
                        + "*.analytics.google.com "
                        //+ "https://www.compare-school-performance.service.gov.uk "
                        + "https://api.postcodes.io "
                        + "https://*.doubleclick.net "
                        + "https://*.clarity.ms "
                        + "https://c.bing.com "
                        + "https://*.applicationinsights.azure.com/ "
                        + "https://*.visualstudio.com/ "
                        + "http://localhost:* https://localhost:* ws://localhost:* wss://localhost:*;";
            }

            // Apply headers
            context.Response.Headers["Content-Security-Policy"] = csp;
            context.Response.Headers["Referrer-Policy"] = "no-referrer";
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";

            await _next(context);
        }
    }

    public static class SecurityHeadersMiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}
