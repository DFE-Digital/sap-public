using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]
    [FeatureGate("EnablePrimary")]
    public class KS2Controller(ILogger<KS2Controller> logger) : Controller
    {
        [HttpGet]
        [Route("school/{urn}/{schoolName}/admissions/primary", Name = RouteConstants.PrimaryAdmissions)]
        public async Task<IActionResult> Admissions(
            [FromServices] IAdmissionsService admissionsService,
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            return View();
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/curriculum/primary", Name = RouteConstants.PrimaryCurriculum)]
        public async Task<IActionResult> CurriculumAndExtraCurricularActivities(string urn, string schoolName, CancellationToken ct)
        {
            return View();
        }

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
