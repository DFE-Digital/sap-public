using SAPPub.Core.ServiceModels.Search;

namespace SAPPub.Web.Models.Search;

public class SearchResultViewModel
{
    public string URN { get; set; } = string.Empty;
    public string EstablishmentName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? ReligiousCharacter { get; set; }
    public string? GenderName { get; set; }

    public static SearchResultViewModel FromServiceModel(SchoolSearchResultServiceModel serviceModel)
    {
        return new SearchResultViewModel
        {
            URN = serviceModel.URN.ToString(),
            EstablishmentName = serviceModel.EstablishmentName,
            Address = serviceModel.Address,
            ReligiousCharacter = serviceModel.ReligiousCharacterName,
            GenderName = serviceModel.GenderName
        };
    }
}

public class SearchResultsViewModel
{
    public string? NameSearchTerm { get; set; }
    public int SearchResultsCount { get; set; }

    public List<SearchResultViewModel> SearchResults { get; set; } = new List<SearchResultViewModel>();

    public static List<SearchResultViewModel> FromServiceModel(IEnumerable<SchoolSearchResultsServiceModel> serviceModel)
    {
        return serviceModel.SelectMany(e => e.SchoolSearchResults.Select(r => SearchResultViewModel.FromServiceModel(r))).ToList();
    }
}
