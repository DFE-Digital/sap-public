using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Extensions;

public static class MeasureExtensions
{
    public static string DisplayText(this Measure measure, string notAvailableText = "Not available")
    {
        if (!measure.Value.HasValue)
        {
            return notAvailableText;
        }

        return measure.Unit == DataUnit.Percentage
            ? measure.Value.Value!.Value.AsPercentage()
            : measure.Value.ToString();
    }
}
