namespace SAPPub.Web.Helpers;
using SAPPub.Web.Constants;

public static class CommonHelper
{
    public static string GetLocalAuthorityDisplayName(string? laName) =>
        string.IsNullOrWhiteSpace(laName) || laName.Length > 19
        ? Constants.LocalCouncilAverage
        : $"{laName} {Constants.Average}";

    public static double? AddNullable(double? a, double? b)
    {
        return a.HasValue && b.HasValue ? a + b : a ?? b;
    }

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
}
