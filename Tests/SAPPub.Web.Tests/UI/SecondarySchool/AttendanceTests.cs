using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AttendancePageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/secondary/attendance";

    [Fact]
    public async Task AttendancePage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);
        
        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }
}
