using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.ViewComponents.VerticalNavigationMySchools;

public class VerticalNavigationMySchools : ViewComponent
{
    public IViewComponentResult Invoke(VerticalNavigationMySchoolsModel model)
    {
        return View("~/ViewComponents/VerticalNavigationMySchools/Default.cshtml", model);
    }
}
