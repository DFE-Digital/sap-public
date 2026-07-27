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
public class EnglishMathsAcademicPerformacePageTests : PageTestsBase
{
    private string _pageRoute = "/16-to-19-performance/english-and-maths";
    private string _urn = "100279";
    private string _laName = "Test LA";
    private readonly EstablishmentServiceModel _establishment = new();
    private readonly EnglishMathsQualificationModel _englishMathsQualificationModel;
    private readonly Mock<IEnglishAndMathsQualificationsService> _englishAndMathsQualificationsService = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;


    public EnglishMathsAcademicPerformacePageTests(WebAppFixture fixture) : base(fixture)
    {
        _englishAndMathsQualificationsService = UseMock<IEnglishAndMathsQualificationsService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _establishment = new EstablishmentTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage5(true)
            .WithSixthForm(true)
            .WithLAName(_laName)
            .BuildServiceModel();

        _englishMathsQualificationModel = GetEnglishMathsQualificationModel();

        _mockEstablishmentService
           .Setup(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(_establishment);

        _englishAndMathsQualificationsService
            .Setup(s => s.GetEnglishAndMathsQualificationDetailsAsync(_urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_englishMathsQualificationModel);
    }

    [Fact]
    public async Task EnglishMathsQualifications_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var expectedTitle = $"School100279 - 16 to 19 - English and maths - School Profiles - GOV.UK";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Fact]
    public async Task EnglishMathsQualifications_DisplaysHeading()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelectorAll("h2");
        Assert.NotNull(heading[1]);
        Assert.Contains("English and maths", heading[1].TextContent.Trim());
    }

    [Fact]
    public async Task EnglishMathsQualifications_Displays_VerticalNavigation()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.NotNull(doc.QuerySelector(".moj-side-navigation"));
        Assert.Equal(3, doc.QuerySelectorAll(".moj-side-navigation__item").Length);
        Assert.Single(doc.QuerySelectorAll(".moj-side-navigation__item--active"));
    }

    [Fact]
    public async Task EnglishMathsQualifications_Has_Correct_Sub_Navigation_Links()
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
    public async Task EnglishMathsQualifications_DisplaysTechnicalGuidanceLink()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var techGuidanceLink = doc.QuerySelector("#tech-guidance-link");
        Assert.NotNull(techGuidanceLink);
        Assert.Contains("https://www.gov.uk/government/publications/16-to-19-accountability-headline-measures-technical-guide", techGuidanceLink.GetAttribute("href"));
        Assert.Equal("16 to 19 accountability technical guide (opens in a new tab)", techGuidanceLink.InnerHtml.Trim());
    }

    [Fact]
    public async Task EnglishMathsQualifications_Displays_ProgressScore()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("Number of students", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 0, 1));
        Assert.Contains("School or college", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 0, 2));
        Assert.Contains($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 0, 3));
        Assert.Contains("England average", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 0, 4));

        var expectedModel = GetEnglishMathsQualificationModel();

        Assert.Equal("Average English progress", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 1, 0));
        Assert.Equal(expectedModel.AverageEnglishProgress!.NumberOfStudents!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 1, 0));
        Assert.Equal(expectedModel.AverageEnglishProgress!.SchoolOrCollege!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 1, 1));
        Assert.Equal(expectedModel.AverageEnglishProgress!.LaAverage!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 1, 2));
        Assert.Equal(expectedModel.AverageEnglishProgress!.EnglandAverage!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 1, 3));

        Assert.Equal("Entered for English qualification", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 2, 0));
        Assert.Equal(string.Empty, doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 2, 0));
        Assert.Equal(expectedModel.EnteredForEnglishQualification!.SchoolOrCollege!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 2, 1));
        Assert.Equal(expectedModel.EnteredForEnglishQualification!.LaAverage!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 2, 2));
        Assert.Equal(expectedModel.EnteredForEnglishQualification!.EnglandAverage!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 2, 3));

        Assert.Equal("Average maths progress", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 3, 0));
        Assert.Equal(expectedModel.AverageMathsProgress!.NumberOfStudents!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 3, 0));
        Assert.Equal(expectedModel.AverageMathsProgress!.SchoolOrCollege!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 3, 1));
        Assert.Equal(expectedModel.AverageMathsProgress!.LaAverage!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 3, 2));
        Assert.Equal(expectedModel.AverageMathsProgress!.EnglandAverage!.Value.ToString(), doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 3, 3));

        Assert.Equal("Entered for maths qualification", doc.GetTableHeaderContentByIdAndIndex("english-maths-scores-and-progress", 4, 0));
        Assert.Equal(string.Empty, doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 4, 0));
        Assert.Equal(expectedModel.EnteredForMathsQualification!.SchoolOrCollege!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 4, 1));
        Assert.Equal(expectedModel.EnteredForMathsQualification!.LaAverage!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 4, 2));
        Assert.Equal(expectedModel.EnteredForMathsQualification!.EnglandAverage!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("english-maths-scores-and-progress", 4, 3));

    }

    private EnglishMathsQualificationModel GetEnglishMathsQualificationModel()
    {
        return new EnglishMathsQualificationModel
        {
            Urn = _establishment.URN,
            SchoolName = _establishment.EstablishmentName,
            IsKS2 = false,
            IsKS4 = false,
            IsKS5 = true,
            LAName = "Test LA",
            AverageEnglishProgress = new EnglishMathsScoreModel
            {
                NumberOfStudents = new CodedDouble(1, string.Empty, "1"),
                SchoolOrCollege = new CodedDouble(2, string.Empty, "2"),
                LaAverage = new CodedDouble(3, string.Empty, "3"),
                EnglandAverage = new CodedDouble(4, string.Empty, "4")
            },
            AverageMathsProgress = new EnglishMathsScoreModel
            {
                NumberOfStudents = new CodedDouble(5, string.Empty, "5"),
                SchoolOrCollege = new CodedDouble(6, string.Empty, "6"),
                LaAverage = new CodedDouble(7, string.Empty, "7"),
                EnglandAverage = new CodedDouble(8, string.Empty, "8")
            },
            EnteredForEnglishQualification = new EnglishMathsScoreModel
            {
                NumberOfStudents = CodedDouble.Empty,
                SchoolOrCollege = new CodedDouble(10, string.Empty, "10"),
                LaAverage = new CodedDouble(11, string.Empty, "11"),
                EnglandAverage = new CodedDouble(12, string.Empty, "12")
            },
            EnteredForMathsQualification = new EnglishMathsScoreModel
            {
                NumberOfStudents = CodedDouble.Empty,
                SchoolOrCollege = new CodedDouble(14, string.Empty, "14"),
                LaAverage = new CodedDouble(15, string.Empty, "15"),
                EnglandAverage = new CodedDouble(16, string.Empty, "16")
            },
        };
    }
}
