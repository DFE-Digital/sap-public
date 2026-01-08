using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Controllers
{
    public class SearchController : Controller
    {
        private IEstablishmentService _establishmentService;

        public SearchController(
            IEstablishmentService establishmentService
            )
        {
            _establishmentService = establishmentService;
        }

        public IActionResult Index()
        {
            var listOfEstabs = SearchViewModel.FromEstablishmentCoreEntity(_establishmentService.GetAllEstablishments());
            return View(listOfEstabs);
        }
    }
}
