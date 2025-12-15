using FluentAssertions;
using SAPPub.Tests.UI.Helpers;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Tests.UI.SecondarySchool;

public class CurriculumAndExtraCurricularActivitiesTests : BasePageTest
{
    private string _pageUrl = "school/1/kes/secondary/curriculum-and-extra-curricular-activities";

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        response.Should().NotBeNull();
        response.Status.Should().Be(200);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        title.Should().Match("Curriculum and extra-curricular activities*");
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        heading.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await GoToPageAysnc(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Curriculum_Summary()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#current-curriculum-summary").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_Displays_Extra_Curriculum_Summary()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#current-extra-curricular-activities-offered-summary").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }

    [Fact]
    public async Task CurriculumAndExtraCurricularActivitiesPage_DisplaysPagination()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#current-extra-curricular-activities-pagination").IsVisibleAsync();

        // Assert
        isVisible.Should().BeTrue();
    }
}
