using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareSecondarySchoolBaseViewModel
{
    public required List<string> URNs { get; set; } = [];

    public required bool ListContainsSpecialSchool { get; set; }
    public required bool ListContainsNonSpecialSchool { get; set; }

    public string RouteQueryString => URNs.Count == 0 ? string.Empty : $"?{string.Join("&", URNs.Select(urn => $"{RouteConstants.URNs}={urn}"))}";
}
