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
        var searchTerm = "school";
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
        await Page.FillAsync("#NameSearchTerm", searchTerm);
        await Page.ClickAsync("#search");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
        // assert text box contains search term
        var searchBoxValue = await Page.InputValueAsync("#NameSearchTerm ");
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

    [Fact]
    public async Task SearchPage_EnterValidPostcode_ShowsViewWithResults()
    {
        // Arrange
        var searchTerm = "M21 7SW";
        var response = await Page.GotoAsync(_pageUrl);

        // Act
        await Page.FillAsync("#LocationSearchTerm", searchTerm);
        await Page.ClickAsync("#search");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
        // assert text box contains search term
        var searchBoxValue = await Page.InputValueAsync("#LocationSearchTerm");
        Assert.Equal(searchTerm, searchBoxValue);

        // assert that at least one search result is displayed
        var rows = Page.Locator(".govuk-summary-list .govuk-summary-list__row");
        var rowHandles = await rows.ElementHandlesAsync();
        int count = await rows.CountAsync();
        Assert.True(count > 0, "Expected at least one search result, but found none.");
    }

    [Fact]
    public async Task SearchPage_EnterInvalidPostcode_ShowsViewWithErrorMessge()
    {
        // Arrange
        var searchTerm = "NE1";
        var response = await Page.GotoAsync(_pageUrl);

        // Act
        await Page.FillAsync("#LocationSearchTerm", searchTerm);
        await Page.ClickAsync("#search");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
        // assert text box contains search term
        var searchBoxValue = await Page.InputValueAsync("#LocationSearchTerm");
        Assert.Equal(searchTerm, searchBoxValue);

        // error box is displayed with correct message
        var errorLink = Page.Locator(".govuk-error-summary__list a[href='#LocationSearchTerm']");
        Assert.Equal("Enter a full postcode", await errorLink.InnerTextAsync());

        // assert no list items are displayed
        var rows = Page.Locator(".govuk-summary-list .govuk-summary-list__row");
        var rowHandles = await rows.ElementHandlesAsync();
        int count = await rows.CountAsync();
        Assert.True(count == 0, "Expected no search results, but found some.");
    }
}
