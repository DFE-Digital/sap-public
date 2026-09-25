namespace SAPPub.Integration.Tests.TestData.Models.KS4;

using System.Text.Json.Serialization;

public record AllStateFundedPupilsLaTestDataModel
{
    [JsonPropertyName("urn")]
    public string Urn { get; init; } = string.Empty;

    [JsonPropertyName("attainment8_average")]
    public string Attainment8Average { get; init; } = string.Empty;

    [JsonPropertyName("gcse_91_percent")]
    public string Gcse91Percent { get; init; } = string.Empty;

    [JsonPropertyName("engmath_94_percent")]
    public string EngMath94Percent { get; init; } = string.Empty;

    [JsonPropertyName("engmath_95_percent")]
    public string EngMath95Percent { get; init; } = string.Empty;

    [JsonPropertyName("lan_multiple_entering_percent")]
    public string LanMultipleEnteringPercent { get; init; } = string.Empty;

    [JsonPropertyName("sci_triple_entering_percent")]
    public string SciTripleEnteringPercent { get; init; } = string.Empty;

    [JsonPropertyName("qual_entries_average")]
    public string QualEntriesAverage { get; init; } = string.Empty;

    [JsonPropertyName("gcse_entries_average")]
    public string GcseEntriesAverage { get; init; } = string.Empty;

    [JsonPropertyName("pupil_count")]
    public string PupilCount { get; init; } = string.Empty;
}
