using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS2;

[Collection("WebAppCollection")]
public class GirlsAndBoysPupilsPageTests : PageTestsBase
{
    private const string Route = "/primary-performance/pupil-progress/girls-and-boys";
    private const string LocalAuthorityName = "Hammersmith and Fulham";
    private readonly string _urn = "143034";
    private readonly string _schoolName = "St Paul's Church of England Academy";
    private readonly Mock<IKS2PupilProgressService> _mockProgressService;

    public GirlsAndBoysPupilsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockProgressService = UseMock<IKS2PupilProgressService>();

        var establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
            .WithLAName(LocalAuthorityName)
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(false)
            .BuildServiceModel();

        UseMock<IEstablishmentService>()
            .Setup(a => a.GetEstablishmentMinimumAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment);

        var featureManagerMock = UseMock<IFeatureManager>();
        featureManagerMock
            .Setup(f => f.IsEnabledAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
    }

    private static CodedDouble Score(double value) => new(value, string.Empty, value.ToString());

    private void SetupProgress(KS2PupilPerformanceBandings[] groups) =>
        _mockProgressService
            .Setup(a => a.GetGirlBoyPupilProgressAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(groups);

    private void SetupEmptyProgress() =>
        SetupProgress(
            [
                new() { Urn = _urn, CharacteristicName = "Girls" },
                new() { Urn = _urn, CharacteristicName = "Boys" }
            ]);

    [Fact]
    public async Task Page_Displays_AcademicYear_Heading_AndIntroduction()
    {
        SetupEmptyProgress();

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        var body = doc.QuerySelector("#guide-contents")!.TextContent;
        Assert.Contains("Information in this section is for the", body);
        Assert.Contains("academic year.", body);
        Assert.Contains(doc.GetElementsByTagName("h3"), x => x.TextContent.Trim() == "Progress made by girls and boys");
        Assert.Contains("This shows the progress made by girls and boys at this school", body);
    }

    [Fact]
    public async Task Page_Displays_BothGroupsPerSubject_WithAccessibleTables()
    {
        SetupEmptyProgress();

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        var containers = doc.QuerySelectorAll("#guide-contents .data-container");
        Assert.Equal(["Reading", "Writing", "Maths"], containers.Select(c => c.QuerySelector("h4")!.TextContent.Trim()));

        Assert.All(containers, container =>
        {
            Assert.Equal(["Girls", "Boys"], container.QuerySelectorAll("h5").Select(h => h.TextContent.Trim()));
            Assert.Equal(2, container.QuerySelectorAll("table").Length);
            var subject = container.QuerySelector("h4")!.TextContent.Trim();
            foreach (var group in new[] { "girls", "boys" })
            {
                var table = container.QuerySelector($"#{subject.ToLowerInvariant()}-{group}-progress-table")!;
                Assert.Equal($"{subject} progress scores for {group}", table.QuerySelector("caption")!.TextContent.Trim());
                Assert.Equal(2, table.QuerySelectorAll("thead th[scope='col']").Length);
                Assert.Equal(3, table.QuerySelectorAll("tbody th[scope='row']").Length);
            }
        });
    }

    [Fact]
    public async Task Page_Displays_ProgressTables_WithSchoolLocalAuthorityAndEnglandScores()
    {
        SetupProgress(
        [
            new KS2PupilPerformanceBandings
            {
                Urn = _urn,
                CharacteristicName = "Girls",
                EstablishmentReadingScore = Score(1.1),
                EstablishmentWritingScore = Score(1.2),
                EstablishmentMathsScore = Score(1.3),
                LocalAuthorityReadingScore = Score(-1.1),
                LocalAuthorityWritingScore = Score(-2.2),
                LocalAuthorityMathsScore = Score(-1.3),
                EnglandAverageReadingScore = Score(0.1),
                EnglandAverageWritingScore = Score(0.2),
                EnglandAverageMathsScore = Score(0.3)
            },
            new KS2PupilPerformanceBandings
            {
                Urn = _urn,
                CharacteristicName = "Boys",
                EstablishmentReadingScore = Score(2.1),
                EstablishmentWritingScore = Score(2.2),
                EstablishmentMathsScore = Score(2.3),
                LocalAuthorityReadingScore = Score(-2.1),
                LocalAuthorityWritingScore = Score(-2.2),
                LocalAuthorityMathsScore = Score(-2.3),
                EnglandAverageReadingScore = Score(0.4),
                EnglandAverageWritingScore = Score(0.5),
                EnglandAverageMathsScore = Score(0.6)
            }
        ]);

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        AssertProgressTable(doc, "reading", "Girls", "1.1", "-1.1", "0.1");
        AssertProgressTable(doc, "writing", "Girls", "1.2", "-2.2", "0.2");
        AssertProgressTable(doc, "maths", "Girls", "1.3", "-1.3", "0.3");
        AssertProgressTable(doc, "reading", "Boys", "2.1", "-2.1", "0.4");
        AssertProgressTable(doc, "writing", "Boys", "2.2", "-2.2", "0.5");
        AssertProgressTable(doc, "maths", "Boys", "2.3", "-2.3", "0.6");
    }

    [Fact]
    public async Task Page_Displays_NotAvailable_WhenScoresMissing()
    {
        SetupEmptyProgress();

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        foreach (var subject in new[] { "reading", "writing", "maths" })
        {
            foreach (var group in new[] { "girls", "boys" })
            {
                var tableId = $"{subject}-{group}-progress-table".ToLowerInvariant();
                for (var row = 1; row <= 3; row++)
                {
                    Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex(tableId, row, 0));
                }
            }
        }
    }

