namespace SAPPub.Core.ServiceModels;

public class EstablishmentLink
{
    public required string Urn { get; set; }
    public required string Name { get; set; }
}

public class EstablishmentLinks
{
    public List<EstablishmentLink>? PredecessorLinks { get; set; } = new();
    public List<EstablishmentLink>? SuccessorLinks { get; set; } = new();
}
