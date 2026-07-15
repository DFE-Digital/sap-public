using SAPPub.Core.Helpers;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.ViewModels;

public class ProfileBaseViewModel
{
    public required string URN { get; set; }

    public required string SchoolName { get; set; }

    public string SchoolNameClean => TextHelpers.CleanForUrl(SchoolName);

    public Dictionary<string, string> RouteAttributes =>
        new() { { RouteConstants.URN, URN }, { RouteConstants.SchoolName, SchoolNameClean } };

    public bool IsKS2 { get; set; }
    
    public bool IsKS4 { get; set; }

    public bool IsKS5 { get; set; }

    public bool IsExclusivelyKS5 => !IsKS2 && !IsKS4 && IsKS5;
}
