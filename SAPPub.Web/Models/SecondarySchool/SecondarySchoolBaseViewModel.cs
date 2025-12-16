using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class SecondarySchoolBaseViewModel
{
    public required int Urn { get; set; }

    public required string SchoolName { get; set; }

    public Dictionary<string, string> RouteAttributes =>
        new() { { RouteConstants.Urn, Urn.ToString() }, { RouteConstants.SchoolName, SchoolName } };
}
