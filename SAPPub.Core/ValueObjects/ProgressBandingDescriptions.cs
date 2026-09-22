using SAPPub.Core.Enums;

namespace SAPPub.Core.ValueObjects;

/// <summary>
/// Holds the historical reasoning description for each Progress banding value,
/// so that the description matching a given establishment's current banding can be looked up.
/// Reused across KS2, KS4 and KS5 since the banding shape (5 bands) is the same for each
/// subject/cohort.
/// </summary>
public readonly record struct ProgressBandingDescriptions(
    CodedString WellAboveAverage,
    CodedString AboveAverage,
    CodedString Average,
    CodedString BelowAverage,
    CodedString WellBelowAverage)
{
    public static ProgressBandingDescriptions Empty => new(CodedString.Empty, CodedString.Empty, CodedString.Empty, CodedString.Empty, CodedString.Empty);

    public CodedString GetDescriptionFor(ProgressBanding? banding) => banding switch
    {
        ProgressBanding.WellAboveAverage => WellAboveAverage,
        ProgressBanding.AboveAverage => AboveAverage,
        ProgressBanding.Average => Average,
        ProgressBanding.BelowAverage => BelowAverage,
        ProgressBanding.WellBelowAverage => WellBelowAverage,
        _ => CodedString.Empty
    };
}
