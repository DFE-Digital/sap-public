using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.Compare;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
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
    private readonly Mock<IAboutSchoolService> _mockAboutSchoolService = new();
    private readonly Mock<IEnglishAndMathsComparisionService> _mockEnglishAndMathsComparisonService = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService = new();

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
        var easting1 = "532301";
        var northing1 = "181746";
        var aboutSchoolsCompareModelList = new List<AboutSchoolComparisonModel>
            {
                new() { Urn = urn1, SchoolName = "Test School", Easting = easting1, Northing = northing1 },
                new() { Urn = urn2, SchoolName = "Test School 2" },
            };

        _mockAboutSchoolService
            .Setup(a => a.GetAboutSchoolForComparisonAsync(urnList, It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolsCompareModelList);

        var longLat = MappingHelper.ConvertToLatLon(easting1, northing1);

        // Act
        var result = await controller.AboutYourSchools(_mockAboutSchoolService.Object, urnList, It.IsAny<CancellationToken>()) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareAboutYourSchoolsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
        Assert.Equal(longLat?.Latitude, model.MapData.FirstOrDefault()?.Lat);
        Assert.Equal(longLat?.Longitude, model.MapData.FirstOrDefault()?.Lng);
        Assert.Equal("Test School", model.MapData.FirstOrDefault()?.Name);
        Assert.Equal(1, model.MapData?.Count());
        Assert.Equal(2, model.CompareAboutSchools.Count());
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
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = 90, PreviousYear = 80.5, TwoYearsAgo = 78 }
                    },
                    new()
                    {
                        Urn = urn2,
                        SchoolName = "Test School2",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = 60, PreviousYear = 70, TwoYearsAgo = 85.5 }
                    }

            ],
            EnglandAverage = new RelativeYearValues<double?> { CurrentYear = 80, PreviousYear = 89.3, TwoYearsAgo = 90.5 }
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

        // Assert DataOverTime

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);

        Assert.Equal("2022 to 2023", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal(
            [
                expectedResult.Establishments[0].EstablishmentData.TwoYearsAgo!.Value,
                    expectedResult.Establishments[1].EstablishmentData.TwoYearsAgo!.Value,
                    expectedResult.EnglandAverage.TwoYearsAgo!.Value,
                ],
            model.AllGcseOverTimeData.Datasets[0].Data
        );

        Assert.Equal("2023 to 2024", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal(
            [
                expectedResult.Establishments[0].EstablishmentData.PreviousYear!.Value,
                    expectedResult.Establishments[1].EstablishmentData.PreviousYear!.Value,
                    expectedResult.EnglandAverage.PreviousYear!.Value,
                ],
            model.AllGcseOverTimeData.Datasets[1].Data
        );

        Assert.Equal("2024 to 2025", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal(
            [
                expectedResult.Establishments[0].EstablishmentData.CurrentYear!.Value,
                    expectedResult.Establishments[1].EstablishmentData.CurrentYear!.Value,
                    expectedResult.EnglandAverage.CurrentYear!.Value,
                ],
            model.AllGcseOverTimeData.Datasets[2].Data
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
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = null, PreviousYear = null, TwoYearsAgo = null }
                    },
                    new()
                    {
                        Urn = urn2,
                        SchoolName = "Test School2",
                        EstablishmentData = new RelativeYearValues<double?> { CurrentYear = null, PreviousYear = null, TwoYearsAgo = null }
                    }

            ],
            EnglandAverage = new RelativeYearValues<double?> { CurrentYear = null, PreviousYear = null, TwoYearsAgo = null }
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

        // Assert DataOverTime

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);

        Assert.Equal("2022 to 2023", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[0].Data);

        Assert.Equal("2023 to 2024", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[1].Data);

        Assert.Equal("2024 to 2025", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[2].Data);
    }

        [Fact]
        public async Task NextSteps_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var controller = new SecondaryController();
            var urn1 = "111111";
            var urn2 = "222222";
            var schoolName1 = "zxy School";
            var schoolName2 = "abc School";
            var urnList = new List<string> { urn1, urn2 };

            _mockEstablishmentService
                .Setup(a => a.GetEstablishmentsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                [
                    new() { URN = urn1, EstablishmentName = schoolName1 }, 
                    new() { URN = urn2, EstablishmentName = schoolName2 }
                ]);

            // Act
            var result = await controller
                .NextSteps(_mockEstablishmentService.Object, urnList, It.IsAny<CancellationToken>()) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as CompareNextStepsViewModel;
            Assert.NotNull(model);
            Assert.Equal(2, model.URNs.Count);
            Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
            Assert.Equal(2, model.SchoolDetailList.Count());
            Assert.Equal(schoolName2, model.SchoolDetailList?.FirstOrDefault()?.EstablishmentName);
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
        var viewModel = result.Model as CompareDestinationsViewModel;
        Assert.NotNull(viewModel);
        Assert.Equal(2, viewModel.URNs.Count);

        var orderedEstablishments = (
            _httpContext.Items["Establishments"] as List<Establishment>)?
            .OrderBy(e => e.EstablishmentName).ToList();

        Assert.Collection(viewModel.SchoolDetails.Select(s => s.URN),
            first => Assert.Equal(orderedEstablishments![0].URN, first),
            second => Assert.Equal(orderedEstablishments![1].URN, second)
        );

        for (int i = 0; i < orderedEstablishments!.Count; i++)
        {
            Assert.Contains(viewModel.SchoolDetails, d =>
                d.URN == orderedEstablishments![i].URN &&
                d.SchoolName == orderedEstablishments[i].EstablishmentName
                );
            Assert.Contains(viewModel.SchoolDetails, d =>
                d.PercentInEducationEmploymentOrTraining == destinationsResultsModel.SchoolDetails.ToList()[i].PercentInEducationEmploymentOrTraining &&
                d.SixthForm!.Value == true
            );
        }

        Assert.Collection(viewModel.AllDestinationsData.Labels,
            first => Assert.Equal(viewModel.SchoolDetails.ToList()[0].SchoolName, first),
            second => Assert.Equal(viewModel.SchoolDetails.ToList()[1].SchoolName, second),
            third => Assert.Equal("England average", third));
        Assert.Collection(viewModel.AllDestinationsData.Data,
            first => Assert.Equal(viewModel.SchoolDetails.ToList()[0].PercentInEducationEmploymentOrTraining, first),
            second => Assert.Equal(viewModel.SchoolDetails.ToList()[1].PercentInEducationEmploymentOrTraining, second),
            third => Assert.Equal(viewModel.EnglandPercentage, third));

        Assert.Equal(englandPercentage, viewModel.EnglandPercentage);
    }
}