using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;

[Collection("Playwright Tests")]
public class SearchTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "search";

    [Fact]
    public async Task SearchPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task SearchPage_Has_NotificationBannner()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        var banner = Page.Locator(".govuk-notification-banner");
        var isBannerVisible = await banner.IsVisibleAsync();

        var bannerElement = Page.Locator(".govuk-notification-banner__content p.govuk-notification-banner__heading");
        var bannerText = await bannerElement.InnerTextAsync();

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);

        Assert.True(isBannerVisible);
        Assert.Equal("This is a new service. It’s a trial and may change.", bannerText);
    }
  
    [Fact]
    public async Task SearchPage_Enter_Closed_SchoolName_ShowsViewWithResults()
    {
        // Arrange
        var searchTerm = "Todmorden High School";
        var response = await Page.GotoAsync(_pageUrl);

        // Act
        await Page.FillAsync("#NameSearchTerm", searchTerm);
        await Page.ClickAsync("#search");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
        // assert text box contains search term
        var searchBoxValue = await Page.InputValueAsync("#NameSearchTerm");
        Assert.Equal(searchTerm, searchBoxValue);

        // assert that at least one search result is displayed
        var rows = Page.GetByTestId("school-closed-tag");

        var rowHandles = await rows.ElementHandlesAsync();
        int count = await rows.CountAsync();
        Assert.True(count == 1, "Expected one closed school, but found more than one.");

        var value = await rows.First.TextContentAsync();
        Assert.NotNull(value);
        Assert.Equal("Closed in March 2025", value.Trim());
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_Click_Next_Link()
    {
        // Arrange
        var searchTerm = "School";
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}&PageNumber=1");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var previousLink = Page.Locator(".govuk-pagination__next a");

        // Act
        await previousLink.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // assert 
        Assert.Contains("pageNumber=2", Page.Url);
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_Click_Previous_Link()
    {
        // Arrange
        var searchTerm = "School";
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}&PageNumber=2");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var previousLink = Page.Locator(".govuk-pagination__prev a");

        // Act
        await previousLink.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // assert 
        Assert.Contains("pageNumber=1", Page.Url);
    }
}
