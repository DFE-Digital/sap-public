using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.UI.Infrastructure;

namespace SAPPub.Web.Tests.UI.KS5;


[Collection("Playwright Tests")]
public class KS5SubjectsEnteredTests(WebApplicationSetupFixture fixture) : BasePageTest(fixture)
{
    private const string _urn = "130499";
    private const string _schoolName = "holy-cross-college";
    private const string _schoolDisplayName = "Holy Cross College";

    private const string _basePageUrl = $"school/{_urn}/{_schoolName}/16-to-19-performance/subjects-entered/allqualifications";
    private const string _academicUrl = $"school/{_urn}/{_schoolName}/16-to-19-performance/subjects-entered/academicqualifications";
    private const string _vocationalUrl = $"school/{_urn}/{_schoolName}/16-to-19-performance/subjects-entered/vocationalandtechnicalqualifications";


    [Fact]
    public async Task KS5SubjectsEnteredPage_LoadsSuccessfully()
    {
        var response = await Page.GotoAsync(_basePageUrl);

        Assert.NotNull(response);
        Assert.Equal(200, response.Status);
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_HasCorrectTitle()
    {
        await Page.GotoAsync(_basePageUrl);

        var title = await Page.TitleAsync();

        Assert.Contains($"{_schoolDisplayName}", title);
        Assert.Contains("Subjects entered", title);
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_DisplaysMainHeading()
    {
        await Page.GotoAsync(_basePageUrl);

        var heading = await Page.Locator("h1").TextContentAsync();

        Assert.NotNull(heading);
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_DisplaysVerticalNavigation()
    {
        var nav = new VerticalNavigationHelper(Page);
        await Page.GotoAsync(_basePageUrl);

        await nav.ShouldBeVisibleAsync();
        await nav.ShouldHaveOneActiveItemAsync();
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_DisplaysSubNavigation()
    {
        await Page.GotoAsync(_basePageUrl);
        Assert.True(await Page.Locator("#sub-navigation-academic-performance").IsVisibleAsync());
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_DisplaysSubjectsTable()
    {
        await Page.GotoAsync(_basePageUrl);

        Assert.True(await Page.Locator("#filterSection").IsVisibleAsync());
        Assert.True(await Page.Locator("#qualificationSelector").IsVisibleAsync());
    }


    [Fact]
    public async Task KS5SubjectsEnteredPage_AcademicFilter_OnlyShowsAcademicRows()
    {
        await Page.GotoAsync(_academicUrl);
        var rows = Page.Locator("table.govuk-table tbody tr");
        var count = await rows.CountAsync();

        Assert.True(count > 0, "Expected at least one academic subject row");

        var content = await Page.ContentAsync();
        Assert.DoesNotContain("BTEC National Sport", content);
        Assert.DoesNotContain("Cambridge Technical ID", content);
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_VocationalFilter_OnlyShowsVocationalRows()
    {
        await Page.GotoAsync(_vocationalUrl);
        var rows = Page.Locator("table.govuk-table tbody tr");
        var count = await rows.CountAsync();

        Assert.True(count > 0, "Expected at least one vocational subject row");

        var content = await Page.ContentAsync();
        Assert.DoesNotContain("A level Mathematics", content);
        Assert.DoesNotContain("A level Biology", content);
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_DisplaysPagination()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

        // Act
        Assert.True(await Page.Locator("#subjects-entered-pagination").IsVisibleAsync());

    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_PaginationPrevious_PointsToEnglishAndMaths()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

        // Act
        var prevLink = Page.Locator("#subjects-entered-pagination .govuk-pagination__prev a");
        var text = await prevLink.TextContentAsync();

        Assert.Contains("English and maths", text?.Trim());
    }

    [Fact]
    public async Task KS5SubjectsEnteredPage_PaginationNext_PointsToDestinations()
    {
        // Arrange
        await Page.GotoAsync(_basePageUrl);

        // Act
        var prevLink = Page.Locator("#subjects-entered-pagination .govuk-pagination__next a");
        var text = await prevLink.TextContentAsync();

        Assert.Contains("Destinations", text?.Trim());
    }
}
