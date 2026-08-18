using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.ViewComponents.ChartWithTableToggle;

public class ChartWithTableToggle : ViewComponent
{
    public IViewComponentResult Invoke(ChartWithTableToggleModel model)
    {
        return View("~/ViewComponents/ChartWithTableToggle/Default.cshtml", model);
    }
}
