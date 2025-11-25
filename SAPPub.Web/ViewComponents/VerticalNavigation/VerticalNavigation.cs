using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.ViewComponents.VerticalNavigation;

public class VerticalNavigation : ViewComponent
{
    public IViewComponentResult Invoke(VerticalNavigationModel model)
    {
        return View("~/ViewComponents/VerticalNavigation/Default.cshtml", model);
    }
}
