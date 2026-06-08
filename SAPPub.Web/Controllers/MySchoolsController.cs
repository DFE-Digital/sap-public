using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.MySchools;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers;

[Route("my-schools")]
public class MySchoolsController(
    IEstablishmentComparisonService mySchoolListService,
    IEstablishmentService establishmentService) : Controller
{
    [HttpGet]
    [Route("view", Name = RouteConstants.MySchoolsView)]
    [FeatureGate(EstablishmentComparisonEnabled)]
    public async Task<IActionResult> Index()
    {
        var establishmentUrns = mySchoolListService.GetSavedEstablishments();

        if (establishmentUrns == null || !establishmentUrns.Any())
        {
            return RedirectToAction(nameof(NoSchoolsAdded));
        }

        var establishments = await Task.WhenAll(establishmentUrns.Select(async urn =>
                {
                    try
                    {
                        return await establishmentService.GetEstablishmentAsync(urn);
                    }
                    catch (NotFoundException)
                    {
                        return null;
                    }
                }));

        var model = establishments
            .Where(e => e != null)
            .Select(e => MySchoolModel.MapFrom(e!))
            .OrderBy(x => x.Name)
            .ToList();

        var viewModel = new MySchoolsListViewModel
        {
            MySchools = model
        };

        return View(viewModel);
    }

    [HttpGet]
    [Route("no-schools-added")]
    [FeatureGate(EstablishmentComparisonEnabled)]
    public IActionResult NoSchoolsAdded()
    {
        if (mySchoolListService.GetSavedEstablishments().Count > 0)
        {
            // In case user has this bookmarked as "their list"
            return RedirectToAction(nameof(Index));
        }

        return View();
    }
}
