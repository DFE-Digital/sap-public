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
    public required DisplayField<CodedDouble> LAGrammarAtExpectedStandard { get; init; }
    public required DisplayField<CodedDouble> LAGrammarAtHigherStandard { get; init; }
    public required DisplayField<CodedDouble> EnglandGrammarAtExpectedStandard { get; init; }
    public required DisplayField<CodedDouble> EnglandGrammarAtHigherStandard { get; init; }
    public required string LAName { get; set; }


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
            LAGrammarAtExpectedStandard = kS2AdditionalMeasuresModel.LAGrammarAtExpectedStandard.ToDisplayField(),
            LAGrammarAtHigherStandard = kS2AdditionalMeasuresModel.LAGrammarAtHigherStandard.ToDisplayField(),
            EnglandGrammarAtExpectedStandard = kS2AdditionalMeasuresModel.EnglandGrammarAtExpectedStandard.ToDisplayField(),
            EnglandGrammarAtHigherStandard = kS2AdditionalMeasuresModel.EnglandGrammarAtHigherStandard.ToDisplayField()
        };
    }
}