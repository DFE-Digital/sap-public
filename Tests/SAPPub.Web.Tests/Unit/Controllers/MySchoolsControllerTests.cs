using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Controllers;
using SAPPub.Web.Models.MySchools;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class MySchoolsControllerTests
{
    private readonly Mock<IEstablishmentComparisonService> _comparisonServiceMock = new();
    private readonly Mock<IEstablishmentService> _establishmentServiceMock = new();

    [Fact]
    public async Task Index_EstablishmentsInList_ReturnsViewResultWithMySchoolsListViewModel()
    {
        // Arrange
        _comparisonServiceMock.Setup(
            s => s.GetSavedEstablishments()).Returns(new List<string> { "123456", "123457" });
        _establishmentServiceMock
            .Setup(s => s.GetEstablishmentAsync("123456", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Establishment
                    {
                        URN = "123456",
                        EstablishmentName = "Test School 1",
                        StatusCode = 1
                    });
        _establishmentServiceMock
            .Setup(s => s.GetEstablishmentAsync("123457", It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Establishment
                    {
                        URN = "123457",
                        EstablishmentName = "Test School 2",
                        StatusCode = 2
                    });

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = await controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as MySchoolsListViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.MySchools.Count);
        Assert.Contains(model.MySchools, s => s.Urn == "123456" && s.SchoolName == "Test School 1" && s.EstablishmentStatus == EstablishmentStatus.Open);
        Assert.Contains(model.MySchools, s => s.Urn == "123457" && s.SchoolName == "Test School 2" && s.EstablishmentStatus == EstablishmentStatus.Closed);
    }

    [Fact]
    public async Task Index_NoEstablishmentsInList_ReturnsViewResultWithMySchoolsListViewModel()
    {
        // Arrange
        _comparisonServiceMock.Setup(
            s => s.GetSavedEstablishments()).Returns(new List<string>());

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = await controller.Index();

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("AddNoSchoolsActionHere", redirect.ActionName);
    }
}
