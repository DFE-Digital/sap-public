using SAPPub.Core.Helpers;

namespace SAPPub.Web.ViewComponents.VerticalNavigation;

public class VerticalNavigationModel
{
    public required string ActivePage { get; set; }

    public required string URN { get; set; }

    public required string SchoolName { get; set; }

    public required bool IsKS2 { get; set; } = false;
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; } = false;

    public string SchoolNameClean => TextHelpers.CleanForUrl(SchoolName);

}
