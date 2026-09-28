using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Helpers;

/// <summary>
/// Builds the display sentence for a resolved banding. Description resolution lives in
/// <see cref="ProgressBandingExtensions"/> (SAPPub.Core).
/// </summary>
public static class ProgressBandingHelper
{
    public static DisplayField<string> ToBandingContextStatement(this string? progressBanding, ProgressBandingDescriptions bandingDescriptions)
    {
        return progressBanding.ToProgressBanding().ToBandingContextStatement(bandingDescriptions);
    }

    public static DisplayField<string> ToBandingContextStatement(this ProgressBanding? bandingEnum, ProgressBandingDescriptions bandingDescriptions)
    {
        if (bandingEnum is null)
        {
            return DisplayField<string>.NotAvailable();
        }

        var description = bandingDescriptions.GetDescriptionFor(bandingEnum);

        return bandingEnum.ToBandingContextStatement(description);
    }

    /// <summary>
    /// Builds the statement from a raw text banding value and its already-resolved description.
    /// </summary>
    public static DisplayField<string> ToBandingContextStatement(this string? progressBanding, CodedString bandingContextDescription)
    {
        return progressBanding.ToProgressBanding().ToBandingContextStatement(bandingContextDescription);
    }

    /// <summary>
    /// Builds the statement from a numeric-coded banding value and its already-resolved description.
    /// </summary>
    public static DisplayField<string> ToBandingContextStatement(this CodedString progressBanding, CodedString bandingContextDescription)
    {
        return progressBanding.ToProgressBanding().ToBandingContextStatement(bandingContextDescription);
    }

    private static DisplayField<string> ToBandingContextStatement(this ProgressBanding? bandingEnum, CodedString description)
    {
        if (bandingEnum is null)
        {
            return DisplayField<string>.NotAvailable();
        }

        var statement = description.HasValue
            ? $"This is {bandingEnum.GetDisplayName()} because {description}."
            : $"This is {bandingEnum.GetDisplayName()}.";

        return statement.ToDisplayField();
    }
}