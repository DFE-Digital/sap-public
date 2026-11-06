using System.Globalization;

namespace SAPPub.Core.Extensions;

public static class DoubleExtensions
{
    public static string AsPercentage(this double value)
    {
        return $"{value}%";
    }

    public static string? AsRounded(this double? value, int decimalPlaces = 1)
    {
        if (!value.HasValue)
        {
            return null;
        }

        return Math.Round(value.Value, decimalPlaces).ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture);
    }

    public static string AsPercentageRounded(this double value, int decimalPlaces = 1)
    {
        return Math.Round(value, decimalPlaces).ToString($"F{decimalPlaces}", CultureInfo.InvariantCulture) + "%";
    }
}
