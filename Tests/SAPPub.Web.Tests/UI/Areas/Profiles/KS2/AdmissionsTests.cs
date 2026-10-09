using Microsoft.Playwright;
using SAPPub.Playwright.Testing;
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
    public async Task AdmissionsPage_KS2_AccessibilityNavigation()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        //bypass cookies
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");
        await Page.Keyboard.PressAsync("Tab");

        //start of page - _Layout page
        await Page.Keyboard.PressAsync("Tab");

        var activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        var prop = await activeElement.GetPropertyAsync("textContent");
        string textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip to main content", textContent);

        //start of content - Default page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("textContent");
        textContent = await prop.JsonValueAsync<string>();
        Assert.Equal("Skip contents", textContent);

        //start of right side content - Admissions KS2 page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Equal("https://www.gov.uk/schools-admissions/school-starting-age", href);
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
    [InlineData("143034", 7)]
    [InlineData("150009", 9 )]
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
