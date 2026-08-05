namespace SAPPub.Core.ServiceModels.Performance;

public class AverageResultModel
{
    public required AverageResult Establishment { get; init; }

    public required AverageResult LocalAuthority { get; init; }

    public required AverageResult England { get; init; }
}
