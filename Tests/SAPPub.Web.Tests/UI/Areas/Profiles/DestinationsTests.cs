using Microsoft.Playwright;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.Profiles;

[Collection("Playwright Tests")]
public class DestinationsTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private readonly string _url = "school/105574/loreto-high-school-chorlton/destinations/16-to-19";
    
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
    public async Task KS5DestinationsPage_TogglesBetweenChart_And_Table()
    { 
        // Arrange
        await Page.GotoAsync(_url);

        var content = await Page.ContentAsync();

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
}
