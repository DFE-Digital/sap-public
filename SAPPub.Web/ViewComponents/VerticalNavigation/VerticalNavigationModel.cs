using SAPPub.Core.Helpers;

namespace SAPPub.Web.ViewComponents.VerticalNavigation;

public class VerticalNavigationModel
{
    public required string ActivePage { get; set; }

    public required string URN { get; set; }

    public required string SchoolName { get; set; }

    public bool IsKS2 { get; set; }

    public bool IsKS4 { get; set; }

    public bool IsKS5 { get; set; }

    public bool IsExclusivelyKS5 => !IsKS2 && !IsKS4 && IsKS5;

    public string SchoolNameClean => TextHelpers.CleanForUrl(SchoolName);

}
