using FluentAssertions;
using SAPPub.Tests.UI.Helpers;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Tests.UI.SecondarySchool;

public class AcademicPerformanceTests : BasePageTest
{
    private string _pageUrl = "school/1/kes/secondary/academic-performance";

    [Fact]
    public async Task AcademicPerformancePage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be(200);
    }

    [Fact]
    public async Task AcademicPerformancePage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        title.Should().Match("Academic Performance*");
    }

    [Fact]
    public async Task AcademicPerformancePage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        heading.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task AcademicPerformancePage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await GoToPageAysnc(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task AcademicPerformancePage_Displays_Gcse_Grades_Explained()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#details-gcse-grades-explained").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }
}
