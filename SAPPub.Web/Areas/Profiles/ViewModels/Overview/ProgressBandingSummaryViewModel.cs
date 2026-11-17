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

    public static ProgressBandingSummaryViewModel Map(string subject, CodedString? codedProgressValue)
    {
        var banding = ParseBanding(codedProgressValue);

        return new ProgressBandingSummaryViewModel
        {
            Id = subject.ToLowerInvariant(),
            Subject = subject,
            Banding = banding,
            BandingLabel = GetLabel(banding),
            CssClass = GetCssClass(banding),
            Description = GetDescription(banding)
        };
    }

    private static ProgressBanding? ParseBanding(CodedString? codedProgressValue)
    {
        if (codedProgressValue is null)
        {
            return null;
        }

        var banding = codedProgressValue.Value.ToProgressBanding();

        return banding.HasValue && Enum.IsDefined(banding.Value) ? banding : null;
    }

    private static string GetLabel(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => "Well above average",
        ProgressBanding.AboveAverage => "Above average",
        ProgressBanding.Average => "Average",
        ProgressBanding.BelowAverage => "Below average",
        ProgressBanding.WellBelowAverage => "Well below average",
        _ => Constants.Constants.NotAvailable
    };


    private static string GetDescription(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => "This school's pupils made much more progress than pupils nationally",
        ProgressBanding.AboveAverage => "This school's pupils made more progress than pupils nationally",
        ProgressBanding.Average => "This school's pupils made similar progress to pupils nationally",
        ProgressBanding.BelowAverage => "This school's pupils made less progress than pupils nationally",
        ProgressBanding.WellBelowAverage => "This school's pupils made much less progress than pupils nationally",
        _ => Constants.Constants.NotAvailable
    };

    private static string GetCssClass(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => "govuk-tag--green",
        ProgressBanding.AboveAverage => "govuk-tag--light-green",
        ProgressBanding.Average => "govuk-tag--blue",
        ProgressBanding.BelowAverage => "govuk-tag--orange",
        ProgressBanding.WellBelowAverage => "govuk-tag--red",
        _ => Constants.Constants.NotAvailable
    };

}