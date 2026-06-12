using SAPPub.Web.Constants;

namespace SAPPub.Web.ViewComponents.VerticalNavigationCompareSecondary;

public class VerticalNavigationCompareSecondaryModel
{
    public required List<string> URNs { get; set; } = [];

    public string RouteQueryString => URNs.Count == 0 ? string.Empty : $"?{string.Join("&", URNs.Select(urn => $"{RouteConstants.URNs}={urn}"))}";

    public required string ActivePage { get; set; }
}
