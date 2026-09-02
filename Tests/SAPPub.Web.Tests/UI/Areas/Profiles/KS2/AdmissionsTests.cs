using Microsoft.Playwright;
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
    public async Task AdmissionsPage_DisplaysExpectedHeadings()
    {
        // Arrange / Act
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Assert - school name is the H1
        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Level = 1,
                    Name = "St Paul's Church of England Academy",
                    Exact = true
                }))
            .ToBeVisibleAsync();

        // Assert - page title is an H2
        await Expect(
            Page.GetByRole(
                AriaRole.Heading,
                new()
                {
                    Level = 2,
                    Name = "Admissions",
                    Exact = true
                }))
            .ToBeVisibleAsync();
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
