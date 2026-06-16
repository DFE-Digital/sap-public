using Microsoft.AspNetCore.Mvc;
using SAPPub.Web.Areas.Compare.Controllers;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;

namespace SAPPub.Web.Tests.Unit.Areas.Compare.Controllers
{
    public class SecondaryControllerTests
    {
        [Fact]
        public async Task AcademicPerformancePupilProgressAndAttainment_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var controller = new SecondaryController();
            var urn1 = "123456";
            var urn2 = "234567";
            var urnList = new List<string> { urn1, urn2 };

            // Act
            var result = await controller.AcademicPerformancePupilProgressAndAttainment(urnList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as CompareAcademicPerformanceProgressAndAttainmentViewModel;
            Assert.NotNull(model);
            Assert.Equal(2, model.URNs.Count);
            Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
        }

        [Fact]
        public async Task AcademicPerformanceEnglishAndMathsResults_ReturnsViewResultWithCorrectModel()
        {
            // Arrange
            var controller = new SecondaryController();
            var urn1 = "111111";
            var urn2 = "222222";
            var urnList = new List<string> { urn1, urn2 };

            // Act
            var result = await controller.AcademicPerformanceEnglishAndMathsResults(urnList) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as CompareAcademicPerformanceEnglishAndMathsResultsViewModel;
            Assert.NotNull(model);
            Assert.Equal(2, model.URNs.Count);
            Assert.Equal(model.RouteQueryString, $"?urns={urn1}&urns={urn2}");
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
    }
}