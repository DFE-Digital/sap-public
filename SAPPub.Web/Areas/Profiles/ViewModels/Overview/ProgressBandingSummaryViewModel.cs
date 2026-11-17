using SAPPub.Core.Enums;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Web.Areas.Profiles.ViewModels.Overview;

public class ProgressBandingSummaryViewModel
{
    public required string Id { get; init; }

    public required string Subject { get; init; }

    public ProgressBanding? Banding { get; init; }

    public required string BandingLabel { get; init; }

    public required string CssClass { get; init; }

    public required string Description { get; init; }

    public bool IsNotStatisticallySignificant { get; init; }

    public static ProgressBandingSummaryViewModel Map(
        string subject, 
        CodedString? codeBanding,
        CodedString? codedSignificance = null)
    {
        var banding = ParseBanding(codeBanding);

        return new ProgressBandingSummaryViewModel
        {
            Id = subject.ToLowerInvariant(),
            Subject = subject,
            Banding = banding,
            BandingLabel = GetLabel(banding),
            CssClass = GetCssClass(banding),
            Description = GetDescription(banding),
            IsNotStatisticallySignificant = IsNotSignificant(codedSignificance)
        };
    }

    private static bool IsNotSignificant(CodedString? codedSignificance)=>
        codedSignificance is { HasValue: true } significance && significance.Value!.Trim() == "0";

    private static ProgressBanding? ParseBanding(CodedString? codeBanding)
    {
        if (codeBanding is null)
        {
            return null;
        }

        var banding = codeBanding.Value.ToProgressBanding();

        return banding.HasValue && Enum.IsDefined(banding.Value) ? banding : null;
    }

    private static string GetLabel(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => "Well above average",
        ProgressBanding.AboveAverage => "Above average",
        ProgressBanding.Average => "Average",
        ProgressBanding.BelowAverage => "Below average",
        ProgressBanding.WellBelowAverage => "Well below average",
        _ => Constants.Constants.NotApplicable
    };


    private static string GetDescription(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => "This school's pupils made much more progress than pupils nationally.",
        ProgressBanding.AboveAverage => "This school's pupils made more progress than pupils nationally.",
        ProgressBanding.Average => "This school's pupils made similar progress to pupils nationally.",
        ProgressBanding.BelowAverage => "This school's pupils made less progress than pupils nationally.",
        ProgressBanding.WellBelowAverage => "This school's pupils made much less progress than pupils nationally.",
        _ => "This school's data is 'Not applicable'"
    };

    private static string GetCssClass(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => "govuk-tag--green",
        ProgressBanding.AboveAverage => "app-tag--above-average",
        ProgressBanding.Average => "govuk-tag--blue",
        ProgressBanding.BelowAverage => "govuk-tag--orange",
        ProgressBanding.WellBelowAverage => "govuk-tag--red",
        _ => "govuk-tag--grey"
    };
}