using Dapper;
using SAPPub.Core.ValueObjects;
using System.Data;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public sealed class CodedStringTypeHandler : SqlMapper.TypeHandler<CodedString>
{
    private readonly IReasonCodeLookup _lookup;

    public CodedStringTypeHandler(IReasonCodeLookup lookup) => _lookup = lookup;

    public override void SetValue(IDbDataParameter parameter, CodedString value)
        => parameter.Value = string.IsNullOrWhiteSpace(value.Raw) ? DBNull.Value : value.Raw;

    public override CodedString Parse(object value)
    {
        if (value is null || value is DBNull) return CodedString.Empty;

        var raw = value.ToString()?.Trim() ?? string.Empty;
        if (raw.Length == 0) return CodedString.Empty;

        // If the raw value is a known reason code, return it as a reason.
        if (_lookup.TryGet(raw, out var reason))
            return new CodedString(null, reason, raw);

        // Otherwise treat it as an actual string value.
        return new CodedString(raw, string.Empty, raw);
    }
}

