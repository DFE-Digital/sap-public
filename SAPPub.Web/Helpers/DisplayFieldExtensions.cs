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
}
