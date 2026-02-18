using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Controllers
{
    public class SchoolController(IEstablishmentService establishmentService) : Controller
    {
        private readonly IEstablishmentService _establishmentService = establishmentService;

        [HttpGet]
        [Route("school/{urn}")]
        public async Task<IActionResult> Index(string urn, CancellationToken ct)
        {
            var schoolDetails = await _establishmentService.GetEstablishmentAsync(urn, ct);

            return RedirectToRoute(
                RouteConstants.SecondaryAboutSchool,
                new { urn, schoolName = schoolDetails.EstablishmentNameClean });
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}")]
        public IActionResult Index(string urn, string schoolName)
        {
            return RedirectToRoute(RouteConstants.SecondaryAboutSchool, new { urn, schoolName });
        }

        [HttpGet("/map/schools/{urn}")]
        public async Task<IActionResult> Schools(string urn, CancellationToken ct)
        {
            var data = await _establishmentService.GetEstablishmentAsync(urn, ct);

            if (string.IsNullOrWhiteSpace(data?.URN))
            {
                return Json(null);
            }

            var longLat = MappingHelper.ConvertToLongLat(data.Easting, data.Northing);

            return Json(new { name = data.EstablishmentName, lat = longLat?.Latitude, lon = longLat?.Longitude });
        }
    }
}
