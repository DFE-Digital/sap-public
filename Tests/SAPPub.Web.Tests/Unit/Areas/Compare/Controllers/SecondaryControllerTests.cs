using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services.Compare;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Compare.Controllers;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Compare.Controllers;

public class SecondaryControllerTests
{
    private SecondaryController _controllerUnderTest = new();
    private readonly Mock<IDestinationsComparisonService> _mockDestinationsService = new();
    private readonly Mock<IEnglishAndMathsComparisionService> _mockEnglishAndMathsComparisonService = new();

    private List<string> _urns = ["123456", "234567"];
    private HttpContext _httpContext = new DefaultHttpContext();

    public SecondaryControllerTests()
    {
        // The action filter adds establishments to the HttpContext.Items collection, so simulate that in the test setup
        _httpContext.Items["Establishments"] = _urns
            .Select(urn => new EstablishmentTestBuilder()
                .WithURN(urn)
                .WithIsKeyStage4(true)
                .WithSixthForm(true)
                .Build())
            .ToList();

        _controllerUnderTest.ControllerContext = new ControllerContext
        {
            HttpContext = _httpContext
        };
    }

    [Fact]
    public async Task AboutYourSchools_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var controller = new SecondaryController();
        var urn1 = "123456";
        var urn2 = "234567";
        var urnList = new List<string> { urn1, urn2 };

        // Act
        var result = await controller.AboutYourSchools(urnList) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareAboutYourSchoolsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
    }

    [Fact]
    public async Task AcademicPerformancePupilProgressAndAttainment_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var controller = new SecondaryController();
        var urn1 = "123456";
        var urn2 = "234567";
        var urnList = new List<string> { urn1, urn2 };

        // Act
        var result = await controller.AcademicPerformancePupilAttainment(urnList) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareAcademicPerformancePupilAttainmentViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResults_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var urn1 = "123456";
        var urn2 = "789456";
        var expectedResult = new EnglishAndMathsComparisionResultsModel
        {
            Establishments =
            [
                new()
                    {
                        Urn = urn1,
                        SchoolName = "Test School1",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = 90 }
                    },
                    new()
                    {
                        Urn = urn2,
                        SchoolName = "Test School2",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = 60 }
                    }

            ],
            EnglandAverage = new RelativeYearValues<double?> { CurrentYear = 80 }
        };

        var controller = new SecondaryController();
        var urns = new List<string> { urn1, urn2 };

        _mockEnglishAndMathsComparisonService
            .Setup(s => s.GetComparisionResultsAsync(urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsComparisonService.Object,
            urns,
            It.IsAny<CancellationToken>()) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareAcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");

        // Assert Data
        Assert.Equal(
            [
                expectedResult.Establishments[0].EstablishmentData.CurrentYear!.Value,
                    expectedResult.Establishments[1].EstablishmentData.CurrentYear!.Value,
                    expectedResult.EnglandAverage.CurrentYear!.Value
            ],
            model.AllGcseData.Data
        );

        // Assert Labels
        Assert.Equal(
            [
                expectedResult.Establishments[0].SchoolName,
                    expectedResult.Establishments[1].SchoolName,
                    "England average"
            ],
            model.AllGcseData.Labels
        );

        // Assert Background colours
        Assert.Equal(
            [
                EstablishmentChartColour,
                    EstablishmentChartColour,
                    EnglandAverageChartColour,
                ],
            model.AllGcseData.BackgroundColors!
        );
    }

    [Fact]
    public async Task AcademicPerformanceEnglishAndMathsResults_NotAvailable_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var urn1 = "123456";
        var urn2 = "789456";
        var expectedResult = new EnglishAndMathsComparisionResultsModel
        {
            Establishments =
            [
                new()
                    {
                        Urn = urn1,
                        SchoolName = "Test School1",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = null }
                    },
                    new()
                    {
                        Urn = urn2,
                        SchoolName = "Test School2",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = null }
                    }

            ],
            EnglandAverage = new RelativeYearValues<double?> { CurrentYear = null }
        };

        var controller = new SecondaryController();
        var urns = new List<string> { urn1, urn2 };

        _mockEnglishAndMathsComparisonService
            .Setup(s => s.GetComparisionResultsAsync(urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsComparisonService.Object,
            urns,
            It.IsAny<CancellationToken>()) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareAcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");

        // Assert Data
        Assert.Equal([null, null, null], model.AllGcseData.Data);

        // Assert Labels
        Assert.Equal(
            [
                expectedResult.Establishments[0].SchoolName,
                    expectedResult.Establishments[1].SchoolName,
                    "England average"
            ],
            model.AllGcseData.Labels
        );

        // Assert Background colours
        Assert.Equal(
            [
                EstablishmentChartColour,
                    EstablishmentChartColour,
                    EnglandAverageChartColour,
                ],
            model.AllGcseData.BackgroundColors!
        );
    }

    [Fact]
    public async Task NextSteps_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var controller = new SecondaryController();
        var urn1 = "111111";
        var urn2 = "222222";
        var urnList = new List<string> { urn1, urn2 };

        // Act
        var result = await controller.NextSteps(urnList) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareNextStepsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
    }

    [Fact]
    public async Task Destinations_HasFullData_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var englandPercentage = 50.0;
        var establishmentDestinations = _urns.Select(urn =>
            new SchoolDestinationDetails
            {
                URN = urn,
                PercentInEducationEmploymentOrTraining = new Bogus.Faker().Random.Double(5, 100)
            }).ToList();
        var destinationsResultsModel = new DestinationsComparisonResultModel
        {
            EnglandPercentage = englandPercentage,
            SchoolDetails = establishmentDestinations
        };

        _mockDestinationsService
            .Setup(s => s.GetDestinationsDetailsAsync(_urns, It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsResultsModel);

        // Act
        var result = await _controllerUnderTest.Destinations(_mockDestinationsService.Object, _urns) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareDestinationsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);

        var orderedEstablishments = (
            _httpContext.Items["Establishments"] as List<Establishment>)?
            .OrderBy(e => e.EstablishmentName).ToList();

        Assert.Collection(model.URNs,
            first => Assert.Equal(orderedEstablishments![0].URN, first),
            second => Assert.Equal(orderedEstablishments![1].URN, second)
        );

        for (int i = 0; i < orderedEstablishments!.Count; i++)
        {
            Assert.Contains(model.SchoolDetails, d =>
                d.URN == orderedEstablishments![i].URN &&
                d.SchoolName == orderedEstablishments[i].EstablishmentName
                );
            Assert.Contains(model.SchoolDetails, d =>
                d.PercentInEducationEmploymentOrTraining == destinationsResultsModel.SchoolDetails.ToList()[i].PercentInEducationEmploymentOrTraining &&
                d.SixthForm.Value == true
            );
        }

        Assert.Equal(englandPercentage, model.EnglandPercentage);
    }
}