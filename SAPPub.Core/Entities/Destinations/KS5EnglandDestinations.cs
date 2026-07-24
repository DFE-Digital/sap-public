using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Entities.Destinations;

[ExcludeFromCodeCoverage]
public record KS5EnglandDestinations
{
    //
    // Student Destinations average for England - KS5
    //
    public CodedDouble TOT_OVERALLPER_Eng_Current_Pct_Coded { get; set; } = new();

    public double? TOT_OVERALLPER_Eng_Current_Pct { get; set; }

    public string? TOT_OVERALLPER_Eng_Current_Pct_Reason { get; set; }
}
