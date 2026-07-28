using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Web.Areas.Profiles.Filters;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
public class AdmissionsController(ILogger<AdmissionsController> logger, IFeatureManager featureManager) : Controller
{
    [Route("school/{urn}/{schoolName}/admissions", Name = RouteConstants.AdmissionsRoot)]
    public async Task<IActionResult> Index([FromServices] IAboutSchoolService aboutSchoolService,
            string urn, string schoolName,
            CancellationToken ct)
    {

        var schoolDetails = await aboutSchoolService.GetAboutSchoolDetailsAsync(urn, ct);

        if (string.IsNullOrWhiteSpace(schoolDetails.Urn))
        {
            logger.LogWarning("No establishment details found for URN: {URN}", urn);
            return View("Error");
        }

        if (await featureManager.IsEnabledAsync(Constants.Constants.EnablePrimary) && schoolDetails.IsKS2)
        {
            return RedirectToAction("KS2", new { urn, schoolName });
        }
        else if (schoolDetails.IsKS4)
        {
            return RedirectToAction("KS4", new { urn, schoolName });
        }
        else if (await featureManager.IsEnabledAsync(Constants.Constants.Enable16to19) && schoolDetails.IsKS5)
        {
            return RedirectToAction("KS5", new { urn, schoolName });
        }
        return View("Error");
    }

    [HttpGet]
    [FeatureGate(Constants.Constants.EnablePrimary)]
    [ServiceFilter(typeof(PrimaryQueryValidationFilter))]
    [Route("school/{urn}/{schoolName}/admissions/primary", Name = RouteConstants.PrimaryAdmissions)]
    public async Task<IActionResult> KS2(
        [FromServices] IAdmissionsService admissionsService,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var admissionsDetails = await admissionsService.GetAdmissionsDetailsAsync(urn, ct);
        var model = ViewModels.KS2.AdmissionsViewModel.MapFrom(admissionsDetails, urn);
        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/admissions/secondary", Name = RouteConstants.SecondaryAdmissions)]
    public async Task<IActionResult> KS4(
        [FromServices] IAdmissionsService admissionsService,
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var admissionsDetails = await admissionsService.GetAdmissionsDetailsAsync(urn, ct);
        var model = SAPPub.Web.Models.SecondarySchool.AdmissionsViewModel.MapFrom(admissionsDetails, urn);
        return View(model);
    }
}
