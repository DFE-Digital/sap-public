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

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        Assert.False(paginationIsVisible, "Pagination should not be visible when no results");
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

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        Assert.False(paginationIsVisible, "Pagination should not be visible when there are no multiple pages");
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

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        Assert.False(paginationIsVisible, "Pagination should not be visible when there are validation errors");
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
    public async Task SearchPage_Enter_SchoolName_Shows_Results_With_Pagination()
    {
        // Arrange
        var searchTerm = "School";
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

        // assert pagination
        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        Assert.True(paginationIsVisible);
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_Has_PageNumbers()
    {
        // Arrange
        var searchTerm = "School";

        // Act
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // assert
        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        var paginationItems = Page.Locator(".govuk-pagination__item");
        var itemsCount = await paginationItems.CountAsync();

        Assert.True(paginationIsVisible);
        Assert.True(itemsCount > 0);
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_FirstPage_HasNoPreviousLink()
    {
        // Arrange
        var searchTerm = "School";
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}&PageNumber=1");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        var previousLink = Page.Locator(".govuk-pagination__prev");
        var isPreviousLinkVisible = await previousLink.IsVisibleAsync();

        // assert pagination
        Assert.True(paginationIsVisible);
        Assert.False(isPreviousLinkVisible);
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_HasPreviousLink()
    {
        // Arrange
        var searchTerm = "School";
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}&PageNumber=2");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        var previousLink = Page.Locator(".govuk-pagination__prev");
        var isPreviousLinkVisible = await previousLink.IsVisibleAsync();

        // assert pagination
        Assert.True(paginationIsVisible);
        Assert.True(isPreviousLinkVisible);
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_HasNextLink()
    {
        // Arrange
        var searchTerm = "School";
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}&PageNumber=1");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        var nextLink = Page.Locator(".govuk-pagination__next");
        var isNextLinkVisible = await nextLink.IsVisibleAsync();

        // assert pagination
        Assert.True(paginationIsVisible);
        Assert.True(isNextLinkVisible);
    }

    [Fact]
    public async Task SearchResultsPage_With_Pagination_HasNoNextLink()
    {
        // Arrange
        var searchTerm = "School";
        await Page.GotoAsync($"{_pageUrl}/results?NameSearchTerm={searchTerm}&PageNumber=2");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var pagination = Page.Locator(".govuk-pagination");
        var paginationIsVisible = await pagination.IsVisibleAsync();

        var nextLink = Page.Locator(".govuk-pagination__next");
        var isNextLinkVisible = await nextLink.IsVisibleAsync();

        // assert pagination
        Assert.True(paginationIsVisible);
        Assert.False(isNextLinkVisible);
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
