using Microsoft.AspNetCore.Mvc;

namespace SAPPub.Web.Controllers;

public class ErrorController(IHostEnvironment env) : Controller
{
    /// <summary>
    /// This action is used to test the unhandled exception handling in non-production environments. 
    /// In production, it returns a 404 Not Found response
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public IActionResult Throw()
    {
        if (env.EnvironmentName == Environments.Production)
        {
            return NotFound();
        }
        else
        {
            throw new System.Exception("Test unhandled exception");
        }
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
