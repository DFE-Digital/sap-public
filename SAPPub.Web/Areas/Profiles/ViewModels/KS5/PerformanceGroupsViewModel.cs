using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class PerformanceGroupsViewModel
{
    public required PerformanceSummaryViewModel DisadvantagedStudents { get; init; }

    public required PerformanceSummaryViewModel NonDisadvantagedStudents { get; init; }

    public static PerformanceGroupsViewModel Map(
        PerformanceSummaryModel disadvantagedModel,
        PerformanceSummaryModel nonDisadvantatedModel)
    {
        return new PerformanceGroupsViewModel
        { 
            DisadvantagedStudents = PerformanceSummaryViewModel.Map(disadvantagedModel),
            NonDisadvantagedStudents = PerformanceSummaryViewModel.Map(nonDisadvantatedModel)
        };
    }
}
