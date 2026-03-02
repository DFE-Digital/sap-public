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

    [HttpPost]
    public IActionResult Index(SearchResultsViewModel model)
    {
        var searchKeyWord = model.NameSearchTerm;
        var searchLocation = model.LocationSearchTerm;
        if (ModelState.IsValid)
        {
            return RedirectToAction("SearchResults", model);
        }
        else
        {
            return View(model);
        }
    }

    public async Task<IActionResult> SearchResults(SearchResultsViewModel model)
    {
        var searchKeyWord = model.NameSearchTerm;
        var searchLocation = model.LocationSearchTerm;
        var searchQuery = new SearchQuery() { Name = searchKeyWord, Location = searchLocation };
        if (searchKeyWord != null || searchLocation != null)
        {
            var searchResults = await schoolSearchService.SearchAsync(searchQuery);
            if (searchResults.Status == SchoolSearchStatus.InvalidPostcode)
            {
                // postcode may be a valid format but nor be found by the postcode API
                ModelState.AddModelError(nameof(SearchResultsViewModel.LocationSearchTerm), "Enter a valid postcode");
            }
            var viewResults = new SearchResultsViewModel()
            {
                NameSearchTerm = searchKeyWord,
                LocationSearchTerm = searchLocation,
                SearchResultsCount = searchResults.Count,
                SearchResults = SearchResultsViewModel.FromServiceModel(searchResults.SchoolSearchResults.OrderBy(r => r.Distance))
            };
            return View(viewResults);
        }
        else return View(new SearchResultsViewModel()
        {
            NameSearchTerm = searchKeyWord,
            LocationSearchTerm = searchLocation,
            SearchResultsCount = 0,
            SearchResults = new List<SearchResult>()
        });
    }
}
