using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Areas.Compare.Filters;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;
using SAPPub.Web.Constants;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Areas.Compare.Controllers;

[Area("Compare")]
[FeatureGate(EstablishmentComparisonEnabled)]
[Route("compare/secondary")]
[ServiceFilter(typeof(SecondaryComparisonQueryValidationFilter))]
public class SecondaryController : Controller, IEstablishmentsList
{
    public List<EstablishmentServiceModel> Establishments { get; set; } = [];

    [HttpGet]
    [Route("about-your-schools", Name = RouteConstants.CompareSecondaryAboutYourSchools)]
    public async Task<IActionResult> AboutYourSchools(
        [FromServices] IAboutSchoolService aboutSchoolService,
        List<string> urns,
        CancellationToken ct)
    {
        var aboutSchoolsCompareModelList = await aboutSchoolService.GetAboutSchoolForComparisonAsync(urns, ct);
        var model = CompareAboutYourSchoolsViewModel.Map(urns, aboutSchoolsCompareModelList, Establishments);
        return View(model);
    }

    [HttpGet]
    [Route("pupil-attainment", Name = RouteConstants.CompareSecondaryAcademicPerformancePupilAttainment)]
    public async Task<IActionResult> AcademicPerformancePupilAttainment(List<string> urns)
    {
        var model = new CompareAcademicPerformancePupilAttainmentViewModel
        {
            URNs = urns,
            ListContainsSpecialSchool = Establishments.Any(e => e.IsSpecialSchool),
        };
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
        var model = CompareAcademicPerformanceEnglishAndMathsResultsViewModel.Map(urns, results, Establishments);
        return View(model);
    }

    [HttpGet]
    [Route("next-steps", Name = RouteConstants.CompareSecondaryNextSteps)]
    public async Task<IActionResult> NextSteps(
         [FromServices] IEstablishmentService establishmentService,
         List<string> urns,
         CancellationToken ct = default)
    {
        var model = CompareNextStepsViewModel.Map(urns, Establishments);
        return View(model);
    }

    [HttpGet]
    [Route("destinations-after-year-11", Name = RouteConstants.CompareSecondaryDestinations)]
    public async Task<IActionResult> Destinations([FromServices] IDestinationsComparisonService destinationsService, List<string> urns)
    {
        var destinationsResult = await destinationsService.GetDestinationsDetailsAsync(urns, CancellationToken.None);

        var model = CompareDestinationsViewModel.Map(urns, destinationsResult, Establishments.ToList());
        return View(model);
    }
}
