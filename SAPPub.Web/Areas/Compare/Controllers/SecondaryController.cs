using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Web.Areas.Compare.Compare.Secondary;
using SAPPub.Web.Constants;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.Controllers;

[Area("Compare")]
[FeatureGate(EstablishmentComparisonEnabled)]
[Route("compare/secondary")]
public class SecondaryController : Controller
{
    [HttpGet]
    [Route("pupil-performance-attainment-and-progress", Name = RouteConstants.CompareSecondaryAcademicPerformancePupilProgressAndAttainment)]
    public async Task<IActionResult> AcademicPerformancePupilProgressAndAttainment(List<string> urns)
    {
        var model = new CompareAcademicPerformanceProgressAndAttainmentViewModel { URNs = urns };
        return View(model);
    }

    [HttpGet]
    [Route("english-and-maths-results", Name = RouteConstants.CompareSecondaryAcademicPerformanceEnglishAndMathsResults)]
    public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(List<string> urns)
    {
        var model = new CompareAcademicPerformanceEnglishAndMathsResultsViewModel { URNs = urns };
        return View(model);
    }
}
