using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;
using System.Text.RegularExpressions;

namespace SAPPub.Web.Tests.UI.Areas.Profiles;

[Collection("Playwright Tests")]
public class DestinationsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private readonly string _url = "school/135600/ark-academy/destinations/16-to-19";
    
    [Fact]
    public async Task KS5DestinationsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_url);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);

    }

    [Fact]
    public async Task DestinationsPage_AccessibilityNavigation()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_url);

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

        //start of right side content - Destinations page
        await Page.Keyboard.PressAsync("Enter");
        await Page.Keyboard.PressAsync("Tab");
        //await Page.Keyboard.PressAsync("Tab");

        activeElement = await Page.EvaluateHandleAsync("document.activeElement");
        prop = await activeElement.GetPropertyAsync("href");
        string href = await prop.JsonValueAsync<string>();
        Assert.Contains("/school/135600/ark-academy/destinations/secondary", href);
    }

    [Fact]
    public async Task KS5DestinationsPage_TogglesBetweenChart_And_Table()
    { 
        // Arrange
        await Page.GotoAsync(_url);

        // Act
        await Page.ClickAsync("#all-ks5-dest-data-show-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var chart = Page.Locator("#all-ks5-dest-data-chart-container");
        var table = Page.Locator("#all-ks5-dest-data-table-container");
        var toggleButton = Page.Locator("#all-ks5-dest-data-show-btn");

        var isChartVisible = await chart.IsVisibleAsync();
        var isTableVisible = await table.IsVisibleAsync();
        var toggleButtonText = await toggleButton.TextContentAsync();
        
        // Assert
        Assert.True(isTableVisible);
        Assert.False(isChartVisible);
        Assert.Equal("Show as a chart", toggleButtonText);

        // Act (toggle back to chart)
        await Page.ClickAsync("#all-ks5-dest-data-show-btn");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        isChartVisible = await chart.IsVisibleAsync();
        isTableVisible = await table.IsVisibleAsync();
        toggleButtonText = await toggleButton.TextContentAsync();

        // Assert
        Assert.False(isTableVisible);
        Assert.True(isChartVisible);
        Assert.Equal("Show as a table", toggleButtonText);

    }

    [Fact]
    public async Task KS5DestinationsPage_WithoutJavaScript_ShowsTableOnly_WithoutChartOrShowAsTableButton()
    {
        // Arrange 
        await using var context = await Browser.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = BaseUrl.TrimEnd('/'),
            IgnoreHTTPSErrors = true,
            JavaScriptEnabled = false
        });

        // Act
        var page = await context.NewPageAsync();
        await page.GotoAsync(_url);

        // Assert
        await Expect(page.Locator("#all-ks5-dest-data-table-container")).ToBeVisibleAsync();
        await Expect(page.Locator("#all-ks5-dest-data-chart-container")).ToBeHiddenAsync();
        await Expect(page.Locator("#all-ks5-dest-data-show-btn")).ToBeHiddenAsync();
    }
}
