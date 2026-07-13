using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using NpgsqlTypes;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Web.Areas.Profiles.ViewModels.Destinations;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Areas.Profiles.Controllers
{
    [Area("Profiles")]

    public class DestinationsController(ILogger<DestinationsController> logger) : Controller
    {
        [Route("school/{urn}/{schoolName}/destinations", Name = RouteConstants.DestinationsRoot)]
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

            if (!destinationDetails.IsKS4)
            {
                return View("Error");
            }

            var model = DestinationsViewModel.Map(destinationDetails);
            return View(model);
        }

        [FeatureGate("Enable16to19")]
        [Route("school/{urn}/{schoolName}/destinations/16-to-19", Name = RouteConstants.KS5Destinations)]
        public async Task<IActionResult> KS5([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName,
            CancellationToken ct)
        {
            // This all needs to be refactored into a model, but this gets the structure up
            var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
            {
                logger.LogWarning("No establishment details found for URN: {URN}", urn);
                return View("Error");
            }

            if (!schoolDetails.IsKS5)
            {
                return View("Error");
            }
            var model = AboutSchoolViewModel.Map(schoolDetails);
            return View(model);
        }

        [FeatureGate("Enable16to19")]
        [Route("school/{urn}/{schoolName}/destinations/16-to-19-higher-level-study", Name = RouteConstants.KS5DestinationsHigher)]
        public async Task<IActionResult> KS5HigherLevel([FromServices] IAboutSchoolService aboutSchoolService, 
            string urn, string schoolName,
            CancellationToken ct)
        {
            // This all needs to be refactored into a model, but this gets the structure up
            var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
            {
                logger.LogWarning("No establishment details found for URN: {URN}", urn);
                return View("Error");
            }

            if (!schoolDetails.IsKS5)
            {
                return View("Error");
            }
            var model = AboutSchoolViewModel.Map(schoolDetails);
            return View(model);
        }
    }
}
