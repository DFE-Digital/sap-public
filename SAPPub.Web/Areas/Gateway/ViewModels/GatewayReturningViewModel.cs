using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Areas.Gateway.ViewModels
{
    public class GatewayReturningViewModel
    {
        [EmailAddress(ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        [Required(ErrorMessage = "Enter your email address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
