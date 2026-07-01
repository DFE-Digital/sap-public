using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Web.Areas.Compare.Filters;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;
using SAPPub.Web.Constants;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.Controllers;

[Area("Compare")]
[FeatureGate(EstablishmentComparisonEnabled)]
[Route("compare/secondary")]
[SecondaryComparisonQueryValidation]
public class SecondaryController : Controller
{
    [HttpGet]
    [Route("about-your-schools", Name = RouteConstants.CompareSecondaryAboutYourSchools)]
    public async Task<IActionResult> AboutYourSchools(
        [FromServices] IAboutSchoolService aboutSchoolService,
        List<string> urns,
        CancellationToken ct)
    {
        var aboutSchoolsCompareModelList = await aboutSchoolService.GetAboutSchoolForComparisonAsync(urns, ct);
        var model = CompareAboutYourSchoolsViewModel.Map(urns, aboutSchoolsCompareModelList);
        return View(model);
    }

    [HttpGet]
    [Route("pupil-attainment", Name = RouteConstants.CompareSecondaryAcademicPerformancePupilAttainment)]
    public async Task<IActionResult> AcademicPerformancePupilAttainment(List<string> urns)
    {
        var model = new CompareAcademicPerformancePupilAttainmentViewModel { URNs = urns };
        return View(model);
    }

    [HttpGet]
    [Route("english-and-maths-results", Name = RouteConstants.CompareSecondaryAcademicPerformanceEnglishAndMathsResults)]
    public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(
    [FromServices] IEnglishAndMathsComparisionService englishAndMathsComparisionService,
    List<string> urns,
    CancellationToken ct = default)
    {
        var results = await englishAndMathsComparisionService.GetComparisionResultsAsync(urns, ct);
        var model = CompareAcademicPerformanceEnglishAndMathsResultsViewModel.Map(urns, results);
        return View(model);
    }

    [HttpGet]
    [Route("next-steps", Name = RouteConstants.CompareSecondaryNextSteps)]
    public async Task<IActionResult> NextSteps(List<string> urns)
    {
        var model = new CompareNextStepsViewModel { URNs = urns };
        return View(model);
    }

    [HttpGet]
    [Route("destinations-after-year-11", Name = RouteConstants.CompareSecondaryDestinations)]
    public async Task<IActionResult> Destinations(List<string> urns)
    {
        var model = new CompareDestinationsViewModel { URNs = urns };
        return View(model);
    }
}
