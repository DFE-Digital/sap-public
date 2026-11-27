using System.Reflection;
using System.Text.Json.Serialization;
using SAPPub.E2E.Tests.TestData;

namespace SAPPub.E2E.TestDataGenerator;

/// <summary>
/// Works out which CSV header a model property reads from: the CsvColumn attribute if present,
/// otherwise the JsonPropertyName.
/// </summary>
internal static class CsvColumnResolver
{
    public static string GetColumnName(PropertyInfo property) =>
        property.GetCustomAttribute<CsvColumnAttribute>()?.Name
        ?? property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
        ?? throw new InvalidOperationException(
            $"{property.DeclaringType?.Name}.{property.Name} needs [CsvColumn] or [JsonPropertyName].");
}
