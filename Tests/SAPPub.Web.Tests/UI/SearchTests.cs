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
    public async Task SearchPage_EnterSchoolName_ShowsViewWithResults()
    {
        // Arrange
        var searchTerm = "manchester";
        var response = await Page.GotoAsync(_pageUrl);

        // Act
        await Page.FillAsync("#searchKeyWord", searchTerm);
        await Page.ClickAsync("#search");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
        // assert text box contains search term
        var searchBoxValue = await Page.InputValueAsync("#searchKeyWord");
        Assert.Equal(searchTerm, searchBoxValue);

        // assert that at least one search result is displayed
        var rows = Page.Locator(".govuk-summary-list .govuk-summary-list__row");
        var rowHandles = await rows.ElementHandlesAsync();
        int count = await rows.CountAsync();
        Assert.True(count > 0, "Expected at least one search result, but found none.");
    }

    [Fact]
    public async Task SearchPage_EnterSchoolName_NoResults_ShowsViewWithNoResults()
    {
        // Arrange
        var searchTerm = "xyz";
        var response = await Page.GotoAsync(_pageUrl);

        // Act
        await Page.FillAsync("#searchKeyWord", searchTerm);
        await Page.ClickAsync("#search");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
        // assert text box contains search term
        var searchBoxValue = await Page.InputValueAsync("#searchKeyWord");
        Assert.Equal(searchTerm, searchBoxValue);

        // assert no list items are displayed
        var rows = Page.Locator(".govuk-summary-list .govuk-summary-list__row");
        var rowHandles = await rows.ElementHandlesAsync();
        int count = await rows.CountAsync();
        Assert.True(count == 0, "Expected no search results, but found some.");

        // assert that the no results message is displayed
        var noResultsMessage = await Page.Locator("[data-testid='no-results-heading']").InnerTextAsync();
        Assert.Contains("Try another search", noResultsMessage);
    }
}
