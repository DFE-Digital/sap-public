using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.Areas.KS2;

[Collection("Playwright Tests")]
public class AdmissionsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{

    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["143034"] = "school/143034/st-pauls-church-of-england-academy/admissions/primary",
        ["150009"] = "school/150009/abraham-moss-community-school/admissions/primary" // KS2 + KS4 school
    };

    [Fact]
    public async Task AdmissionsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AdmissionsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Primary Admissions", title);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading?.Replace(" ", ""));
    }

    [Fact]
    public async Task AdmissionsPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var schoolNameCaptionLocator = Page.Locator("#school-name-caption");
        var isVisible = await schoolNameCaptionLocator.IsVisibleAsync();
        var schoolNameCaption = await schoolNameCaptionLocator.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.NotNull(schoolNameCaption);
        Assert.Equal("St Paul's Church of England Academy", schoolNameCaption);
    }

    [Theory]
    [InlineData("143034", 5)]
    [InlineData("150009", 7 )]
    public async Task AdmissionsPage_Displays_VerticalNavigation(string schoolUrn, int expectedItemCount)
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap[schoolUrn]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(expectedItemCount);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_schoolUrnToUrlMap[schoolUrn]);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysStartingPrimarySchoolSummaryCard()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var summaryCard = Page.GetByTestId("starting-primary-school-summary");
        await summaryCard.WaitForAsync();
        var schoolWebsiteLink = summaryCard.GetByTestId("school-website-link");
        var laWebsiteLink = summaryCard.GetByTestId("la-website-link");
        var schoolWebsiteHref = await schoolWebsiteLink.GetAttributeAsync("href");
        var schoolWebsiteText = await schoolWebsiteLink.TextContentAsync();
        var laWebsiteHref = await laWebsiteLink.GetAttributeAsync("href");
        var laWebsiteText = await laWebsiteLink.TextContentAsync();

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());
        Assert.True(await schoolWebsiteLink.IsVisibleAsync());
        Assert.True(await laWebsiteLink.IsVisibleAsync());

        Assert.NotNull(schoolWebsiteHref);
        Assert.NotNull(schoolWebsiteText);
        Assert.NotNull(laWebsiteHref);
        Assert.NotNull(laWebsiteText);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMovingSchoolsDuringYearSummaryCard()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var summaryCard = Page.GetByTestId("moving-schools-during-year-summary-card");
        await summaryCard.WaitForAsync();

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());

        // The link is always rendered, but may be empty if no LA URL data exists
        var link = summaryCard.GetByTestId("link");
        Assert.True(await link.IsVisibleAsync());

        var text = await link.TextContentAsync();
        Assert.NotNull(text);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMoreInfoOnPrimarySchoolAdmissionsAccordion()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var admissionsAccordion = Page.Locator("#more-info-on-primary-school-admissions-accordion");
        await admissionsAccordion.WaitForAsync();

        // Assert
        Assert.True(await admissionsAccordion.IsVisibleAsync());
    }

    [Fact]
    public async Task Admissions_DisplaysStartingPrimarySchool_Info()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var summaryCard = Page.GetByTestId("starting-primary-school-summary");
        await summaryCard.WaitForAsync();

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info");
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var isVisible = await Page.Locator("#admissions-pagination").IsVisibleAsync();

        // Act
        var previousPaginationLink = Page.Locator("#admissions-pagination .govuk-pagination__prev a");
        var nextPaginationLink = Page.Locator("#admissions-pagination .govuk-pagination__next a");

        var previousPaginationText = await previousPaginationLink.TextContentAsync();
        var nextPaginationText = await nextPaginationLink.TextContentAsync();

        // Assert
        Assert.True(isVisible);
        Assert.Equal("About the school", previousPaginationText?.Trim());
        Assert.Equal("Curriculum and extra-curricular activities", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AdmissionsPage_DoesNotDisplay_SubNavigation_WhenOnlyKS2()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["143034"]);

        // Act
        var subNav = Page.Locator("#sub-navigation-academic-performance");
        var isVisible = await subNav.IsVisibleAsync();

        // Assert
        Assert.False(isVisible);
    }

    [Fact]
    public async Task AdmissionsPage_Displays_SubNavigation_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var subNav = Page.Locator("#sub-navigation-academic-performance");
        var isVisible = await subNav.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AdmissionsPage_SubNavigation_HasCorrectLinks_WhenMultiplePhases()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["150009"]);

        // Act
        var primaryLink = Page.Locator("#sub-navigation-academic-performance a[aria-current='page']");
        var secondaryLink = Page.Locator("#sub-navigation-academic-performance a:not([aria-current='page'])");

        var primaryLinkText = await primaryLink.TextContentAsync();
        var secondaryLinkText = await secondaryLink.TextContentAsync();

        // Assert
        Assert.Equal("Primary Admissions", primaryLinkText?.Trim());
        Assert.Equal("Secondary Admissions", secondaryLinkText?.Trim());
    }
}
