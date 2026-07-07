using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Areas.Compare.ViewModels.Secondary;

public class CompareAcademicPerformancePupilAttainmentViewModel : CompareSecondarySchoolBaseViewModel
{   
    public required IEnumerable<SchoolAttainmentDetailsViewModel> SchoolDetails { get; init; }

    public required double? EnglandPercentage { get; init; }

    public required DisplayField<string> EnglandAttainment8ScoreContextDescription { get; init; }

    public static CompareAcademicPerformancePupilAttainmentViewModel Map(
        List<string> urns,
        AttainmentAndProgressComparisonResultsModel attainmentAndProgressComparisionResultsModel,
        List<EstablishmentServiceModel> establishments)
    {
        var schoolDetails = attainmentAndProgressComparisionResultsModel.SchoolDetails
            .Join(
                establishments,
                d => d.Urn,
                e => e.URN,
                SchoolAttainmentDetailsViewModel.Map)
            .OrderBy(d => d.SchoolName)
            .ToList();

        var englandAttainment8ContextSentence = AttainmentHelper.EstablishmentAttainment8ContextStatement(attainmentAndProgressComparisionResultsModel.EnglandAverage);

        return new CompareAcademicPerformancePupilAttainmentViewModel
        {
            URNs = urns,
            SchoolDetails = schoolDetails,
            EnglandPercentage = attainmentAndProgressComparisionResultsModel.EnglandAverage,
            EnglandAttainment8ScoreContextDescription = englandAttainment8ContextSentence != null
                ? $"Pupils generally scored the equivalent of {englandAttainment8ContextSentence} in their 8 best GCSE-level subjects.".ToDisplayField()
                : DisplayField<string>.NotAvailable(),
        };
    }
}
