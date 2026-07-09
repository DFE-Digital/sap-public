using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Web.Areas.Profiles.ViewModels.Destinations;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]
    public class DestinationsController(ILogger<DestinationsController> logger) : Controller
    {
        [Route("school/{urn}/{schoolName}/destinations")]
        public async Task<IActionResult> Index([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName,
            CancellationToken ct)
        {

            var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
            {
                logger.LogWarning("No establishment details found for URN: {URN}", urn);
                return View("Error");
            }

            if (schoolDetails.IsKS4)
            {
                return RedirectToAction("KS4", new { urn = urn, schoolName = schoolName });
            }
            else if (schoolDetails.IsKS5)
            {
                return RedirectToAction("KS5", new { urn = urn, schoolName = schoolName });
            }
            return View("Error");
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}/destinations/secondary", Name = RouteConstants.SecondaryDestinations)]
        public async Task<IActionResult> KS4(
            [FromServices] IDestinationsService destinationsService,
            string urn, string schoolName, CancellationToken ct)
        {
            var destinationDetails = await destinationsService.GetDestinationsDetailsAsync(urn, ct);

            var model = DestinationsViewModel.Map(destinationDetails);
            return View(model);
        }

        [Route("school/{urn}/{schoolName}/destinations/16to19")]
        public IActionResult KS5(string urn, string schoolName)
        {
            return View();
        }

        [Route("school/{urn}/{schoolName}/destinations/16to19-higher-level-study")]
        public IActionResult KS5HigherLevel(string urn, string schoolName)
        {
            return View();
        }
    }
}
