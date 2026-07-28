using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
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
        var selectedAcademicYear = AcademicYearSelectionExtensions.FromRouteSegment(selectedAcademicYearName);
        if (!selectedAcademicYear.HasValue)
        {
            return NotFound();
        }
        var results = await attainmentAndProgressService.GetAttainmentAndProgressAsync(urn, selectedAcademicYear!.Value, ct);

        var model = AcademicPerformanceAttainmentAndProgressViewModel.Map(results, selectedAcademicYear!.Value);
        return View(model);
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
        var gradeName = SelectedGrade.ToRouteSegment();

        return RedirectToAction(nameof(AcademicPerformanceEnglishAndMathsResults), new { urn, schoolName, gradeName });
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
        var grade = GcseGradeSelectionExtensions.FromRouteSegment(gradeName);
        if (!grade.HasValue)
        {
            return NotFound();
        }
        var results = await academicPerformanceEnglishAndMathsResultsService.GetEnglishAndMathsResultsAsync(urn, grade!.Value.ToGradeValue(), ct);

        var model = AcademicPerformanceEnglishAndMathsResultsViewModel.Map(results, grade!.Value);
        return View(model);
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
