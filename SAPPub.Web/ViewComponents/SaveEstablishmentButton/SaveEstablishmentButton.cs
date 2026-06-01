using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.EstablishmentComparison;

namespace SAPPub.Web.ViewComponents.SaveEstablishmentButton
{
    public class SaveEstablishmentButton : ViewComponent
    {
        private readonly IEstablishmentComparisonService _establishmentComparisonService;
        private readonly IFeatureManager _featureManager;

        public SaveEstablishmentButton(IEstablishmentComparisonService establishmentComparisonService, IFeatureManager featureManager)
        {
            _establishmentComparisonService = establishmentComparisonService;
            _featureManager = featureManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string urn, string schoolName, string saveText = Constants.Constants.EstablishmentComparisonSave, string savedText = Constants.Constants.EstablishmentComparisonSaved)
        {
            var viewModel = new EstablishmentComparisonButtonViewModel
            {
                Urn = urn,
                SavedText = savedText,
                SaveText = saveText,
                IsSaved = _establishmentComparisonService.IsSaved(urn),
                ShowAddSuccessNotification = TempData["BannerAddSuccess"] is not null,
                ShowRemoveSuccessNotification = TempData["BannerRemoveSuccess"] is not null,
                IsComparisonLimitReached = _establishmentComparisonService.IsComparisonLimitReached(),
                ComparisonPageUrl = _establishmentComparisonService.GetComparisonPageUrl(),
                IsFeatureEnabled = await _featureManager.IsEnabledAsync("EstablishmentComparisonEnabled")
            };
            
            return View("~/ViewComponents/SaveEstablishmentButton/Default.cshtml", viewModel);
        }
    }
}
