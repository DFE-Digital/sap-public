using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Controllers;

public class CookiesController : Controller
{
    public IActionResult Preferences()
    {
        return View();
    }

    public IActionResult CookieSettings(bool acceptAnalyticsCookies, string returnUrl)
    {
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMonths(12),
            IsEssential = true,
            Secure = true,
            HttpOnly = true
        };

        Response.Cookies.Append(
            "analytics_preference",
            acceptAnalyticsCookies ? "true" : "false",
            options
        );

        if(!acceptAnalyticsCookies)
        {
            RemoveGaCookies();
        }

        if (!Url.IsLocalUrl(returnUrl))
        {
            return RedirectToAction(nameof(Preferences));
        }

        return Redirect(returnUrl);
    }

    public IActionResult HideBanner(bool? hideBanner, string returnUrl)
    {
        var options = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMonths(12),
            IsEssential = true,
            Secure = true,
            HttpOnly = true
        };

        Response.Cookies.Append(
            "hide_banner",
            hideBanner.HasValue && hideBanner.Value ? "true" : "false",
            options
        );

        // Prevent open redirects
        if (!Url.IsLocalUrl(returnUrl))
        {
            returnUrl = "/";
        }

        return Redirect(returnUrl);
    }

    private void RemoveGaCookies()
    {       
        foreach( string cookie in Request.Cookies.Keys)
        {
            if (cookie.StartsWith("_ga", StringComparison.OrdinalIgnoreCase))
            {
                var host = Request.Host.Host;
                var domain = host.StartsWith(".") ? host : $".{host}";

                Response.Cookies.Delete(cookie);
                Response.Cookies.Delete(cookie, new CookieOptions { Path = "/" });
                Response.Cookies.Delete(cookie, new CookieOptions { Path = "/", Domain = host });
                Response.Cookies.Delete(cookie, new CookieOptions { Path = "/", Domain = domain });
            }
        }
    }
}