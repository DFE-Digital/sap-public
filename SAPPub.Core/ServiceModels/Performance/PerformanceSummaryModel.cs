namespace SAPPub.Core.ServiceModels.Performance;

public class PerformanceSummaryModel
{
    public PerformanceData? Establishment { get; set; }

    public required PerformanceData LocalAuthority { get; set; }

    public required PerformanceData England { get; set; }
}
