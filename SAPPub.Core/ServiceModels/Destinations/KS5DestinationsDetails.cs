using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.ServiceModels.Destinations;

[ExcludeFromCodeCoverage]
public record KS5DestinationsDetails
{
    public required string Urn { get; init; }

    public required string SchoolName { get; init; }

    public required bool IsKS2 { get; set; }
    
    public required bool IsKS4 { get; set; }
    
    public required bool IsKS5 { get; set; }

    public required string LocalAuthorityName { get; init; }

    public double? EstablishmentTotalCohortFor { get; set; }

    public double? EstablishmentTotalOverall { get; set; }

    public double? LATotalOverall { get; set; }
    
    public double? EnglandOverall { get; set; }
}
