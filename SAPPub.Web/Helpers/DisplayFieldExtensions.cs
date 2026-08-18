using SAPPub.Core.Extensions;
using SAPPub.Core.ValueObjects;
using System.Globalization;

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

    public static string DisplayNumber(
        this DisplayField<CodedDouble> prop,
        string? format = null,
        string notAvailableText = "Not available",
        bool displayReason = false)
    {
        return prop.DisplayText(d => format is null 
            ? d.Value!.Value.ToString() 
            : d.Value!.Value.ToString(format),
            notAvailableText, displayReason);
    }

    public static string DisplayPercentage(
        this DisplayField<CodedDouble> prop,
        string notAvailableText = "Not available",
        bool displayReason = false)
    {
        return prop.DisplayText(d => d.Value!.Value.AsPercentage(), notAvailableText, displayReason);        
    }

    public static DisplayField<CodedDouble> Round(
        this DisplayField<CodedDouble> prop,
        int decimalPlaces = 1)
    {
        if (!prop.IsAvailable)
        {
            return prop;
        }

        var originalValue = prop.Value!;

        if (originalValue.Value is double value)
        {
            var roundedValue = Math.Round(value, decimalPlaces);
            var rounded = new CodedDouble(roundedValue, originalValue.Reason, originalValue.Raw);
            return DisplayField<CodedDouble>.Available(rounded);
        }

        return prop;
    }
}
