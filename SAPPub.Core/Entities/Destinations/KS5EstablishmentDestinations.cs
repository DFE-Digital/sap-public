using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.Destinations;

[ExcludeFromCodeCoverage]
public record KS5EstablishmentDestinations
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Total number of studens in establishment
    /// </summary>
    public CodedDouble TOT_OVERALLPER_Est_Current_Pct_Coded { get; set; }
    [IgnoreDataMember]
    public double? TOT_OVERALLPER_Est_Current_Pct { get; set; }
    [IgnoreDataMember]
    public string TOT_OVERALLPER_Est_Current_Pct_Reason { get; set; } = string.Empty;

    /// <summary>
    /// Establishment students progressing to either education, apprencticeships or employment
    /// </summary>
    public CodedDouble TOT_COHORT_Est_Current_Num_Coded { get; set; }
    [IgnoreDataMember]
    public double? TOT_COHORT_Est_Current_Num { get; set; }
    [IgnoreDataMember]
    public string TOT_COHORT_Est_Current_Num_Reason { get; set; } = string.Empty;
}
