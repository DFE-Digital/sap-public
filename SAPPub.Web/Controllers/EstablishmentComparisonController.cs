using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

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

            if (isSaved)
            {
                TempData.Set(BannerAddSuccess, true);
            }
            else
            {
                TempData.Set(BannerRemoveSuccess, true);
            }

            return Redirect(returnUrl);
        }
    }
}