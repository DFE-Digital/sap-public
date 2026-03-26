using SAPPub.Core.Enums;

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

    public string DisplayText(Func<T, string>? formatter = null, string notAvailableText = "Not available")
    {
        return Status switch
        {
            FieldStatus.Available => formatter?.Invoke(Value!) ?? Value?.ToString() ?? string.Empty,
            FieldStatus.NotAvailable => notAvailableText,
            _ => notAvailableText
        };
    }
}
