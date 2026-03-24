namespace SAPPub.Web.Helpers;

public static class UrlHelper
{
    public static string EnsureHttpsUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return url;

        return $"https://{url}";
    }
}