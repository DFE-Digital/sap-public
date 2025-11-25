using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SAPPub.Web.ViewComponents.VerticalNavigation;

namespace SAPPub.Tests.Unit.ViewComponents;

public class VerticalNavigationTests
{
    private static VerticalNavigation CreateComponent() => new();

    [Fact]
    public void Invoke_ReturnsDefaultViewWithModel()
    {
        // Arrange
        var item = new VerticalNavigationModel { Urn = 1, SchoolName = "Kes", ActivePage = "About" };
        
        var component = CreateComponent();

        // Act
        var result = component.Invoke(item) as ViewViewComponentResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/ViewComponents/VerticalNavigation/Default.cshtml");

        var model = result.ViewData!.Model.Should().BeOfType<VerticalNavigationModel>().Subject;
        model.Urn.Should().Be(item.Urn);
        model.SchoolName.Should().Be(item.SchoolName);
        model.ActivePage.Should().Be(item.ActivePage);
    }

    [Fact]
    public void Invoke_WithNullModel_DoesNotThrow()
    {
        // Arrange
        var component = CreateComponent();

        // Act
        var result = component.Invoke(null!) as ViewViewComponentResult;

        // Assert
        result.Should().NotBeNull();
        result.ViewName.Should().Be("~/ViewComponents/VerticalNavigation/Default.cshtml");

        result.ViewData!.Model.Should().BeNull();
    }
}
