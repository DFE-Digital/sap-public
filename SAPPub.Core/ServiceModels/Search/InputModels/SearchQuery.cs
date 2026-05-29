namespace SAPPub.Core.ServiceModels.Search.InputModels;

public record SearchQuery
{
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
    public int? Distance { get; init; }
    public string? Name { get; init; }
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 10;

}
