using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Helpers;
using System.Xml.Linq;

namespace SAPPub.Web.Controllers
{
    public class SchoolController : Controller
    {
        private readonly IEstablishmentService _establishmentService;
        public SchoolController(IEstablishmentService establishmentService)
        {
            _establishmentService = establishmentService;
        }

        [HttpGet]
        [Route("school/{urn}")]
        public IActionResult Index(string urn)
        {
            var schoolDetails = _establishmentService.GetEstablishment(urn);
            return RedirectToRoute(RouteConstants.SecondaryAboutSchool, new { urn = urn, schoolName = schoolDetails.EstablishmentNameClean });
        }

        [HttpGet]
        [Route("school/{urn}/{schoolName}")]
        public IActionResult Index(string urn, string schoolName)
        {
            return RedirectToRoute(RouteConstants.SecondaryAboutSchool, new { urn = urn, schoolName = schoolName });
        }

        [HttpGet("/map/schools/{urn}")]
        public IActionResult Schools(string urn)
        {
            var data = _establishmentService.GetEstablishment(urn);
            if (data?.URN == null)
            {
                return Json(null);
            }
            var longLat = Helpers.MappingHelper.ConvertToLongLat(data.Easting, data.Northing);
            return Json(new { name = data.EstablishmentName, lat = longLat?.Latitude, lon = longLat?.Longitude });
        }
    }
}
