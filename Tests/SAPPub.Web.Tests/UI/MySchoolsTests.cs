using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;


[Collection("Playwright Tests")]
public class MySchoolsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "my-schools/view";

    [Fact]
    public async Task Page_LoadsSuccessfully()
    {
        // Arrange
        var urn = "105574";

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = urn, Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task Page_SelectSchoolsFromList_Compare()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "102848" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - select 1 school and expect error message
        var checkbox = await Page.CheckboxStrictAsync("Secondary schools", "SS Peter and Paul's Catholic Primary School");
        await checkbox.CheckAsync();
        await Page.ClickButton("Compare selected schools");

        // Assert
        Assert.True(await Page.HasErrorSummary());

        // Act select another school and expect to be able to compare
        checkbox = await Page.CheckboxStrictAsync("Secondary schools", "Loreto High School Chorlton");
        await checkbox.CheckAsync();

        await Page.ClickButton("Compare selected schools");

        var comparePageTitle = await Page.TitleAsync();

        // Assert
        Assert.Contains("Comparing your schools", comparePageTitle);
    }

    [Fact]
    public async Task Page_UrnIsNotInSavedEstablishments_IgnoresUrn()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "102848", "999999" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        var checkbox = await Page.CheckboxStrictAsync("Secondary schools", "SS Peter and Paul's Catholic Primary School");
        Assert.NotNull(checkbox);
        checkbox = await Page.CheckboxStrictAsync("Secondary schools", "Loreto High School Chorlton");
        Assert.NotNull(checkbox);
    }
}
