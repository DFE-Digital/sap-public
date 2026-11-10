namespace SAPPub.E2E.Tests.TestData.Models.KS4;

public class SubjectEntriesDataModel
{
    public string SchoolUrn { get; set; } = string.Empty;
    public List<(string, int)> ExpectedGcseSubjects { get; set; } = new();
    public List<(string, int)> ExpectedTechLevelSubjects { get; set; } = new();
    public List<(string, int)> ExpectedOtherSubjects { get; set; } = new();
}
