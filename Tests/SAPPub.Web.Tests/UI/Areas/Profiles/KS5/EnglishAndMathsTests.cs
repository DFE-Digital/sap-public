using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Web.Tests.UI.Infrastructure;
using PageConstants = SAPPub.Playwright.Testing.KS5.AcademicPerformanceLevel3QualificationsPageConstants;

namespace SAPPub.Web.Tests.UI.Areas.Profiles.KS5;

[Collection("Playwright Tests")]
public class EnglishAndMathsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private const string _urn = "130499";
    private const string _schoolName = "holy-cross-college";

    private static string GetPageUrl(Level2 level2Qualification) => $"school/{_urn}/{_schoolName}/16-to-19-performance/level-2-qualifications/{level2Qualification.ToString().ToLower()}";

    [Theory]
    [InlineData(Level2.TechCert)]    
    public async Task EnglishAndMathsPage_LoadsSuccessfully(Level2 level2Qualification)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(GetPageUrl(level2Qualification));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task EnglishAndMathsPage_KS5_AccessibilityNavigation(Level2 level2Qualification)
    {
        // Arrange && Act
        var response = await Page.GotoAsync(GetPageUrl(level2Qualification));

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

        //start of right side content - EnglishAndMaths KS5 page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Equal("https://explore-education-statistics.service.gov.uk/find-statistics", href);
    }

}

