using SAPPub.Core.ValueObjects;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public class CodedLightweightParser
{
    private readonly IReasonCodeLookup _lookup;

    public CodedLightweightParser(IReasonCodeLookup lookup)
    {
        _lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
    }

    public CodedDouble ParseDouble(string? raw)
    {
        var (value, reason) = ValueCodeParser.Parse(raw, _lookup);
        return new CodedDouble(value, reason, raw?.Trim() ?? string.Empty);
    }

    public CodedString ParseString(string? raw)
    {
        var trimmed = raw?.Trim() ?? string.Empty;
        if (trimmed.Length == 0)
            return CodedString.Empty;

        if (_lookup.TryGet(trimmed, out var reason))
            return new CodedString(null, reason, trimmed);

        return new CodedString(trimmed, string.Empty, trimmed);
    }
}
