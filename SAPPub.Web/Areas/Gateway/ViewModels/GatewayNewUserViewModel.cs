using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Areas.Gateway.ViewModels
{
    public class GatewayNewUserViewModel
    {
        public Guid LocalAuthorityId { get; set; }
        public string LocalAuthorityName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "You must accept or reject the analytics cookies to continue")]
        public string AcceptCookies { get; set; } = string.Empty;
    }
}
