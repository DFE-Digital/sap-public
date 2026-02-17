using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers;

public class SearchController(ISchoolSearchService schoolSearchService) : Controller
{
    public IActionResult Index()
    {
        return View(new SearchResultsViewModel());
    }

    public async Task<IActionResult> SearchResults(string? searchKeyWord)
    {
        if (searchKeyWord != null)
        {
            var searchResults = await schoolSearchService.SearchAsync(searchKeyWord);
            var viewResults = new SearchResultsViewModel()
            {
                NameSearchTerm = searchKeyWord,
                SearchResultsCount = searchResults.TotalCount,
                SearchResults = searchResults.Results.Select(r => new SearchResult
                {
                    URN = r.URN,
                    EstablishmentName = r.EstablishmentName,
                    Address = r.Address,
                    GenderName = r.GenderName,
                    ReligiousCharacter = r.ReligiousCharacterName
                }).ToList()
            };
            return View(viewResults);
        }
        else return View(new SearchResultsViewModel()
        {
            NameSearchTerm = searchKeyWord,
            SearchResultsCount = 0,
            SearchResults = new List<SearchResult>()
        });
    }
}
