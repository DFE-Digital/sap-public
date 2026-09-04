using SAPPub.Core.Enums;

namespace SAPPub.Core.Entities;

public record RelativeYearValues<T>
{
    public required T CurrentYear { get; init; }
    public T? PreviousYear { get; init; }
    public T? TwoYearsAgo { get; init; }
}

public static class RelativeYearValuesExtensions
{
    public static T? GetValueForYear<T>(
        this RelativeYearValues<T> values,
        AcademicYearSelection year)
    {
        return year switch
        {
            AcademicYearSelection.Current => values.CurrentYear,
            AcademicYearSelection.Previous => values.PreviousYear,
            AcademicYearSelection.Previous2 => values.TwoYearsAgo,
            _ => throw new ArgumentOutOfRangeException(nameof(year), year, null)
        };
    }
}
