using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS4;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class CurriculumControllerTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IFeatureManager> _mockFeatureManager = new();
    private readonly Mock<ILogger<CurriculumController>> _mockLogger = new();
    private readonly CurriculumController _controller;
    private EstablishmentMinimumServiceModel _fakeEstablishment;

    public CurriculumControllerTests()
    {
        _fakeEstablishment = new EstablishmentMinimumTestBuilder()
            .WithEstablishmentName("cool school")
            .WithWebsite("https://www.gov.uk/")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService = new();

        _mockEstablishmentService
            .Setup(es => es.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fakeEstablishment);

        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new CurriculumController(_mockLogger.Object, _mockFeatureManager.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Get_KS4_Info_ReturnsOk()
    {
        var result = await _controller.KS4(
            _mockEstablishmentService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.SchoolWebsite.Value);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(null, FieldStatus.NotAvailable)]
    [InlineData("", FieldStatus.NotAvailable)]
    [InlineData(" ", FieldStatus.NotAvailable)]
    [InlineData("test", FieldStatus.Available)]
    public async Task Get_KS4_Info_SchoolWebsite(string? website, FieldStatus fieldStatus)
    {
        _fakeEstablishment.Website = website!;

        var result = await _controller.KS4(_mockEstablishmentService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;

        Assert.NotNull(model);
        Assert.Equal(fieldStatus, model.SchoolWebsite.Status);

        if (fieldStatus == FieldStatus.Available)
        {
            Assert.False(model.SchoolWebsite.IsNotAvailable);
            Assert.True(model.SchoolWebsite.IsAvailable);
            Assert.Equal(website, model.SchoolWebsite.Value);
            Assert.Equal(website, model.SchoolWebsite.DisplayText());
        }
        else
        {
            Assert.False(model.SchoolWebsite.IsAvailable);
            Assert.True(model.SchoolWebsite.IsNotAvailable);
            Assert.Equal("Not available", model.SchoolWebsite.DisplayText());
        }
    }
}
