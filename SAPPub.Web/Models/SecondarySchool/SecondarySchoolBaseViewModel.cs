using SAPPub.Core.Helpers;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Models.SecondarySchool;

public class SecondarySchoolBaseViewModel
{
    public required string URN { get; set; }

    public required string SchoolName { get; set; }

    public string SchoolNameClean => TextHelpers.CleanForUrl(SchoolName);

    public Dictionary<string, string> RouteAttributes =>
        new() { { RouteConstants.URN, URN }, { RouteConstants.SchoolName, SchoolNameClean } };
}
