using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
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
            return RedirectToAction("AdvancedLevel", new {  urn, schoolName, qualification = "alevel" });

            // if establishment has Level 2 data
            //return RedirectToAction("IntermediateLevel", new { urn = urn, schoolName = schoolName, qualification = "techcert" });
        }


        [Route("school/{urn}/{schoolName}/16-to-19-performance/advanced-level", Name = RouteConstants.KS5AcademicPerformanceLevel3)]
        public IActionResult AdvancedLevelRedirect(string urn, string schoolName, int? level3qualification)
        {
            level3qualification ??= 1;

            var qualSelected = ((Core.Enums.KS5Qualifications.Level3)level3qualification).ToString();
            if (string.IsNullOrWhiteSpace(qualSelected))
            {
                qualSelected = ((Core.Enums.KS5Qualifications.Level3)1).ToString();
            }

            return RedirectToAction("AdvancedLevel", new { urn = urn, schoolName = schoolName, qualification = qualSelected.ToLower() });
        }


        [Route("school/{urn}/{schoolName}/16-to-19-performance/advanced-level/{qualification}", Name = RouteConstants.KS5AcademicPerformanceLevel3Filter)]
        public async Task<IActionResult> AdvancedLevel([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName, Core.Enums.KS5Qualifications.Level3? qualification,
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
            ks5Model.Level3Qualification = qualification.Value;
            return View(ks5Model);
        }

        [Route("school/{urn}/{schoolName}/16-to-19-performance/intermediate-level", Name = RouteConstants.KS5AcademicPerformanceLevel2)]
        public IActionResult IntermediateLevelRedirect(string urn, string schoolName, int? level2qualification)
        {
            // if establishment has Level 2 data 
            level2qualification ??= 1;

            var qualSelected = ((Core.Enums.KS5Qualifications.Level2)level2qualification).ToString();
            if (string.IsNullOrWhiteSpace(qualSelected))
            {
                qualSelected = ((Core.Enums.KS5Qualifications.Level2)1).ToString();
            }

            return RedirectToAction("IntermediateLevel", new { urn = urn, schoolName = schoolName, qualification = qualSelected.ToLower() });
        }


        [Route("school/{urn}/{schoolName}/16-to-19-performance/intermediate-level/{qualification}", Name = RouteConstants.KS5AcademicPerformanceLevel2Filter)]
        public async Task<IActionResult> IntermediateLevel([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName, Core.Enums.KS5Qualifications.Level2? qualification,
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
        public async Task<IActionResult> EnglishAndMaths([FromServices] IAboutSchoolService aboutSchoolService, 
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
