using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Controllers
{
    public class SecondarySchoolController : Controller
    {
        const string CspPolicy = "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net;";

        private readonly ILogger<SecondarySchoolController> _logger;

        public SecondarySchoolController(ILogger<SecondarySchoolController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/about", Name = RouteConstants.SecondaryAboutSchool)]
        public IActionResult AboutSchool(int urn, string schoolName)
        {
            var model = new AboutSchoolViewModel 
            { 
                Urn = urn,
                SchoolName = schoolName,
                Name = schoolName,
                Telephone = "01234455677",
                Address = "Address line 1",
                Website = "https://design-system.service.gov.uk/components/summary-list/",
                AcademyTrust = "Test Trust",
                AcademyTrustUpdatedIn = "2024",
                LocalAuthority = "Sheffield",
                LocalAuthorityWebsite = "https://design-system.service.gov.uk/components/summary-list/",
                YouDistanceFromThisSchool = "1.5 miles"
            };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/admissions", Name = RouteConstants.SecondaryAdmissions)]
        public IActionResult Admissions(int urn, string schoolName)
        {
            var model = new AdmissionsViewModel { Urn = urn, SchoolName = schoolName };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/attendance", Name = RouteConstants.SecondaryAttendance)]
        public IActionResult Attendance(int urn, string schoolName)
        {
            var model = new AttendanceViewModel { Urn = urn, SchoolName = schoolName };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/curriculum-and-extra-curricular-activities", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
        public IActionResult CurriculumAndExtraCurricularActivities(int urn, string schoolName)
        {
            var model = new CurriculumAndExtraCurricularActivitiesViewModel { Urn = urn, SchoolName = schoolName };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-pupil-progress", Name = RouteConstants.SecondaryAcademicPerformancePupilProgress)]
        public IActionResult AcademicPerformancePupilProgress(int urn, string schoolName)
        {
            Response.Headers["Content-Security-Policy"] = CspPolicy;
            var gcseDatamodel = new GcseDataViewModel
            {
                Lables = ["School", "Sheffield Average", "England Average"],
                GcseData = [75, 65, 55],
                ChartTitle = "GCSE English and Maths (Grade 5 and above)",
            };

            var model = new AcademicPerformancePupilProgressViewModel 
            { 
                Urn = urn,
                SchoolName = schoolName,
                GcseChartData = gcseDatamodel,
            };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-english-and-maths-results", Name = RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults)]
        public IActionResult AcademicPerformanceEnglishAndMathsResults(int urn, string schoolName)
        {
            var model = new AcademicPerformanceEnglishAndMathsResultsViewModel
            {
                Urn = urn,
                SchoolName = schoolName,
            };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-subjects-entered", Name = RouteConstants.SecondaryAcademicPerformanceSubjectsEntered)]
        public IActionResult AcademicPerformanceSubjectsEntered(int urn, string schoolName)
        {
            var model = new AcademicPerformanceSubjectsEnteredViewModel
            {
                Urn = urn,
                SchoolName = schoolName,
            };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/destinations", Name = RouteConstants.SecondaryDestinations)]
        public IActionResult Destinations(int urn, string schoolName)
        {
            var model = new DestinationsViewModel { Urn = urn, SchoolName = schoolName };
            return View(model);
        }
    }
}
