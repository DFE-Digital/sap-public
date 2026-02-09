using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;
using static SAPPub.Web.Models.SecondarySchool.AcademicPerformanceEnglishAndMathsResultsViewModel;

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
        public async Task<IActionResult> Admissions([FromServices] IAdmissionsService admissionsService, string urn, string schoolName)
        {
            var admissionsDetails = await admissionsService.GetAdmissionsDetailsAsync(urn);
            var model = AdmissionsViewModel.MapFrom(admissionsDetails, urn, schoolName);
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
        public IActionResult AcademicPerformanceEnglishAndMathsResults([FromServices] IAcademicPerformanceEnglishAndMathsResultsService academicPerformanceEnglishAndMathsResultsService, string urn, string schoolName, GcseGradeDataSelection SelectedGrade = GcseGradeDataSelection.Grade5AndAbove)
        {
            Response.Headers["Content-Security-Policy"] = CspPolicy;
            var establishment = _establishmentService.GetEstablishment(urn);
            if (establishment?.URN == null)
            {
                return View("Error");
            }

            var results = academicPerformanceEnglishAndMathsResultsService.ResultsOfSpecifiedGradeAndAbove(urn, Convert.ToInt32(SelectedGrade));

            var model = AcademicPerformanceEnglishAndMathsResultsViewModel.Map(establishment, results, SelectedGrade);

            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-subjects-entered", Name = RouteConstants.SecondaryAcademicPerformanceSubjectsEntered)]
        public IActionResult AcademicPerformanceSubjectsEntered([FromServices] IEstablishmentSubjectEntriesService subjectEntriesService, string urn, string schoolName)
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
