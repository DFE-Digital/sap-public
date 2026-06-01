using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers;

public class CookiesController(IEstablishmentComparisonService establishmentComparisonService) : Controller
{
    private readonly IEstablishmentComparisonService _esablishmentComparisonService = establishmentComparisonService;

    [HttpGet]
    public async Task<IActionResult> Preferences()
    {
        var model = new CookiesViewModel
        {
            ShowSuccessBanner = TempData.Get<bool>(CookiesConfirmation),
            IsEstablishmentComparisonFeatureEnabled = await _esablishmentComparisonService.IsFeatureEnabled()
        };
        return View(model);
    }

    [HttpPost]
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

        if (string.IsNullOrEmpty(returnUrl))
        {
            TempData.Set(CookiesConfirmation, true);
        }

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
        if (!HasGaCookies())
            return;

        var hostName = Request.Host.Host;
        var parts = hostName.Split('.');

        var possibleDomains = new List<string?> { null, hostName };

        for (int i = 0; i < parts.Length - 1; i++)
        {
            var domain = "." + string.Join(".", parts.Skip(i+1));
            possibleDomains.Add(domain);
        }

        foreach (var (cookie, domain) in Request.Cookies.Keys
            .Where(cookie => cookie.StartsWith("_ga", StringComparison.OrdinalIgnoreCase))
            .SelectMany(cookie => possibleDomains.Distinct().Select(domain => (cookie, domain))))
        {
            Response.Cookies.Delete(cookie, new CookieOptions 
            {
                Domain = domain,
                Path = "/",
                Secure = false
            });
        }
    }

    private bool HasGaCookies()
    {
        return Request.Cookies.Keys.Any(k => !string.IsNullOrWhiteSpace(k) && k.StartsWith("_ga", StringComparison.OrdinalIgnoreCase));
    }
}