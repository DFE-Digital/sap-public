using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.SecondarySchool;

public class DestinationsPageTests : BasePageTest
{
    private string _pageUrl = "school/105574/Loreto%20High%20School%20Chorlton/secondary/destinations";

    [Fact]
    public async Task Destinations_LoadsSuccessfully()
    {
        // Arrange && Act
        var response = await GoToPageAysnc(_pageUrl);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task DestinationsPage_HasCorrectTitle()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var title = await Page.TitleAsync();

        // Assert
        Assert.Contains("Destinations", title);
    }

    [Fact]
    public async Task DestinationsPage_DisplaysMainHeading()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var heading = await Page.Locator("h1").TextContentAsync();

        // Assert
        Assert.NotNull(heading.Replace(" ", ""));

    }

    [Fact]
    public async Task DestinationsPage_Displays_SchoolName_Caption()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

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
    public async Task DestinationsPage_Displays_VerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await GoToPageAysnc(_pageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveItemsCountAsync(6);
        await nav.ShouldHaveOneActiveItemAsync();
        await nav.ShouldHaveActiveHrefAsync(_pageUrl);
    }

    [Fact]
    public async Task DestinationsPage_DisplaysPagination()
    {
        // Arrange
        await GoToPageAysnc(_pageUrl);

        // Act
        var isVisible = await Page.Locator("#destinations-pagination").IsVisibleAsync();

        // Assert
        Assert.True(isVisible);
    }
}
