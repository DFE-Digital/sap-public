using SAPPub.Core.Enums;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS2;

public class AcademicPerformanceAdditionalMeasuresViewModel : BaseViewModel
{
    public required DisplayField<CodedDouble> EstablishmentGrammarAtExpectedStandard { get; init; }
    public required DisplayField<CodedDouble> EstablishmentGrammarAtHigherStandard { get; init; }
    public required DisplayField<CodedDouble> EstablishmentEHCPPopulation { get; init; }
    public required DisplayField<CodedDouble> EstablishmentSENSupportPopulation { get; init; }
    public required DisplayField<CodedDouble> LAGrammarAtExpectedStandard { get; init; }
    public required DisplayField<CodedDouble> LAGrammarAtHigherStandard { get; init; }
    public required DisplayField<CodedDouble> EnglandEHCPPopulation { get; init; }
    public required DisplayField<CodedDouble> EnglandSENSupportPopulation { get; init; }
    public required DisplayField<CodedDouble> EnglandGrammarAtExpectedStandard { get; init; }
    public required DisplayField<CodedDouble> EnglandGrammarAtHigherStandard { get; init; }
    public required string LAName { get; set; }

    public required DisplayField<CodedDouble> EstablishmentNumPupilsEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> LANumPupilsEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> EnglandNumPupilsEndOfKS2 { get; init; }

    public required DisplayField<CodedDouble> EstablishmentNumGirlsEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> EstablishmentNumBoysEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> EstablishmentNumEALEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> EstablishmentNumNonMobileEndOfKS2 { get; init; }

    public required DisplayField<CodedDouble> EstablishmentNumDisadvantagedEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> LANumDisadvantagedEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> EnglandNumDisadvantagedEndOfKS2 { get; init; }

    public required DisplayField<CodedDouble> LANumNonDisadvantagedEndOfKS2 { get; init; }
    public required DisplayField<CodedDouble> EnglandNumNonDisadvantagedEndOfKS2 { get; init; }

    public required DisplayField<string> EstablishmentNumberOfPupils { get; set; }
    public required DisplayField<CodedDouble> EnglandNumberOfPupils { get; set; }

    public string? CurrentYear = AcademicYearSelection.Current.GetDisplayName();

    public static AcademicPerformanceAdditionalMeasuresViewModel Map(EstablishmentMinimumServiceModel establishment, KS2AdditionalMeasuresModel kS2AdditionalMeasuresModel)
    {
        return new AcademicPerformanceAdditionalMeasuresViewModel
        {
            URN = establishment.URN,
            SchoolName = establishment.EstablishmentName,
            IsKS2 = establishment.IsKS2,
            IsKS4 = establishment.IsKS4,
            IsKS5 = establishment.IsKS5,
            LAName = establishment.LAName,
            EstablishmentGrammarAtExpectedStandard = kS2AdditionalMeasuresModel.EstablishmentGrammarAtExpectedStandard.ToDisplayField(),
            EstablishmentGrammarAtHigherStandard = kS2AdditionalMeasuresModel.EstablishmentGrammarAtHigherStandard.ToDisplayField(),
            EstablishmentEHCPPopulation = kS2AdditionalMeasuresModel.EstablishmentEHCPPopulation.ToDisplayField(),
            EstablishmentSENSupportPopulation = kS2AdditionalMeasuresModel.EstablishmentSENSupportPopulation.ToDisplayField(),
            LAGrammarAtExpectedStandard = kS2AdditionalMeasuresModel.LAGrammarAtExpectedStandard.ToDisplayField(),
            LAGrammarAtHigherStandard = kS2AdditionalMeasuresModel.LAGrammarAtHigherStandard.ToDisplayField(),
            EnglandGrammarAtExpectedStandard = kS2AdditionalMeasuresModel.EnglandGrammarAtExpectedStandard.ToDisplayField(),
            EnglandGrammarAtHigherStandard = kS2AdditionalMeasuresModel.EnglandGrammarAtHigherStandard.ToDisplayField(),
            EnglandEHCPPopulation = kS2AdditionalMeasuresModel.EnglandEHCPPopulation.ToDisplayField().Round(2),
            EnglandSENSupportPopulation = kS2AdditionalMeasuresModel.EnglandSENSupportPopulation.ToDisplayField().Round(2),

            EstablishmentNumPupilsEndOfKS2 = kS2AdditionalMeasuresModel.EstablishmentNumPupilsEndOfKS2.ToDisplayField(),
            LANumPupilsEndOfKS2 = kS2AdditionalMeasuresModel.LANumPupilsEndOfKS2.ToDisplayField(),
            EnglandNumPupilsEndOfKS2 = kS2AdditionalMeasuresModel.EnglandNumPupilsEndOfKS2.ToDisplayField(),
            EstablishmentNumGirlsEndOfKS2 = kS2AdditionalMeasuresModel.EstablishmentNumGirlsEndOfKS2.ToDisplayField(),
            EstablishmentNumBoysEndOfKS2 = kS2AdditionalMeasuresModel.EstablishmentNumBoysEndOfKS2.ToDisplayField(),
            EstablishmentNumEALEndOfKS2 = kS2AdditionalMeasuresModel.EstablishmentNumEALEndOfKS2.ToDisplayField(),
            EstablishmentNumNonMobileEndOfKS2 = kS2AdditionalMeasuresModel.EstablishmentNumNonMobileEndOfKS2.ToDisplayField(),
            EstablishmentNumDisadvantagedEndOfKS2 = kS2AdditionalMeasuresModel.EstablishmentNumDisadvantagedEndOfKS2.ToDisplayField(),

            LANumDisadvantagedEndOfKS2 = kS2AdditionalMeasuresModel.LANumDisadvantagedEndOfKS2.ToDisplayField(),
            EnglandNumDisadvantagedEndOfKS2 = kS2AdditionalMeasuresModel.EnglandNumDisadvantagedEndOfKS2.ToDisplayField(),
            LANumNonDisadvantagedEndOfKS2 = kS2AdditionalMeasuresModel.LANumNonDisadvantagedEndOfKS2.ToDisplayField(),
            EnglandNumNonDisadvantagedEndOfKS2 = kS2AdditionalMeasuresModel.EnglandNumNonDisadvantagedEndOfKS2.ToDisplayField(),


            EstablishmentNumberOfPupils = kS2AdditionalMeasuresModel.EstablishmentPupilTotal.ToDisplayField(),
            EnglandNumberOfPupils = kS2AdditionalMeasuresModel.EnglandPupilTotal.ToDisplayField()
        };
    }
}