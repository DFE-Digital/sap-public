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
}
