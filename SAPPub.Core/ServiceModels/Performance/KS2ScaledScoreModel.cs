using SAPPub.Core.Entities;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2ScaledScoreModel
{
    public required string LAName { get; init; }
    public required RelativeYearValues<CodedDouble> ReadAverageEstablishment { get; init; }
    public required RelativeYearValues<CodedDouble> ReadAverageLA { get; init; }
    public required RelativeYearValues<CodedDouble> ReadAverageEngland { get; init; }
    public required RelativeYearValues<CodedDouble> MathsAverageEstablishment { get; init; }
    public required RelativeYearValues<CodedDouble> MathsAverageLA { get; init; }
    public required RelativeYearValues<CodedDouble> MathsAverageEngland { get; init; }

    /* Girls and boys breakdown */
    public required CodedDouble GirlsAverageReading { get; init; }
    public required CodedDouble GirlsAverageMaths { get; init; }
    public required CodedDouble BoysAverageReading { get; init; }
    public required CodedDouble BoysAverageMaths { get; init; }
    public required CodedDouble AllPupilsAverageReading { get; init; }
    public required CodedDouble AllPupilsAverageMaths { get; init; }

    /* English as an additional language */
    public required CodedDouble EALAverageReading { get; init; }
    public required CodedDouble EALAverageMaths { get; init; }
    public required CodedDouble EALTotalAverageReading { get; init; }
    public required CodedDouble EALTotalAverageMaths { get; init; }

    /* Non-mobile pupils */
    public required CodedDouble NonMobileAverageReading { get; init; }
    public required CodedDouble NonMobileAverageMaths { get; init; }


    /* Disadvantaged pupils */
    public required CodedDouble DisadvantagedAverageReadingEstablishment { get; init; }
    public required CodedDouble DisadvantagedAverageMathsEstablishment { get; init; }
    public required CodedDouble DisadvantagedAverageReadingLA { get; init; }
    public required CodedDouble DisadvantagedAverageMathsLA { get; init; }
    public required CodedDouble DisadvantagedAverageReadingEngland { get; init; }
    public required CodedDouble DisadvantagedAverageMathsEngland { get; init; }

    /* Non-disadvantaged pupils */
    public required CodedDouble NonDisadvantagedAverageReadingLA { get; init; }
    public required CodedDouble NonDisadvantagedAverageMathsLA { get; init; }
    public required CodedDouble NonDisadvantagedAverageReadingEngland { get; init; }
    public required CodedDouble NonDisadvantagedAverageMathsEngland { get; init; }
}
