using SAPPub.Core.Enums;

namespace SAPPub.Web.Helpers;

public class AttainmentHelper
{
    public static string? EstablishmentAttainment8ContextStatement(double? score)
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

    public static string? NationalAttainment8ContextStatement(double? nationalScore, double? schoolScore)
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

    public static string? LocalAuthorityAttainment8ContextStatement(double? localAuthorityScore, double? schoolScore)
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

    public static DisplayField<string> EstablishmentProgress8BandingContextStatement(string? progressBanding)
    {
        var bandingEnum = progressBanding.ToProgress8Banding();

        return bandingEnum != Progress8Banding.NotAvailable
            ? $"This is {bandingEnum.GetDisplayName()}.".ToDisplayField()
            : DisplayField<string>.NotAvailable();
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
