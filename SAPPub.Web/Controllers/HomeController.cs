using Microsoft.AspNetCore.Mvc;


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
}
