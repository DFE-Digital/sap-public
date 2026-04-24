namespace SAPPub.Core.ServiceModels.Search.InputModels;

public record SchoolSearchServiceQuery
{
    public string? Name { get; init; }
    public string? Location { get; init; }
    public int? Distance { get; init; }
    public int? PageNumber { get; init; }
}