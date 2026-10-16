using Microsoft.Playwright;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SAPPub.E2E.Tests.TestData.Models.KS4;

public record KS4DestinationsTestDataModel
{
    [JsonPropertyName("school_urn")]
    public string SchoolUrn { get; init; } = string.Empty;

    [JsonPropertyName("old_la_code")]
    public string OldLaCode { get; init; } = string.Empty;

    [JsonPropertyName("time_period")]
    public string TimePeriod { get; init; } = string.Empty;

    [JsonPropertyName("disadvantage_status")]
    public string DisadvantageStatus { get; init; } = string.Empty;

    [JsonPropertyName("destination_group")]
    public string DestinationGroup { get; init; } = string.Empty;

    [JsonPropertyName("cohort_group")]
    public string DestinationDescription { get; init; } = string.Empty;

    [JsonPropertyName("cohort_count")]
    public string CohortCount { get; init; } = string.Empty;

    [JsonPropertyName("cohort_group")]
    public string CohortGroup { get; init; } = string.Empty;

    [JsonPropertyName("pupil_percent")]
    public string PupilPercent { get; init; } = string.Empty;


}

public static class Ks4DestinationsTestDataExtensions
{
    public const string CurrentYear = "202324";
    public const string PreviousYear = "202223";
    public const string Previous2Year = "202122";

    public static string CohortCount(
        this IEnumerable<KS4DestinationsTestDataModel> rows,
        string timePeriod = CurrentYear,
        string disadvantageStatus = "Total") =>
            rows.Single(static a => a.DestinationGroup == "Overall destination", timePeriod, disadvantageStatus, "cohort").CohortCount;

    private static KS4DestinationsTestDataModel Single(
        this IEnumerable<KS4DestinationsTestDataModel> rows,
        Func<KS4DestinationsTestDataModel, bool> predicate,
        string timePeriod,
        string disadvantageStatus,
        string description
        )
    {
        var matches = rows
            .Where(a => a.TimePeriod == timePeriod & a.DisadvantageStatus == disadvantageStatus)
            .Where(predicate)
            .ToList();
    
        return matches.Count == 1
            ? matches[0]
            : throw new InvalidOperationException($"Expected 1 destinations test data row for {description}");
    }
}