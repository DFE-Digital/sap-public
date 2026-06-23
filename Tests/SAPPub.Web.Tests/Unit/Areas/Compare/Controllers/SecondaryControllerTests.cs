using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Compare.Controllers;
using SAPPub.Web.Areas.Compare.ViewModels.Secondary;

namespace SAPPub.Web.Tests.Unit.Areas.Compare.Controllers;

public class SecondaryControllerTests
{
    private SecondaryController _controllerUnderTest = new();
    private readonly Mock<IDestinationsService> _mockDestinationsService = new();
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

    [Fact]
    public async Task Destinations_HasFullData_ReturnsViewResultWithCorrectModel()
    {
        // Arrange
        var englandPercentage = 50.0;
        var destinationsDetailsBuilder = new DestinationsDetailsBuilder()
            .WithEnglandPercentage(englandPercentage);
        var establishmentDestinations = _urns.Select(urn => destinationsDetailsBuilder.WithUrn(urn).Build()).ToList();

        _urns.Select(urn =>
            _mockDestinationsService
                .Setup(s => s.GetDestinationsDetailsAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentDestinations.Single(x => x.Urn == urn))).ToList();

        // Act
        var result = await _controllerUnderTest.Destinations(_mockDestinationsService.Object, _urns) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as CompareDestinationsViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.URNs.Count);
        Assert.Collection(model.URNs,
            first => Assert.Equal(_urns[0], first),
            second => Assert.Equal(_urns[1], second)
        );
        Assert.Collection(model.SchoolDetails,
            first => Assert.Equal(establishmentDestinations[0].Urn, first.URN),
            second => Assert.Equal(establishmentDestinations[1].Urn, second.URN)
        );
        Assert.Collection(model.SchoolDetails,
            first =>
            {
                Assert.Equal(establishmentDestinations[0].Urn, first.URN);
                Assert.Equal(establishmentDestinations[0].SchoolName, first.SchoolName);
                Assert.Equal(establishmentDestinations[0].SchoolAll.CurrentYear, first.PercentInEducationEmploymentOrTraining);
                Assert.Equal("Yes", first.SixthForm.DisplayText());
            },
            second =>
            {
                Assert.Equal(establishmentDestinations[1].Urn, second.URN);
                Assert.Equal(establishmentDestinations[1].SchoolName, second.SchoolName);
                Assert.Equal(establishmentDestinations[1].SchoolAll.CurrentYear, second.PercentInEducationEmploymentOrTraining);
                Assert.Equal("Yes", second.SixthForm.DisplayText());
            }
        );

        Assert.Equal(englandPercentage, model.EnglandPercentage);
    }
}