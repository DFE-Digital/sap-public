using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Areas.Gateway.ViewModels
{
    public class GatewayNewUserViewModel
    {
        public Guid LocalAuthorityId { get; set; }
        public string LocalAuthorityName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        [Required(ErrorMessage = "Enter your email address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Accept or reject analytics cookies")]
        public string AcceptCookies { get; set; } = string.Empty;
    }
}
