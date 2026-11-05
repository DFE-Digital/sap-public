using Newtonsoft.Json;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public sealed class CodedStringJsonConverter : JsonConverter
{
    private readonly IReasonCodeLookup _lookup;

    public CodedStringJsonConverter(IReasonCodeLookup lookup)
    {
        _lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
    }

    public override bool CanConvert(Type objectType) =>
        objectType == typeof(CodedString) || objectType == typeof(CodedString?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType is JsonToken.Null or JsonToken.Undefined)
            return objectType == typeof(CodedString?) ? null : CodedString.Empty;

        var raw = reader.Value?.ToString()?.Trim() ?? string.Empty;
        if (raw.Length == 0)
            return CodedString.Empty;

        if (_lookup.TryGet(raw, out var reason))
            return new CodedString(null, reason, raw);

        return new CodedString(raw, string.Empty, raw);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not CodedString codedString || string.IsNullOrEmpty(codedString.Raw))
            writer.WriteNull();
        else
            writer.WriteValue(codedString.Raw);
    }
}
