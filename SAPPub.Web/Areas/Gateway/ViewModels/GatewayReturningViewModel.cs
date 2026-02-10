using System.ComponentModel.DataAnnotations;

namespace SAPPub.Web.Areas.Gateway.ViewModels
{
    public class GatewayReturningViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Required(ErrorMessage = "Please enter your email address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
