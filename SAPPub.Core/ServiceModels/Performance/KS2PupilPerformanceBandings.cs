using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2PupilPerformanceBandings
{
    public required string Urn { get; init; }
    public CodedDouble EstablishmentReadingScore { get; init; }
    public CodedString? EstablishmentReadingProgressBanding { get; init; }

    public CodedDouble EstablishmentWritingScore { get; init; }
    public CodedString? EstablishmentWritingProgressBanding { get; init; }

    public CodedDouble EstablishmentMathsScore { get; init; }
    public CodedString? EstablishmentMathsProgressBanding { get; init; }

    public CodedString? EstablishmentReadingProgressSignificance { get; init; }
    public CodedString? EstablishmentWritingProgressSignificance { get; init; }
    public CodedString? EstablishmentMathsProgressSignificance { get; init; }


    public CodedDouble? LocalAuthorityReadingScore { get; init; }
    public CodedDouble? LocalAuthorityWritingScore { get; init; }
    public CodedDouble? LocalAuthorityMathsScore { get; init; }

    public CodedDouble? EnglandAverageReadingScore { get; init; }
    public CodedDouble? EnglandAverageWritingScore { get; init; }
    public CodedDouble? EnglandAverageMathsScore { get; init; }

}
