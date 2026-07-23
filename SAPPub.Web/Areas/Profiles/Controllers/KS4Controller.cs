using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
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
        var selectedYearName = selectedAcademicYear.ToString();

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
        var success = Enum.TryParse<AcademicYearSelection>(selectedAcademicYearName, out var selectedAcademicYear);
        if (!success)
        {
            return NotFound();
        }

        var results = await attainmentAndProgressService
            .GetAttainmentAndProgressAsync(urn, selectedAcademicYear, ct);

        var model = AcademicPerformanceAttainmentAndProgressViewModel.Map(results, selectedAcademicYear);
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
        return RedirectToAction(nameof(AcademicPerformanceEnglishAndMathsResults), new { urn, schoolName, gradeName = SelectedGrade.ToString() });
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
        var success = Enum.TryParse<GcseGradeDataSelection>(gradeName, out var SelectedGrade);
        if (!success)
        {
            return NotFound();
        }
        var results = await academicPerformanceEnglishAndMathsResultsService
            .GetEnglishAndMathsResultsAsync(urn, Convert.ToInt32(SelectedGrade), ct);

        var model = AcademicPerformanceEnglishAndMathsResultsViewModel.Map(results, SelectedGrade);
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
