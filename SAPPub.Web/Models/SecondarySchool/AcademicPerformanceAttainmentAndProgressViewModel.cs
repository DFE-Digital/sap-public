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
    public bool ShowProgress8Info => EstablishmentProgress8Score.HasValue;

    public double? EstablishmentProgress8Score { get; init; }

    public double? EstablishmentProgress8CILower { get; init; }

    public double? EstablishmentProgress8CIUpper { get; init; }

    public string? EstablishmentProgress8Banding { get; init; }

    public required DisplayField<string> EstablishmentProgress8BandingContextDescription { get; init; }

    public double? LocalAuthorityProgress8Score { get; init; }

    public double? EstablishmentAttainment8Score { get; init; }
    public required DisplayField<string> EstablishmentAttainment8ScoreContextDescription { get; init; }

    public double? LocalAuthorityAttainment8Score { get; init; }
    public required DisplayField<string> LocalAuthorityAttainment8ScoreContextDescription { get; init; }

    public double? EnglandAttainment8Score { get; init; }
    public required DisplayField<string> EnglandAttainment8ScoreContextDescription { get; init; }

    public double? EstablishmentProgress8TotalPupils { get; init; }

    public double? EstablishmentTotalPupils { get; init; }

    public List<SelectListItem> AcademicYearsSelectList => [.. Enum.GetValues(typeof(AcademicYearSelection)).Cast<AcademicYearSelection>().Select(x => new SelectListItem
    {
        Text = x.GetDisplayName(),
        Value = x.ToString(),
    })];

    public static AcademicPerformanceAttainmentAndProgressViewModel Map(AttainmentAndProgressModel attainmentAndProgressModel, AcademicYearSelection selectedAcademicYear)
    {
        var establishmentAttainment8ContextSentence = AttainmentHelper.EstablishmentAttainment8ContextStatement(attainmentAndProgressModel.EstablishmentAttainment8Score);
        var englandAttainment8ContextSentence = AttainmentHelper.NationalAttainment8ContextStatement(nationalScore: attainmentAndProgressModel.EnglandAttainment8Score, schoolScore: attainmentAndProgressModel.EstablishmentAttainment8Score);
        var localAuthorityAttainment8ContextSentence = AttainmentHelper.LocalAuthorityAttainment8ContextStatement(localAuthorityScore: attainmentAndProgressModel.LocalAuthorityAttainment8Score, schoolScore: attainmentAndProgressModel.EstablishmentAttainment8Score);
        var establishmentProgress8BandingContextDescription = AttainmentHelper.EstablishmentProgress8BandingContextStatement(attainmentAndProgressModel.EstablishmentProgress8Banding);

        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            SelectedAcademicYear = selectedAcademicYear,
            EstablishmentProgress8Score = attainmentAndProgressModel.EstablishmentProgress8Score,
            EstablishmentProgress8CILower = attainmentAndProgressModel.EstablishmentProgress8CILower,
            EstablishmentProgress8CIUpper = attainmentAndProgressModel.EstablishmentProgress8CIUpper,
            EstablishmentProgress8Banding = attainmentAndProgressModel.EstablishmentProgress8Banding,
            EstablishmentProgress8BandingContextDescription = establishmentProgress8BandingContextDescription,
            LocalAuthorityProgress8Score = attainmentAndProgressModel.LocalAuthorityProgress8Score,
            EstablishmentAttainment8Score = attainmentAndProgressModel.EstablishmentAttainment8Score,
            EstablishmentAttainment8ScoreContextDescription = establishmentAttainment8ContextSentence != null
                ? $"This means that pupils generally scored the equivalent of {establishmentAttainment8ContextSentence} in their 8 best GCSE-level subjects.".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
            LocalAuthorityAttainment8ScoreContextDescription = localAuthorityAttainment8ContextSentence != null
                ? $"{localAuthorityAttainment8ContextSentence}".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
            EnglandAttainment8ScoreContextDescription = englandAttainment8ContextSentence != null
                ? $"{englandAttainment8ContextSentence}".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
            LocalAuthorityAttainment8Score = attainmentAndProgressModel.LocalAuthorityAttainment8Score,
            EnglandAttainment8Score = attainmentAndProgressModel.EnglandAttainment8Score,
            EstablishmentProgress8TotalPupils = attainmentAndProgressModel.EstablishmentProgress8TotalPupils,
            EstablishmentTotalPupils = attainmentAndProgressModel.EstablishmentTotalPupils
        };
    }
}
