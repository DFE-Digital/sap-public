using SAPPub.Core.Entities;

namespace SAPPub.Web.Models.Search;

public class SearchResult
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? PupilSex { get; set; }
}

public class SearchResultsViewModel
{
    public string? NameSearchTerm { get; set; }
    public int SearchResultsCount { get; set; }

    public List<SearchResult> SearchResults { get; set; } = new List<SearchResult>();

    public static SearchResultsViewModel FromEstablishmentCoreEntity(Establishment establishment)
    {
        return new SearchResultsViewModel
        {
            URN = establishment.URN,
            EstablishmentName = establishment.EstablishmentName,
            Address = establishment.Address,
            ReligiousCharacter = establishment.ReligiousCharacterName,
            PupilSex = establishment.GenderName
        };
    }

    public static List<SearchResultsViewModel> FromEstablishmentCoreEntity(IEnumerable<Establishment> establishments)
    {
        var list = new List<SearchResultsViewModel>();
        foreach (var establishment in establishments)
        {
            list.Add(FromEstablishmentCoreEntity(establishment));
        }
        return list;
    }
}
