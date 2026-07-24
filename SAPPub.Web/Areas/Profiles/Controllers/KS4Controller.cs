using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
public class KS4Controller(IEstablishmentService establishmentService) : Controller
{
    [HttpGet]
    [Route("school/{urn}/{schoolName}/admissions/secondary", Name = RouteConstants.SecondaryAdmissions)]
    public async Task<IActionResult> Admissions(
        [FromServices] IAdmissionsService admissionsService,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var admissionsDetails = await admissionsService.GetAdmissionsDetailsAsync(urn, ct);
        var model = AdmissionsViewModel.MapFrom(admissionsDetails, urn);
        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/curriculum/secondary", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
    public async Task<IActionResult> CurriculumAndExtraCurricularActivities(string urn, string schoolName, CancellationToken ct)
    {
        var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);
        var model = CurriculumAndExtraCurricularActivitiesViewModel.Map(establishmentDetails);
        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/secondary-performance/progress-attainment", Name = RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress)]
    public IActionResult AcademicPerformanceAttainmentAndProgressRedirect(
        [FromServices] IAttainmentAndProgressService attainmentAndProgressService,
        string urn,
        string schoolName,
        AcademicYearSelection selectedAcademicYear = AcademicYearSelection.Current,
        CancellationToken ct = default)
    {
        var selectedYearName = AcademicYearSelectionExtensions.ToRouteSegment(selectedAcademicYear);

        return RedirectToAction(nameof(AcademicPerformanceAttainmentAndProgress), new { urn, schoolName, selectedAcademicYearName = selectedYearName });
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/secondary-performance/progress-attainment/{selectedAcademicYearName}")]
    public async Task<IActionResult> AcademicPerformanceAttainmentAndProgress(
        [FromServices] IAttainmentAndProgressService attainmentAndProgressService,
        string urn,
        string schoolName,
        string selectedAcademicYearName,
        CancellationToken ct = default)
    {
        try
        {
            var selectedAcademicYear = AcademicYearSelectionExtensions.FromRouteSegment(selectedAcademicYearName);

            var results = await attainmentAndProgressService.GetAttainmentAndProgressAsync(urn, selectedAcademicYear, ct);

            var model = AcademicPerformanceAttainmentAndProgressViewModel.Map(results, selectedAcademicYear);
            return View(model);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/secondary-performance/english-and-maths", Name = RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults)]
    public IActionResult AcademicPerformanceEnglishAndMathsResultsRedirect(
        [FromServices] IAcademicPerformanceEnglishAndMathsResultsService academicPerformanceEnglishAndMathsResultsService,
        string urn,
        string schoolName,
        GcseGradeDataSelection SelectedGrade = GcseGradeDataSelection.Grade5AndAbove,
        CancellationToken ct = default)
    {
        try
        {
            var gradeName = SelectedGrade.ToRouteSegment();
            return RedirectToAction(nameof(AcademicPerformanceEnglishAndMathsResults), new { urn, schoolName, gradeName });
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/secondary-performance/english-and-maths/{gradeName}")]
    public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(
        [FromServices] IAcademicPerformanceEnglishAndMathsResultsService academicPerformanceEnglishAndMathsResultsService,
        string urn,
        string schoolName,
        string gradeName,
        CancellationToken ct = default)
    {
        try
        {
            var grade = GcseGradeSelectionExtensions.FromRouteSegment(gradeName);
            var results = await academicPerformanceEnglishAndMathsResultsService.GetEnglishAndMathsResultsAsync(urn, grade.ToGradeValue(), ct);

            var model = AcademicPerformanceEnglishAndMathsResultsViewModel.Map(results, grade);
            return View(model);
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/secondary-performance/subjects-entered", Name = RouteConstants.SecondaryAcademicPerformanceSubjectsEntered)]
    public async Task<IActionResult> AcademicPerformanceSubjectsEntered(
        [FromServices] IEstablishmentSubjectEntriesService subjectEntriesService,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(establishmentDetails?.URN))
        {
            return View("Error");
        }

        var (gcseSubjectEntries, vocationalSubjectEntries, otherSubjectEntries) =
            await subjectEntriesService.GetSubjectEntriesByUrnAsync(urn, ct);

        var model = AcademicPerformanceSubjectsEnteredViewModel.Map(
            establishmentDetails,
            gcseSubjectEntries,
            vocationalSubjectEntries,
            otherSubjectEntries);

        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/secondary-performance/additional-measures", Name = RouteConstants.SecondaryAcademicPerformanceAdditionalMeasures)]
    public async Task<IActionResult> AcademicPerformanceAdditionalMeasures(
        [FromServices] IAdditionalMeasuresService additionalMeasuresService,
        string urn, string schoolName, CancellationToken ct)
    {
        var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);
        var additionalMeasures = await additionalMeasuresService.GetAsync(urn, establishmentDetails.LAId, ct);

        var model = AcademicPerformanceAdditionalMeasuresViewModel.MapToMeasuresInTableFormat(additionalMeasures, establishmentDetails);
        return View(model);
    }
}
