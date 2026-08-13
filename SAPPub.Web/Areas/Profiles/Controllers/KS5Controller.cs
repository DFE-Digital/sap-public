using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.Services.Performance;
using SAPPub.Web.Areas.Profiles.ViewModels.KS5;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
[FeatureGate("Enable16to19")]
public class KS5Controller(ILogger<KS5Controller> logger) : Controller
{
    [Route("school/{urn}/{schoolName}/16-to-19-performance", Name = RouteConstants.KS5AcademicPerformanceRoot)]
    public IActionResult Index(string urn, string schoolName)
    {
        return RedirectToAction("Level3Qualifications", new { urn, schoolName, qualification = Level3.ALevel.ToString().ToLower() });
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/level-3-qualifications", Name = RouteConstants.KS5AcademicPerformanceLevel3)]
    public IActionResult Level3QualificationsRedirect(string urn, string schoolName, Level3 level3qualification = Level3.ALevel)
    {
        return RedirectToAction("Level3Qualifications", new { urn, schoolName, qualification = level3qualification.ToString().ToLower() });
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/level-3-qualifications/{qualification}", Name = RouteConstants.KS5AcademicPerformanceLevel3Filter)]
    public async Task<IActionResult> Level3Qualifications(
        [FromServices] ILevel3QualificationsService level3QualificationsService,
        string urn,
        string schoolName,
        Level3 qualification,
        CancellationToken ct)
    {
        var qualificationDetailsModel = await level3QualificationsService
            .GetLevel3QualificationDetailsAsync(urn, qualification, ct);

        if (!qualificationDetailsModel.IsKS5)
        {
            logger.LogWarning("Attempted to view KS5 page with no KS5 data URN: {URN}", urn);
            return View("Error");
        }

        var model = Level3QualificationViewModel.Map(qualificationDetailsModel);
        return View(model);
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/level-2-qualifications", Name = RouteConstants.KS5AcademicPerformanceLevel2)]
    public IActionResult Level2QualificationsRedirect(string urn, string schoolName, Level2 level2qualification = Level2.TechCert)
    {
        return RedirectToAction("Level2Qualifications", new { urn, schoolName, qualification = level2qualification.ToString().ToLower() });
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/level-2-qualifications/{qualification}", Name = RouteConstants.KS5AcademicPerformanceLevel2Filter)]
    public async Task<IActionResult> Level2Qualifications(
        [FromServices] ILevel2QualificationsService level2QualificationsService,
        string urn,
        string schoolName,
        Level2 qualification,
        CancellationToken ct)
    {
        var qualificationDetailsModel = await level2QualificationsService
            .GetLevel2QualificationDetailsAsync(urn, qualification, ct);

        if (!qualificationDetailsModel.IsKS5)
        {
            logger.LogWarning("Attempted to view KS5 page with no KS5 data URN: {URN}", urn);
            return View("Error");
        }

        var model = Level2QualificationViewModel.Map(qualificationDetailsModel);
        return View(model);
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/english-and-maths", Name = RouteConstants.KS5AcademicPerformanceEnglishMaths)]
    public async Task<IActionResult> EnglishAndMaths(
        [FromServices] IEnglishAndMathsQualificationsService englishAndMathsQualificationsService,
        string urn, string schoolName,
        CancellationToken ct)
    {
        var englishMathsQualifications = await englishAndMathsQualificationsService.GetEnglishAndMathsQualificationDetailsAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(englishMathsQualifications.Urn))
        {
            logger.LogWarning("No establishment details found for URN: {URN}", urn);
            return View("Error");
        }

        if (!englishMathsQualifications.IsKS5)
        {
            logger.LogWarning("Attempted to view KS5 page with no KS5 data URN: {URN}", urn);
            return View("Error");
        }

        var englishMathsQualificationsViewModel = EnglishMathsQualificationsViewModel.Map(englishMathsQualifications);
        return View(englishMathsQualificationsViewModel);
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/subjects-entered/", Name = RouteConstants.KS5AcademicPerformanceSubjectsEntered)]
    public IActionResult SubjectsEnteredRedirect(
        string urn,
        string schoolName,
        QualificationType? qualificationType)
    {
        qualificationType ??= 0;

        var qualTypeSelected = ((QualificationType)qualificationType).ToString();
        if (string.IsNullOrWhiteSpace(qualTypeSelected))
        {
            qualTypeSelected = ((QualificationType)0).ToString();
        }

        return RedirectToAction(nameof(SubjectsEntered), new { urn, schoolName, qualification = qualTypeSelected.ToLower() });
    }

    [Route("school/{urn}/{schoolName}/16-to-19-performance/subjects-entered/{qualification}", Name = RouteConstants.KS5AcademicPerformanceSubjectsEnteredFilter)]
    public async Task<IActionResult> SubjectsEntered(
        [FromServices] IAboutSchoolService aboutSchoolService,
        [FromServices] IKS5EstablishmentSubjectEntriesService establishmentSubjectEntriesService,
        QualificationType? qualification,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        if (qualification is null)
        {
            return View("Error");
        }

        var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
        {
            logger.LogWarning("No establishment details found for URN: {URN}", urn);
            return View("Error");
        }

        if (!schoolDetails.IsKS5)
        {
            logger.LogWarning("Attempted to view KS5 page with no KS5 data URN: {URN}", urn);
            return View("Error");
        }

        var subjectEntries = await establishmentSubjectEntriesService.GetSubjectEntriesByUrnAsync(urn, qualification, ct);

        var ks5Model = Ks5SubjectEnteredViewModel.Map(schoolDetails, subjectEntries);
        ks5Model.QualificationType = qualification.Value;
        return View(ks5Model);
    }
}
