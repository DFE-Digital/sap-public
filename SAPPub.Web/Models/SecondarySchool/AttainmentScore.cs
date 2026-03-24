using SAPPub.Web.Helpers;

namespace SAPPub.Web.Models.SecondarySchool;

public class AttainmentScore
{
    public required DisplayField<double> Score { get; init; }
    public required Func<double?, string?> DescriptionMap { get; init; }
    public string? Description => DescriptionMap(Score?.Value);
}

public static class AttainmentScoreMaps
{
    public static readonly Func<double?, string?> EstablishmentAttainment8 = score =>
    {
        if (!score.HasValue || score.Value < 0 || score.Value > 90)
            return null;

        if (score >= 90)
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
    };
}