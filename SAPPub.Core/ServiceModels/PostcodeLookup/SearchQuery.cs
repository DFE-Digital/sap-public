namespace SAPPub.Core.ServiceModels.PostcodeLookup;

public record SearchQuery
{
    public float? Latitude { get; init; }
    public float? Longitude { get; init; }
    public int? Distance { get; init; }
    public string? Name { get; init; }
}
