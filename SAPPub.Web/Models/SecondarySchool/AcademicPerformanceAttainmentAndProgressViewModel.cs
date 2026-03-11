using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceAttainmentAndProgressViewModel : SecondarySchoolBaseViewModel
{
    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;

    public AcademicYearSelection SelectedAcademicYear { get; set; } = _currentAcademicYear;

    public bool ShowProgress8NotAvailableInfo => SelectedAcademicYear == _currentAcademicYear;

    public double? EstablishmentProgress8Score { get; init; }

    public double? LocalAuthorityProgress8Score { get; init; }

    public double? EstablishmentAttainment8Score { get; init; }

    public double? LocalAuthorityAttainment8Score { get; init; }

    public double? EnglandAttainment8Score { get; init; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = AcademicYearHelper.GetAcademicYearText(x),
        Value = x.ToString(),
    })];

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(AttainmentAndProgressModel attainmentAndProgressModel, AcademicYearSelection selectedAcademicYear)
    {
        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            SelectedAcademicYear = selectedAcademicYear,
            EstablishmentProgress8Score = attainmentAndProgressModel.EstablishmentProgress8Score,
            LocalAuthorityProgress8Score = attainmentAndProgressModel.LocalAuthorityProgress8Score,
            EstablishmentAttainment8Score = attainmentAndProgressModel.EstablishmentAttainment8Score,
            LocalAuthorityAttainment8Score = attainmentAndProgressModel.LocalAuthorityAttainment8Score,
            EnglandAttainment8Score = attainmentAndProgressModel.EnglandAttainment8Score
        };
    }
}
