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
        public IActionResult Index(string urn)
        {
            var schoolDetails = _establishmentService.GetEstablishment(urn);
            return RedirectToRoute(RouteConstants.SecondaryAboutSchool, new { urn, schoolName = schoolDetails.EstablishmentNameClean });
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}")]
        public IActionResult Index(string urn, string schoolName)
        {
            return RedirectToRoute(RouteConstants.SecondaryAboutSchool, new { urn, schoolName });
        }

        [HttpGet("/map/schools/{urn}")]
        public IActionResult Schools(string urn)
        {
            var data = _establishmentService.GetEstablishment(urn);

            if (data?.URN == null)
            {
                return Json(null);
            }

            var longLat = MappingHelper.ConvertToLongLat(data.Easting, data.Northing);

            return Json(new { name = data.EstablishmentName, lat = longLat?.Latitude, lon = longLat?.Longitude });
        }
    }
}
