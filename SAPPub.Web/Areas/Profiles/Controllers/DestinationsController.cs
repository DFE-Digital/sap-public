using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    public class DestinationsController : Controller
    {
        [Route("school/{urn}/{schoolName}/destinations")]
        public IActionResult Index(string urn, string schoolName)
        {
            // if establishment has KS4 
            //return RedirectToAction("AdvancedLevel", new { filter = "alevel" });

            // if establishment has ks5 
            return RedirectToAction("AdvancedLevel", new { filter = "alevel" });
        }

        [Route("school/{urn}/{schoolName}/destinations/16to19")]
        public IActionResult KS5(string urn, string schoolName)
        {
            // if establishment has KS4 
            //return RedirectToAction("AdvancedLevel", new { filter = "alevel" });

            // if establishment has ks5 
            return RedirectToAction("AdvancedLevel", new { filter = "alevel" });
        }

        [Route("school/{urn}/{schoolName}/destinations/16to19-higher-level-study")]
        public IActionResult KS5HigherLevel(string urn, string schoolName)
        {
            // if establishment has KS4 
            //return RedirectToAction("AdvancedLevel", new { filter = "alevel" });

            // if establishment has ks5 
            return RedirectToAction("AdvancedLevel", new { filter = "alevel" });
        }
    }
}
