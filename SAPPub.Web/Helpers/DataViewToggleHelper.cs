using Microsoft.Extensions.Primitives;

namespace SAPPub.Web.Helpers;

public static class DataViewToggleHelper
{
    public const string OverTimeView = "over-time";
    public const string CurrentView = "current";
    public static string QueryKey(string idPrefix) => $"{idPrefix}-view";
    public static string AnchorId(string idPrefix) => $"{idPrefix}-chart-table-toggle";
    public static bool IsOverTimeView(HttpRequest request, string idPrefix) =>
        string.Equals(request.Query[QueryKey(idPrefix)].ToString(), OverTimeView, StringComparison.OrdinalIgnoreCase);

    public static string ViewUrl(HttpRequest request, string idPrefix, bool overTime)
    {
        var key = QueryKey(idPrefix);

        var query = request.Query
            .Where(a => !string.Equals(a.Key, key, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (overTime)
        {
            query.Add(new KeyValuePair<string, StringValues>(key, OverTimeView));
        }

        return $"{request.PathBase}{request.Path}{QueryString.Create(query)}#{AnchorId(idPrefix)}";
    }
}