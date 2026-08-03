namespace SAPPub.Core.ValueObjects;

public readonly record struct CodedDouble(double? Value, string Reason, string Raw)
{
    public bool HasValue => Value.HasValue;
    public static CodedDouble Empty => new(null, "", "");

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }
}