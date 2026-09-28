using System.Text.Json.Serialization;

namespace SAPPub.Integration.Tests.TestData.Models.KS4;

public class InformationAboutSchoolsTestDataModel
{
    [JsonPropertyName("school_urn")]
    public string SchoolUrn { get; set; } = string.Empty;

    [JsonPropertyName("progress8_banding")]
    public string Progress8Banding { get; set; } = string.Empty;

    [JsonPropertyName("attainment8_diffn")]
    public string Attainment8Diffn { get; set; } = string.Empty;

    [JsonPropertyName("sen_with_ehcp_pupil_percent")]
    public string SenWithEhcpPupilPercent { get; set; } = string.Empty;

    [JsonPropertyName("sen_no_ehcp_pupil_percent")]
    public string SenNoEhcpPupilPercent { get; set; } = string.Empty;
}
