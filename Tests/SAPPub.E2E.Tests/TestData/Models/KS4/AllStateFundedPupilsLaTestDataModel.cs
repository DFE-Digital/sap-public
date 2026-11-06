namespace SAPPub.E2E.Tests.TestData.Models.KS4;

using System.Text.Json.Serialization;

public record AllStateFundedPupilsLaTestDataModel : AllStateFundedPupilsTestDataModel
{
    [JsonPropertyName("urn")]
    public string Urn { get; init; } = string.Empty;

    [JsonPropertyName("progress8_average")]
    public string Progress8Average { get; init; } = string.Empty;
}
