using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.MySchools;

namespace SAPPub.Web.Controllers;

public class MySchoolsController(
    IEstablishmentComparisonService mySchoolListService,
    IEstablishmentService establishmentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        var establishmentUrns = mySchoolListService.GetSavedEstablishments();

        if (establishmentUrns == null || !establishmentUrns.Any())
        {
            return RedirectToAction("AddNoSchoolsActionHere");
        }

        var establishments = await Task.WhenAll(establishmentUrns
            .Select(urn => establishmentService.GetEstablishmentAsync(urn)));

        var model = establishments
            .Where(establishment => establishment != null)
            .Select(e => MySchoolModel.MapFrom(e))
            .ToList();

        var viewModel = new MySchoolsListViewModel
        {
            MySchools = model
        };

        return View(viewModel);
    }
}
