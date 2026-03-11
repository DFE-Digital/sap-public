namespace SAPPub.Core.ServiceModels.Search;

public record SchoolSearchResultsServiceModel
{
    public IList<SchoolSearchResultServiceModel> SchoolSearchResults { get; init; } = new List<SchoolSearchResultServiceModel>();
    public int Count { get; init; }
}

public record SchoolSearchResultServiceModel
{
    public string? URN { get; init; }
    public string? EstablishmentName { get; init; }
    public string? Address { get; init; }
    public string? GenderName { get; init; }
    public string? ReligiousCharacterName { get; init; }
}
