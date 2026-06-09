using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using System.Text.Json;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class EstablishmentComparisonControllerTests
{
    private readonly EstablishmentComparisonController _controller;
    private readonly Mock<IEstablishmentComparisonService> _mockEstablishmentComparisonService = new();

    public EstablishmentComparisonControllerTests()
    {
        _controller = new EstablishmentComparisonController(_mockEstablishmentComparisonService.Object);
    }

    [Theory]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(false, false, false)]
    public void ToggleSaveEstablishment_When_IsSearchPage_Is_True_No_Js_ReturnsExpected(bool isComparisionLimitReached, bool urnExists, bool showLimitReachedBanner)
    {
        // Setup
        var urn = "123456";
        var redirectUrl = "/search-results";
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _controller.TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object);

        _mockEstablishmentComparisonService
            .Setup(es => es.IsComparisonLimitReached())
            .Returns(isComparisionLimitReached);

        _mockEstablishmentComparisonService
            .Setup(es => es.IsSaved(urn))
            .Returns(urnExists);

        //Act
        var response = _controller.ToggleSaveEstablishment(urn, true, redirectUrl);

        // Assert
        var result = response as RedirectResult;
        Assert.NotNull(result);

        Assert.Equal(redirectUrl, result.Url);

        var limitReached = _controller.TempData.Get<bool>(ComparisionLimtReached);
        Assert.Equal(showLimitReachedBanner, limitReached);
    }

    [Theory]
    [InlineData(true, true, false)]
    [InlineData(true, false, false)]
    [InlineData(false, false, true)]
    public void ToggleSaveEstablishment_When_IsSearchPage_Is_True__With_Js_ReturnsExpected(bool isComparisionLimitReached, bool urnExists, bool isSaved)
    {
        // Setup
        var urn = "123456";
        var redirectUrl = "/search-results";
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.XRequestedWith = "XMLHttpRequest";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _controller.TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object);

        _mockEstablishmentComparisonService
            .Setup(es => es.IsComparisonLimitReached())
            .Returns(isComparisionLimitReached);

        _mockEstablishmentComparisonService
            .Setup(es => es.IsSaved(urn))
            .Returns(urnExists);

        _mockEstablishmentComparisonService
            .Setup(es => es.Toggle(urn))
            .Returns(isSaved);

        //Act
        var response = _controller.ToggleSaveEstablishment(urn, true, redirectUrl);

        // Assert
        var result = response as JsonResult;
        Assert.NotNull(result);

        var json = JsonSerializer.Serialize(result.Value);
        var expectedIsSave = isSaved;
        var expectedIsLimitReached = !urnExists && isComparisionLimitReached;

        Assert.Contains($"\"isSaved\":{expectedIsSave.ToString().ToLowerInvariant()}", json);
        Assert.Contains($"\"isLimitReached\":{expectedIsLimitReached.ToString().ToLowerInvariant()}", json);
    }

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(false, true, false)]
    public void ToggleSaveEstablishment_When_IsSearchPage_Is_False_ReturnsExpected(bool urnExists, bool expectedShowAddSuccessBanner, bool expectedShowRemoveSuccessBanner)
    {
        // Setup
        var urn = "123456";
        var redirectUrl = "/search-results";
        var httpContext = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _controller.TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object);

        _mockEstablishmentComparisonService
            .Setup(es => es.IsComparisonLimitReached())
            .Returns(false);

        _mockEstablishmentComparisonService
            .Setup(es => es.IsSaved(urn))
            .Returns(urnExists);

        _mockEstablishmentComparisonService
            .Setup(es => es.Toggle(urn))
            .Returns(expectedShowAddSuccessBanner);

        //Act
        var response = _controller.ToggleSaveEstablishment(urn, false, redirectUrl);

        // Assert
        var result = response as RedirectResult;
        Assert.NotNull(result);

        Assert.Equal(redirectUrl, result.Url);

        var actualShowAddSuccessBanner = _controller.TempData.Get<bool>(BannerAddSuccess);
        var actualShowRemoveSuccessBanner = _controller.TempData.Get<bool>(BannerRemoveSuccess);

        Assert.Equal(expectedShowAddSuccessBanner, actualShowAddSuccessBanner);
        Assert.Equal(expectedShowRemoveSuccessBanner, actualShowRemoveSuccessBanner);
    }
}
