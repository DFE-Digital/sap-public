using FluentAssertions;
using SAPPub.Tests.UI.Helpers;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Tests.UI.SecondarySchool;

public class AcademicPerformancePupilProgressTests : BasePageTest
{
    private string _pageUrl = "school/1/kes/secondary/academic-performance-pupil-progress";

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be(200);
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        title.Should().Match("Academic Performance*");
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        heading.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await GoToPageAysnc(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }
    
    [Fact]
    public async Task AcademicPerformancePupilProgressPage_Displays_Sub_Navigation()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_Displays_Attainment8_Details()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-attainment8").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_Displays_Progress8_Details()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-progress8").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressPage_DisplaysPagination()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#academic-performance-pupil-progress-pagination").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }
}
