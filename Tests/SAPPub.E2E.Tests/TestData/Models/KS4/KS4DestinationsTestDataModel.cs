using System.Text.Json;
using System.Text.Json.Serialization;

namespace SAPPub.E2E.Tests.TestData.Models.KS4;

public record KS4DestinationsInputDataModel
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

    [JsonPropertyName("destination_description")]
    public string DestinationDescription { get; init; } = string.Empty;

    [JsonPropertyName("cohort_count")]
    public string CohortCount { get; init; } = string.Empty;

    [JsonPropertyName("pupil_percent")]
    public string PupilPercent { get; init; } = string.Empty;
}

public record KS4DestinationsTestDataModel
{
    [JsonPropertyName("periods")]
    public Dictionary<string, KS4DestinationsPeriodTestDataModel> Periods { get; init; } = [];
}

// Serialised as one object: "old_la_code" (when present) sits alongside the disadvantage status keys.
[JsonConverter(typeof(KS4DestinationsPeriodJsonConverter))]
public record KS4DestinationsPeriodTestDataModel
{
    public string? OldLaCode { get; init; }

    public Dictionary<string, KS4DestinationsGroupTestDataModel> DisadvantageStatuses { get; init; } = [];
}

public sealed class KS4DestinationsPeriodJsonConverter : JsonConverter<KS4DestinationsPeriodTestDataModel>
{
    private const string OldLaCodePropertyName = "old_la_code";

    public override KS4DestinationsPeriodTestDataModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        string? oldLaCode = null;
        var statuses = new Dictionary<string, KS4DestinationsGroupTestDataModel>(StringComparer.Ordinal);

        foreach (var property in document.RootElement.EnumerateObject())
        {
            if (property.NameEquals(OldLaCodePropertyName))
            {
                oldLaCode = property.Value.GetString();
            }
            else
            {
                statuses[property.Name] = property.Value.Deserialize<KS4DestinationsGroupTestDataModel>(options)!;
            }
        }

        return new KS4DestinationsPeriodTestDataModel
        {
            OldLaCode = oldLaCode,
            DisadvantageStatuses = statuses
        };
    }

    public override void Write(Utf8JsonWriter writer, KS4DestinationsPeriodTestDataModel value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        if (value.OldLaCode is not null)
        {
            writer.WriteString(OldLaCodePropertyName, value.OldLaCode);
        }

        foreach (var (status, group) in value.DisadvantageStatuses)
        {
            writer.WritePropertyName(status);
            JsonSerializer.Serialize(writer, group, options);
        }

        writer.WriteEndObject();
    }
}

public record KS4DestinationsGroupTestDataModel
{
    [JsonPropertyName("cohort_count")]
    public string CohortCount { get; init; } = string.Empty;

    [JsonPropertyName("destinations")]
    public List<KS4DestinationTestDataModel> Destinations { get; init; } = [];
}

public record KS4DestinationTestDataModel
{
    [JsonPropertyName("destination_group")]
    public string DestinationGroup { get; init; } = string.Empty;

    [JsonPropertyName("destination_description")]
    public string DestinationDescription { get; init; } = string.Empty;

    [JsonPropertyName("pupil_percent")]
    public string PupilPercent { get; init; } = string.Empty;
}

public static class Ks4DestinationsTestDataExtensions
{
    public const string CurrentYear = "202324";
    public const string PreviousYear = "202223";
    public const string Previous2Year = "202122";

    public static string AllDestinationsPercent(
        this KS4DestinationsTestDataModel data,
        string timePeriod = CurrentYear,
        string disadvantageStatus = "Total") =>
            data.Single(timePeriod, disadvantageStatus, a => a.DestinationGroup == "Overall destination", "Overall destination").PupilPercent;

    public static string DestinationPercent(
        this KS4DestinationsTestDataModel data,
        string destinationDescription,
        string timePeriod = CurrentYear,
        string disadvantageStatus = "Total") =>
            data.Single(timePeriod, disadvantageStatus, a => a.DestinationDescription == destinationDescription, destinationDescription).PupilPercent;

    public static string CohortCount(
        this KS4DestinationsTestDataModel data,
        string timePeriod = CurrentYear,
        string disadvantageStatus = "Total") =>
            data.Group(timePeriod, disadvantageStatus).CohortCount;

    private static KS4DestinationsGroupTestDataModel Group(
        this KS4DestinationsTestDataModel data,
        string timePeriod,
        string disadvantageStatus)
    {
        if (!data.Periods.TryGetValue(timePeriod, out var period)
            || !period.DisadvantageStatuses.TryGetValue(disadvantageStatus, out var group))
        {
            throw new InvalidOperationException($"No destinations test data for {timePeriod} / {disadvantageStatus}.");
        }

        return group;
    }

    private static KS4DestinationTestDataModel Single(
        this KS4DestinationsTestDataModel data,
        string timePeriod,
        string disadvantageStatus,
        Func<KS4DestinationTestDataModel, bool> predicate,
        string description)
    {
        var group = data.Group(timePeriod, disadvantageStatus);
        var matches = group.Destinations.Where(predicate).ToList();
        return matches.Count == 1
            ? matches[0]
            : throw new InvalidOperationException($"Expected 1 destinations test data row for {description}.");
    }
}