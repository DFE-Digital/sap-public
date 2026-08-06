using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.ServiceModels.Performance;

public class AverageResult
{
    public CodedString Grade { get; init; }

    public CodedDouble Points { get; init; }
}
