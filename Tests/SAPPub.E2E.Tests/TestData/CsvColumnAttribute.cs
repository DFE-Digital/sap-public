namespace SAPPub.E2E.Tests.TestData;

/// <summary>
/// Used on a model property to specify the name of the source CSV header, when it differs from the property's JsonPropertyName.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class CsvColumnAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
