using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Web.Areas.Compare.Filters;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;
using SAPPub.Web.Constants;
using SAPPub.Web.Extensions;
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
    public async Task<IActionResult> AcademicPerformancePupilAttainment(
        [FromServices] IAttainmentAndProgressComparisionService attainmentAndProgressComparisionService,
        List<string> urns,
        CancellationToken ct = default)
    {
        var establishments = HttpContext.Get<List<Establishment>>(Establishments) ?? [];
        var attainmentResults = await attainmentAndProgressComparisionService.GetComparisionResultsAsync(urns, ct);
        var model = CompareAcademicPerformancePupilAttainmentViewModel.Map(urns, attainmentResults, establishments);
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
    public async Task<IActionResult> NextSteps(
         [FromServices] IEstablishmentService establishmentService,
         List<string> urns,
         CancellationToken ct = default)
    {
        var establishments = await establishmentService.GetEstablishmentsAsync(urns, ct);
        var model = CompareNextStepsViewModel.Map(urns, establishments);
        return View(model);
    }

    [HttpGet]
    [Route("destinations-after-year-11", Name = RouteConstants.CompareSecondaryDestinations)]
    public async Task<IActionResult> Destinations([FromServices] IDestinationsComparisonService destinationsService, List<string> urns)
    {
        var establishments = HttpContext.Get<List<Establishment>>(Establishments) ?? [];
        var destinationsResult = await destinationsService.GetDestinationsDetailsAsync(urns, CancellationToken.None);

        var model = CompareDestinationsViewModel.Map(urns, destinationsResult, establishments.ToList());
        return View(model);
    }
}
