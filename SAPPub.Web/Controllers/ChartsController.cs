using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Controllers;

public class ChartsController : Controller
{
    const string CspPolicy = "script-src 'self' 'unsafe-inline' https://cdn.jsdelivr.net;";

    public IActionResult Index()
    {
        Response.Headers["Content-Security-Policy"] = CspPolicy;
        var model = new GcseDataViewModel
        {
            Labels = ["School", "Sheffield Average", "England Average"],
            GcseData = [75, 65, 55],
            ChartTitle = "GCSE English and Maths (Grade 5 and above)",
        };

        return View(model);
    }

    public IActionResult GcseGrades()
    {
        Response.Headers["Content-Security-Policy"] = CspPolicy;
        var model = new GcseGradesViewModel
        {
            SchoolGirls = 76,
            SheffieldGirls = 72,
            EnglandGirls = 74,

            SchoolBoys = 68,
            SheffieldBoys = 80,
            EnglandBoys = 66,

            SchoolSen = 72,
            SheffieldSen = 69,
            EnglandSen = 66,
        };

        return View(model);
    }

    public IActionResult GcseGradesOverTime()
    {
        Response.Headers["Content-Security-Policy"] = CspPolicy;
        var model = new GcseGradesOverTimeViewModel
        {
            Labels = [],
            Years = ["2022 to 2023", "2023 to 2024", "2024 to 2025"],
            School = [74, 76, 38],
            LocalAuthority = [40, 72, 63],
            England = [22, 74, 95]
        };

        return View(model);
    }
}
