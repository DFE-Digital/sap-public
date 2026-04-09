using SAPPub.Integration.Tests;

namespace SAPPub.IntegrationTests.SecondarySchoolTests;

[Collection("Integration Tests")]
public class AboutSchoolPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string PageUrl(string urn) => $"/school/{urn}";

    [Fact]
    public async Task AboutSchoolPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(PageUrl("105574"));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AboutSchoolPage_HasCorrectTitle()
    {
        // PageUrl
        await Page.GotoAsync(PageUrl("105574"));

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("About the school", title);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(PageUrl("105574"));

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Fact]
    public async Task AboutSchoolPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(PageUrl("105574"));

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("Loreto High School Chorlton", schoolNameCaption);
    }

    [Theory]
    [InlineData("114311", true, "31 December 2022")]
    public async Task AboutSchoolPage_Displays_School_Closed_Info(string urn, bool isSchoolClosed, string? date)
    {
        // Act
        await Page.GotoAsync(PageUrl(urn));

        // Assert
        var schoolClosedCard = Page.GetByTestId("school-closed-custom-card");

        Assert.Equal(isSchoolClosed, await schoolClosedCard.IsVisibleAsync());

        if (isSchoolClosed)
        {
            var value = await schoolClosedCard.Locator("p").TextContentAsync();

            var expectedText = date != null ? $"This school closed on {date}" : "Closed";

            Assert.NotNull(value);
            Assert.Equal(expectedText, value.Trim());
        }
    }

    [Theory]
    [InlineData("105574", null)]
    [InlineData("137552", "THE PASSMORES CO-OPERATIVE LEARNING COMMUNITY")]
    public async Task AboutSchoolPage_DisplaysTrustNameRow_WhenTrustNameNotNull(string urn, string? trustName)
    {
        // Act
        await Page.GotoAsync(PageUrl(urn));

        // Assert
        var detailsSummary = Page.Locator("#school-details-summary");

        Assert.True(await detailsSummary.IsVisibleAsync());
        var row = detailsSummary
            .Locator(".govuk-summary-list__row")
            .Filter(new() { Has = Page.Locator(".govuk-summary-list__key", new() { HasText = " Academy Trust " }) });

        if (trustName != null)
        {
            var value = await row.Locator(".govuk-summary-list__value").TextContentAsync();
            Assert.NotNull(value);
            Assert.Equal(trustName, value.Trim());
        }
        else
        {
            Assert.False(await row.IsVisibleAsync());
        }
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolLocation()
    {
        // Arrange
        await Page.GotoAsync(PageUrl("105574"));

        // Act
        var isVisible = await Page.Locator("#school-location-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSpecialistUnit()
    {
        // Arrange
        await Page.GotoAsync(PageUrl("105574"));

        // Act
        var isVisible = await Page.Locator("#details-sen").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolFeatures()
    {
        // Arrange
        await Page.GotoAsync(PageUrl("105574"));

        // Act
        var isVisible = await Page.Locator("#school-features-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}