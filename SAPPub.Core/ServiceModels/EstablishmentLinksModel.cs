namespace SAPPub.Core.ServiceModels;

public class EstablishmentLinksModel
{
    public List<EstablishmentLinkModel>? PredecessorLinks { get; set; } = new();
    public List<EstablishmentLinkModel>? SuccessorLinks { get; set; } = new();
}
