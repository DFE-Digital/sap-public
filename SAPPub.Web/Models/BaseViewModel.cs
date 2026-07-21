using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Helpers;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Models;

public class BaseViewModel
{
    public required string URN { get; set; }
    public required string SchoolName { get; set; }
    public required bool IsKS2 { get; set; }
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; }

    public string SchoolNameClean => TextHelpers.CleanForUrl(SchoolName);

    public Dictionary<string, string> RouteAttributes =>
        new() { { RouteConstants.URN, URN }, { RouteConstants.SchoolName, SchoolNameClean } };

    public static string GetPageTitle(Level3 level3, Level2 level2)
    {
        if (level3 != default)
        {
            switch (level3)
            {
                case Level3.ALevel:
                    return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAlevel;
                case Level3.Academic:
                    return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAcademic;
                case Level3.AppliedGeneral:
                    return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsAppliedGeneral;
                case Level3.TechLevel:
                    return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsTechLevel;
                case Level3.Apprenticeship:
                    return PageTitleConstants.KS5SchoolPageTitles.Level3QualificationsApprenticeship;
            }
        }
        if (level2 != default)
        {
            switch (level2)
            {
                case Level2.TechCert:
                    return PageTitleConstants.KS5SchoolPageTitles.Level2QualificationsTechCert;
                case Level2.Apprenticeship:
                    return PageTitleConstants.KS5SchoolPageTitles.Level2QualificationsApprenticeship;
            }
        }
        return string.Empty;
    }
}
