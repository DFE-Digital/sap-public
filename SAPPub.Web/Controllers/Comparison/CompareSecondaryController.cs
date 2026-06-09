using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers.Comparison;

[FeatureGate(EstablishmentComparisonEnabled)]
public class CompareSecondaryController : Controller
{
    [HttpGet]
    [Route("comparison/ks4", Name = "CompareKS4")]
    public IActionResult Index(List<string>? establishmentUrns)
    {
        return View();
    }
}
