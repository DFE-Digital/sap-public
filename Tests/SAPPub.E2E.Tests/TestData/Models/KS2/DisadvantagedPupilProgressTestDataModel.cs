using System.Text.Json.Serialization;

namespace SAPPub.E2E.Tests.TestData.Models.KS2;

public record DisadvantagedPupilProgressTestDataModel
{
    [JsonPropertyName("school_urn")]
    public string SchoolUrn { get; init; } = string.Empty;

    [JsonPropertyName("reading_score")]
    public string ReadingScore { get; init; } = string.Empty;

    [JsonPropertyName("la_reading_score")]
    public string LaReadingScore { get; init; } = string.Empty;

    [JsonPropertyName("england_reading_score")]
    public string EnglandReadingScore { get; init; } = string.Empty;

    [JsonPropertyName("writing_score")]
    public string WritingScore { get; init; } = string.Empty;

    [JsonPropertyName("la_writing_score")]
    public string LaWritingScore { get; init; } = string.Empty;

    [JsonPropertyName("england_writing_score")]
    public string EnglandWritingScore { get; init; } = string.Empty;

    [JsonPropertyName("maths_score")]
    public string MathsScore { get; init; } = string.Empty;

    [JsonPropertyName("la_maths_score")]
    public string LaMathsScore { get; init; } = string.Empty;

    [JsonPropertyName("england_maths_score")]
    public string EnglandMathsScore { get; init; } = string.Empty;
}