using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Areas.Profiles.Filters;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS2;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
[FeatureGate(Constants.Constants.EnablePrimary)]
[ServiceFilter(typeof(PrimaryQueryValidationFilter))]
public class KS2Controller(IOptions<UrlLinksOptions> urlLinksOptions) : Controller, IEstablishment
{
    public EstablishmentMinimumServiceModel Establishment { get; set; } = null!; // set by the PrimaryQueryValidationFilter

    [HttpGet]
    [Route("school/{urn}/{schoolName}/primary-performance/pupil-progress", Name = RouteConstants.PrimaryAcademicPerformancePupilProgress)]
    public IActionResult AcademicPerformancePupilProgress(
        string urn,
        string schoolName,
        AcademicYearSelection selectedAcademicYear = AcademicYearSelection.Current)
    {
        var selectedYearName = AcademicYearSelectionExtensions.ToRouteSegment(selectedAcademicYear);

        return RedirectToAction(nameof(AcademicPerformancePupilProgress), new { urn, schoolName, selectedAcademicYearName = selectedYearName });
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/primary-performance/pupil-progress/{selectedAcademicYearName}")]
    public async Task<IActionResult> AcademicPerformancePupilProgress(
        [FromServices] IKS2PupilProgressService ks2PupilProgressService,
        string urn,
        string schoolName,
        string selectedAcademicYearName,
        CancellationToken ct = default)
    {
        var selectedAcademicYear = AcademicYearSelectionExtensions.FromRouteSegment(selectedAcademicYearName);
        if (!selectedAcademicYear.HasValue)
        {
            return NotFound();
        }

        var ks2PupilPerformance = await ks2PupilProgressService.GetPupilProgressAsync(urn, selectedAcademicYear!.Value, ct);

        var model = AcademicPerformancePupilProgressViewModel.Map(ks2PupilPerformance, Establishment, selectedAcademicYear!.Value, urlLinksOptions.Value);
        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/primary-performance/meeting-or-exceeding-standards", Name = RouteConstants.PrimaryAcademicPerformanceMeetingOrExceedingStandards)]
    public async Task<IActionResult> AcademicPerformanceMeetingOrExceedingStandards(
        [FromServices] IKS2MeetingOrExceedingStandardsService ks2MeetingOrExceedingStandardsService,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var meetingOrExceedingStandardsModel = await ks2MeetingOrExceedingStandardsService.GetMeetingOrExceedingStandardsPercentages(urn, ct);

        var model = AcademicPerformanceMeetingOrExceedingStandardsViewModel.Map(Establishment, meetingOrExceedingStandardsModel);
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


        var model = AcademicPerformanceSubjectScaledScoresViewModel.Map(Establishment, scaledScoreModel);
        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/primary-performance/additional-measures", Name = RouteConstants.PrimaryAcademicPerformanceAdditionalMeasures)]
    public async Task<IActionResult> AcademicPerformanceAdditionalMeasures(
        [FromServices] IKS2AdditionalMeasuresService kS2AdditionalMeasuresService,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var additionalServicesModel = await kS2AdditionalMeasuresService.GetAdditionalMeasures(urn, Establishment.LAId, ct);

        var model = AcademicPerformanceAdditionalMeasuresViewModel.Map(Establishment, additionalServicesModel);
        return View(model);
    }
}
