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
    }
}
