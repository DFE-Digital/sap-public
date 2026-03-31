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
        var establishmentAttainment8ContextSentence = EstablishmentAttainment8ContextStatement(attainmentAndProgressModel.EstablishmentAttainment8Score);
        var englandAttainment8ContextSentence = NationalAttainment8ContextStatement(nationalScore: attainmentAndProgressModel.EnglandAttainment8Score, schoolScore: attainmentAndProgressModel.EstablishmentAttainment8Score);
        var localAuthorityAttainment8ContextSentence = LocalAuthorityAttainment8ContextStatement(localAuthorityScore: attainmentAndProgressModel.LocalAuthorityAttainment8Score, schoolScore: attainmentAndProgressModel.EstablishmentAttainment8Score);
        return new AcademicPerformanceAttainmentAndProgressViewModel
        {
            URN = attainmentAndProgressModel.Urn,
            SchoolName = attainmentAndProgressModel.SchoolName ?? string.Empty,
            SelectedAcademicYear = selectedAcademicYear,
            EstablishmentProgress8Score = attainmentAndProgressModel.EstablishmentProgress8Score,
            LocalAuthorityProgress8Score = attainmentAndProgressModel.LocalAuthorityProgress8Score,
            EstablishmentAttainment8Score = attainmentAndProgressModel.EstablishmentAttainment8Score,
            EstablishmentAttainment8ScoreContextDescription = establishmentAttainment8ContextSentence != null
                ? establishmentAttainment8ContextSentence.ToDisplayField()
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

    private static string? EstablishmentAttainment8ContextStatement(double? score)
    {
        string returnClause = null!;
        if (!score.HasValue || score.Value < 0 || score.Value > 90)
            return null;

        if (score == 90)
            returnClause = "grade 9";

        if (score < 10)
            returnClause = "below a grade 1";

        int baseGrade = (int)(score.Value / 10);   // 10–19 → 1, 20–29 → 2, etc.
        float remainder = (float)Math.Round(score.Value % 10, 1);

        if (remainder <= 0.9f)
            returnClause = $"grade {baseGrade}";

        if (remainder <= 2.9f)
            returnClause = $"just above grade {baseGrade}";

        if (remainder <= 8.0f)
            returnClause = $"between grade {baseGrade} and grade {baseGrade + 1}";

        returnClause = $"just below grade {baseGrade + 1}";

        return $"This means that pupils generally scored the equivalent of {returnClause} in their 8 best GCSE-level subjects.";
    }

    private static string? NationalAttainment8ContextStatement(double? nationalScore, double? schoolScore)
    {
        if (!nationalScore.HasValue || !schoolScore.HasValue)
            return null;

        var firstClause = SentenceFirstClause(nationalScore.Value, schoolScore.Value) ?? "Not available";

        var secondClause = SentenceSecondClause(nationalScore.Value, schoolScore.Value) ?? "Not available";

        return $"""
            It's {firstClause} the national average of {nationalScore.Value:0.0}, 
            meaning pupils are performing {secondClause} the national average.
        """;
    }

    private static string? LocalAuthorityAttainment8ContextStatement(double? localAuthorityScore, double? schoolScore)
    {
        if (!localAuthorityScore.HasValue || !schoolScore.HasValue)
            return null;

        var firstClause = SentenceFirstClause(localAuthorityScore.Value, schoolScore.Value) ?? "Not available";

        var secondClause = SentenceSecondClause(localAuthorityScore.Value, schoolScore.Value) ?? "Not available";

        return $"""
            An Attainment 8 score of {schoolScore:0.0} is {firstClause} the local council average of {localAuthorityScore.Value:0.0}. 
            This means pupils are performing {secondClause} pupils at other schools in the area.
        """;
    }

    private static string? SentenceFirstClause(double contextualScore, double schoolScore)
    {
        return (float)Math.Round(schoolScore - contextualScore, 1) switch
        {
            > 0.0f => $"{Math.Abs(schoolScore - contextualScore):0.0} points higher than",
            < 0.0f => $"{Math.Abs(schoolScore - contextualScore):0.0} points lower than",
            _ => "the same as"
        };
    }

    private static string? SentenceSecondClause(double contextualScore, double schoolScore)
    {
        return (float)Math.Round(schoolScore - contextualScore, 1) switch
        {
            > 2.9f => "above",
            > 0.9f => "just above",
            > -1.0f and <= 0.9f => "similar to",
            > -3.0f and <= -1.0f => "just below",
            < -2.9f => "below",
            _ => "Not available",
        };
    }
}
