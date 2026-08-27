using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS2;

[Collection("Playwright Tests")]
public class AdmissionsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{

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

    [Theory]
    [InlineData("143034", 6)]
    [InlineData("150009", 8 )]
    public async Task AdmissionsPage_Displays_VerticalNavigation(string schoolUrn, int expectedItemCount)
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap[schoolUrn]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(expectedItemCount);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap[schoolUrn].Replace("/primary", ""));
    }    
}
