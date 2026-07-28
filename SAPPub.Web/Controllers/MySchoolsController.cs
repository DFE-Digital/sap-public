using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Banner;
using SAPPub.Web.Models.MySchools;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Controllers;

[Route("my-schools")]
[FeatureGate(EstablishmentComparisonEnabled)]
public class MySchoolsController(
    IMySchoolsListService mySchoolListService,
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

        var schoolsList = await PopulateSchoolList();

        var viewModel = new MySchoolsListViewModel
        {
            MySchools = schoolsList
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
    [Route("view", Name = RouteConstants.SubmitMySchoolsView)]
    public async Task<IActionResult> Index(MySchoolsListViewModel viewModel, string submitAction)
    {
        if (submitAction == ActionRemove)
        {
            if (viewModel.SelectedEstablishmentUrns.Count == 0)
            {
                ModelState.AddModelError(nameof(viewModel.SelectedEstablishmentUrns), "Select at least one school to remove");                
                return View(nameof(Index), await GetMySchoolsListModel(viewModel));
            }

            TempData.Set(SelectedEstablishmentUrns, viewModel.SelectedEstablishmentUrns);
            return RedirectToAction(nameof(RemoveConfirm));            
        }
        else
        {
            if (viewModel.SelectedEstablishmentUrns.Count < 2 || viewModel.SelectedEstablishmentUrns.Count > 6)
            {
                ModelState.AddModelError(nameof(viewModel.SelectedEstablishmentUrns), "Select between 2 and 6 schools to compare");
                var schoolsList = await PopulateSchoolList();
                return View(await GetMySchoolsListModel(viewModel));
            }
            return RedirectToRoute(RouteConstants.CompareSecondaryAboutYourSchools, new { urns = viewModel.SelectedEstablishmentUrns });
        }
    }

    [HttpGet]
    [Route("remove-confirm", Name = RouteConstants.MySchoolsRemoveConfirm)]
    public async Task<IActionResult> RemoveConfirm()
    {
        var establishmentUrns = TempData.Peek<List<string>>(SelectedEstablishmentUrns);

        if (establishmentUrns == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var schoolsList = await PopulateRemoveSchoolList(establishmentUrns);

        var viewModel = new RemoveSchoolsConfirmationViewModel
        {
            Schools = schoolsList
        };

        TempData.Set(SelectedSchoolsForRemoval, schoolsList);

        return View(viewModel);
    }

    [HttpPost]
    [Route("remove-confirm", Name = RouteConstants.SubmitMySchoolsRemoveConfirm)]
    public IActionResult ConfirmRemove()
    {
        var schoolsToRemove = TempData.Get<List<RemoveSchoolViewModel>>(SelectedSchoolsForRemoval);

        if (schoolsToRemove == null)
        {
            return RedirectToAction(nameof(Index));
        }

        mySchoolListService.Remove(schoolsToRemove.Select(s => s.Urn));

        TempData.Remove(SelectedEstablishmentUrns);
        TempData.Set(BannerModel, GetBannerModel(schoolsToRemove));

        return RedirectToAction(nameof(Index));
    }

    private async Task<MySchoolsListViewModel> GetMySchoolsListModel(MySchoolsListViewModel viewModel)
    {
        return new MySchoolsListViewModel
        {
            MySchools = await PopulateSchoolList(),
            SelectedEstablishmentUrns = viewModel.SelectedEstablishmentUrns
        };
    }

    private async Task<List<RemoveSchoolViewModel>> PopulateRemoveSchoolList(IEnumerable<string> urns)
    {
        var establishments = await establishmentService.GetEstablishmentsAsync(urns);

        var model = establishments
            .Where(e => e != null)
            .Select(e => RemoveSchoolViewModel.MapFrom(e!))
            .OrderBy(x => x.Name)
            .ToList();

        return model;
    }

    private async Task<List<MySchoolModel>> PopulateSchoolList()
    {
        var establishmentUrns = mySchoolListService.GetSavedEstablishments();
        var establishments = await establishmentService.GetEstablishmentsAsync(establishmentUrns);

        var model = establishments
            .Where(e => e != null)
            .Select(e => MySchoolModel.MapFrom(e!))
            .OrderBy(x => x.Name)
            .ToList();

        return model;
    }

    private static BannerViewModel GetBannerModel(List<RemoveSchoolViewModel> schoolsToRemove)
    {
        var hasMultipleSchools = schoolsToRemove.Count > 1;
        return new BannerViewModel
        {
            Title = "Success",
            HeaderContent = $"Saved {(hasMultipleSchools ? "schools" : "school")} removed from your compare list",
            BodyContent = hasMultipleSchools ? null : schoolsToRemove[0].Name,
            Role = "alert",
            Type = "success"
        };
    }
}
