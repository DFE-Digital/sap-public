using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles;

[Collection("WebAppCollection")]
public class ScaledScoresAcademicPerformacePageTests : PageTestsBase
{
    private string _pageRoute = "/primary-performance/subject-scaled-scores";
    private string _urn = "149976";
    private string _laName = "Test LA";
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly KS2ScaledScoreModel _scaledScoreModel;
    private readonly Mock<IKS2ScaledScoreService> _scaledScoreService  = new();
    
    public ScaledScoresAcademicPerformacePageTests(WebAppFixture fixture) : base(fixture)
    {
        _scaledScoreService = UseMock<IKS2ScaledScoreService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage2(true)
            .WithLAName(_laName)
            .BuildServiceModel();

        _scaledScoreModel = GetScaledScoreModel();

        _mockEstablishmentService
           .Setup(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(_establishment);

        _scaledScoreService
            .Setup(s => s.GetScaledScoreModel(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_scaledScoreModel);
    }

    [Fact]
    public async Task ScaledScore_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var expectedTitle = $"School149976 - Primary Subject scaled scores - School Profiles - GOV.UK";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Fact]
    public async Task ScaledScore_DisplaysHeading()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelectorAll("h2");
        Assert.NotNull(heading[1]);
        Assert.Contains("Scaled scores", heading[1].TextContent.Trim());
    }

    [Fact]
    public async Task ScaledScore_Displays_VerticalNavigation()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(5, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task ScaledScore_Has_Correct_Sub_Navigation_Links()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(4, links.Length);
    }

    [Fact]
    public async Task ScaledScore_Displays_Read_AverageScore()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 1, 0));
        Assert.Contains($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 3, 0));

        var expectedModel = GetScaledScoreModel();

        Assert.Equal("2022 to 2023", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 0, 1));
        Assert.Equal(expectedModel.ReadAverageEstablishment!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 1, 0));
        Assert.Equal(expectedModel.ReadAverageLA!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 2, 0));
        Assert.Equal(expectedModel.ReadAverageEngland!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 3, 0));

        Assert.Equal("2023 to 2024", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 0, 2));
        Assert.Equal(expectedModel.ReadAverageEstablishment!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 1, 1));
        Assert.Equal(expectedModel.ReadAverageLA!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 2, 1));
        Assert.Equal(expectedModel.ReadAverageEngland!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 3, 1));

        Assert.Equal("2024 to 2025", doc.GetTableHeaderContentByIdAndIndex("read-data-overtime-table", 0, 3));
        Assert.Equal(expectedModel.ReadAverageEstablishment!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 1, 2));
        Assert.Equal(expectedModel.ReadAverageLA!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 2, 2));
        Assert.Equal(expectedModel.ReadAverageEngland!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("read-data-overtime-table", 3, 2));
    }


    [Fact]
    public async Task ScaledScore_Displays_Maths_AverageScore()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("School", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 1, 0));
        Assert.Contains($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 2, 0));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 3, 0));

        var expectedModel = GetScaledScoreModel();

        Assert.Equal("2022 to 2023", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 0, 1));
        Assert.Equal(expectedModel.MathsAverageEstablishment!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 1, 0));
        Assert.Equal(expectedModel.MathsAverageLA!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 2, 0));
        Assert.Equal(expectedModel.MathsAverageEngland!.TwoYearsAgo!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 3, 0));

        Assert.Equal("2023 to 2024", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 0, 2));
        Assert.Equal(expectedModel.MathsAverageEstablishment!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 1, 1));
        Assert.Equal(expectedModel.MathsAverageLA!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 2, 1));
        Assert.Equal(expectedModel.MathsAverageEngland!.PreviousYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 3, 1));

        Assert.Equal("2024 to 2025", doc.GetTableHeaderContentByIdAndIndex("maths-data-overtime-table", 0, 3));
        Assert.Equal(expectedModel.MathsAverageEstablishment!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 1, 2));
        Assert.Equal(expectedModel.MathsAverageLA!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 2, 2));
        Assert.Equal(expectedModel.MathsAverageEngland!.CurrentYear!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("maths-data-overtime-table", 3, 2));
    }

    private KS2ScaledScoreModel GetScaledScoreModel()
    {
        return new KS2ScaledScoreModel
        {
            ReadAverageEstablishment = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3"),
            },
            ReadAverageLA = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.1, string.Empty, "1.1"),
                PreviousYear = new CodedDouble(2.1, string.Empty, "2.1"),
                TwoYearsAgo = new CodedDouble(3.1, string.Empty, "3.1"),
            },
            ReadAverageEngland = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.2, string.Empty, "1.2"),
                PreviousYear = new CodedDouble(2.2, string.Empty, "2.2"),
                TwoYearsAgo = new CodedDouble(3.2, string.Empty, "3.2"),
            },
            MathsAverageEstablishment = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.3, string.Empty, "1.3"),
                PreviousYear = new CodedDouble(2.2, string.Empty, "2.3"),
                TwoYearsAgo = new CodedDouble(3.3, string.Empty, "3.3"),
            },
            MathsAverageLA = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.4, string.Empty, "1.4"),
                PreviousYear = new CodedDouble(2.4, string.Empty, "2.4"),
                TwoYearsAgo = new CodedDouble(3.4, string.Empty, "3.4"),
            },
            MathsAverageEngland = new Core.Entities.RelativeYearValues<CodedDouble>()
            {
                CurrentYear = new CodedDouble(1.5, string.Empty, "1.5"),
                PreviousYear = new CodedDouble(2.5, string.Empty, "2.5"),
                TwoYearsAgo = new CodedDouble(3.5, string.Empty, "3.5"),
            }
        };
    }
}
