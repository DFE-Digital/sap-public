using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.Search;

public class SearchResult
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? GenderName { get; set; }

    public static SearchResult FromEstablishmentCoreEntity(Establishment establishment)
    {
        return new SearchResult
        {
            URN = establishment.URN,
            EstablishmentName = establishment.EstablishmentName,
            Address = establishment.Address,
            ReligiousCharacter = establishment.ReligiousCharacterName,
            GenderName = establishment.GenderName
        };
    }
}

public class SearchResultsViewModel
{
    public string? NameSearchTerm { get; set; }
    public int SearchResultsCount { get; set; }

    public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

    public static List<SearchResult> FromEstablishmentCoreEntity(IEnumerable<Establishment> establishments)
    {
        return establishments.Select(e => SearchResult.FromEstablishmentCoreEntity(e)).ToList();
    }
}
