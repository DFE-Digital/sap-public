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
    public IActionResult Index(SearchParamsModel model)
    {
        var searchKeyWord = model.NameSearchTerm;
        var searchLocation = model.LocationSearchTerm;
        if (ModelState.IsValid)
        {
            return RedirectToAction("SearchResults", model);
        }
        else
        {
            return View(new SearchResultsViewModel()
            {
                SearchParams = model
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchResults(SearchParamsModel model)
    {
        var searchKeyWord = model.NameSearchTerm;
        var searchLocation = model.LocationSearchTerm;
        var searchQuery = new SearchQuery() { Name = searchKeyWord, Location = searchLocation, Distance = model.Distance };
        if (searchKeyWord != null || searchLocation != null)
        {
            var searchResults = await schoolSearchService.SearchAsync(searchQuery);
            if (searchResults.Status == SchoolSearchStatus.InvalidPostcode)
            {
                // postcode may be a valid format but not be found by the postcode API
                ModelState.AddModelError(nameof(SearchParamsModel.LocationSearchTerm), "Enter a valid postcode");
            }
            var viewResults = new SearchResultsViewModel()
            {
                SearchParams = model,
                SearchResultsCount = searchResults.Count,
                SearchResults = SearchResultsViewModel.FromServiceModel(searchResults.SchoolSearchResults.OrderBy(r => r.Distance))
            };
            return View(viewResults);
        }
        else return View(new SearchResultsViewModel()
        {
            SearchParams = model,
            SearchResultsCount = 0,
            SearchResults = new List<SearchResult>()
        });
    }
}
