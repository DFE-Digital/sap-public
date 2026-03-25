namespace SAPPub.Core.Extensions;

public static class StringExtensions
{
    public static int? ToInt(this string? value)
    {
        return IsInt(value) ? int.Parse(value!) : null;
    }

    public static bool IsInt(this string? value)
    {
        return int.TryParse(value, out _);
    }

    public static DateOnly? ToDateOnly(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return DateOnly.TryParse(value, out var date) ? date : null;
    }
}
