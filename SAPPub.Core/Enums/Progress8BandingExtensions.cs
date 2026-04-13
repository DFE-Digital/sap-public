namespace SAPPub.Core.Enums;

public static class Progress8BandingExtensions
{
    public static Progress8Banding ToProgress8Banding(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Progress8Banding.NotAvailable;
        var normalized = value.Trim().ToLowerInvariant();
        return normalized switch
        {
            "well above average" => Progress8Banding.WellAboveAverage,
            "above average" => Progress8Banding.AboveAverage,
            "average" => Progress8Banding.Average,
            "below average" => Progress8Banding.BelowAverage,
            "well below average" => Progress8Banding.WellBelowAverage,
            //blank and "SUPP" values map to Not Available, Suppressed banding can be added at a later date if needed
            _ => Progress8Banding.NotAvailable
        };
    }
}
