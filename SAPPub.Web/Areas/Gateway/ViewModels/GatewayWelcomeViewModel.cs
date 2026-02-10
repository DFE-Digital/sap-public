using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Areas.Gateway.ViewModels
{
    public class GatewayWelcomeViewModel
    {
        [Required(ErrorMessage = "Please select an option")]
        public string NewOrReturning { get; set; } = string.Empty;
        public Guid LocalAuthorityId { get; set; }
        public string LocalAuthority { get; set; } = string.Empty;
    }
}
