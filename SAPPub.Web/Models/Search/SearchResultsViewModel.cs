using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Web.Models.Search;

public class SearchResultsViewModel
{
    public string? NameSearchTerm { get; set; }
    public string? LocationSearchTerm { get; set; }
    public int Miles => 3;
    public int SearchResultsCount { get; set; }
    public string Heading => $"{SearchResultsCount} results {HeadingClause2}";

    public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

    private string? HeadingClause2 =>
        !String.IsNullOrEmpty(NameSearchTerm)
            ? $"for '{NameSearchTerm}'" + (!String.IsNullOrEmpty(LocationSearchTerm) ? $" within {Miles} miles of {LocationSearchTerm}" : String.Empty)
            : !String.IsNullOrEmpty(LocationSearchTerm)
                ? $"within {Miles} miles of {LocationSearchTerm}"
                : String.Empty;
    public static List<SearchResult> FromServiceModel(IEnumerable<SchoolSearchResultServiceModel> serviceModel)
    {
        return serviceModel.Select(r => SearchResult.FromServiceModel(r)).ToList();
    }
}

public class SearchResult
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? GenderName { get; set; }

    public static SearchResult FromServiceModel(SchoolSearchResultServiceModel serviceModel)
    {
        return new SearchResult
        {
            URN = serviceModel.URN?.ToString() ?? string.Empty,
            EstablishmentName = serviceModel.EstablishmentName ?? string.Empty,
            Address = serviceModel.Address ?? string.Empty,
            ReligiousCharacter = serviceModel.ReligiousCharacterName ?? string.Empty,
            GenderName = serviceModel.GenderName ?? string.Empty
        };
    }
}
