using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers
{
    public class MySchoolsListController(IMySchoolsListService establishmentComparisonService)
        : Controller
    {
        [HttpPost]
        public IActionResult ToggleSaveEstablishment(string urn, bool isSearchPage, string returnUrl)
        {
            if (isSearchPage)
            {
                var isComparisionLimitReached = establishmentComparisonService.IsListLimitReached();
                var urnExists = establishmentComparisonService.IsSaved(urn);
                var isJsRequest = Request.Headers.XRequestedWith == "XMLHttpRequest";

                var canToggle = urnExists || !isComparisionLimitReached;
                var saved = false;
                var limitReached = !canToggle;

                if (canToggle)
                {
                    saved = establishmentComparisonService.Toggle(urn);
                }
                else if (!isJsRequest)
                {
                    TempData.Set(ComparisionLimtReached, limitReached);
                }

                if (isJsRequest)
                    return Json(new { isSaved = saved, isLimitReached = limitReached });
            }
            else
            {
                var isSaved = establishmentComparisonService.Toggle(urn);

                if (isSaved)
                {
                    TempData.Set(BannerAddSuccess, true);
                }
                else
                {
                    TempData.Set(BannerRemoveSuccess, true);
                }
            }

            return Redirect(returnUrl);
        }
    }
}