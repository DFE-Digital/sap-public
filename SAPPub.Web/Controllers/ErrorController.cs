using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("error/{statusCode:int}")]
    public IActionResult HandleErrorCode(int statusCode)
    {
        Response.StatusCode = statusCode;

        return statusCode switch
        {
            404 => View("PageNotFound"),
            500 => View("ProblemWithService"),
            _ => View("ProblemWithService"),
        };
    }
}
