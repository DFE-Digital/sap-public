using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.SchoolSearch;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers;

public class SearchController(IEstablishmentService establishmentService,
        ISchoolSearchService schoolSearchService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchResults(string? searchKeyWord)
    {
        var listOfEstabs = SearchResultsViewModel.FromEstablishmentCoreEntity(establishmentService.GetAllEstablishments());
        if (searchKeyWord != null)
        {
            var searchResults = await schoolSearchService.SearchAsync(searchKeyWord);
            var viewResults = searchResults.Select(r => new SearchResultsViewModel
            {
                URN = r.URN,
                EstablishmentName = r.EstablishmentName
            }).ToList();
            return View(viewResults);
        }
        else
        {
            return View(listOfEstabs);
        }
    }
}
