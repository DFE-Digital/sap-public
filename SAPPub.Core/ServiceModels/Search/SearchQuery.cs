namespace SAPPub.Core.ServiceModels.Search;

public record SearchQuery
{
    public string? Name { get; init; }
    public string? Location { get; init; }
    public int? Distance { get; init; }
}