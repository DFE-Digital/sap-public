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
}
