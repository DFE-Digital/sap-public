using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.ViewComponents.VerticalNavigationCompareSecondary;

public class VerticalNavigationCompareSecondary : ViewComponent
{
    public IViewComponentResult Invoke(VerticalNavigationCompareSecondaryModel model)
    {
        return View("~/ViewComponents/VerticalNavigationCompareSecondary/Default.cshtml", model);
    }
}
