using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.MySchools;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.ViewComponents.MySchools;

public class AddSchoolButton(IMySchoolsListService mySchoolsListService, IFeatureManager featureManager) : ViewComponent
{
    private readonly IMySchoolsListService _mySchoolsListService = mySchoolsListService;
    private readonly IFeatureManager _featureManager = featureManager;

    public async Task<IViewComponentResult> InvokeAsync(string urn, bool isSearchPage = false)
    {
        var viewModel = new AddSchoolButtonViewModel
        {
            Urn = urn,
            IsSearchPage = isSearchPage,
            SavedText = isSearchPage ? Saved : MySchoolsSaved,
            SaveText = isSearchPage ? Save : MySchoolsSave,
            IsSaved = _mySchoolsListService.IsSaved(urn),
            IsListLimitReached = _mySchoolsListService.IsListLimitReached(),
            IsFeatureEnabled = await _featureManager.IsEnabledAsync(EstablishmentComparisonEnabled)
        };

        return View("~/ViewComponents/MySchools/AddSchoolButton/Default.cshtml", viewModel);
    }
}
