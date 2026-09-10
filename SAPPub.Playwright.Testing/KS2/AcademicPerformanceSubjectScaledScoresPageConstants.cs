namespace SAPPub.Playwright.Testing.KS2.Performance.SubjectScaledScores;

public class PageConstants
{
    public static readonly IReadOnlyDictionary<string, string> ContentIds =
    new Dictionary<string, string>()
    {
        ["readShowDataOverTimeBtn"] = "#read-show-data-over-time-btn",
        ["readDataOverTimeShowAsTableBtn"] = "#read-data-over-time-show-btn",
        ["read-data-over-time-table"] = "read-data-overtime-table",
        ["mathsShowDataOverTimeBtn"] = "#maths-show-data-over-time-btn",
        ["mathsDataOverTimeShowAsTableBtn"] = "#maths-data-over-time-show-btn",
        ["maths-data-over-time-table"] = "maths-data-overtime-table",
        ["girls-boys-table"] = "#girls-boys-table",
        ["eal-table"] = "#eal-table",
        ["non-mobile-table"] = "#nonmobile-pupils-table",
        ["disadvantaged-pupils-table"] = "#disadvantaged-pupils-table"
    };
}