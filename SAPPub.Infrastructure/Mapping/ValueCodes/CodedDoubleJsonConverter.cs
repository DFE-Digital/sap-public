using Newtonsoft.Json;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Infrastructure.Mapping.ValueCodes;

public sealed class CodedDoubleJsonConverter : JsonConverter
{
    private readonly IReasonCodeLookup _lookup;

    public CodedDoubleJsonConverter(IReasonCodeLookup lookup)
    {
        _lookup = lookup ?? throw new ArgumentNullException(nameof(lookup));
    }

    public override bool CanConvert(Type objectType) =>
        objectType == typeof(CodedDouble) || objectType == typeof(CodedDouble?);

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType is JsonToken.Null or JsonToken.Undefined)
            return objectType == typeof(CodedDouble?) ? null : CodedDouble.Empty;

        var raw = reader.Value?.ToString();
        var (value, reason) = ValueCodeParser.Parse(raw, _lookup);
        return new CodedDouble(value, reason, raw?.Trim() ?? string.Empty);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is not CodedDouble codedDouble || string.IsNullOrEmpty(codedDouble.Raw))
            writer.WriteNull();
        else
            writer.WriteValue(codedDouble.Raw);
    }
}