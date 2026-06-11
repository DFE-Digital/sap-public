using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.Compare.Secondary;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers.Comparison;

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
    public async Task<IActionResult> AcademicPerformancePupilProgressAndAttainment(List<string>? establishmentUrns)
    {
        var model = new CompareAcademicPerformanceProgressAndAttainmentViewModel { EstablishmentUrns = establishmentUrns };
        return View(model);
    }

    [HttpGet]
    [Route("academic-performance/english-and-maths-results", Name = RouteConstants.CompareSecondaryAcademicPerformanceEnglishAndMathsResults)]
    public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(List<string>? establishmentUrns)
    {
        var model = new CompareAcademicPerformanceEnglishAndMathsResultsViewModel { EstablishmentUrns = establishmentUrns };
        return View(model);
    }
}
