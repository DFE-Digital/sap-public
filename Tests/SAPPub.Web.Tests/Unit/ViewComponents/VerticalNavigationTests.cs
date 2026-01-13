using Microsoft.AspNetCore.Mvc.ViewComponents;
using SAPPub.Web.ViewComponents.VerticalNavigation;

namespace SAPPub.Web.Tests.Unit.ViewComponents;

public class VerticalNavigationTests
{
    private static VerticalNavigation CreateComponent() => new();

    [Fact]
    public void Invoke_ReturnsDefaultViewWithModel()
    {
        // Arrange
        var item = new VerticalNavigationModel { URN = "1", SchoolName = "Kes", ActivePage = "About" };
        
        var component = CreateComponent();

        // Act
        var result = component.Invoke(item) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("~/ViewComponents/VerticalNavigation/Default.cshtml", result.ViewName);


        var model = result.ViewData!;
        Assert.IsType<VerticalNavigationModel>(model.Model);  
        Assert.Equal(item.URN, ((VerticalNavigationModel)model.Model).URN);
        Assert.Equal(item.SchoolName, ((VerticalNavigationModel)model.Model).SchoolName);
        Assert.Equal(item.ActivePage, ((VerticalNavigationModel)model.Model).ActivePage);
    }

    [Fact]
    public void Invoke_WithNullModel_DoesNotThrow()
    {
        // Arrange
        var component = CreateComponent();

        // Act
        var result = component.Invoke(null!) as ViewViewComponentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("~/ViewComponents/VerticalNavigation/Default.cshtml", result.ViewName);

        Assert.Null(result.ViewData.Model);
    }
}
