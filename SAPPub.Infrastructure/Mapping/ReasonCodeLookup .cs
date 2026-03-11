using System.Globalization;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public sealed class ReasonCodeLookup : IReasonCodeLookup
{
    private readonly Dictionary<string, string> _map;
    public ReasonCodeLookup(Dictionary<string, string> map) =>
        _map = new Dictionary<string, string>(map, StringComparer.OrdinalIgnoreCase);

    public bool TryGet(string code, out string reasonText) =>
        _map.TryGetValue(code, out reasonText!);
}

public static class ValueCodeParser
{
    public static (double? value, string reason) Parse(string? raw, IReasonCodeLookup lookup)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (null, string.Empty);

        raw = raw.Trim();

        if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
            return (d, string.Empty);

        return lookup.TryGet(raw, out var text)
            ? (null, text)
            : (null, raw); // unknown code: keep it visible
    }
}
