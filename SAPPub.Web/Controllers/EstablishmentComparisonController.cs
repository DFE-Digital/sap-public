using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;

namespace SAPPub.Web.Controllers
{
    public class EstablishmentComparisonController(IEstablishmentComparisonService establishmentComparisonService) 
        : Controller
    {
        [HttpPost]
        public IActionResult ToggleSaveEstablishment(string urn, string returnUrl)
        {
            establishmentComparisonService.Toggle(urn);

            var isSaved = !establishmentComparisonService.IsSaved(urn);

            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                return Json(new { saved = isSaved });
            }

            if (isSaved)
            {
                TempData["BannerAddSuccess"] = true;
            }
            else
            {
                TempData["BannerRemoveSuccess"] = true;
            }

            return Redirect(returnUrl);
        }
    }
}
