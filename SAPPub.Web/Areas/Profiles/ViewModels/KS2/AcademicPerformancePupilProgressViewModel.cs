using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformancePupilProgressViewModel : BaseViewModel
{
    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;

    public string? AcademicYearInfoParagraph => $"Information in this section is for the {SelectedAcademicYear.GetDisplayName()} academic year.";

    public AcademicYearSelection SelectedAcademicYear { get; set; } = _currentAcademicYear;

    public string? PrimarySchoolAccountabilityLinkUrl { get; set; }
    public bool PrimarySchoolAccountabilityLinkNewTab { get; set; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformancePupilProgressViewModel Map(
        EstablishmentServiceModel establishment, 
        AcademicYearSelection selectedAcademicYear,
        UrlLinksOptions urlLinksOptions)
    {
        return new AcademicPerformancePupilProgressViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            SelectedAcademicYear = selectedAcademicYear,
            PrimarySchoolAccountabilityLinkUrl = urlLinksOptions.PrimarySchoolAccountability.Url,
            PrimarySchoolAccountabilityLinkNewTab = urlLinksOptions.PrimarySchoolAccountability.NewTab
        };
    }
}