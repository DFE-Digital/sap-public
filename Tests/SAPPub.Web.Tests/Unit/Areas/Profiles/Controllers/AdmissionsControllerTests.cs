using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Constants;
using SAPPub.Web.Areas.Profiles.ViewModels.Admissions;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class AdmissionsControllerTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IAdmissionsService> _mockAdmissionsService = new();
    private readonly Mock<IFeatureManager> _mockFeatureManager = new();
    private readonly Mock<ILogger<AdmissionsController>> _mockLogger = new();
    private readonly AdmissionsController _controller;
    private EstablishmentServiceModel _fakeEstablishment;

    public AdmissionsControllerTests()
    {
        _fakeEstablishment = new EstablishmentTestBuilder()
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
            .WithGenderName("GenderName")
            .WithReligiousCharacterName("ReligiousCharacter")
            .WithSixthForm(false)
            .WithResourcedProvisionName("Resourced provision")
            .WithEstablishmentTypeGroupId((int)EstablishmentTypeGroup.Colleges)
            .WithStatusCode(1)
            .WithOpenReasonId(10)
            .WithOpenDate()
            .WithSenTypes("VI - Visual Impairment, HI - Hearing Impairment")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService = new();

        _mockEstablishmentService
            .Setup(es => es.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fakeEstablishment);

        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new AdmissionsController(_mockLogger.Object, _mockFeatureManager.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Get_KS4_Info_ReturnsExpectedViewModel()
    {
        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel
            {
                SchoolName = _fakeEstablishment.EstablishmentName,
                SchoolWebsite = _fakeEstablishment.Website,
                LAName = laName,
                LASchoolAdmissionsUrl = lASchoolAdmissionsUrl,
                EstablishmentStatus = EstablishmentStatus.Open,
                IsIndependentSchool = false,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false

            });

        var result = await _controller.KS4(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.SchoolWebsite.Value);
        Assert.Equal(lASchoolAdmissionsUrl, model.LASchoolAdmissionsLinkUrl);
        Assert.Equal(laName, model.LAName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.False(model.IsSchoolClosed);
        Assert.False(model.IsIndependentSchool);
    }

    [Theory]
    [InlineData(null, FieldStatus.NotAvailable)]
    [InlineData("", FieldStatus.NotAvailable)]
    [InlineData(" ", FieldStatus.NotAvailable)]
    [InlineData("test", FieldStatus.Available)]
    public async Task Get_KS4_Info_SchoolWebsite(string? website, FieldStatus fieldStatus)
    {
        _fakeEstablishment.Website = website!;

        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel
            {
                SchoolName = _fakeEstablishment.EstablishmentName,
                SchoolWebsite = _fakeEstablishment.Website,
                LAName = laName,
                LASchoolAdmissionsUrl = lASchoolAdmissionsUrl,
                EstablishmentStatus = EstablishmentStatus.Open,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false,
                IsIndependentSchool = false
            });

        var result = await _controller.KS4(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;

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

        Assert.False(model.IsSchoolClosed);
    }

    [Theory]
    [InlineData(EstablishmentStatus.Open, false)]
    [InlineData(EstablishmentStatus.Closed, true)]
    public async Task Get_KS4_Info_IsSchoolClosed(EstablishmentStatus? statusCode, bool expectedResult)
    {
        _fakeEstablishment.StatusCode = statusCode.ToStatusCode();

        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel
            {
                SchoolName = _fakeEstablishment.EstablishmentName,
                SchoolWebsite = _fakeEstablishment.Website,
                LAName = laName,
                LASchoolAdmissionsUrl = lASchoolAdmissionsUrl,
                EstablishmentStatus = statusCode,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false,
                IsIndependentSchool = false
            });

        var result = await _controller.KS4(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;

        Assert.NotNull(model);
        Assert.Equal(expectedResult, model.IsSchoolClosed);
    }

}
