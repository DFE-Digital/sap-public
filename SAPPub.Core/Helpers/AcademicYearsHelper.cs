namespace SAPPub.Core.Helpers;

public static class AcademicYearsHelper
{
    public static bool IsWithinLastThreeAcademicYears(DateOnly openDate)
    {
        var today = DateTime.Today;
        var previousSept12 = (today.Month < 9 || (today.Month == 9 && today.Day < 12))
            ? new DateTime(today.Year - 1, 9, 12)
            : new DateTime(today.Year, 9, 12);

        var cutoffDate = previousSept12.AddYears(-3);
        return openDate.ToDateTime(TimeOnly.MinValue) >= cutoffDate;
    }
}
