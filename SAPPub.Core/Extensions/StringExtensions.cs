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
}
