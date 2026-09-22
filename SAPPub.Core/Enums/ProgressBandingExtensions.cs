namespace SAPPub.Core.Enums;

public static class ProgressBandingExtensions
{
    public static ProgressBanding? ToProgressBanding(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim().ToLowerInvariant();

        return normalized switch
        {
            "well above average" => ProgressBanding.WellAboveAverage,
            "above average" => ProgressBanding.AboveAverage,
            "average" => ProgressBanding.Average,
            "below average" => ProgressBanding.BelowAverage,
            "well below average" => ProgressBanding.WellBelowAverage,
            //blank and "SUPP" values map to null (not available), suppressed banding can be added at a later date if needed
            _ => null
        };
    }
}
