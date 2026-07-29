using SAPPub.Core.Extensions;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Helpers;

public static class DisplayFieldExtensions
{
    public static DisplayField<string> ToDisplayField(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DisplayField<string>.NotAvailable();
        }

        return DisplayField<string>.Available(value.Trim());
    }

    public static DisplayField<T> ToDisplayField<T>(this T? value)
    {
        if (value == null || value.Equals(default(T)))
        {
            return DisplayField<T>.NotAvailable();
        }

        return DisplayField<T>.Available(value);
    }

    public static DisplayField<T> ToDisplayField<T>(this T? value)
        where T : struct
    {
        if (value == null || value.Equals(default(T)))
        {
            return DisplayField<T>.NotAvailable();
        }

        return DisplayField<T>.Available(value.Value);
    }

    public static string DisplayNumber(this DisplayField<CodedDouble> prop, string? format = null, bool displayReason = false)
    {
        return prop.DisplayText(d => d.HasValue
            ? (format is null ? d.Value!.Value.ToString() : d.Value!.Value.ToString(format))
            : displayReason ? d.Reason : Constants.Constants.NotAvailable,
            Constants.Constants.NotAvailable);
    }

    public static string DisplayPercentage(this DisplayField<CodedDouble> prop, bool displayReason = false)
    {
        return prop.DisplayText(d => d.HasValue
            ? d.Value!.Value.AsPercentage()
            : displayReason ? d.Reason : Constants.Constants.NotAvailable,
            Constants.Constants.NotAvailable);
    }
}
