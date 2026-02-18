using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IEstablishmentService _establishmentService;

        public SearchController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var establishments = await _establishmentService.GetAllEstablishmentsAsync(ct);
            var listOfEstabs = SearchViewModel.FromEstablishmentCoreEntity(establishments);

            return View(listOfEstabs);
        }
    }
}
