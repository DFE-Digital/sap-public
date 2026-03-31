using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Helpers;

namespace SAPPub.Web.Controllers;

public class HelpController : Controller
{
    [HttpGet]
    [Route("terms-and-conditions", Name = RouteConstants.TermsAndConditions)]
    public IActionResult TermsAndConditions()
    {
        return View();
    }
}
