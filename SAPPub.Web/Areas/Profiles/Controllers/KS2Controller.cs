using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Areas.Profiles.Filters;
using SAPPub.Web.Areas.Profiles.ViewModels.KS2;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]
    [FeatureGate(Constants.Constants.EnablePrimary)]
    [ServiceFilter(typeof(PrimaryQueryValidationFilter))]
    public class KS2Controller(ILogger<KS2Controller> logger) : Controller, IEstablishment
    {
        public EstablishmentServiceModel Establishment { get; set; } = null!; // set by the PrimaryQueryValidationFilter

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/progress-attainment", Name = RouteConstants.PrimaryAcademicPerformanceAttainmentAndProgress)]
        public async Task<IActionResult> AcademicPerformanceAttainmentAndProgress(
            string urn,
            string schoolName,
            CancellationToken ct = default)
        {
            var model = AcademicPerformanceAttainmentAndProgressViewModel.Map(Establishment);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/pupil-progress", Name = RouteConstants.PrimaryAcademicPerformancePupilProgress)]
        public async Task<IActionResult> AcademicPerformancePupilProgress(
            string urn,
            string schoolName,
            CancellationToken ct = default)
        {
            var model = AcademicPerformancePupilProgressViewModel.Map(Establishment);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/additional-measures", Name = RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures)]
        public async Task<IActionResult> AcademicPerformanceAdditionalMeasures(
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            var model = AcademicPerformanceAdditionalMeasuresViewModel.Map(Establishment);
            return View(model);
        }
    }
}
