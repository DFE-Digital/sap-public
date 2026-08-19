namespace SAPPub.Core.ServiceModels.Performance;

public class AverageResultModel
{
    public required PerformanceResult Establishment { get; init; }

    public required PerformanceResult LocalAuthority { get; init; }

    public required PerformanceResult England { get; init; }
}
