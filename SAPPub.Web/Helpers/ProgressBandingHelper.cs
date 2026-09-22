using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Helpers;

public static class ProgressBandingHelper
{
    public static DisplayField<string> ToBandingString(this CodedString codedString)
    {
        return ToBandingString(codedString, ProgressBandingDescriptions.Empty);
    }

    public static DisplayField<string> ToBandingString(this CodedString codedString, ProgressBandingDescriptions bandingDescriptions)
    {
        if (!int.TryParse(codedString.Value, out int val) || !codedString.HasValue)
        {
            return DisplayField<string>.NotAvailable();
        }

        var bandingEnum = (ProgressBanding?)(ProgressBanding)val;

        return bandingEnum.ToBandingContextStatement(bandingDescriptions);
    }

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

        var statement = description.HasValue
            ? $"This is {bandingEnum.GetDisplayName()} because {description}."
            : $"This is {bandingEnum.GetDisplayName()}.";

        return statement.ToDisplayField();
    }
}