using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AdmissionsPageTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/loreto-high-school-chorlton/secondary/admissions";

    private Dictionary<string, string> _schoolUrnToUrlMap = new Dictionary<string, string>
    {
        ["105574"] = "school/105574/loreto-high-school-chorlton/secondary/admissions",
        ["100273"] = "school/100273/saint-paul-roman-catholic-infant-school/secondary/admissions",
        ["107564"] = "school/107564/todmorden-high-school/secondary/admissions"
    };

    [Fact]
    public async Task AdmissionsPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AdmissionsPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Admissions", title);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading?.Replace(" ", ""));
    }

    [Fact]
    public async Task AdmissionsPage_Displays_SchoolName_Caption()
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
    public async Task AdmissionsPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(5);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysStartingSecondarySchoolSummaryCard()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var summaryCard = Page.GetByTestId("starting-secondary-school-summary");
        await summaryCard.WaitForAsync();
        var schoolWebsiteLink = summaryCard.GetByTestId("school-website-link");
        var laWebsiteLink = summaryCard.GetByTestId("la-website-link");
        var schoolWebsiteHref = await schoolWebsiteLink.GetAttributeAsync("href");
        var schoolWebsiteText = await schoolWebsiteLink.TextContentAsync();
        var laWebsiteHref = await laWebsiteLink.GetAttributeAsync("href");
        var lalWebsiteText = await laWebsiteLink.TextContentAsync();

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());
        Assert.True(await schoolWebsiteLink.IsVisibleAsync());
        Assert.True(await laWebsiteLink.IsVisibleAsync());
        
        Assert.NotNull(schoolWebsiteHref);
        Assert.NotNull(schoolWebsiteText);
        Assert.NotNull(laWebsiteHref);
        Assert.NotNull(lalWebsiteText);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMovingSchoolsDuringYearSummaryCard()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var summaryCard = Page.GetByTestId("moving-schools-during-year-summary-card");
        await summaryCard.WaitForAsync();
        var link = summaryCard.GetByTestId("link");

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());
        Assert.True(await link.IsVisibleAsync());
        var href = await link.GetAttributeAsync("href");
        var text = await link.TextContentAsync();
        Assert.Contains("https://", href); // Todo - we need tests that allow us to know what these values will be
        Assert.NotNull(text);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysMoreInfoOnSecondarySchoolAdmissionsAccordion()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var admissionsAccordion = Page.Locator("#more-info-on-secondary-school-admissions-accordion");
        await admissionsAccordion.WaitForAsync();

        // Assert
        Assert.True(await admissionsAccordion.IsVisibleAsync());
    }

    [Fact]
    public async Task Admissions_DisplaysStartingSecondarySchool_Info()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var summaryCard = Page.GetByTestId("starting-secondary-school-summary");
        await summaryCard.WaitForAsync();

        // Assert
        Assert.True(await summaryCard.IsVisibleAsync());

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info");
        Assert.False(await contactSchoolInfo.IsVisibleAsync());
    }

    [Fact]
    public async Task Admissions_DisplaysStartingSecondarySchool_Info_ContactSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["100273"]);

        // Act
        var summaryCard = Page.GetByTestId("starting-secondary-school-summary");
        await summaryCard.WaitForAsync();

        var contactSchoolInfo = summaryCard.GetByTestId("contact-school-info");
        var isVisible = await contactSchoolInfo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task Admissions_DisplaysStartingSecondarySchool_Info_CannotApplyToSchoolText()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["107564"]);

        // Act
        var summaryCard = Page.GetByTestId("starting-secondary-school-summary");
        await summaryCard.WaitForAsync();

        var cannotApplySchoolInfo = summaryCard.GetByTestId("cannot-apply-info");
        var isVisible = await cannotApplySchoolInfo.IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AdmissionsPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_schoolUrnToUrlMap["105574"]);

        // Act
        var isVisible = await Page.Locator("#admissions-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
