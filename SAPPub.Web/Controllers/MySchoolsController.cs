using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Exceptions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Constants;
using SAPPub.Web.Models.MySchools;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers;

[Route("my-schools")]
[FeatureGate(EstablishmentComparisonEnabled)]
public class MySchoolsController(
    IEstablishmentComparisonService mySchoolListService,
    IEstablishmentService establishmentService) : Controller
{
    [HttpGet]
    [Route("view", Name = RouteConstants.MySchoolsView)]
    
    public async Task<IActionResult> Index()
    {
        var establishmentUrns = mySchoolListService.GetSavedEstablishments();

        if (establishmentUrns == null || establishmentUrns.Count == 0)
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
    [Route("no-schools-added", Name = RouteConstants.MySchoolsNoSchoolsView)]
    public IActionResult NoSchoolsAdded()
    {
        if (mySchoolListService.GetSavedEstablishments().Count > 0)
        {
            // In case user has this bookmarked as "their list"
            return RedirectToAction(nameof(Index));
        }

        return View();
    }

    [HttpPost]
    [Route("my-schools/view", Name = RouteConstants.MySchoolsView)]
    public async Task<IActionResult> Index(MySchoolsListViewModel viewModel)
    {
        var SelectedEstablishmentUrns = viewModel.SelectedEstablishmentUrns;
        if (SelectedEstablishmentUrns.Count < 2 || SelectedEstablishmentUrns.Count > 6)
        {
            ModelState.AddModelError(nameof(SelectedEstablishmentUrns), "Please select between 2 and 6 schools to compare");
            var schoolsList = await PopulateSchoolList();
            return View(new MySchoolsListViewModel
            {
                MySchools = schoolsList,
                SelectedEstablishmentUrns = SelectedEstablishmentUrns
            });
        }
        else return RedirectToAction("Index", "CompareSecondary", new { establishmentUrns = SelectedEstablishmentUrns });
    }

    private async Task<List<MySchoolModel>> PopulateSchoolList()
    {
        var establishmentUrns = mySchoolListService.GetSavedEstablishments();

        //if (establishmentUrns == null || !establishmentUrns.Any())
        //{
        //    return RedirectToAction("AddNoSchoolsActionHere");
        //}
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

        return model;
    }
}
