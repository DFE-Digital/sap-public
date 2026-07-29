using SAPPub.Core.ValueObjects;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SAPPub.Core.Entities.Destinations;

[ExcludeFromCodeCoverage]
public record KS5LADestinations
{
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Local Authority students progressing to either education, apprencticeships or employment
    /// </summary>
    public CodedDouble TOT_OVERALLPER_LA_Current_Pct_Coded { get; set; }
   
    [IgnoreDataMember]
    public double? TOT_OVERALLPER_LA_Current_Pct { get; set; }
    
    [IgnoreDataMember]
    public string TOT_OVERALLPER_LA_Current_Pct_Reason { get; set; } = string.Empty;
}