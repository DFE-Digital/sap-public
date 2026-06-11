using SAPPub.Web.Constants;

namespace SAPPub.Web.Models.Compare.Secondary;

public class CompareSecondarySchoolBaseViewModel
{
    public required List<string>? EstablishmentUrns { get; set; } = [];

    public Dictionary<string, List<string>> RouteAttributes => new() { { RouteConstants.URN, EstablishmentUrns! } };
}
