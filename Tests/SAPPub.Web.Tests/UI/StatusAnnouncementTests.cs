using Microsoft.Playwright;
using Microsoft.Playwright.Xunit.v3;
using SAPPub.Playwright.Testing;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI;


/// <summary>
/// Button state changes that happen in place (without a page load) need to be announced by screen readers
/// as per WCAG 2.2 4.1.3 Status messages. The announcment is written to a shared, visually hidden live region
/// </summary>
/// <param name="fixture"></param>
[Collection("Playwright Tests")]
public class StatusAnnouncementTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private const string EnglishMathsUrl = "school/105574/loreto-high-school-chorlton/secondary-performance/english-and-maths";
    private const string KS5DestinationsUrl = "school/135600/ark-academy/destinations/16-to-19";
    private const string CompareEnglishMathsUrl = "compare/secondary/english-and-maths-results?urns=105574&urns=137020";
    private const string OverviewUrl = "school/149328/king-edward-vi-high-school/overview";

    private ILocator StatusRegion => Page.Locator("[role='status']");

    [Fact]
    public async Task Layout_HasPoliteVisuallyHiddenStatusRegion()
    {
        // Arrange/Act
        await Page.GotoAsync("/");

        // Assert
        await Expect(StatusRegion).ToHaveCountAsync(1);
        await Expect(StatusRegion).ToHaveAttributeAsync("aria-live", "polite");
        await Expect(StatusRegion).ToHaveAttributeAsync("aria-atomic", "true");
        await Expect(StatusRegion).ToHaveClassAsync("govuk-visually-hidden");
        await Expect(StatusRegion).ToHaveTextAsync("");
    }

    [Fact]
    public async Task EnglishMathsPage_Onload_AccouncesNothing()
    {
        // Arrange/Act
        await Page.GotoAsync(EnglishMathsUrl);

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("");
    }

    [Fact]
    public async Task EnglishMathsPage_ShowDataOverTimeAndShowCurrentData_AnnounceChanges()
    {
        // Arrange
        await Page.GotoAsync(EnglishMathsUrl);

        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name="Show data over time", Exact = true }).ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing data over time");

        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name = "Show current data", Exact = true }).ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing current data");
    }

    [Fact]
    public async Task EnglishMathsPage_ShowAsTableAndChart_AnnounceChanges()
    {
        // Arrange
        await Page.GotoAsync(EnglishMathsUrl);
        var button = Page.Locator("#all-gcse-current-year-show-btn");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing table");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing chart");
    }

    [Fact]
    public async Task EnglishMathsPage_ShowBreakdownAsTableAndChart_AnnounceChanges()
    {
        // Arrange
        await Page.GotoAsync(EnglishMathsUrl);
        var button = Page.Locator("#breakdown-gcse-current-year-show-btn");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing table");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing chart");
    }


    [Fact]
    public async Task KS5DestinationsPage_ShowAsTableAndChart_AnnounceChanges()
    {
        // Arrange
        await Page.GotoAsync(KS5DestinationsUrl);
        var button = Page.Locator("#all-ks5-dest-data-show-btn");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing table");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing chart");
    }

    [Fact]
    public async Task CompareEnglishMathsPage_ShowDataOverTimeAndShowCurrentData_AnnounceChanges()
    {
        // Arrange
        await Page.GotoAsync(CompareEnglishMathsUrl);

        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name = "Show data over time", Exact = true }).ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing data over time");

        // Act
        await Page.GetByRole(AriaRole.Button, new() { Name = "Show current data", Exact = true }).ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing current data");
    }

    [Fact]
    public async Task OverviewPage_ShowAsTableAndChart_AnnounceChanges()
    {
        // Arrange
        await Page.GotoAsync(OverviewUrl);
        await Page.GetByRole(AriaRole.Button, new() { Name = "English and maths GCSE results", Exact = false }).ClickAsync();
        var button = Page.Locator("#overview-english-maths-current-year-show-btn");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing table");

        // Act
        await button.ClickAsync();

        // Assert
        await Expect(StatusRegion).ToHaveTextAsync("Showing chart");
    }

    [Fact]
    public async Task OverviewPage_SaveToMySchoolList_ShowsFocussedStatusBannerAnnouncement()
    {
        // Arrange
        await Page.GotoAsync(OverviewUrl);

        // Act
        await Page.Locator("#toggleSaveEstablishment").ClickAsync();
        

        // Assert
        var banner = Page.Locator("[id$='-add-success']");
        await Expect(banner).ToBeVisibleAsync();
        await Expect(banner).ToHaveAttributeAsync("role", "status");
        await Expect(banner).ToBeFocusedAsync();

        // Act
        await Page.Locator("#toggleSaveEstablishment").ClickAsync();

        // Assert
        var removedBanner = Page.Locator("[id$='-remove-success']");
        await Expect(removedBanner).ToBeVisibleAsync();
        await Expect(removedBanner).ToHaveAttributeAsync("role", "status");
        await Expect(removedBanner).ToBeFocusedAsync();
    }
}
