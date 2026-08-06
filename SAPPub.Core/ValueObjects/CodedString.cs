namespace SAPPub.Core.ValueObjects;

public readonly record struct CodedString(string? Value, string Reason, string Raw)
{
    public bool HasValue => !string.IsNullOrWhiteSpace(Value);

    public static CodedString Empty => new(null, "", "");

    public override string ToString()
    {
        return Value ?? string.Empty;
    }
}