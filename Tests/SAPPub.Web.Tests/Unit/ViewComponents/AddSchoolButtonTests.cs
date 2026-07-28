using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Models.MySchools;
using SAPPub.Web.ViewComponents.MySchools;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.ViewComponents;

public class AddSchoolButtonTests
{
    [Fact]
    public async Task InvokeAsync_IsKS4False_DisablesFeature_AndSkipsFeatureFlagCheck()
    {
        // Arrange
        var mySchoolsService = new Mock<IMySchoolsListService>();
        var featureManager = new Mock<IFeatureManager>();

        mySchoolsService.Setup(s => s.IsSaved("123456")).Returns(false);
        mySchoolsService.Setup(s => s.IsListLimitReached()).Returns(false);

        var sut = new AddSchoolButton(mySchoolsService.Object, featureManager.Object);

        // Act
        var result = await sut.InvokeAsync("123456", isSearchPage: false, isKS4: false);

        // Assert
        var viewResult = Assert.IsType<ViewViewComponentResult>(result);
        var model = Assert.IsType<AddSchoolButtonViewModel>(viewResult.ViewData!.Model);
        Assert.False(model.IsFeatureEnabled);

        featureManager.Verify(f => f.IsEnabledAsync(EstablishmentComparisonEnabled), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_IsKS4True_FeatureFlagOn_EnablesFeature()
    {
        // Arrange
        var mySchoolsService = new Mock<IMySchoolsListService>();
        var featureManager = new Mock<IFeatureManager>();

        mySchoolsService.Setup(s => s.IsSaved("123456")).Returns(false);
        mySchoolsService.Setup(s => s.IsListLimitReached()).Returns(false);
        featureManager.Setup(f => f.IsEnabledAsync(EstablishmentComparisonEnabled)).ReturnsAsync(true);

        var sut = new AddSchoolButton(mySchoolsService.Object, featureManager.Object);

        // Act
        var result = await sut.InvokeAsync("123456", isSearchPage: false, isKS4: true);

        // Assert
        var viewResult = Assert.IsType<ViewViewComponentResult>(result);
        var model = Assert.IsType<AddSchoolButtonViewModel>(viewResult.ViewData!.Model);
        Assert.True(model.IsFeatureEnabled);

        featureManager.Verify(f => f.IsEnabledAsync(EstablishmentComparisonEnabled), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_IsKS4True_FeatureFlagOff_DisablesFeature()
    {
        // Arrange
        var mySchoolsService = new Mock<IMySchoolsListService>();
        var featureManager = new Mock<IFeatureManager>();

        mySchoolsService.Setup(s => s.IsSaved("123456")).Returns(false);
        mySchoolsService.Setup(s => s.IsListLimitReached()).Returns(false);
        featureManager.Setup(f => f.IsEnabledAsync(EstablishmentComparisonEnabled)).ReturnsAsync(false);

        var sut = new AddSchoolButton(mySchoolsService.Object, featureManager.Object);

        // Act
        var result = await sut.InvokeAsync("123456", isSearchPage: false, isKS4: true);

        // Assert
        var viewResult = Assert.IsType<ViewViewComponentResult>(result);
        var model = Assert.IsType<AddSchoolButtonViewModel>(viewResult.ViewData!.Model);
        Assert.False(model.IsFeatureEnabled);

        featureManager.Verify(f => f.IsEnabledAsync(EstablishmentComparisonEnabled), Times.Once);
    }
}
