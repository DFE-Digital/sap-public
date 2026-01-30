using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Controllers
{
    public class SecondarySchoolController(
        ILogger<SecondarySchoolController> logger,
        IEstablishmentService establishmentService,
        ISecondarySchoolService secondarySchoolService) : Controller
    {
        const string CspPolicy = "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net;"; //ToDo - Fix this.

        private readonly ILogger<SecondarySchoolController> _logger = logger;
        private readonly IEstablishmentService _establishmentService = establishmentService;
        private readonly ISecondarySchoolService _secondarySchoolService = secondarySchoolService;

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/about", Name = RouteConstants.SecondaryAboutSchool)]
        public IActionResult AboutSchool(string urn, string schoolName)
        {
            var establishmentDetails = _establishmentService.GetEstablishment(urn);

            if (establishmentDetails?.URN == null)
            {
                return View("Error");
            }

            var model = AboutSchoolViewModel.Map(establishmentDetails);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/admissions", Name = RouteConstants.SecondaryAdmissions)]
        public IActionResult Admissions(string urn, string schoolName)
        {
            var model = new AdmissionsViewModel { URN = urn, SchoolName = schoolName };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/attendance", Name = RouteConstants.SecondaryAttendance)]
        public IActionResult Attendance(string urn, string schoolName)
        {
            var model = new AttendanceViewModel { URN = urn, SchoolName = schoolName, SchoolWebsite = "https://www.gov.uk/" };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/curriculum-and-extra-curricular-activities", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
        public IActionResult CurriculumAndExtraCurricularActivities(string urn, string schoolName)
        {
            var model = new CurriculumAndExtraCurricularActivitiesViewModel { URN = urn, SchoolName = schoolName };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-pupil-progress", Name = RouteConstants.SecondaryAcademicPerformancePupilProgress)]
        public IActionResult AcademicPerformancePupilProgress(string urn, string schoolName)
        {
            var model = new AcademicPerformancePupilProgressViewModel
            {
                URN = urn,
                SchoolName = schoolName,
            };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-english-and-maths-results", Name = RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults)]
        public IActionResult AcademicPerformanceEnglishAndMathsResults(string urn, string schoolName)
        {
            Response.Headers["Content-Security-Policy"] = CspPolicy;
            var gcseDatamodel = new GcseDataViewModel
            {
                Labels = ["School", "Sheffield Average", "England Average"],
                GcseData = [75, 65, 55],
                ChartTitle = "GCSE English and Maths (Grade 5 and above)",
            };

            var model = new AcademicPerformanceEnglishAndMathsResultsViewModel
            {
                URN = urn,
                SchoolName = schoolName,
                GcseChartData = gcseDatamodel,
            };
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-subjects-entered", Name = RouteConstants.SecondaryAcademicPerformanceSubjectsEntered)]
        public IActionResult AcademicPerformanceSubjectsEntered(string urn, string schoolName, [FromServices] IEstablishmentSubjectEntriesService subjectEntriesService)
        {
            var establishmentDetails = _establishmentService.GetEstablishment(urn);
            var (coreSubjectEntries, additionalSubjectEntries) = subjectEntriesService.GetSubjectEntriesByUrn(urn);
            if (establishmentDetails?.URN == null)
            {
                return View("Error");
            }

            var model = AcademicPerformanceSubjectsEnteredViewModel.Map(establishmentDetails, coreSubjectEntries, additionalSubjectEntries);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/destinations", Name = RouteConstants.SecondaryDestinations)]
        public IActionResult Destinations(string urn, string schoolName)
        {
            var destinationDetails = _secondarySchoolService.GetDestinationsDetails(urn);

            var model = DestinationsViewModel.Map(destinationDetails);
            return View(model);
        }
    }
}
