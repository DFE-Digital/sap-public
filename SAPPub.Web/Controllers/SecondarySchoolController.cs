using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Controllers
{
    public class SecondarySchoolController : Controller
    {
        private readonly ILogger<SecondarySchoolController> _logger;

        public SecondarySchoolController(ILogger<SecondarySchoolController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/about", Name = RouteConstants.SecondaryAboutSchool)]
        public IActionResult AboutSchool(int urn, string schoolName)
        {
            return View();
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/admissions", Name = RouteConstants.SecondaryAdmissions)]
        public IActionResult Admissions(int urn, string schoolName)
        {
            return View();
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/secondary/curriculum-and-extra-curricular-activities", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
        public IActionResult CurriculumAndExtraCurricularActivities(int urn, string schoolName)
        {
            return View();
        }
    }
}
