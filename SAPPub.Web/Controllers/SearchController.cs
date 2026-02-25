using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers;

public class SearchController(ISchoolSearchService schoolSearchService) : Controller
{
    public IActionResult Index()
    {
        return View(new SearchResultsViewModel());
    }

    public async Task<IActionResult> SearchResults(string? searchKeyWord = null, string? searchLocation = null)
    {
        var searchQuery = new SearchQuery() { Name = searchKeyWord, Location = searchLocation };
        if (searchKeyWord != null)
        {
            var searchResults = await schoolSearchService.SearchAsync(searchQuery);
            var viewResults = new SearchResultsViewModel()
            {
                NameSearchTerm = searchKeyWord,
                SearchResultsCount = searchResults.Count,
                SearchResults = SearchResultsViewModel.FromServiceModel(searchResults.SchoolSearchResults)
            };
            return View(viewResults);
        }
        else return View(new SearchResultsViewModel()
        {
            NameSearchTerm = searchKeyWord,
            LocationSearchTerm = searchLocation,
            SearchResultsCount = 0,
            SearchResults = new List<SearchResultViewModel>()
        });
    }
}
