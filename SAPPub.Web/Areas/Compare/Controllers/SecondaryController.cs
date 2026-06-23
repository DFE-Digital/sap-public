using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services.KS4;
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
    public async Task<IActionResult> AboutYourSchools(List<string> urns)
    {
        var model = new CompareAboutYourSchoolsViewModel { URNs = urns };
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
    public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(List<string> urns)
    {
        var model = new CompareAcademicPerformanceEnglishAndMathsResultsViewModel { URNs = urns };
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
    public async Task<IActionResult> Destinations([FromServices] IDestinationsService destinationsService, List<string> urns)
    {
        var establishments = HttpContext.Items["Establishments"] as List<Establishment> ?? [];
        var destinations = await Task.WhenAll(urns.Select(async d => await destinationsService.GetDestinationsDetailsAsync(d)));

        var model = CompareDestinationsViewModel.Map(urns, destinations.ToList(), establishments.ToList());
        return View(model);
    }
}
