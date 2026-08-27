using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Helpers;
using SAPPub.Web.Areas.Profiles.ViewModels;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.Charts;

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
            }
        }
        if (level2 != default)
        {
            switch (level2)
            {
                case Level2.TechCert:
                    return PageTitleConstants.KS5SchoolPageTitles.Level2QualificationsTechCert;
            }
        }
        return string.Empty;
    }

    protected static DataOverTimeViewModel GetDataOverTimeViewModel(
        double? estPrevious2, double? estPrevious, double? estCurrent,
        double? laPrevious2, double? laPrevious, double? laCurrent,
        double? engPrevious2, double? engPrevious, double? engCurrent,
        string laAverageLabel)
    {
        return new DataOverTimeViewModel
        {
            Labels = ["2022 to 2023", "2023 to 2024", "2024 to 2025"], // TODO - Need academic year to calculate current, previous and TwoYearsAgo
            Datasets =
            [
                new DatasetViewModel { Label = "School", Data = [estPrevious2, estPrevious, estCurrent] },
                new DatasetViewModel { Label = laAverageLabel, Data = [laPrevious2, laPrevious, laCurrent] },
                new DatasetViewModel { Label = "England average", Data = [engPrevious2, engPrevious, engCurrent] }
            ],
        };
    }
}
