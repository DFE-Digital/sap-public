using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Models;

namespace SAPPub.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public record School(string Name, double Lat, double Lon);

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Map()
    {
        var schools = GetPrimarySchools();
        return View("~/Views/Map/Index.cshtml", schools);
    }

    // Shared data source (replace with data from csv later)
    private static IReadOnlyList<School> GetPrimarySchools() => new[]
    {
        new School("Manorfield Primary School",        51.5151, -0.0195),
        new School("Culloden Primary Academy",         51.5134, -0.0138),
        new School("Mayflower Primary School",         51.5147, -0.0209),
        new School("Lansbury Lawrence Primary School", 51.5158, -0.0173),
        new School("Stebon Primary School",            51.5176, -0.0201),
        new School("Old Palace Primary School",        51.5183, -0.0125),
        new School("Sir William Burrough Primary",     51.5112, -0.0224),
        new School("Bygrove Primary School",           51.5153, -0.0087),
        new School("Marner Primary School",            51.5187, -0.0265),
        new School("St Paul's Way Foundation School",  51.5187, -0.0265)
        //new School("St Paul's Way Foundation School",  51.5240, -0.0055)
    };

    // JSON endpoint used by map-schools.js
    [HttpGet("/map/schools")]
    public IActionResult Schools()
    {
        var data = GetPrimarySchools().Select(s => new { name = s.Name, lat = s.Lat, lon = s.Lon });
        return Json(data);
    }

    public IActionResult Accordion()
    {
        return View("~/Views/Accordion/Index.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
