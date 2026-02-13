using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Services;
using SAPPub.Core.Services.KS4.Admissions;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Tests.ControllerAndServicesTests.SecondarySchool;

public class AdmissionsTests
{
    private readonly Mock<ILogger<SecondarySchoolController>> _mockLogger;
    private readonly Mock<ILaUrlsRepository> _mockLaUrlsRepository = new();
    private readonly Mock<IEstablishmentRepository> _mockEstablishmentRepository = new();
    private readonly Mock<IDestinationsService> _mockDestinationsService = new();
    private readonly Mock<ILookupService> _mockLookupService = new();
    private readonly IEstablishmentService _establishmentService;
    private readonly IAdmissionsService _admissionsService;
    private readonly SecondarySchoolController _controller;
    private Establishment _establishment;

    public AdmissionsTests()
    {
        _establishment = AdmissionsTests.Establishment;

        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _establishmentService = new EstablishmentService(_mockEstablishmentRepository.Object, _mockLookupService.Object);
        _admissionsService = new EstablishmentAdmissionsService(_establishmentService, _mockLaUrlsRepository.Object);
        _controller = new SecondarySchoolController(_mockLogger.Object, _establishmentService, _mockDestinationsService.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    public static Establishment Establishment => new EstablishmentTestBuilder()
        .WithTrustName("Trust")
        .WithWebsite("https://www.gov.uk/")
        .WithTelephoneNum("012154896")
        .WithAddressStreet("Street")
        .WithAddressLocality("Locality")
        .WithAddressTown("Town")
        .WithAddressPostcode("Postcode")
        .WithLAName("Sheffield")
        .WithLAGssCode("123")
        .WithTypeOfEstablishmentName("EstablishmentName")
        .WithHeadteacherTitle("Title")
        .WithHeadteacherFirstName("FirstName")
        .WithHeadteacherLastName("LastName")
        .WithAgeRangeLow("11")
        .WithAgeRangeHigh("18")
        .WithTotalPupils("1117")
        .Build();

    public static Establishment BuildEstablishmentWithNullGssCode => new EstablishmentTestBuilder()
        .WithTrustName("Trust")
        .WithWebsite("https://www.gov.uk/")
        .WithTelephoneNum("012154896")
        .WithAddressStreet("Street")
        .WithAddressLocality("Locality")
        .WithAddressTown("Town")
        .WithAddressPostcode("Postcode")
        .WithLAName("Sheffield")
        .WithLAGssCode(null)
        .WithTypeOfEstablishmentName("EstablishmentName")
        .WithHeadteacherTitle("Title")
        .WithHeadteacherFirstName("FirstName")
        .WithHeadteacherLastName("LastName")
        .WithAgeRangeLow("11")
        .WithAgeRangeHigh("18")
        .WithTotalPupils("1117")
        .Build();

    [Theory]
    [InlineData("https://www.example.com/manchester/school-admissions", "Manchester")]
    [InlineData(null, "Manchester")]
    public async Task Get_Admissions_ReturnsExpectedViewModel(string? lASchoolAdmissionsUrl, string? laName)
    {
        _mockEstablishmentRepository.Setup(r => r.GetEstablishment(_establishment.URN)).Returns(_establishment);
        _mockLaUrlsRepository.Setup(r => r.GetLaAsync(_establishment.GSSLACode!)).ReturnsAsync(new LaUrls
        {
            Name = laName,
            LAMainUrl = lASchoolAdmissionsUrl
        });

        // Act
        var result = await _controller.Admissions(_admissionsService, _establishment.URN, _establishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_establishment.URN, model.URN);
        Assert.Equal(_establishment.EstablishmentName, model.SchoolName);
        Assert.Equal(lASchoolAdmissionsUrl, model.LASecondarySchoolAdmissionsLinkUrl);
        Assert.Equal(laName, model.LAName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_establishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_establishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData("https://www.example.com/manchester/school-admissions", null)]
    public async Task Get_Admissions_LANameIsNull_ReturnsGenericLAString(string? lASchoolAdmissionsUrl, string? laName)
    {
        _mockEstablishmentRepository.Setup(r => r.GetEstablishment(_establishment.URN)).Returns(_establishment);
        _mockLaUrlsRepository.Setup(r => r.GetLaAsync(_establishment.GSSLACode!)).ReturnsAsync(new LaUrls
        {
            Name = laName,
            LAMainUrl = lASchoolAdmissionsUrl
        });

        // Act
        var result = await _controller.Admissions(_admissionsService, _establishment.URN, _establishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal("Local authority", model.LAName);
    }

    [Fact]
    public async Task Get_Admissions_EstablishmentGssCodeIsNull_ReturnsPartlyPopulatedViewModel()
    {
        var establishment = BuildEstablishmentWithNullGssCode;
        _mockEstablishmentRepository.Setup(r => r.GetEstablishment(establishment.URN)).Returns(establishment);

        // Act
        var result = await _controller.Admissions(_admissionsService, establishment.URN, establishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal("Local authority", model.LAName);
    }

    [Fact]
    public async Task Get_Admissions_LAUrlsForGssCode_ReturnsPartlyPopulatedViewModel()
    {
        _mockEstablishmentRepository.Setup(r => r.GetEstablishment(_establishment.URN)).Returns(_establishment);
        _mockLaUrlsRepository.Setup(r => r.GetLaAsync(_establishment.GSSLACode!)).ReturnsAsync((LaUrls?)null);

        // Act
        var result = await _controller.Admissions(_admissionsService, _establishment.URN, _establishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal("Local authority", model.LAName);
    }
}
