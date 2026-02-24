namespace SAPPub.Core.Entities.SchoolSearch;

public record SearchQuery(
    float? Latitude,
    float? Longitude,
    string? Name);
