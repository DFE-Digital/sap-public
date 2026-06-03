using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Models.Banner;

namespace SAPPub.Web.ViewComponents.Banner;

public class Banner : ViewComponent
{
    public IViewComponentResult Invoke(BannerViewModel bannerViewModel)
    {
        return View("~/ViewComponents/Banner/Default.cshtml", bannerViewModel);
    }
}
