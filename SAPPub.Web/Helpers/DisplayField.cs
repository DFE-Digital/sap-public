using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Helpers;

public sealed class DisplayField<T>
{
    public FieldStatus Status { get; }
    public T? Value { get; }

    public bool IsAvailable => Status == FieldStatus.Available;

    public bool IsNotAvailable => Status == FieldStatus.NotAvailable;

    private DisplayField(T? value, FieldStatus status)
    {
        Value = value;
        Status = status;
    }

    public static DisplayField<T> Available(T value) => new(value, FieldStatus.Available);

    public static DisplayField<T> NotAvailable() => new(default, FieldStatus.NotAvailable);
    
    public string DisplayText(
        Func<T, string>? formatter = null,
        string notAvailableText = "Not available",
        bool displayReason = false)
    {
        return Status switch
        {
            FieldStatus.Available => FormatValue(Value, formatter, notAvailableText, displayReason),
            FieldStatus.NotAvailable => notAvailableText,
            _ => notAvailableText
        };
    }

    private static string FormatValue(
        T? value,
        Func<T, string>? formatter,
        string notAvailableText,
        bool displayReason)
    {
        return value switch
        {
            null => string.Empty,
            CodedDouble cd when !cd.HasValue => displayReason ? cd.Reason : notAvailableText,
            _ => formatter?.Invoke(value) ?? value?.ToString() ?? string.Empty
        };
    }
}
