using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers;

public class SearchController(ISchoolSearchService schoolSearchService) : Controller
{
    private const string BroadSearchErrorMessage = "Enter a school name or search by postcode";
    private const string BroadSearchErrorTempDataKey = "BroadSearchError";

    [HttpGet]
    [Route("search", Name = RouteConstants.Search)]
    public IActionResult Index()
    {
        if (TempData.TryGetValue(BroadSearchErrorTempDataKey, out var broadSearchError) && broadSearchError is string)
        {
            AddBroadSearchValidationError($"{nameof(SearchResultsViewModel.SearchParams)}.{nameof(SearchParamsModel.NameSearchTerm)}");
        }

        return View(new SearchResultsViewModel());
    }

    [HttpPost]
    public IActionResult Index(SearchParamsModel model)
    {
        model.PageNumber = 1; // always reset pagenumber, when searching

        if (IsSchoolOnlySearchTerm(model.NameSearchTerm) && string.IsNullOrWhiteSpace(model.LocationSearchTerm))
        {
            AddBroadSearchValidationError(nameof(SearchParamsModel.NameSearchTerm));
            PrefixModelStateKeys("SearchParams");

            return View(new SearchResultsViewModel
            {
                SearchParams = model
            });
        }

        if (ModelState.IsValid)
        {
            return RedirectToAction("SearchResults", model);
        }

        if (!ModelState.IsValid)
        {
            PrefixModelStateKeys("SearchParams");
        }
        
        return View(new SearchResultsViewModel()
        {
            SearchParams = model
        });
    }

    [HttpGet]
    [Route("search/results", Name = RouteConstants.SearchResults)]
    public async Task<IActionResult> SearchResults(SearchParamsModel model)
    {
        if (IsSchoolOnlySearchTerm(model.NameSearchTerm) && string.IsNullOrWhiteSpace(model.LocationSearchTerm))
        {
            TempData[BroadSearchErrorTempDataKey] = BroadSearchErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            PrefixModelStateKeys("SearchParams");
        }
        var searchKeyWord = model.NameSearchTerm;
        var searchLocation = model.LocationSearchTerm;
        var searchQuery = new SchoolSearchServiceQuery()
        {
            Name = searchKeyWord,
            Location = searchLocation,
            Distance = searchLocation != null ? model.Distance : null,
            PageNumber = model.PageNumber,
            EstablishmentPhases = model.Phase,
            EstablishmentTypes = model.SchoolType

        };

        SchoolSearchResultsServiceModel? searchResults = null;
        if (searchKeyWord != null || searchLocation != null)
        {
            searchResults = await schoolSearchService.SearchAsync(searchQuery);
            if (searchResults.Status == SchoolSearchStatus.InvalidPostcode)
            {
                // postcode may be a valid format but not be found by the postcode API
                ModelState.AddModelError(nameof(SearchParamsModel.LocationSearchTerm), "Enter a valid postcode");
            }
        }

        var searchResultsModel = SearchResultsViewModel.FromServiceModel(model, searchResults);
        return View(searchResultsModel);
    }

    // fix the model state keys so that validation messages are correctly associated with the form fields 
    private void PrefixModelStateKeys(string prefix)
    {
        var keys = ModelState.Keys.ToList();
        foreach (var oldKey in keys)
        {
            // Skip already-prefixed keys
            if (oldKey.StartsWith(prefix + ".")) continue;
            var newKey = $"{prefix}.{oldKey}";
            var entry = ModelState[oldKey]!;

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

    private static bool IsSchoolOnlySearchTerm(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return false;
        }

        var normalizedSearchTerm = string.Concat(searchTerm.Where(c => !char.IsWhiteSpace(c)));
        return string.Equals(normalizedSearchTerm, "school", StringComparison.OrdinalIgnoreCase);
    }

    private void AddBroadSearchValidationError(string modelStateKey)
    {
        ModelState.AddModelError(modelStateKey, BroadSearchErrorMessage);
    }
}
