using SAPPub.Core.ServiceModels.Performance;

namespace SAPPub.Web.Areas.Profiles.ViewModels.KS5;

public class PerformanceSummaryViewModel
{
    public PerformanceDataViewModel? Establishment { get; init; }

    public required PerformanceDataViewModel LocalAuthority { get; init; }

    public required PerformanceDataViewModel England { get; init; }

    public static PerformanceSummaryViewModel Map(PerformanceSummaryModel model)
    {
        return new PerformanceSummaryViewModel
        {
            Establishment = model.Establishment != null ? PerformanceDataViewModel.Map(model.Establishment) : null,
            LocalAuthority = PerformanceDataViewModel.Map(model.LocalAuthority),
            England = PerformanceDataViewModel.Map(model.England)
        };
    }
}
