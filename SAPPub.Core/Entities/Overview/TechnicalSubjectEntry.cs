using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Overview;

[ExcludeFromCodeCoverage]
public class TechnicalSubjectEntry
{
    public string URN { get; init; } = string.Empty;
    public string SubjectName { get; init; } = string.Empty;

    public double? PercentageEntering { get; init; }
}