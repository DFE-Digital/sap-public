using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Interfaces.Services.Overview;
using SAPPub.Core.ServiceModels.Overview;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.Overview;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class OverviewControllerTests
{
    private readonly Mock<ILogger<OverviewController>> _logger = new();
    private readonly Mock<IOverviewService> _service = new();
    private readonly OverviewController _sut;

    public OverviewControllerTests()
    {
        _sut = new OverviewController(_logger.Object, _service.Object);
    }

    [Fact]
    public async Task Overview_ReturnsMappedViewModel_WhenOverviewExists()
    {
        var serviceModel = CreateOverviewModel();

        _service
            .Setup(s => s.GetOverviewAsync("123456", It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceModel);

        var result = await _sut.Overview(
            "123456",
            "test-school",
            CancellationToken.None);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<OverviewViewModel>(viewResult.Model);

        Assert.Null(viewResult.ViewName);
        Assert.Equal("123456", model.URN);
        Assert.Equal("Test School", model.SchoolName);
        Assert.Equal("Secondary", model.EducationPhase.Value);
        Assert.Equal("11 to 16", model.AgeRange.Value);
        Assert.Equal("1,234", model.NumberOfPupils.Value);
    }

    [Fact]
    public async Task Overview_ReturnsErrorView_WhenServiceReturnsNull()
    {
        _service
            .Setup(s => s.GetOverviewAsync("999999", It.IsAny<CancellationToken>()))
            .ReturnsAsync((OverviewModel?)null);

        var result = await _sut.Overview(
            "999999",
            "missing-school",
            CancellationToken.None);

        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Error", viewResult.ViewName);
        Assert.Null(viewResult.Model);
    }

    [Fact]
    public async Task Overview_ForwardsUrnAndCancellationToken()
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        _service
            .Setup(s => s.GetOverviewAsync("123456", token))
            .ReturnsAsync(CreateOverviewModel());

        await _sut.Overview("123456", "ignored-route-name", token);

        _service.Verify(s => s.GetOverviewAsync("123456", token), Times.Once);
    }

    private static OverviewModel CreateOverviewModel() => new()
    {
        Urn = "123456",
        SchoolName = "Test School",
        AgeRangeLow = "11",
        AgeRangeHigh = "16",
        NumberOfPupils = "1234",
        IsKS4 = true
    };
}
