using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Constants;

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
