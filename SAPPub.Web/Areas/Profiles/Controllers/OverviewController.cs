using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Web.Areas.Profiles.ViewModels.Overview;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
[FeatureGate("EnableOverview")]
public class OverviewController(
    ILogger<OverviewController> logger,
    IOverviewService overviewService) : Controller
{
    [HttpGet]
    [Route("school/{urn}/{schoolName}/overview", Name = RouteConstants.Overview)]
    public async Task<IActionResult> Overview(
        string urn,
        string schoolName,
        CancellationToken ct)
    {
        var overviewDetails = await overviewService.GetOverviewAsync(urn, ct);

        if (overviewDetails is null)
        {
            logger.LogWarning("No overview details found for URN: {URN}", urn);

            return View("Error");
        }

        var model = OverviewViewModel.Map(overviewDetails);

        return View(model);
    }
}