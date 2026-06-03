using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class CookiesControllerTests
{
    private readonly CookiesController _controller;
    private readonly Mock<IUrlHelper> _mockUrlHelper = new();
    private readonly Mock<IFeatureManager> _featureManager = new();

    public CookiesControllerTests()
    {
        _controller = new CookiesController(_featureManager.Object);

        _mockUrlHelper.Setup(u => u.IsLocalUrl(It.IsAny<string>())).Returns(true);
        _controller.Url = _mockUrlHelper.Object;
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Preferences_Should_Show_Cookie_Confirmation_Banner(bool showBanner)
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _controller.TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object);

        if (showBanner)
        {
            _controller.TempData.Set(CookiesConfirmation, showBanner);
        }

        //Act
        var response = _controller.Preferences();

        // Assert
        var showConfirmationBanner = _controller.TempData.Get<bool>(CookiesConfirmation);
        Assert.Equal(showBanner, showConfirmationBanner);
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

        _controller.TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object);

        //Act
        var response = _controller.CookieSettings(acceptCookies, redirectUrl);

        // Assert
        var result = response as RedirectResult;
        Assert.NotNull(result);
        var setCookieHeader = httpContext.Response.Headers["Set-Cookie"].ToString();
        var expectedValue = acceptCookies ? "analytics_preference=true" : "analytics_preference=false";
        Assert.Contains(expectedValue, setCookieHeader);
        Assert.Equal(redirectUrl, result.Url);

        var showConfirmationBanner = _controller.TempData.Get<bool>(CookiesConfirmation);
        Assert.False(showConfirmationBanner);
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
