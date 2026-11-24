using AngleSharp.Dom;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Core.ServiceModels.Overview;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles;

[Collection("WebAppCollection")]
public class OverviewKs2ProgressBandingsPageTests : PageTestsBase
{
    private const string Urn = "143034";
    private const string SchoolName = "St Paul's Church of England Academy";
    private const string SmallCohortText = "These results are based on a small number of pupils. This means that a few pupils with particularly high or low results can have a greater impact on the overall result.";

    private readonly Mock<IOverviewService> _mockOverviewService;

    public OverviewKs2ProgressBandingsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockOverviewService = UseMock<IOverviewService>();
    }

    [Fact]
    public async Task FeatureFlagDisabled_DisplaysExistingComponent_AndNotBandings()
    {
        SetupFeatureFlag(false);
        SetupOverview();

        var doc = await BrowseToOverview();

        Assert.Null(doc.QuerySelector("#primary-progress-bandings"));
        Assert.Null(doc.QuerySelector("#primary-progress-significance-warning"));
        Assert.Contains("COVID-19", PupilProgressContent(doc).TextContent);
    }

    [Fact]
    public async Task FeatureFlagEnabled_DisplaysBandings_AndRemovesCovidCaveat()
    {
        SetupFeatureFlag(true);
        SetupOverview();

        var doc = await BrowseToOverview();

        Assert.NotNull(doc.QuerySelector("#primary-progress-bandings"));
        Assert.DoesNotContain("COVID-19", PupilProgressContent(doc).TextContent);
    }

    [Fact]
    public async Task FeatureFlagEnabled_DisplaysHeaderIntroAndAcademicYear()
    {
        SetupFeatureFlag(true);
        SetupOverview();

        var doc = await BrowseToOverview();
        var content = PupilProgressContent(doc);

        Assert.Equal("Progress made between year 2 and year 6", content.QuerySelector(".govuk-summary-card__title")!.TextContent.Trim());

        var text = Normalise(content.TextContent);

        Assert.Contains("This shows the progress pupils made at this school between year 2 (key stage 1) and year 6 (key stage 2). Their progress is compared with pupils across England who started at a similar level at the end of year 2.", text);
        Assert.Contains("Data for the 2025 to 2026 academic year.", text);
    }

    [Fact]
    public async Task FeatureFlagEnabled_DisplaysHowPupilsProgressedHeading()
    {
        SetupFeatureFlag(true);
        SetupOverview();

        var doc = await BrowseToOverview();

        var heading = PupilProgressContent(doc)
            .QuerySelectorAll("h5")
            .SingleOrDefault(h => h.TextContent.Trim() == "How pupils progressed");

        Assert.NotNull(heading);
    }

    [Fact]
    public async Task FeatureFlagEnabled_DisplaysReadingWritingAndMathsSectionsInOrder()
    {
        SetupFeatureFlag(true);
        SetupOverview();

        var doc = await BrowseToOverview();

        var subjects = Bandings(doc)
            .Select(b => b.QuerySelector(".ataglance-progress-banding__subject")!.TextContent.Trim())
            .ToList();

        Assert.Equal(["Reading", "Writing", "Maths"], subjects);
    }

    [Theory]
    [InlineData("1", "Well above average", "govuk-tag--green", "This school's pupils made much more progress than pupils nationally.")]
    [InlineData("2", "Above average", "app-tag--above-average", "This school's pupils made more progress than pupils nationally.")]
    [InlineData("3", "Average", "govuk-tag--blue", "This school's pupils made similar progress to pupils nationally.")]
    [InlineData("4", "Below average", "govuk-tag--orange", "This school's pupils made less progress than pupils nationally.")]
    [InlineData("5", "Well below average", "govuk-tag--red", "This school's pupils made much less progress than pupils nationally.")]
    [InlineData("SUPP", "Not applicable", "govuk-tag--grey", "This school's data is 'Not applicable'")]
    public async Task FeatureFlagEnabled_DisplaysCorrectLabelColourAndExplanation(
        string code,
        string expectedLabel,
        string expectedCssClass,
        string expectedDescription)
    {
        SetupFeatureFlag(true);
        SetupOverview(reading: code);

        var doc = await BrowseToOverview();

        var reading = Bandings(doc)[0];
        var tag = reading.QuerySelector(".govuk-tag")!;

        Assert.Equal(expectedLabel, tag.TextContent.Trim());
        Assert.Contains(expectedCssClass, tag.ClassList);
        Assert.Equal(expectedDescription, reading.QuerySelector(".ataglance-progress-banding__description")!.TextContent.Trim());
    }

    [Fact]
    public async Task FeatureFlagEnabled_DisplaysEachSubjectsOwnBanding()
    {
        SetupFeatureFlag(true);
        SetupOverview(reading: "1", writing: "3", maths: "5");

        var doc = await BrowseToOverview();

        var labels = Bandings(doc)
            .Select(b => b.QuerySelector(".govuk-tag")!.TextContent.Trim())
            .ToList();

        Assert.Equal(["Well above average", "Average", "Well below average"], labels);
    }

    [Fact]
    public async Task FeatureFlagEnabled_WhenAllSignificant_DoesNotDisplayWarning()
    {
        SetupFeatureFlag(true);
        SetupOverview();

        var doc = await BrowseToOverview();

        Assert.Null(doc.QuerySelector("#primary-progress-significance-warning"));
        Assert.DoesNotContain(SmallCohortText, Normalise(doc.Body!.TextContent));
    }

    [Theory]
    [InlineData("0", "1", "1", "Reading results based on a small number of pupils")]
    [InlineData("1", "0", "1", "Writing results based on a small number of pupils")]
    [InlineData("1", "1", "0", "Maths results based on a small number of pupils")]
    [InlineData("0", "0", "1", "Reading and writing results based on a small number of pupils")]
    [InlineData("0", "1", "0", "Reading and maths results based on a small number of pupils")]
    [InlineData("1", "0", "0", "Writing and maths results based on a small number of pupils")]
    [InlineData("0", "0", "0", "Reading, writing and maths results based on a small number of pupils")]
    public async Task FeatureFlagEnabled_WhenNotSignificant_DisplaysWarningWithIconAndSmallCohortText(
        string readingSignificance,
        string writingSignificance,
        string mathsSignificance,
        string expectedWarning)
    {
        SetupFeatureFlag(true);
        SetupOverview(
            readingSignificance: readingSignificance,
            writingSignificance: writingSignificance,
            mathsSignificance: mathsSignificance);

        var doc = await BrowseToOverview();

        var warning = doc.QuerySelector("#primary-progress-significance-warning");
        Assert.NotNull(warning);

        Assert.NotNull(warning.QuerySelector(".govuk-warning-text__icon"));

        var warningText = warning.QuerySelector(".govuk-warning-text__text")!.TextContent;
        Assert.Contains(expectedWarning, Normalise(warningText));

        Assert.Contains(SmallCohortText, Normalise(warning.TextContent));
    }

    [Fact]
    public async Task FeatureFlagEnabled_WhenNoKs2Data_DisplaysNotApplicableForAllSubjects()
    {
        SetupFeatureFlag(true);
        _mockOverviewService
            .Setup(s => s.GetOverviewAsync(Urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OverviewModel { Urn = Urn, SchoolName = SchoolName, IsKS2 = true });

        var doc = await BrowseToOverview();

        var bandings = Bandings(doc);

        Assert.Equal(3, bandings.Count);
        Assert.All(bandings, b =>
        {
            Assert.Equal("Not applicable", b.QuerySelector(".govuk-tag")!.TextContent.Trim());
            Assert.Contains("govuk-tag--grey", b.QuerySelector(".govuk-tag")!.ClassList);
        });
        Assert.Null(doc.QuerySelector("#primary-progress-significance-warning"));
    }

    [Fact]
    public async Task FeatureFlagEnabled_RetainsPupilProgressLink()
    {
        SetupFeatureFlag(true);
        SetupOverview();

        var doc = await BrowseToOverview();

        var link = PupilProgressContent(doc)
            .QuerySelectorAll("a")
            .SingleOrDefault(a => a.TextContent.Contains("Find out more about the progress pupils make at this school for previous years"));

        Assert.NotNull(link);
        Assert.Contains("/primary-performance/pupil-progress", link.GetAttribute("href"));
    }

    private async Task<IDocument> BrowseToOverview() => await Fixture.BrowseToPage(BuildUrl(Urn, SchoolName, "/overview"));

    private static IElement PupilProgressContent(IDocument doc)
    {
        var content = doc.QuerySelector("#primary-at-a-glance-accordion-content-1");
        Assert.NotNull(content);
        return content;
    }

    private static List<IElement> Bandings(IDocument doc) => [.. doc.QuerySelectorAll("#primary-progress-bandings .ataglance-progress-banding")];

    private static string Normalise(string text) =>
        string.Join(' ', text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));

    private void SetupFeatureFlag(bool enabled)
    {
        var featureManager = UseMock<IFeatureManager>();
        featureManager
            .Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        featureManager
            .Setup(f => f.IsEnabledAsync(Constants.Constants.EnableKS2PupilCharacteristicDropdown))
            .ReturnsAsync(enabled);
    }

    private void SetupOverview(
        string reading = "2",
        string writing = "3",
        string maths = "4",
        string readingSignificance = "1",
        string writingSignificance = "1",
        string mathsSignificance = "1")
    {
        _mockOverviewService
            .Setup(s => s.GetOverviewAsync(Urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OverviewModel
            {
                Urn = Urn,
                SchoolName = SchoolName,
                IsKS2 = true,
                ReadingProgressBanding = Coded(reading),
                WritingProgressBanding = Coded(writing),
                MathsProgressBanding = Coded(maths),
                ReadingProgressSignificance = Coded(readingSignificance),
                WritingProgressSignificance = Coded(writingSignificance),
                MathsProgressSignificance = Coded(mathsSignificance)
            });
    }

    private static CodedString Coded(string value) => new(value, string.Empty, value);
}
