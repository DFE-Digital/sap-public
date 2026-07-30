using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.KS2;

[Collection("Playwright Tests")]
public class AdmissionsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/143034/st-pauls-church-of-england-academy/admissions/primary";

    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/admissions/primary",
        ["150009"] = "school/150009/abraham-moss-community-school/admissions/primary" // KS2 + KS4 school
    };

    [Fact]
    public async Task AdmissionsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AdmissionsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Primary Admissions", title);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading?.Replace(" ", ""));
    }

    [Fact]
    public async Task AdmissionsPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("St Paul's Church of England Academy", schoolNameCaption);
    }

    [Fact(Skip = "Not implemented yet due to changes in site map")]
    public async Task AdmissionsPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task AdmissionsPage_DoesNotDisplay_SubNavigation_WhenOnlyKS2()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var subNav = Page.Locator("#sub-navigation-academic-performance");
        var isVisible = await subNav.IsVisibleAsync();

        // Assert
        Assert.False(isVisible);
    }

    [Fact]
    public async Task AdmissionsPage_Displays_SubNavigation_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var subNav = Page.Locator("#sub-navigation-academic-performance");
        var isVisible = await subNav.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AdmissionsPage_SubNavigation_HasCorrectLinks_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var primaryLink = Page.Locator("#sub-navigation-academic-performance a[aria-current='page']");
        var secondaryLink = Page.Locator("#sub-navigation-academic-performance a:not([aria-current='page'])");

        var primaryLinkText = await primaryLink.TextContentAsync();
        var secondaryLinkText = await secondaryLink.TextContentAsync();

        // Assert
        Assert.Equal("Primary Admissions", primaryLinkText?.Trim());
        Assert.Equal("Secondary Admissions", secondaryLinkText?.Trim());
    }
}
