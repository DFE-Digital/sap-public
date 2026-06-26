using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Areas.Compare.Controllers;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Compare.Controllers
{
    public class SecondaryControllerTests
    {
        private readonly Mock<IEnglishAndMathsComparisionService> _mockEnglishAndMathsComparisonService = new();               
        private readonly Mock<IAboutSchoolService> _mockAboutSchoolService = new(); 

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
            Assert.Equal([null,null,null], model.AllGcseData.Data);

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
        public async Task Destinations_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var controller = new SecondaryController();
            var urn1 = "111111";
            var urn2 = "222222";
            var urnList = new List<string> { urn1, urn2 };

            // Act
            var result = await controller.Destinations(urnList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as CompareDestinationsViewModel;
            Assert.NotNull(model);
            Assert.Equal(2, model.URNs.Count);
            Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
        }
    }
}