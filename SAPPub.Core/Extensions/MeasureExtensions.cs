using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Extensions;

public static class MeasureExtensions
{
    public static string DisplayText(
        this Measure measure, 
        string notAvailableText = "Not available",
        int? decimalPlaces = null)
    {
        if (!measure.Value.HasValue)
        {
            return notAvailableText;
        }

        return measure.Unit == DataUnit.Percentage
            ? (decimalPlaces is int dp ? measure.Value.Value!.Value.AsPercentageRounded(dp) : measure.Value.Value!.Value.AsPercentage())
            : measure.Value.ToString();
    }
}
