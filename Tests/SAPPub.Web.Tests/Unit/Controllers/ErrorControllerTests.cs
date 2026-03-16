using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;
using SAPPub.Web.Controllers;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class ErrorControllerTests
{
    private readonly ErrorController _controller;

    public ErrorControllerTests()
    {
        // Arrange
        var mockEnv = new Mock<IHostEnvironment>();
        mockEnv.SetupGet(e => e.EnvironmentName).Returns("Development");

        _controller = new ErrorController(mockEnv.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public void Error_Returns_PageNotFound_For404()
    {
        var result = _controller.HandleErrorCode(404) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("PageNotFound", result.ViewName);
        Assert.Equal(404, _controller.HttpContext.Response.StatusCode);
    }

    [Fact]
    public void Error_Returns_ProblemWithService_For500()
    {
        var result = _controller.HandleErrorCode(500) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("ProblemWithService", result.ViewName);
        Assert.Equal(500, _controller.HttpContext.Response.StatusCode);
    }

    [Fact]
    public void Error_Returns_Default_Error_View_ForUnknownCode()
    {
        var result = _controller.HandleErrorCode(555) as ViewResult;

        Assert.NotNull(result);
        Assert.Equal("ProblemWithService", result.ViewName);
        Assert.Equal(555, _controller.HttpContext.Response.StatusCode);
    }
}