    [Theory]
    [InlineData("c")]
    [InlineData("z")]
    [InlineData("x")]
    public async Task Page_Displays_NotAvailableForCodedScores_AndPreservesZeroAndAvailableScores(string code)
    {
        SetupProgress(
        [
            new()
            {
                Urn = _urn,
                CharacteristicName = "Girls",
                EstablishmentReadingScore = new CodedDouble(null, "Suppressed", code),
                LocalAuthorityReadingScore = Score(0),
                EnglandAverageReadingScore = Score(1.26)
            },
            new()
            {
                Urn = _urn,
                CharacteristicName = "Boys",
                EstablishmentReadingScore = Score(-1.1),
                LocalAuthorityReadingScore = new CodedDouble(null, "Suppressed", code),
                EnglandAverageReadingScore = Score(0)
            }
        ]);

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        AssertProgressTable(doc, "reading", "Girls", "Not available", "0.0", "1.3");
        AssertProgressTable(doc, "reading", "Boys", "-1.1", "Not available", "0.0");
        Assert.Equal(6, doc.QuerySelectorAll("#guide-contents table").Length);
    }

    private void AssertProgressTable(IDocument doc,
        string subject,
        string group,
        string expectedSchoolScore,
        string expectedLocalAuthorityScore,
        string expectedEnglandScore)
    {
        var tableId = $"{subject}-{group}-progress-table".ToLowerInvariant();

        Assert.Equal("Comparison group", doc.GetTableHeaderContentByIdAndIndex(tableId, 0, 0));
        Assert.Equal("Progress score", doc.GetTableHeaderContentByIdAndIndex(tableId, 0, 1));
        Assert.Equal($"{group} at {_schoolName}", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
        Assert.Equal(expectedSchoolScore, doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
        Assert.Equal($"{LocalAuthorityName} average", doc.GetTableHeaderContentByIdAndIndex(tableId, 2, 0));
        Assert.Equal(expectedLocalAuthorityScore, doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
        Assert.Equal("England average", doc.GetTableHeaderContentByIdAndIndex(tableId, 3, 0));
        Assert.Equal(expectedEnglandScore, doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));

    }
}