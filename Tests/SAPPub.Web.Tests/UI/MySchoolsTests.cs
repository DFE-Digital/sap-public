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
        var cookieListOfUrns = new List<string> { "105574", "100279" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - select 1 school and expect error message
        var checkbox = await Page.CheckboxStrictAsync("Secondary schools", "Stoke Newington School and Sixth Form");
        await checkbox.CheckAsync();
        await Page.ClickButton("Compare selected schools");

        // Assert
        Assert.True(await Page.HasErrorSummary());

        // Act select another school and expect to be able to compare
        checkbox = await Page.CheckboxStrictAsync("Secondary schools", "Loreto High School Chorlton");
        await checkbox.CheckAsync();

        await Page.ClickButton("Compare selected schools");

        var comparePageTitle = await Page.Locator("h1").TextContentAsync();

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

    [Fact]
    public async Task Page_SelectNoSchoolsFromList_Remove()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "100279" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - with out selecting any schools click remove selected schools button and expect error message
        await Page.ClickButton("Remove selected schools");

        // Assert
        Assert.True(await Page.HasErrorSummary());
    }

    [Fact]
    public async Task Page_SelectSchoolFromList_Remove()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "100279" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act select another school and expect to be able to compare
        var checkbox = await Page.CheckboxStrictAsync("Secondary schools", "Loreto High School Chorlton");
        await checkbox.CheckAsync();

        await Page.ClickButton("Remove selected schools");

        var removeConfirmPageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("Remove school from your compare list", removeConfirmPageTitle);
    }

    [Fact]
    public async Task Page_SelectSchoolsFromList_Remove()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "100279" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - select 1 school and expect error message
        var checkbox1 = await Page.CheckboxStrictAsync("Secondary schools", "Stoke Newington School and Sixth Form");
        await checkbox1.CheckAsync();

        var checkbox2 = await Page.CheckboxStrictAsync("Secondary schools", "Loreto High School Chorlton");
        await checkbox2.CheckAsync();

        await Page.ClickButton("Remove selected schools");

        var removeConfirmPageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("Remove schools from your compare list", removeConfirmPageTitle);
    }

    [Fact]
    public async Task Page_SelectSchoolsToRemove_RedirectsToRemoveConfirm_RemovesSelectedSchools_RedirectsToView()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "100279", "145179" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - select schools
        var checkbox1 = await Page.CheckboxStrictAsync("Secondary schools", "Stoke Newington School and Sixth Form");
        await checkbox1.CheckAsync();

        var checkbox2 = await Page.CheckboxStrictAsync("Secondary schools", "Loreto High School Chorlton");
        await checkbox2.CheckAsync();

        await Page.ClickButton("Remove selected schools");

        var removeConfirmPageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("Remove schools from your compare list", removeConfirmPageTitle);

        // Act
        await Page.ClickButton("Remove schools from list");

        var comparePageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("My schools list", comparePageTitle);

        var successBanner = Page.Locator(".govuk-notification-banner");
        Assert.True(await successBanner.IsVisibleAsync());

        var bannerTitleContent = await successBanner.Locator(".govuk-notification-banner__title").TextContentAsync();
        Assert.NotNull(bannerTitleContent);
        Assert.Equal("Success", bannerTitleContent.Trim());

        var bannerHeaderContent = await successBanner.Locator(".govuk-notification-banner__heading").TextContentAsync();
        Assert.NotNull(bannerHeaderContent);
        Assert.Equal("Saved schools removed from your compare list", bannerHeaderContent.Trim());
    }

    [Fact]
    public async Task Page_SelectSchoolToRemove_RedirectsToRemoveConfirm_RemovesSelectedSchool_RedirectsToView()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "100279", "145179" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - select 1 school to remove
        var checkbox1 = await Page.CheckboxStrictAsync("Secondary schools", "Stoke Newington School and Sixth Form");
        await checkbox1.CheckAsync();

        await Page.ClickButton("Remove selected schools");

        var removeConfirmPageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("Remove school from your compare list", removeConfirmPageTitle);

        // Act
        await Page.ClickButton("Remove school from list");

        var comparePageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("My schools list", comparePageTitle);

        var successBanner = Page.Locator(".govuk-notification-banner");
        Assert.True(await successBanner.IsVisibleAsync());

        var bannerHeaderContent = await successBanner.Locator(".govuk-notification-banner__heading").TextContentAsync();
        Assert.NotNull(bannerHeaderContent);
        Assert.Equal("Saved school removed from your compare list", bannerHeaderContent.Trim());
    }

    [Fact]
    public async Task Page_SelectSchoolsToRemove_RedirectsToRemoveConfirm_CancelRemove_RedirectsToView()
    {
        // Arrange
        var cookieListOfUrns = new List<string> { "105574", "100279" };

        await Page.Context.ClearCookiesAsync();
        await Page.Context.AddCookiesAsync([new Cookie { Name = "MySchoolsList", Value = String.Join(",", cookieListOfUrns), Domain = "127.0.0.1", Path = "/", SameSite = SameSiteAttribute.Lax, Secure = true }]);

        // act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);

        // Act - select 1 school
        var checkbox1 = await Page.CheckboxStrictAsync("Secondary schools", "Stoke Newington School and Sixth Form");
        await checkbox1.CheckAsync();

        await Page.ClickButton("Remove selected schools");

        var removeConfirmPageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("Remove school from your compare list", removeConfirmPageTitle);

        // Act
        await Page.ClickLink("Cancel");

        var comparePageTitle = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.Contains("My schools list", comparePageTitle);
        Assert.False(await Page.Locator(".govuk-notification-banner").IsVisibleAsync());
    }
}
