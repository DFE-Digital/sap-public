using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.EstablishmentComparison;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.ViewComponents.SaveEstablishmentButton;

public class SaveEstablishmentButton(IEstablishmentComparisonService establishmentComparisonService, IFeatureManager featureManager) : ViewComponent
{
    private readonly IEstablishmentComparisonService _establishmentComparisonService = establishmentComparisonService;
    private readonly IFeatureManager _featureManager = featureManager;

    public async Task<IViewComponentResult> InvokeAsync(string urn, string saveText = Constants.Constants.EstablishmentComparisonSave, string savedText = Constants.Constants.EstablishmentComparisonSaved)
    {
        var viewModel = new EstablishmentComparisonButtonViewModel
        {
            Urn = urn,
            SavedText = savedText,
            SaveText = saveText,
            IsSaved = _establishmentComparisonService.IsSaved(urn),
            IsComparisonLimitReached = _establishmentComparisonService.IsComparisonLimitReached(),
            AddedSchoolListPageUrl = _establishmentComparisonService.GetAddedSchoolListPageUrl(),
            IsFeatureEnabled = await _featureManager.IsEnabledAsync(EstablishmentComparisonEnabled),
        };
        
        return View("~/ViewComponents/SaveEstablishmentButton/Default.cshtml", viewModel);
    }
}
