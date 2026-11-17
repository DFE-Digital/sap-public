using AngleSharp.Dom;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class HeaderServiceNavigationTests : PageTestsBase
{
    private const string ActiveItemClass = "govuk-service-navigation__item--active";
    private readonly string _pageUrl = "terms-and-conditions";
    private readonly Mock<IMySchoolsListService> _comparisonService = new();
    private readonly Mock<IEstablishmentService> _establishmentService = new();


    public HeaderServiceNavigationTests(WebAppFixture fixture) : base(fixture)
    {
        _comparisonService = UseMock<IMySchoolsListService>();
        _establishmentService = UseMock<IEstablishmentService>();
    }

    [Fact]
    public async Task ServiceTitle_IsCorrect_And_LinksToSearch()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var serviceLink = doc.QuerySelector("span.govuk-service-navigation__service-name a");
        var text = serviceLink?.TextContent.Trim();
        var href = serviceLink?.GetAttribute("href");

        // Assert
        Assert.NotNull(serviceLink);
        Assert.Equal("Find and compare school and college profiles", text?.Trim());
        Assert.Equal("/search", href, ignoreCase: true);
    }

    [Fact]
    public async Task ServiceNavigation_IsCorrect_And_LinksToMySchoolsView()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act
        var mySchoolsViewLinkWrapper = doc.QuerySelector("#my-schools-view-link");
        var mySchoolsViewLink = mySchoolsViewLinkWrapper?.QuerySelector("a");
        var text = mySchoolsViewLink?.TextContent.Trim();
        var href = mySchoolsViewLink?.GetAttribute("href");

        // Assert
        Assert.NotNull(mySchoolsViewLink);
        Assert.Equal("My schools list", text?.Trim());
        Assert.Equal("/my-schools/view", href, ignoreCase: true);
    }

    [Fact]
    public async Task ServiceNavigation_MySchoolsLink_IsNotActive_OutsideMySchoolsSelection()
    {
        // Arrange
        var doc = await Fixture.BrowseToPage(_pageUrl);

        // Act  
        var (item, link) = GetMySchoolsNavigationItem(doc);

        // Assert
        Assert.DoesNotContain(ActiveItemClass, item?.ClassList!);
        Assert.Null(link?.GetAttribute("aria-current"));
    }

    [Fact]
    public async Task ServiceNavigation_MySchoolsLink_IsActiveAndCurrent_OnMySchoolsViewPage()
    {
        // Arrange
        _comparisonService
            .Setup(a => a.GetSavedEstablishments())
            .Returns(["123456", "654321"]);

        _establishmentService
            .Setup(a => a.GetEstablishmentsAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(
                [
                    new(){ URN = "123456", EstablishmentName = "School 1" }
                ]);

        var doc = await Fixture.BrowseToPage("/my-schools/view");

        // Act
        var (item, link) = GetMySchoolsNavigationItem(doc);

        // Assert
        Assert.Contains(ActiveItemClass, item?.ClassList!);
        Assert.Equal("page", link?.GetAttribute("aria-current"));
    }

    [Fact]
    public async Task ServiceNavigation_MySchoolsLink_IsActiveButNotCurrent_OnNoSchoolsAddedPage()
    {
        // Arrange
        _comparisonService
            .Setup(a => a.GetSavedEstablishments())
            .Returns([]);

        var doc = await Fixture.BrowseToPage("/my-schools/no-schools-added");

        // Act
        var (item, link) = GetMySchoolsNavigationItem(doc);

        // Assert
        Assert.Contains(ActiveItemClass, item?.ClassList!);
        Assert.Equal("true", link?.GetAttribute("aria-current"));
    }

    private static (IElement? item, IElement? link) GetMySchoolsNavigationItem(IDocument doc)
    {
        var mySchoolsViewLinkWrapper = doc.QuerySelector("#my-schools-view-link");
        Assert.NotNull(mySchoolsViewLinkWrapper);
        var mySchoolsViewLink = mySchoolsViewLinkWrapper?.QuerySelector("a");
        Assert.NotNull(mySchoolsViewLink);
        return (mySchoolsViewLinkWrapper, mySchoolsViewLink);
    }
}
