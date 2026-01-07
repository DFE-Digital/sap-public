using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Tests.Unit.Controllers;

public class SecondarySchoolControllerTests
{
    private readonly Mock<ILogger<SecondarySchoolController>> _mockLogger;
    private readonly Mock<IEstablishmentService> _mockEstablishment;
    private readonly SecondarySchoolController _controller;

    private readonly Establishment fakeEstablishment = new()
    {
        URN = "1",
        EstablishmentName = "Test School",
        Website = "https://www.gov.uk/",
    };


    public SecondarySchoolControllerTests()
    {
        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();
        _mockEstablishment = new Mock<IEstablishmentService>();
        _mockEstablishment.Setup(es => es.GetEstablishment(It.IsAny<string>())).Returns(fakeEstablishment);

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockLogger.Object, _mockEstablishment.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

    }

    [Fact]
    public void Get_AboutSchool_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AboutSchool(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_Admissions_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Admissions(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AdmissionsViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_Attendance_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Attendance(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AttendanceViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.SchoolWebsite.Should().Be(fakeEstablishment.Website);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.CurriculumAndExtraCurricularActivities(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_AcademicPerformancePupilProgress_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformancePupilProgress(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformancePupilProgressViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_AcademicPerformance_EnglishAndMathsResults_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformanceEnglishAndMathsResults(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformanceSubjectsEntered(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformanceSubjectsEnteredViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_Destinations_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Destinations(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as DestinationsViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }
}
