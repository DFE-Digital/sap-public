using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Web.Areas.Profiles.ViewModels.AboutSchool;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers;


[Area("Profiles")]
public class AboutController(
    ILogger<AboutController> logger, 
    IAboutSchoolService aboutSchoolService) : Controller
{
    [HttpGet]
    [Route("school/{urn}/{schoolName}/about", Name = RouteConstants.SecondaryAboutSchool)]
    public async Task<IActionResult> AboutSchool(string urn, string schoolName, CancellationToken ct)
    {
        var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
        {
            logger.LogWarning("No establishment details found for URN: {URN}", urn);
            return View("Error");
        }

        var model = AboutSchoolViewModel.Map(schoolDetails);
        return View(model);
    }
}
