using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Areas.Gateway.ViewModels;

namespace SAPPub.Web.Areas.Gateway.Controllers
{
    [Area("Gateway")]
    public class GatewayController : Controller
    {
        private List<string> _allowedLAs = new List<string>
        {
            "sheffield",
            "barnsley",
            "rotherham",
            "doncaster"
        };

        public GatewayController()
        {
        }

        [HttpGet]
        [Route("gateway")]
        [Route("gateway/{id}")]
        public IActionResult Index(string id)
        {
            if (_allowedLAs.Contains(id, StringComparer.OrdinalIgnoreCase))
            {
                return RedirectToAction("Welcome", new { id = id });
            }
            return View("GatewayError");
        }


        [HttpGet]
        [Route("gateway/{id}/welcome")]
        public IActionResult Welcome(string id)
        {
            var model = new GatewayWelcomeViewModel
            {
                LocalAuthority = id
            };
            return View(model);
        }

        [HttpPost]
        [Route("gateway/{id}")]
        public IActionResult Welcome(GatewayWelcomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.HasBeenHereBefore == true)
                {

                }
                return RedirectToAction("Consent", "Gateway", new { la = model.LocalAuthority });
            }

            return View(model);
        }

        [HttpGet]
        [Route("gateway/{la}/consent")]
        public IActionResult Consent(string la)
        {
            var model = new GatewayConsentViewModel
            {
                LocalAuthority = la
            };
            //read cookies for prefill convienience



            return View(model);
        }

        [HttpPost]
        [Route("gateway/{la}/consent")]
        public IActionResult Consent(GatewayConsentViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction();
            }

            return View(model);
        }
    }
}
