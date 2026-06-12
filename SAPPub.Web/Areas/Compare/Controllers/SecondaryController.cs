using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Web.Areas.Compare.Compare.Secondary;
using SAPPub.Web.Constants;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.Controllers;

[Area("Compare")]
[FeatureGate(EstablishmentComparisonEnabled)]
[Route("compare/secondary")]
public class CompareSecondaryController : Controller
{
    [HttpGet]
    [Route("view", Name = RouteConstants.CompareSecondary)]
    public IActionResult Index(List<string>? establishmentUrns)
    {
        return View();
    }

    [HttpGet]
    [Route("academic-performance/pupil-performance-attainment-and-progress", Name = RouteConstants.CompareSecondaryAcademicPerformancePupilProgressAndAttainment)]
    public async Task<IActionResult> AcademicPerformancePupilProgressAndAttainment(List<string> urns)
    {
        var model = new CompareAcademicPerformanceProgressAndAttainmentViewModel { URNs = urns };
        return View(model);
    }

    [HttpGet]
    [Route("academic-performance/english-and-maths-results", Name = RouteConstants.CompareSecondaryAcademicPerformanceEnglishAndMathsResults)]
    public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(List<string> urns)
    {
        var model = new CompareAcademicPerformanceEnglishAndMathsResultsViewModel { URNs = urns };
        return View(model);
    }
}
