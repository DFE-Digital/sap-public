using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Web.Controllers;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class CookiesControllerTests
{
    private readonly CookiesController _controller;
    private readonly Mock<IUrlHelper> _mockUrlHelper = new();

    public CookiesControllerTests()
    {
        _controller = new CookiesController();

        _mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string>())).Returns(true);
        _controller.Url = _mockUrlHelper.Object;
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CookieSettings_Should_Set_Cookie_And_Redirect(bool acceptCookies)
    {
        //Arrange
        var redirectUrl = "/search";
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        //Act
        var response = _controller.CookieSettings(acceptCookies, redirectUrl);

        // Assert
        var result = response as RedirectResult;
        Assert.NotNull(result);
        var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
        var expectedValue = acceptCookies ? "analytics_preference=true" : "analytics_preference=false";
        Assert.Contains(expectedValue, setCookieHeader);
        Assert.Equal(redirectUrl, result.Url);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void HideBanner_Should_Set_Cookie_And_Redirect(bool hideBanner)
    {
        //Arrange
        var redirectUrl = "/search";
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        //Act
        var response = _controller.HideBanner(hideBanner, redirectUrl);

        // Assert
        var result = response as RedirectResult;
        Assert.NotNull(result);
        var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
        var expectedValue = hideBanner ? "hide_banner=true" : "hide_banner=false";
        Assert.Contains(expectedValue, setCookieHeader);
        Assert.Equal(redirectUrl, result.Url);
    }

    [Fact]
    public void CookieSettings_Should_Remove_Ga_Cookies()
    {
        //Arrange
        var acceptCookies = false;
        var redirectUrl = "/search";
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        httpContext.Request.Headers.Cookie = "_ga=test1; _gid=test2; _gat=test3";

        //Act
        var response = _controller.CookieSettings(acceptCookies, redirectUrl);

        // Assert
        var result = response as RedirectResult;
        Assert.NotNull(result);

        var setCookieHeader = httpContext.Response.Headers.SetCookie.ToString();
        var expectedValue = acceptCookies ? "analytics_preference=true" : "analytics_preference=false";
        var cookies = httpContext.Response.Headers.SetCookie.ToList();

        Assert.Contains(expectedValue, setCookieHeader);
        Assert.Equal(redirectUrl, result.Url);

        Assert.All(cookies, cookie =>
        {
            Assert.Contains("expires=", cookie?.ToLower());
        });
    }
}
