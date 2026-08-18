using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Helpers;

public static class ProgressBandingHelper
{
    public static DisplayField<string> ToBandingString(this CodedString codedString)
    {
        if (!int.TryParse(codedString.Value, out int val))
        {
            return DisplayField<string>.NotAvailable();    
        }

        ProgressBanding? bandingEnum = codedString.HasValue ? (ProgressBanding)val : null;

        return bandingEnum is null
            ? DisplayField<string>.NotAvailable()
            : $"This is {bandingEnum.GetDisplayName()}.".ToDisplayField();
    }
}