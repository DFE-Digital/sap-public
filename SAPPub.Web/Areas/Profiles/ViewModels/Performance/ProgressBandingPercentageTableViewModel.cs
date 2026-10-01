

using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Performance;

public class ProgressBandingPercentageTableViewModel
{
    public required string Summary { get; init; }

    public required string Description { get; init; }

    public required IReadOnlyList<ProgressBandingPercentageRowViewModel> Rows { get; init; }

    private const string SchoolsDescription = "This shows the percentage of schools in England which are in each banding for this measure.";

    private const string SchoolsAndCollegesDescription = "This shows the percentage of schools and colleges in England which are in each banding for this measure.";

    public static ProgressBandingPercentageTableViewModel Ks4Progress8(
IReadOnlyList<CodedDouble> percentages)
    {
        return Create(
        percentages,
        summary: "How many schools are in each banding?",
        description: SchoolsDescription);
    }

    public static ProgressBandingPercentageTableViewModel Ks2ProgressReading(
    IReadOnlyList<CodedDouble> percentages)
    => Ks2Progress(percentages, "Reading");

    public static ProgressBandingPercentageTableViewModel Ks2ProgressWriting(
    IReadOnlyList<CodedDouble> percentages)
    => Ks2Progress(percentages, "Writing");

    public static ProgressBandingPercentageTableViewModel Ks2ProgressMaths(
    IReadOnlyList<CodedDouble> percentages)
    => Ks2Progress(percentages, "Maths");

    private static ProgressBandingPercentageTableViewModel Ks2Progress(
    IReadOnlyList<CodedDouble> percentages,
    string subject)
    {
        return Create(
        percentages,
        summary: $"How many schools are in the {subject} banding?",
        description: SchoolsDescription);
    }

    public static ProgressBandingPercentageTableViewModel Ks5Progress(
IReadOnlyList<CodedDouble> percentages)
    {
        return Create(
        percentages,
        summary: "How many schools and colleges are in each banding?",
        description: SchoolsAndCollegesDescription);
    }

    public static ProgressBandingPercentageTableViewModel Create(
        IReadOnlyList<CodedDouble> percentages,
        string summary,
        string description)
    {
        return new ProgressBandingPercentageTableViewModel
        {
            Summary = summary,
            Description = description,
            Rows = [.. Enum.GetValues<ProgressBanding>()
                .Select((banding, index) => new ProgressBandingPercentageRowViewModel
                {
                    Banding = banding.GetDisplayName()!.ToSentenceCase(),
                    Percentage = percentages.Count > index
                ? percentages[index].ToDisplayField().DisplayPercentage()
                : "Not available"
                })]
        };
    }
}

public class ProgressBandingPercentageRowViewModel
{
    public required string Banding { get; init; }

    public required string Percentage { get; init; }
}