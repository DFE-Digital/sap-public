using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Web.Areas.Profiles.ViewModels.KS5;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]
    [FeatureGate("Enable16to19")]
    public class KS5Controller(ILogger<KS5Controller> logger) : Controller
    {
        [Route("school/{urn}/{schoolName}/16-to-19-performance", Name = RouteConstants.KS5AcademicPerformanceRoot)]
        public IActionResult Index(string urn, string schoolName)
        {
            //Not a required for the structure, but might be worth considering? What if there's no Level 3 data

            // if establishment has Level 3 data 
            return RedirectToAction("Level3Qualifications", new {  urn, schoolName, qualification = "alevel" });

            // if establishment has Level 2 data
            //return RedirectToAction("Level2Qualifications", new { urn = urn, schoolName = schoolName, qualification = "techcert" });
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
        public IActionResult Level2QualificationsRedirect(string urn, string schoolName, int? level2qualification)
        {
            // if establishment has Level 2 data 
            level2qualification ??= 1;

            var qualSelected = ((Level2)level2qualification).ToString();
            if (string.IsNullOrWhiteSpace(qualSelected))
            {
                qualSelected = ((Level2)1).ToString();
            }

            return RedirectToAction("Level2Qualifications", new { urn, schoolName, qualification = qualSelected.ToLower() });
        }


        [Route("school/{urn}/{schoolName}/16-to-19-performance/level-2-qualifications/{qualification}", Name = RouteConstants.KS5AcademicPerformanceLevel2Filter)]
        public async Task<IActionResult> Level2Qualifications([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName, Level2? qualification,
            CancellationToken ct)
        {
            if (qualification == null)
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

            var ks5Model = KS5ViewModel.Map(schoolDetails);
            ks5Model.Level2Qualification = qualification.Value;
            return View(ks5Model);
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

        [Route("school/{urn}/{schoolName}/16-to-19-performance/subject-entered", Name = RouteConstants.KS5AcademicPerformanceSubjectsEntered)]
        public async Task<IActionResult> SubjectEntered([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName,
            CancellationToken ct)
        {
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

            var ks5Model = KS5ViewModel.Map(schoolDetails);
            return View(ks5Model);
        }
    }
}
