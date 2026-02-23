using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Web.Controllers
{
    public class SecondarySchoolController(
        ILogger<SecondarySchoolController> logger,
        IEstablishmentService establishmentService,
        IDestinationsService destinationsService) : Controller
    {
        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/about", Name = RouteConstants.SecondaryAboutSchool)]
        public async Task<IActionResult> AboutSchool(string urn, string schoolName, CancellationToken ct)
        {
            var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(establishmentDetails?.URN))
            {
                logger.LogWarning("No establishment details found for URN: {URN}", urn);
                return View("Error");
            }

            var model = AboutSchoolViewModel.Map(establishmentDetails);
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
            var admissionsDetails = await admissionsService.GetAdmissionsDetailsAsync(urn);
            var model = AdmissionsViewModel.MapFrom(admissionsDetails, urn, schoolName);
            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/attendance", Name = RouteConstants.SecondaryAttendance)]
        public IActionResult Attendance(string urn, string schoolName)
        {
            var model = new AttendanceViewModel
            {
                URN = urn,
                SchoolName = schoolName,
                SchoolWebsite = "https://www.gov.uk/"
            };

            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/curriculum-and-extra-curricular-activities", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
        public IActionResult CurriculumAndExtraCurricularActivities(string urn, string schoolName)
        {
            var model = new CurriculumAndExtraCurricularActivitiesViewModel
            {
                URN = urn,
                SchoolName = schoolName
            };

            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/academic-performance-pupil-progress", Name = RouteConstants.SecondaryAcademicPerformancePupilProgress)]
        public IActionResult AcademicPerformancePupilProgress(
            string urn,
            string schoolName,
            int selectedAcademicYear = (int)AcademicYearSelection.AY_2024_2025)
        {
            var model = new AcademicPerformancePupilProgressViewModel
            {
                URN = urn,
                SchoolName = schoolName,
                SelectedAcademicYear = selectedAcademicYear,
            };

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

            var (coreSubjectEntries, additionalSubjectEntries) =
                await subjectEntriesService.GetSubjectEntriesByUrnAsync(urn, ct);

            var model = AcademicPerformanceSubjectsEnteredViewModel.Map(
                establishmentDetails,
                coreSubjectEntries,
                additionalSubjectEntries);

            return View(model);
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/destinations", Name = RouteConstants.SecondaryDestinations)]
        public async Task<IActionResult> Destinations(string urn, string schoolName, CancellationToken ct)
        {
            var destinationDetails = await destinationsService.GetDestinationsDetailsAsync(urn, ct);

            var model = DestinationsViewModel.Map(destinationDetails);
            return View(model);
        }
    }
}
