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
            if (!ModelState.IsValid)
            {
                PrefixModelStateKeys("SearchParams");
            }
            return View(new SearchResultsViewModel()
            {
                SearchParams = model
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> SearchResults(SearchParamsModel model)
    {
        if (!ModelState.IsValid)
        {
            PrefixModelStateKeys("SearchParams");
        }
        var searchKeyWord = model.NameSearchTerm;
        var searchLocation = model.LocationSearchTerm;
        var searchQuery = new SearchQuery() { Name = searchKeyWord, Location = searchLocation, Distance = searchLocation != null ? model.Distance : null };
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

    // fix the model state keys so that validation messages are correctly associated with the form fields after redirecting to the SearchResults action
    private void PrefixModelStateKeys(string prefix)
    {
        var keys = ModelState.Keys.ToList();

        foreach (var oldKey in keys)
        {
            // Skip already-prefixed keys
            if (oldKey.StartsWith(prefix + ".")) continue;

            var newKey = $"{prefix}.{oldKey}";

            var entry = ModelState[oldKey];

            // 1. Move attempted value
            ModelState.SetModelValue(newKey, entry.RawValue, entry.AttemptedValue);

            // 2. Move errors
            foreach (var error in entry.Errors)
            {
                ModelState.AddModelError(newKey, error.ErrorMessage);
            }

            // 3. Remove the old key
            ModelState.Remove(oldKey);
        }
    }
}
