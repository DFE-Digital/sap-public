using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Gateway;
using SAPPub.Web.Areas.Gateway.ViewModels;
using SAPPub.Web.Models.Config;
using System.Threading.Tasks;

namespace SAPPub.Web.Areas.Gateway.Controllers
{
    [Area("Gateway")]
    public class GatewayController(
        IGatewayUserService UserService,
        IGatewayLocalAuthorityService localAuthorityService,
        IGatewayUserAuditService auditService,
        IGatewayUserLAService gatewayUserLAService,
        IEmailService emailService,
        ILogger<GatewayController> logger,
        IOptions<GatewayOptions> options) : Controller
    {
        private readonly IGatewayUserService _userService = UserService;
        private readonly IGatewayUserAuditService _auditService = auditService;
        private readonly IGatewayLocalAuthorityService _localAuthorityService = localAuthorityService;
        private readonly IGatewayUserLAService _gatewayUserLAService = gatewayUserLAService;
        private readonly ILogger<GatewayController> _logger = logger;
        private readonly IEmailService _emailService = emailService;
        private readonly GatewayOptions _options = options.Value;

        [HttpGet]
        [Route("gateway/welcome/{id}")]
        public async Task<IActionResult> Welcome(string id)
        {
            //Check id is in allowed LAs list
            //If invalid id, return error view
            var localAuthority = await _localAuthorityService.GetByName(id);
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
        public IActionResult Welcome(GatewayWelcomeViewModel viewModel, string id)
        {
            ViewBag.LAName = id;
            if (ModelState.IsValid)
            {
                if (viewModel.NewOrReturning == "new")
                {
                    return RedirectToAction("NewVisitor", new { id = viewModel.LocalAuthority });
                }
                else if (viewModel.NewOrReturning == "return")
                {
                    return RedirectToAction("Returning", new { id = viewModel.LocalAuthority });
                }

                if (string.IsNullOrWhiteSpace(viewModel.LocalAuthority))
                {
                    ModelState.AddModelError("", "There was a problem, please try again.");
                }

            }

            return View(viewModel);
        }



        #region Returning user

        [HttpGet]
        [Route("gateway/returning/{id}")]
        public IActionResult Returning(string id)
        {
            var model = new GatewayReturningViewModel();
            ViewBag.LAName = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("gateway/returning/{id}")]
        public async Task<IActionResult> Returning(GatewayReturningViewModel viewModel, string id)
        {
            ViewBag.LAName = id;
            if (ModelState.IsValid)
            {
                // Check DB for email
                var user = await _userService.GetByEmailAsync(viewModel.EmailAddress);
                if (user == null)
                {
                    ModelState.AddModelError("EmailAddress", "There is no login registered with that email. Use the registration link to sign up to the service.");
                    return View(viewModel);
                }

                // Check expiry
                if (user.TimerStartedOn < DateTime.UtcNow.AddDays(-_options.AllowedDays))
                {
                    ModelState.AddModelError("EmailAddress", $"Your registration occurred over {_options.AllowedDays} days ago and has expired. Thank you for your time.");
                    return View(viewModel);
                }

                _auditService.Insert(user.Id, "Login");

                var hoursLeft = (user.TimerStartedOn.AddDays(_options.AllowedDays) - DateTime.UtcNow).TotalHours;

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
        public async Task<IActionResult> NewVisitor(string id)
        {
            var model = new GatewayNewUserViewModel();

            var localAuthority = await _localAuthorityService.GetByName(id.Replace("-", " "));
            if (localAuthority == null)
            {
                return View("GatewayError");
            }

            var canRegister = await _gatewayUserLAService.CanRegisterNewUsers(localAuthority.Id);
            if (!canRegister)
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
            ViewBag.LAName = id;
            return View(model);
        }

        [HttpPost]
        [Route("gateway/newvisitor/{id}")]
        public async Task<IActionResult> NewVisitor(GatewayNewUserViewModel viewModel, string id)
        {
            ViewBag.LAName = id;
            if (ModelState.IsValid)
            {
                // Check for existing email
                // If exists, redirect or return error?
                var user = await _userService.GetByEmailAsync(viewModel.EmailAddress);
                if (user != null)
                {
                    ModelState.AddModelError("EmailAddress", "User already exists, log in to access the service");
                    return View(viewModel);
                }

                var acceptedCookies = viewModel.AcceptCookies == "true";



                // If not, create new record
                var newUser = new GatewayUser()
                {
                    EmailAddress = viewModel.EmailAddress,
                    LocalAuthorityId = viewModel.LocalAuthorityId,
                    CookiePrefs = acceptedCookies
                };
                var newUserId = await _userService.InsertAsync(newUser);
                var createdUserModel = await _userService.GetById(newUserId);
                _auditService.Insert(newUserId, "Register");



                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMonths(12),
                    IsEssential = true,
                    Secure = true,
                    HttpOnly = true
                };

                Response.Cookies.Append(
                    "analytics_preference",
                    acceptedCookies ? "true" : "false",
                    options
                );

                // Send Email
                if (createdUserModel != null)
                {
                    _emailService.SendGatewayEmail(createdUserModel.EmailAddress, createdUserModel.Id.ToString(), createdUserModel.SignUpMagic);
                }

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

        #region Session ended for user

        [HttpGet]
        [Route("gateway/sessionended")]
        public IActionResult SessionEnded()
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

        #region Service Error

        [HttpGet]
        [Route("gateway/link/{id}")]
        public async Task<IActionResult> MagicLink(string id, string validate)
        {
            var userModel = await _userService.GetById(Guid.Parse(id));

            if (userModel != null && userModel.SignUpMagic == validate)
            {
                if (!userModel.ConfirmedSignup)
                {
                    _userService.UserConfirmed(userModel.Id);
                }

                // Set cookies
                Response.Cookies.Append("gateway", userModel.Id.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(1),
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    HttpOnly = true
                });

                return RedirectToAction("Index", "Home");

            }


            return View("GatewayError");
        }

        #endregion
    }
}
