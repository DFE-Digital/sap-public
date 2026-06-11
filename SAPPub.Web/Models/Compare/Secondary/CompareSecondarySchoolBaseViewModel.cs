using SAPPub.Web.Constants;

namespace SAPPub.Web.Models.Compare.Secondary;

public class CompareSecondarySchoolBaseViewModel
{
    public required List<string> EstablishmentUrns { get; set; } = [];

    public string RouteQueryString => EstablishmentUrns.Count == 0 ? string.Empty : $"?{string.Join("&", EstablishmentUrns.Select(urn => $"{RouteConstants.URN}={urn}"))}";
}
