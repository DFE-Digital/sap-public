using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Web.Areas.Profiles.ViewModels.Destinations;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Controllers
{
    public class SecondarySchoolController(
        ILogger<SecondarySchoolController> logger,
        IEstablishmentService establishmentService) : Controller
    {
        [HttpGet]
        [Route("school/{urn}/{schoolName}/about", Name = RouteConstants.SecondaryAboutSchool)]
        public async Task<IActionResult> AboutSchool(
            [FromServices] IAboutSchoolService aboutSchoolService,
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
            {
                logger.LogWarning("No establishment details found for URN: {URN}", urn);
                return View("Error");
            }

            var model = AboutSchoolViewModel.Map(schoolDetails);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/admissions", Name = RouteConstants.SecondaryAdmissions)]
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
        [Route("school/{urn}/{schoolName}/secondary/attendance", Name = RouteConstants.SecondaryAttendance)]
        public async Task<IActionResult> Attendance(
            [FromServices] IAttendanceService attendanceService,
            string urn,
            string schoolName,
            CancellationToken ct)
        {
            var attendanceDetails = await attendanceService.GetAttendenceDetailsAsync(urn, ct);
            var model = AttendanceViewModel.Map(attendanceDetails);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/curriculum-and-extra-curricular-activities", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
        public async Task<IActionResult> CurriculumAndExtraCurricularActivities(string urn, string schoolName, CancellationToken ct)
        {
            var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);
            var model = CurriculumAndExtraCurricularActivitiesViewModel.Map(establishmentDetails);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-attainment-and-progress", Name = RouteConstants.SecondaryAcademicPerformanceAttainmentAndProgress)]
        public async Task<IActionResult> AcademicPerformanceAttainmentAndProgress(
            [FromServices] IAttainmentAndProgressService attainmentAndProgressService,
            string urn,
            string schoolName,
            AcademicYearSelection selectedAcademicYear = AcademicYearSelection.Current,
            CancellationToken ct = default)
        {
            var results = await attainmentAndProgressService
                .GetAttainmentAndProgressAsync(urn, selectedAcademicYear, ct);

            var model = AcademicPerformanceAttainmentAndProgressViewModel.Map(results, selectedAcademicYear);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-english-and-maths-results", Name = RouteConstants.SecondaryAcademicPerformanceEnglishAndMathsResults)]
        public async Task<IActionResult> AcademicPerformanceEnglishAndMathsResults(
            [FromServices] IAcademicPerformanceEnglishAndMathsResultsService academicPerformanceEnglishAndMathsResultsService,
            string urn,
            string schoolName,
            GcseGradeDataSelection SelectedGrade = GcseGradeDataSelection.Grade5AndAbove,
            CancellationToken ct = default)
        {
            var results = await academicPerformanceEnglishAndMathsResultsService
                .GetEnglishAndMathsResultsAsync(urn, Convert.ToInt32(SelectedGrade), ct);

            var model = AcademicPerformanceEnglishAndMathsResultsViewModel.Map(results, SelectedGrade);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-subjects-entered", Name = RouteConstants.SecondaryAcademicPerformanceSubjectsEntered)]
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
        [Route("school/{urn}/{schoolName}/secondary/destinations", Name = RouteConstants.SecondaryDestinations)]
        public async Task<IActionResult> Destinations(
            [FromServices] IDestinationsService destinationsService,
            string urn, string schoolName, CancellationToken ct)
        {
            var destinationDetails = await destinationsService.GetDestinationsDetailsAsync(urn, ct);

            var model = DestinationsViewModel.Map(destinationDetails);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AdditionalMeasures(
            string urn, string schoolName, CancellationToken ct)
        {
            return View();
        }
    }
}
