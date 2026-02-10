using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Gateway;
using SAPPub.Web.Areas.Gateway.ViewModels;

namespace SAPPub.Web.Areas.Gateway.Controllers
{
    [Area("Gateway")]
    public class GatewayController(
        IGatewayUserService UserService, 
        IGatewayLocalAuthorityService localAuthorityService, 
        IGatewayUserAuditService auditService, 
        IGatewayUserLAService gatewayUserLAService, 
        IEmailService emailService, 
        ILogger<GatewayController> logger) : Controller
    {
        private readonly IGatewayUserService _userService = UserService;
        private readonly IGatewayUserAuditService _auditService = auditService;
        private readonly IGatewayLocalAuthorityService _localAuthorityService = localAuthorityService;
        private readonly IGatewayUserLAService _gatewayUserLAService = gatewayUserLAService;
        private readonly ILogger<GatewayController> _logger = logger;
        private readonly IEmailService _emailService = emailService;

        [HttpGet]
        [Route("gateway/welcome/{id}")]
        public IActionResult Welcome(string id)
        {
            //Check id is in allowed LAs list
            //If invalid id, return error view
            var localAuthority = _localAuthorityService.GetByName(id);
            if (localAuthority == null)
            {
                return View("GatewayError");
            }

            //If valid id, return welcome view
            var viewModel = new GatewayWelcomeViewModel
            {
                LocalAuthority = localAuthority.LocalAuthorityName,
                LocalAuthorityId = localAuthority.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("gateway/welcome/{id}")]
        public IActionResult Welcome(GatewayWelcomeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.NewOrReturning == "new")
                {
                    return RedirectToAction("NewVisitor", new { id = viewModel.LocalAuthority });
                }
                else if (viewModel.NewOrReturning == "return")
                {
                    return RedirectToAction("Returning");
                }

                if (string.IsNullOrWhiteSpace(viewModel.LocalAuthority))
                {
                    ModelState.AddModelError("", "There was a problem, please try again from the beginning.");
                }

            }

            return View(viewModel);
        }



        #region Returning user

        [HttpGet]
        [Route("gateway/returning")]
        public IActionResult Returning()
        {
            var model = new GatewayReturningViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("gateway/returning")]
        public IActionResult Returning(GatewayReturningViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check DB for email
                var user = _userService.GetByEmail(viewModel.EmailAddress);
                if (user == null)
                {
                    ModelState.AddModelError("EmailAddress", "That email address wasn't found to be registered, please use the registration link.");
                    return View(viewModel);
                }

                // Check expiry
                if (user.RegisteredOn < DateTime.UtcNow.AddDays(-7))
                {
                    ModelState.AddModelError("EmailAddress", "Registration occurred over 7 days ago, thank you for your time.");
                    return View(viewModel);
                }

                _auditService.Insert(user.Id);

                var hoursLeft = (user.RegisteredOn.AddDays(7) - DateTime.UtcNow).TotalHours;

                // Set cookie
                Response.Cookies.Append("gateway", user.Id.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddHours(hoursLeft),
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    HttpOnly = true
                });

                // Redirect to application
                return RedirectToAction("Index", "Home");
            }
            return View(viewModel);
        }

        #endregion

        #region New visitor

        [HttpGet]
        [Route("gateway/newvisitor/{id}")]
        public IActionResult NewVisitor(string id)
        {
            var model = new GatewayNewUserViewModel();

            var localAuthority = _localAuthorityService.GetByName(id.Replace("-", " "));
            if (localAuthority == null)
            {
                return View("GatewayError");
            }

            if (!_gatewayUserLAService.CanRegisterNewUsers(localAuthority.Id))
            {
                return RedirectToAction("Closed");
            }

            Request.Cookies.TryGetValue("analytics_preference", out string? analyticsAccepted);

            if (!string.IsNullOrWhiteSpace(analyticsAccepted))
            {
                model.AcceptCookies = analyticsAccepted;
            }

            model.LocalAuthorityName = id;
            model.LocalAuthorityId = localAuthority.Id;
            return View(model);
        }

        [HttpPost]
        [Route("gateway/newvisitor/{id}")]
        public IActionResult NewVisitor(GatewayNewUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check for existing email
                // If exists, redirect or return error?
                var user = _userService.GetByEmail(viewModel.EmailAddress);
                if (user != null)
                {
                    ModelState.AddModelError("EmailAddress", "That user already exists, please log in");
                    return View(viewModel);
                }


                // If not, create new record
                var newUser = new GatewayUser()
                {
                    EmailAddress = viewModel.EmailAddress,
                    LocalAuthorityId = viewModel.LocalAuthorityId,
                    CookiePrefs = true
                };
                var newUserId = _userService.Insert(newUser);

                _auditService.Insert(newUserId);

                // Set cookie
                Response.Cookies.Append("gateway", newUserId.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    HttpOnly = true
                });
                // Send Email
                _emailService.SendGatewayEmail(viewModel.EmailAddress);

                return RedirectToAction("Complete");
            }

            return View(viewModel);
        }

        // Registration complete
        [HttpGet]
        [Route("gateway/complete")]
        public IActionResult Complete()
        {
            return View();
        }

        #endregion


        #region Service closed

        [HttpGet]
        [Route("gateway/closed")]
        public IActionResult Closed()
        {
            return View();
        }

        #endregion

        #region Service Error

        [HttpGet]
        [Route("gateway/error")]
        public IActionResult Error()
        {
            return View("GatewayError");
        }

        #endregion
    }
}
