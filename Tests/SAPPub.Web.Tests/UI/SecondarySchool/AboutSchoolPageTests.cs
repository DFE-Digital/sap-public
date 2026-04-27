using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AboutSchoolPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/secondary/about",
        ["137552"] = "school/137552/stewards-academy-science-specialist-harlow/secondary/about",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/secondary/about",
        ["107564"] = "school/107564/todmorden-high-school/secondary/about"
    };

    [Fact]
    public async Task AboutSchoolPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AboutSchoolPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("About the school", title);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("Loreto High School Chorlton", schoolNameCaption);
    }

    [Fact]
    public async Task AboutSchoolPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(5);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap["105574"]);
    }

    [Theory]
    [InlineData("105574", false, false)]
    [InlineData("137552", true, false)]
    [InlineData("107564", true, true)]    
    public async Task AboutSchoolPage_Displays_School_Closed_Info(string urn, bool isSchoolClosed, bool hasSchoolClosedDate)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var schoolClosedCard = Page.GetByTestId("school-closed-custom-card");

        Assert.Equal(isSchoolClosed, await schoolClosedCard.IsVisibleAsync());

        if (isSchoolClosed)
        {
            var value = await schoolClosedCard.Locator("p").TextContentAsync();

            var expectedText = hasSchoolClosedDate ? "This school closed on 23 March 2025" : "Closed";

            Assert.NotNull(value);
            Assert.Equal(expectedText, value.Trim());
        }        
    }

    [Theory]
    [InlineData("105574", false)]
    [InlineData("137552", true)]
    public async Task AboutSchoolPage_DisplaysSchoolDetails(string urn, bool trustNameIsExpected)
    {
        // Act
        await Page.GotoAsync(_schoolUrnToUrlMap[urn]);

        // Assert
        var detailsSummary = Page.Locator("#school-details-summary");

        Assert.True(await detailsSummary.IsVisibleAsync());
        var row = detailsSummary
            .Locator(".govuk-summary-list__row")
            .Filter(new() { Has = Page.Locator(".govuk-summary-list__key", new() { HasText = " Academy Trust " }) });

        if (trustNameIsExpected)
        {
            var value = await row.Locator(".govuk-summary-list__value").TextContentAsync();
            Assert.NotNull(value);
            Assert.Equal("THE PASSMORES CO-OPERATIVE LEARNING COMMUNITY", value.Trim());
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
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#school-location-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSpecialistUnit()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#details-sen").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolFeatures()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#school-features-summary").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolPolicies()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var schoolPoliciesSummaryCard = Page.Locator("#school-policies-summary");
        var contactSchoolInfo = schoolPoliciesSummaryCard.GetByTestId("contact-school-info");

        // Assert
        Assert.True(await schoolPoliciesSummaryCard.IsVisibleAsync());
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysSchoolPolicies_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act
        var schoolPoliciesSummaryCard = Page.Locator("#school-policies-summary");
        var contactSchoolInfo = schoolPoliciesSummaryCard.GetByTestId("contact-school-info");
        var isVisible = await contactSchoolInfo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AboutSchoolPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#about-the-school-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}