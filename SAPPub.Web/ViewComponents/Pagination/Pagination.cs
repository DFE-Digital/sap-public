using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.ViewComponents.Pagination;

public class Pagination : ViewComponent
{
    public IViewComponentResult Invoke(PaginationModel model)
    {
        return View("~/ViewComponents/Pagination/Default.cshtml", model);
    }
}
