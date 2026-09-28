using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Enums;

/// <summary>
/// Parses raw/numeric-coded banding values into <see cref="ProgressBanding"/> and resolves the
/// matching description. Display sentence building lives in <c>ProgressBandingHelper</c> (SAPPub.Web).
/// </summary>
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

    /// <summary>
    /// Resolves the description that applies to the given raw banding value.
    /// </summary>
    public static CodedString GetBandingDescription(this string? rawBanding, ProgressBandingDescriptions descriptions)
    {
        return descriptions.GetDescriptionFor(rawBanding.ToProgressBanding());
    }

    /// <summary>
    /// Parses a numeric-coded banding value (e.g. KS2 reading/writing/maths) into a <see cref="ProgressBanding"/>.
    /// </summary>
    public static ProgressBanding? ToProgressBanding(this CodedString codedBanding)
    {
        if (!codedBanding.HasValue || !int.TryParse(codedBanding.Value, out int val))
            return null;

        return (ProgressBanding)val;
    }

    /// <summary>
    /// Resolves the description that applies to a numeric-coded raw banding value.
    /// </summary>
    public static CodedString GetBandingDescription(this CodedString rawBanding, ProgressBandingDescriptions descriptions)
    {
        return descriptions.GetDescriptionFor(rawBanding.ToProgressBanding());
    }
}
