using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class SecondarySchoolBaseViewModel
{
    public required string URN { get; set; }

    public required string SchoolName { get; set; }

    public Dictionary<string, string> RouteAttributes =>
        new() { { RouteConstants.URN, URN }, { RouteConstants.SchoolName, SchoolName } };
}
