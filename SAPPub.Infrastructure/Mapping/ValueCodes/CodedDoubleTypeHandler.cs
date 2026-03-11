using Dapper;
using SAPPub.Core.ValueObjects;
using System.Data;
using System.Globalization;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public sealed class CodedDoubleTypeHandler : SqlMapper.TypeHandler<CodedDouble>
{
    private readonly IReasonCodeLookup _lookup;

    public CodedDoubleTypeHandler(IReasonCodeLookup lookup) => _lookup = lookup;

    public override void SetValue(IDbDataParameter parameter, CodedDouble value)
        => parameter.Value = string.IsNullOrWhiteSpace(value.Raw) ? (object)DBNull.Value : value.Raw;

    public override CodedDouble Parse(object value)
    {
        if (value is null || value is DBNull) return CodedDouble.Empty;

        var raw = value.ToString()?.Trim() ?? "";
        if (raw.Length == 0) return CodedDouble.Empty;

        if (double.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
            return new CodedDouble(d, "", raw);

        var reason = _lookup.TryGet(raw, out var text) ? text : raw;
        return new CodedDouble(null, reason, raw);
    }
}

