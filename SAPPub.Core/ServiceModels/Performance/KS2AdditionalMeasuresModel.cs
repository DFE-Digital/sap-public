using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class KS2AdditionalMeasuresModel
{
    public required CodedDouble EstablishmentGrammarAtExpectedStandard { get; init; }
    public required CodedDouble EstablishmentGrammarAtHigherStandard { get; init; }
    public required CodedDouble EstablishmentEHCPPopulation { get; init; }
    public required CodedDouble EstablishmentSENSupportPopulation { get; init; }

    public required CodedDouble LAGrammarAtExpectedStandard { get; init; }
    public required CodedDouble LAGrammarAtHigherStandard { get; init; }
    public required CodedDouble EnglandGrammarAtExpectedStandard { get; init; }
    public required CodedDouble EnglandGrammarAtHigherStandard { get; init; }
    public required CodedDouble EnglandEHCPPopulation { get; init; }
    public required CodedDouble EnglandSENSupportPopulation { get; init; }


    public required CodedDouble EstablishmentNumPupilsEndOfKS2 { get; init; }
    public required CodedDouble LANumPupilsEndOfKS2 { get; init; }
    public required CodedDouble EnglandNumPupilsEndOfKS2 { get; init; }

    public required CodedDouble EstablishmentNumGirlsEndOfKS2 { get; init; }
    public required CodedDouble EstablishmentNumBoysEndOfKS2 { get; init; }
    public required CodedDouble EstablishmentNumEALEndOfKS2 { get; init; }
    public required CodedDouble EstablishmentNumNonMobileEndOfKS2 { get; init; }

    public required CodedDouble EstablishmentNumDisadvantagedEndOfKS2 { get; init; }
    public required CodedDouble LANumDisadvantagedEndOfKS2 { get; init; }
    public required CodedDouble EnglandNumDisadvantagedEndOfKS2 { get; init; }

    public required CodedDouble LANumNonDisadvantagedEndOfKS2 { get; init; }
    public required CodedDouble EnglandNumNonDisadvantagedEndOfKS2 { get; init; }

    public required string? EstablishmentPupilTotal { get; init; }
    public required CodedDouble EnglandPupilTotal { get; init; }
}
