using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Web.Constants;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.Controllers;

[Area("Compare")]
[FeatureGate(EstablishmentComparisonEnabled)]
[Route("compare/secondary")]
public class SecondaryController : Controller
{
    [HttpGet]
    [Route("view", Name = RouteConstants.CompareSecondaryView)]
    public IActionResult Index(List<string>? urns)
    {
        return View();
    }
}
