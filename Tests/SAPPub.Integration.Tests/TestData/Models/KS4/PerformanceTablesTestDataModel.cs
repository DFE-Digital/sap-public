namespace SAPPub.Integration.Tests.TestData.Models.KS4;

using System.Text.Json.Serialization;

public sealed class PerformanceTablesTestDataModel
{
    [JsonPropertyName("school_urn")]
    public string SchoolUrn { get; set; } = string.Empty;

    [JsonPropertyName("progress8_lower_95_ci")]
    public string Progress8Lower95Ci { get; set; } = string.Empty;

    [JsonPropertyName("progress8_upper_95_ci")]
    public string Progress8Upper95Ci { get; set; } = string.Empty;

    [JsonPropertyName("progress8_average")]
    public string Progress8Average { get; set; } = string.Empty;

    [JsonPropertyName("attainment8_average")]
    public string Attainment8Average { get; set; } = string.Empty;

    [JsonPropertyName("attainment8ebacc_average")]
    public string Attainment8EbaccAverage { get; set; } = string.Empty;

    [JsonPropertyName("attainment8eng_average")]
    public string Attainment8EngAverage { get; set; } = string.Empty;

    [JsonPropertyName("attainment8mat_average")]
    public string Attainment8MatAverage { get; set; } = string.Empty;

    [JsonPropertyName("attainment8open_average")]
    public string Attainment8OpenAverage { get; set; } = string.Empty;

    [JsonPropertyName("attainment8open_gcse_average")]
    public string Attainment8OpenGcseAverage { get; set; } = string.Empty;

    [JsonPropertyName("attainment8open_nongcse_average")]
    public string Attainment8OpenNonGcseAverage { get; set; } = string.Empty;

    [JsonPropertyName("progress8_pupil_count")]
    public string Progress8PupilCount { get; set; } = string.Empty;

    [JsonPropertyName("gcse_91_percent")]
    public string Gcse91Percent { get; set; } = string.Empty;

    [JsonPropertyName("engmath_94_percent")]
    public string EngMath94Percent { get; set; } = string.Empty;

    [JsonPropertyName("engmath_95_percent")]
    public string EngMath95Percent { get; set; } = string.Empty;

    [JsonPropertyName("lan_multiple_entering_percent")]
    public string LanMultipleEnteringPercent { get; set; } = string.Empty;

    [JsonPropertyName("sci_triple_entering_percent")]
    public string SciTripleEnteringPercent { get; set; } = string.Empty;

    [JsonPropertyName("qual_entries_average")]
    public string QualEntriesAverage { get; set; } = string.Empty;

    [JsonPropertyName("gcse_entries_average")]
    public string GcseEntriesAverage { get; set; } = string.Empty;

    [JsonPropertyName("pupil_count")]
    public string PupilCount { get; set; } = string.Empty;
}
