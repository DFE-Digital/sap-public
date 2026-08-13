namespace SAPPub.Core.ServiceModels.Performance;

public class KS2PupilPerformance
{
    public required string Urn { get; init; }

    public required bool IsKS2 { get; set; }
    public required bool IsKS4 { get; set; }
    public required bool IsKS5 { get; set; }

    public string? SchoolName { get; init; }

    //public Code? EstablishmentReadingScore { get; init; }


}
