using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Controllers
{
    public class SchoolController(IEstablishmentService establishmentService, IFeatureManager featureManager) : Controller
    {
        private readonly IEstablishmentService _establishmentService = establishmentService;

        [HttpGet]
        [Route("school/{urn}")]
        public async Task<IActionResult> Index(string urn, CancellationToken ct)
        {
            var schoolDetails = await _establishmentService.GetEstablishmentAsync(urn, ct);

            var route = await featureManager.IsEnabledAsync(
                Constants.Constants.EnableOverview)
                ? RouteConstants.Overview
                : RouteConstants.AboutTheSchool;

            return RedirectToRoute(route, new { urn, schoolDetails.EstablishmentNameClean });
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}")]
        public async Task<IActionResult> Index(string urn, string schoolName)
        {
            var route = await featureManager.IsEnabledAsync(
                Constants.Constants.EnableOverview)
                ? RouteConstants.Overview
                : RouteConstants.AboutTheSchool;

            return RedirectToRoute(route, new { urn, schoolName });
        }

        [HttpGet("/map/schools/{urn}")]
        public async Task<IActionResult> Schools(string urn, CancellationToken ct)
        {
            var data = await _establishmentService.GetEstablishmentAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(data?.URN))
            {
                return Json(null);
            }

            var longLat = MappingHelper.ConvertToLatLon(data.Easting, data.Northing);

            return Json(new { name = data.EstablishmentName, lat = longLat?.Latitude, lon = longLat?.Longitude });
        }
    }
}
