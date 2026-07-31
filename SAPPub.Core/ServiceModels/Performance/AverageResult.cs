using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class AverageResult
{
    public string? Grade { get; init; }

    public CodedDouble Points { get; init; }
}
