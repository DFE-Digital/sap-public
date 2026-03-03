using SAPPub.Core.Enums;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

[Collection("Playwright Tests")]
public class AcademicPerformanceAttainmentAndProgressTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/academic-performance-attainment-and-progress";

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await Page.GotoAsync(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_HasCorrectTitle()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Academic Performance", title);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_DisplaysMainHeading()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading);
        Assert.NotEmpty(heading!.Trim());
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

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
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }
    
    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Sub_Navigation()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Attainment8_Details()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-attainment8").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_Progress8_Details()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-progress8").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_Displays_AcademicYear_Selector()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var academicYearSelector = Page.Locator("#academicyearSelector");
        var progress8CustomCard = Page.GetByTestId("progress8-custom-card");

        // Assert
        Assert.True(await academicYearSelector.IsVisibleAsync());
        Assert.True(await progress8CustomCard.IsVisibleAsync());
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResultsPage_Change_AcademicYear_Selected()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var academicyearSelector = Page.Locator("#academicyearSelector");
        await academicyearSelector.SelectOptionAsync([((int)AcademicYearSelection.AY_2022_2023).ToString()]);
        var buttonSelector = Page.Locator("button:has-text(\"Show results\")");
        await buttonSelector.ClickAsync();

        // Assert
        var progress8CustomCard = Page.GetByTestId("progress8-custom-card");
        Assert.False(await progress8CustomCard.IsVisibleAsync());
    }

    [Fact]
    public async Task AcademicPerformanceAttainmentAndProgressPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-attainment-and-progress-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
