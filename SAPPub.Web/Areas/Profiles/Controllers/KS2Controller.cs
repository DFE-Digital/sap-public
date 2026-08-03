using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;
using SAPPub.Web.Areas.Profiles.Filters;
using SAPPub.Web.Areas.Profiles.ViewModels.KS2;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]
    [FeatureGate(Constants.Constants.EnablePrimary)]
    [ServiceFilter(typeof(PrimaryQueryValidationFilter))]
    public class KS2Controller : Controller, IEstablishment
    {
        public EstablishmentServiceModel Establishment { get; set; } = null!; // set by the PrimaryQueryValidationFilter

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
        [Route("school/{urn}/{schoolName}/primary-performance/meeting-or-exceeding-standards", Name = RouteConstants.PrimaryAcademicPerformanceMeetingOrExceedingStandards)]
        public async Task<IActionResult> AcademicPerformanceMeetingOrExceedingStandards(
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            var model = AcademicPerformanceMeetingOrExceedingStandardsViewModel.Map(Establishment);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/primary-performance/subject-scaled-scores", Name = RouteConstants.PrimaryAcademicPerformanceSubjectScaledScores)]
        public async Task<IActionResult> AcademicPerformanceSubjectScaledScores(
            [FromServices] IKS2ScaledScoreService scaledScoreService,
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            var scaledScoreModel = await scaledScoreService
                .GetScaledScoreModel(urn, ct);


            var model = AcademicPerformanceSubjectScaledScoresViewModel.Map(scaledScoreModel);
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
