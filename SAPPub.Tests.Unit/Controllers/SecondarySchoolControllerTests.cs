using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Web.Controllers;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Tests.Unit.Controllers;

public class SecondarySchoolControllerTests
{
    private readonly Mock<ILogger<SecondarySchoolController>> _mockLogger;
    private readonly SecondarySchoolController _controller;

    public SecondarySchoolControllerTests()
    {
        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockLogger.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
    }

    [Fact]
    public void Get_AboutSchool_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";

        // Act
        var result = _controller.AboutSchool(urn, schoolName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();
        model.Urn.Should().Be(urn);
        model.SchoolName.Should().Be(schoolName);
    }

    [Fact]
    public void Get_Admissions_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";

        // Act
        var result = _controller.Admissions(urn, schoolName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AdmissionsViewModel;
        model.Should().NotBeNull();
        model.Urn.Should().Be(urn);
        model.SchoolName.Should().Be(schoolName);
    }

    [Fact]
    public void Get_Attendance_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";

        // Act
        var result = _controller.Attendance(urn, schoolName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AttendanceViewModel;
        model.Should().NotBeNull();
        model.Urn.Should().Be(urn);
        model.SchoolName.Should().Be(schoolName);
    }

    [Fact]
    public void Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";

        // Act
        var result = _controller.CurriculumAndExtraCurricularActivities(urn, schoolName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        model.Should().NotBeNull();
        model.Urn.Should().Be(urn);
        model.SchoolName.Should().Be(schoolName);
    }

    [Fact]
    public void Get_AcademicPerformance_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";
        
        // Act
        var result = _controller.AcademicPerformance(urn, schoolName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformanceViewModel;
        model.Should().NotBeNull();
        model.Urn.Should().Be(urn);
        model.SchoolName.Should().Be(schoolName);
    }

    [Fact]
    public void Get_Destinations_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";

        // Act
        var result = _controller.Destinations(urn, schoolName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as DestinationsViewModel;
        model.Should().NotBeNull();
        model.Urn.Should().Be(urn);
        model.SchoolName.Should().Be(schoolName);
    }
}
