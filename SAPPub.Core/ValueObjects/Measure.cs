using SAPPub.Core.Enums;

namespace SAPPub.Core.ValueObjects;

public record Measure
{
    public required CodedDouble Value { get; init; }

    public required DataUnit Unit { get; init; }
}
