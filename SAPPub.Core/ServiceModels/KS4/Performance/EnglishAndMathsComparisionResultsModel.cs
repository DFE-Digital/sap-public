using SAPPub.Core.Entities;

namespace SAPPub.Core.ServiceModels.KS4.Performance;

public class EnglishAndMathsComparisionResultsModel
{
    public required List<EnglishAndMathsComparisionResultModel> Establishments { get; init; }

    public required RelativeYearValues<double?> EnglandAverage { get; init; }
}
