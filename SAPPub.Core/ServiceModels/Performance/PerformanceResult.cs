using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class PerformanceResult
{
    public CodedString Grade { get; init; }

    public CodedDouble Points { get; init; }
}
