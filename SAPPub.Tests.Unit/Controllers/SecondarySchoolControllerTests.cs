using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Web.Controllers;

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
    }

    [Fact]
    public void Get_AboutSchool_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";
        // Act
        var result = _controller.AboutSchool(urn, schoolName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Get_Admissions_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";
        // Act
        var result = _controller.Admissions(urn, schoolName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Get_Attendance_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";
        // Act
        var result = _controller.Attendance(urn, schoolName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";
        // Act
        var result = _controller.Admissions(urn, schoolName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Get_Destinations_Info_ReturnsOk()
    {
        // Arrange
        int urn = 1;
        string schoolName = "School Name";
        // Act
        var result = _controller.Attendance(urn, schoolName);//

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ViewResult>(result);
    }
}
