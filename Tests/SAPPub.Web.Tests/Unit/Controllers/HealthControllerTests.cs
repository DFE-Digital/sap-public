using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Controllers;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class HealthControllerTests
{
    private readonly Mock<ILogger<HealthController>> _mockLogger;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<IEstablishmentService> _mockService;
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _mockLogger = new Mock<ILogger<HealthController>>();
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockService = new Mock<IEstablishmentService>();

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _mockEnvironment.Setup(e => e.EnvironmentName).Returns("Test");
        _mockEnvironment.Setup(e => e.ApplicationName).Returns("SAPPub.Web");
        _mockEnvironment.Setup(e => e.WebRootPath).Returns(tempPath);

        _controller = new HealthController(_mockLogger.Object, _mockEnvironment.Object, _mockService.Object);
    }

    [Fact]
    public async Task Get_WhenApplicationHealthy_ReturnsOk()
    {
        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsHealthCheckResponse()
    {
        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        // The type is in SAPPub.Web.Controllers namespace
        Assert.IsType<HealthCheckResponse>(okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsHealthyStatus()
    {
        // Act
        var result = await _controller.GetAsync();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var response = Assert.IsType<HealthCheckResponse>(okResult.Value);
        Assert.Equal("Healthy", response.Status);
        Assert.NotEmpty(response.Checks);
    }

    [Fact]
    public async Task Get_ReturnsAllChecks()
    {
        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<HealthCheckResponse>(okResult.Value);


        Assert.Equal(3, response.Checks.Count);
        Assert.Contains(response.Checks, c => c.Name == "ApplicationRunning");
        Assert.Contains(response.Checks, c => c.Name == "StaticFiles");
        Assert.Contains(response.Checks, c => c.Name == "DatabaseConnectivity");
    }

    [Fact]
    public async Task Get_ApplicationRunningCheck_PassesInTestEnvironment()
    {
        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<HealthCheckResponse>(okResult.Value);

        var appCheck = response.Checks.FirstOrDefault(c => c.Name == "ApplicationRunning");
        Assert.NotNull(appCheck);
        Assert.Equal("Pass", appCheck.Status);
        Assert.Contains("Test environment", appCheck.Message);
        Assert.Contains("SAPPub.Web", appCheck.Message);
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Staging")]
    [InlineData("Production")]
    public async Task Get_WorksInAllEnvironments(string environmentName)
    {
        // Arrange
        _mockEnvironment.Setup(e => e.EnvironmentName).Returns(environmentName);

        // Act
        var result = await _controller.GetAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<HealthCheckResponse>(okResult.Value);

        Assert.Equal("Healthy", response.Status);

        var appCheck = response.Checks.FirstOrDefault(c => c.Name == "ApplicationRunning");
        Assert.NotNull(appCheck);
        Assert.Contains(environmentName, appCheck.Message);
    }

    [Fact]
    public async Task Get_ReturnsTimestamp()
    {
        // Arrange
        var beforeCall = DateTime.UtcNow.AddSeconds(-1);

        // Act
        var result = await _controller.GetAsync();
        var afterCall = DateTime.UtcNow.AddSeconds(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<HealthCheckResponse>(okResult.Value);

        Assert.True(response.Timestamp > beforeCall);
        Assert.True(response.Timestamp < afterCall);
    }
}