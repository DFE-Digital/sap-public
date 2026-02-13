using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.SchoolSearch;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers
{
    public class SearchController(IEstablishmentService establishmentService,
            ISchoolSearchService schoolSearchService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var listOfEstabs = SearchViewModel.FromEstablishmentCoreEntity(establishmentService.GetAllEstablishments());
            var searchResults = await schoolSearchService.SearchAsync("Manchester");
            var temp2 = searchResults.Select(r => new SearchViewModel
            {
                URN = r.URN,
                EstablishmentName = r.EstablishmentName
            }).ToList();
            return View(temp2);
        }
    }
}
