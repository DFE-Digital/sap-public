using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Controllers.Comparison;

public class CompareSecondaryController : Controller
{
    [HttpPost]
    public IActionResult Index(List<string> selectedEstablishments)
    {
        return View();
    }
}
