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
public class DisadvantagedPupilsPageTests : PageTestsBase
{
    private const string Route = "/primary-performance/pupil-progress/disadvantaged-pupils";
    private readonly string _urn = "143034";
    private readonly string _schoolName = "St Paul's Church of England Academy";
    private readonly Mock<IKS2PupilProgressService> _mockProgressService;

    public DisadvantagedPupilsPageTests(WebAppFixture fixture) : base(fixture)
    {
        _mockProgressService = UseMock<IKS2PupilProgressService>();

        var establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName(_schoolName)
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

    private void SetupProgress(KS2PupilPerformanceBandings performance) =>
        _mockProgressService
            .Setup(a => a.GetDisadvantagedPupilProgressAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(performance);

    [Fact]
    public async Task Page_Displays_AcademicYear_Heading_AndIntroduction()
    {
        SetupProgress(new KS2PupilPerformanceBandings { Urn = _urn });

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        var body = doc.QuerySelector("#guide-contents")!.TextContent;
        Assert.Contains("Information in this section is for the", body);
        Assert.Contains("academic year.", body);
        Assert.Contains(doc.GetElementsByTagName("h3"), x => x.TextContent.Trim() == "Progress made by disadvantaged pupils");
        Assert.Contains("This shows the progress disadvantaged pupils made at this school between year 2 (key stage 1) and year 6 (key stage 2).", body);
        Assert.Contains("These scores help show how well the school supports pupils in making progress during their time in primary school.", body);
    }

    [Fact]
    public async Task Page_Displays_BothDetailsComponents_Collapsed_WithUniqueIds()
    {
        SetupProgress(new KS2PupilPerformanceBandings { Urn = _urn });

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        var scoreDetails = doc.QuerySelector("#details-progress-score");
        var groupDetails = doc.QuerySelector("#details-disadvantaged-pupils-group");
        Assert.NotNull(scoreDetails);
        Assert.NotNull(groupDetails);
        Assert.Equal(2, doc.QuerySelectorAll("details.govuk-details").Length);
        Assert.False(scoreDetails.HasAttribute("open"));
        Assert.False(groupDetails.HasAttribute("open"));

        Assert.Equal("What the progress score means and how it's worked out", scoreDetails.QuerySelector(".govuk-details__summary-text")!.TextContent.Trim());
        Assert.Contains("The progress score shows how much progress pupils at this school made compared with similar pupils across England.", scoreDetails.TextContent);
        Assert.Contains("How the number of pupils affects the score", scoreDetails.TextContent);

        Assert.Equal("Who is included in the disadvantaged pupils group", groupDetails.QuerySelector(".govuk-details__summary-text")!.TextContent.Trim());
        Assert.Contains("were eligible for free school meals at any time during the last 6 years", groupDetails.TextContent);
        Assert.Contains("have been in the care of the local council for a day or more", groupDetails.TextContent);
        Assert.Contains("have been adopted from care", groupDetails.TextContent);
    }

    [Fact]
    public async Task Page_Displays_ProgressTables_WithSchoolLocalAuthorityAndEnglandScores()
    {
        SetupProgress(new KS2PupilPerformanceBandings
        {
            Urn = _urn,
            EstablishmentReadingScore = Score(1.1),
            EstablishmentWritingScore = Score(2.2),
            EstablishmentMathsScore = Score(3.3),
            LocalAuthorityReadingScore = Score(-1.1),
            LocalAuthorityWritingScore = Score(-2.2),
            LocalAuthorityMathsScore = Score(-3.3),
            EnglandAverageReadingScore = Score(0.4),
            EnglandAverageWritingScore = Score(0.5),
            EnglandAverageMathsScore = Score(0.6)
        });

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        var expected = new Dictionary<string, string[]>
        {
            ["reading"] = ["1.1", "-1.1", "0.4"],
            ["writing"] = ["2.2", "-2.2", "0.5"],
            ["maths"] = ["3.3", "-3.3", "0.6"]
        };

        foreach (var (subject, scores) in expected)
        {
            var tableId = $"{subject}-progress-table";
            Assert.Equal("Comparison group", doc.GetTableHeaderContentByIdAndIndex(tableId, 0, 0));
            Assert.Equal("Progress score", doc.GetTableHeaderContentByIdAndIndex(tableId, 0, 1));
            Assert.Equal($"Disadvantaged pupils at {_schoolName}", doc.GetTableHeaderContentByIdAndIndex(tableId, 1, 0));
            Assert.Equal(scores[0], doc.GetTableCellContentByIdAndIndex(tableId, 1, 0));
            Assert.Equal(scores[1], doc.GetTableCellContentByIdAndIndex(tableId, 2, 0));
            Assert.Equal("England average", doc.GetTableHeaderContentByIdAndIndex(tableId, 3, 0));
            Assert.Equal(scores[2], doc.GetTableCellContentByIdAndIndex(tableId, 3, 0));
        }

        var headings = doc.GetElementsByTagName("h4").Select(h => h.TextContent.Trim()).ToList();
        Assert.Contains("Reading", headings);
        Assert.Contains("Writing", headings);
        Assert.Contains("Maths", headings);
    }

    [Fact]
    public async Task Page_Displays_NotAvailable_WhenScoresMissing()
    {
        SetupProgress(new KS2PupilPerformanceBandings { Urn = _urn });

        var doc = await Fixture.BrowseToPage(BuildUrl(_urn, _schoolName, Route));

        foreach (var subject in new[] { "reading", "writing", "maths" })
        {
            var tableId = $"{subject}-progress-table";
            for (var row = 1; row <= 3; row++)
            {
                Assert.Equal("Not available", doc.GetTableCellContentByIdAndIndex(tableId, row, 0));
            }
        }
    }
}
