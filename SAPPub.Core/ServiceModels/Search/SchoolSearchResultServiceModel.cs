namespace SAPPub.Core.ServiceModels.Search;

public record SchoolSearchResultsServiceModel
{
    public IList<SchoolSearchResultServiceModel> SchoolSearchResults { get; init; } = new List<SchoolSearchResultServiceModel>();
    public int Count { get; init; }
}

public record SchoolSearchResultServiceModel
{
    public string URN { get; init; }
    public string EstablishmentName { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string GenderName { get; init; } = string.Empty;
    public string ReligiousCharacterName { get; init; } = string.Empty;
}
