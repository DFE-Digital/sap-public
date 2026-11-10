using System.Text.Json.Serialization;

namespace SAPPub.E2E.Tests.TestData.Models.KS4;

public class SubjectEntriesInputDataModel
{
    [JsonPropertyName("school_urn")]
    public string SchoolUrn { get; set; } = string.Empty;

    [JsonPropertyName("qualification_type")]
    public string QualificationType { get; set; } = string.Empty;

    [JsonPropertyName("subject_discount_group")]
    public string SubjectDiscountGroup { get; set; } = string.Empty;

    [JsonPropertyName("grade")]
    public string Grade { get; set; } = string.Empty;

    [JsonPropertyName("number_achieving")]
    public string NumberAchieving { get; set; } = string.Empty;
}
