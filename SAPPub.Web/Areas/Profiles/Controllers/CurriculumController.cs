using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Web.Areas.Profiles.Filters;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
public class CurriculumController(ILogger<CurriculumController> logger, IFeatureManager featureManager) : Controller
{
    [Route("school/{urn}/{schoolName}/curriculum", Name = RouteConstants.CurriculumRoot)]
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
        return View("Error");
    }

    [HttpGet]
    [FeatureGate(Constants.Constants.EnablePrimary)]
    [ServiceFilter(typeof(PrimaryQueryValidationFilter))]
    [Route("school/{urn}/{schoolName}/curriculum/primary", Name = RouteConstants.PrimaryCurriculum)]
    public async Task<IActionResult> KS2(
        [FromServices] IEstablishmentService establishmentService,
        string urn, string schoolName, CancellationToken ct)
    {
        var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);
        var model = ViewModels.KS2.CurriculumAndExtraCurricularActivitiesViewModel.Map(establishmentDetails);
        return View(model);
    }

    [HttpGet]
    [Route("school/{urn}/{schoolName}/curriculum/secondary", Name = RouteConstants.SecondaryCurriculumAndExtraCurricularActivities)]
    public async Task<IActionResult> KS4(
        [FromServices] IEstablishmentService establishmentService,
        string urn, string schoolName, CancellationToken ct)
    {
        var establishmentDetails = await establishmentService.GetEstablishmentAsync(urn, ct);
        var isPrimaryFeatureEnabled = await featureManager.IsEnabledAsync(Constants.Constants.EnablePrimary);
        var model = ViewModels.KS4.CurriculumAndExtraCurricularActivitiesViewModel.Map(establishmentDetails, isPrimaryFeatureEnabled);
        return View(model);
    }
}
