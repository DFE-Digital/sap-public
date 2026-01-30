namespace SAPPub.Core.Entities;

public record RelativeYearValues<T>
{
    public required T CurrentYear { get; init; }
    public required T PreviousYear { get; init; }
    public required T TwoYearsAgo { get; init; }
}
