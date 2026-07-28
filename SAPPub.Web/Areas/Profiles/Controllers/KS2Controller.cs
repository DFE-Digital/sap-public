using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Areas.Profiles.Filters;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]
    [FeatureGate("EnablePrimary")]
    [ServiceFilter(typeof(PrimaryQueryValidationFilter))]
    public class KS2Controller(ILogger<KS2Controller> logger) : Controller, IEstablishment
    {
        public EstablishmentServiceModel? Establishment { get; set; }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/progress-attainment", Name = RouteConstants.PrimaryAcademicPerformanceAttainmentAndProgress)]
        public async Task<IActionResult> AcademicPerformanceAttainmentAndProgress(
            [FromServices] IAttainmentAndProgressService attainmentAndProgressService,
            string urn,
            string schoolName,
            CancellationToken ct = default)
        {
            return View();
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/pupil-progress", Name = RouteConstants.PrimaryAcademicPerformancePupilProgress)]
        public IActionResult AcademicPerformancePupilProgress(
            [FromServices] IAcademicPerformanceEnglishAndMathsResultsService academicPerformanceEnglishAndMathsResultsService,
            string urn,
            string schoolName,
            CancellationToken ct = default)
        {
            return View();
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/additional-measures", Name = RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures)]
        public async Task<IActionResult> AcademicPerformanceAdditionalMeasures(
            [FromServices] IAdditionalMeasuresService additionalMeasuresService,
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            return View();
        }
    }
}
