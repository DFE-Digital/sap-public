using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Controllers.Comparison;

public class CompareSecondaryController : Controller
{
    [HttpGet]
    [Route("comparison/ks4", Name = "CompareKS4")]
    public IActionResult Index(List<string>? establishmentUrns)
    {
        return View();
    }
}
