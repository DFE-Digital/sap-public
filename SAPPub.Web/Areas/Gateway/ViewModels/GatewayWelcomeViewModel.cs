using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Areas.Gateway.ViewModels
{
    public class GatewayWelcomeViewModel : GatewayRootViewModel
    {
        [Required (ErrorMessage = "Please select an option")]
        public bool? HasBeenHereBefore { get; set; }
    }
}
