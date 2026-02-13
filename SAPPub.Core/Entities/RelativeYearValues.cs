namespace SAPPub.Core.Entities;

public record RelativeYearValues<T>
{
    public required T CurrentYear { get; init; }
    public T? PreviousYear { get; init; }
    public T? TwoYearsAgo { get; init; }
}
