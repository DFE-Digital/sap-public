using System.Globalization;

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

    public static DateOnly? ToDateOnly(this string? value, string format = "dd-MM-yyyy")
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return DateOnly.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
    }
}
