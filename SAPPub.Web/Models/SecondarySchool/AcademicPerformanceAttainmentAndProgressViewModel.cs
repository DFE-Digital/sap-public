using Microsoft.AspNetCore.Mvc.Rendering;
using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AcademicPerformanceAttainmentAndProgressViewModel : SecondarySchoolBaseViewModel
{
    private const AcademicYearSelection _currentAcademicYear = AcademicYearSelection.Current;

    public string? AcademicYearInfoParagraph => $"Information in this section is for the {SelectedAcademicYear.GetDisplayName()} academic year.";
    public AcademicYearSelection SelectedAcademicYear { get; set; } = _currentAcademicYear;

    public bool ShowProgress8NotAvailableInfo => SelectedAcademicYear == _currentAcademicYear;

    public bool ShowAttainment8Info => EstablishmentAttainment8Score.HasValue;

    public double? EstablishmentProgress8Score { get; init; }

    public double? LocalAuthorityProgress8Score { get; init; }

    public double? EstablishmentAttainment8Score { get; init; }
    public string? EstablishmentAttainment8ScoreContextDescription { get; init; }

    public double? LocalAuthorityAttainment8Score { get; init; }

    public double? EnglandAttainment8Score { get; init; }

    public double? EstablishmentProgress8TotalPupils { get; init; }

    public double? EstablishmentTotalPupils { get; init; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(AttainmentAndProgressModel attainmentAndProgressModel, AcademicYearSelection selectedAcademicYear)
    {
        var establishmentAttainment8Context = EstablishmentAttainment8ContextStatement(attainmentAndProgressModel.EstablishmentAttainment8Score);
        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            SelectedAcademicYear = selectedAcademicYear,
            EstablishmentProgress8Score = attainmentAndProgressModel.EstablishmentProgress8Score,
            LocalAuthorityProgress8Score = attainmentAndProgressModel.LocalAuthorityProgress8Score,
            EstablishmentAttainment8Score = attainmentAndProgressModel.EstablishmentAttainment8Score,
            EstablishmentAttainment8ScoreContextDescription = establishmentAttainment8Context != null
                ? $"This means that pupils generally scored the equivalent of {establishmentAttainment8Context} in their 8 best GCSE-level subjects"
                : null,
            LocalAuthorityAttainment8Score = attainmentAndProgressModel.LocalAuthorityAttainment8Score,
            EnglandAttainment8Score = attainmentAndProgressModel.EnglandAttainment8Score,
            EstablishmentProgress8TotalPupils = attainmentAndProgressModel.EstablishmentProgress8TotalPupils,
            EstablishmentTotalPupils = attainmentAndProgressModel.EstablishmentTotalPupils
        };
    }

    private static string? EstablishmentAttainment8ContextStatement(double? score)
    {
        if (!score.HasValue || score.Value < 0 || score.Value > 90)
            return null;

        if (score == 90)
            return "grade 9";

        if (score < 10)
            return "below a grade 1";

        int baseGrade = (int)(score.Value / 10);   // 10–19 → 1, 20–29 → 2, etc.
        float remainder = (float)Math.Round(score.Value % 10, 1);

        if (remainder <= 0.9f)
            return $"grade {baseGrade}";

        if (remainder <= 2.9f)
            return $"just above grade {baseGrade}";

        if (remainder <= 8.0f)
            return $"between grade {baseGrade} and grade {baseGrade + 1}";

        return $"just below grade {baseGrade + 1}";
    }
}
