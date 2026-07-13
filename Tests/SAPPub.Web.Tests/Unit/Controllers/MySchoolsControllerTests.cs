using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Banner;
using SAPPub.Web.Models.MySchools;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class MySchoolsControllerTests
{
    private readonly Mock<IMySchoolsListService> _comparisonServiceMock = new();
    private readonly Mock<IEstablishmentService> _establishmentServiceMock = new();

    [Fact]
    public async Task Index_EstablishmentsInList_ReturnsViewResultWithMySchoolsListViewModel()
    {
        // Arrange
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentsList = new List<EstablishmentServiceModel>
        {
            establishmentBuilder.WithURN("123456").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Closed).BuildServiceModel()
        }.OrderBy(e => e.EstablishmentName).ToList();

        _comparisonServiceMock.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentsList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentsList)
        {
            _establishmentServiceMock.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var urns = establishmentsList.Select(e => e.URN).ToList();

        _establishmentServiceMock.Setup(s => s.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentsList.AsEnumerable());

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = await controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as MySchoolsListViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.MySchools.Count);
        Assert.Contains(model.MySchools, s => s.Urn == "123456" && s.Status == EstablishmentStatus.Open);
        Assert.Contains(model.MySchools, s => s.Urn == "123457" && s.Status == EstablishmentStatus.Closed);
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
        Assert.Equal("NoSchoolsAdded", redirect.ActionName);
    }

    [Fact]
    public void NoSchoolsAdded_HasSavedEstablishments_RedirectsToPopulatedView()
    {
        // Arrange
        _comparisonServiceMock
            .Setup(s => s.GetSavedEstablishments()).Returns(["123456"]);

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = controller.NoSchoolsAdded();

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }

    [Fact]
    public void NoSchoolsAdded_DoesNotHaveSavedEstablishments_ReturnsView()
    {
        // Arrange
        _comparisonServiceMock.Setup(s => s.GetSavedEstablishments()).Returns(new List<string>());
        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = controller.NoSchoolsAdded() as ViewResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task NoSchoolsToRemove_RedirectsToView()
    {
        // Arrange
        _comparisonServiceMock.Setup(s => s.GetSavedEstablishments()).Returns(new List<string>());
        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = await controller.RemoveConfirm();

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);        
    }

    [Fact]
    public async Task RemoveConfirm_EstablishmentsInList_ReturnsViewResultWithRemoveSchoolsConfirmationViewModel()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();       
        var establishmentBuilder = new EstablishmentTestBuilder();
        var establishmentsList = new List<EstablishmentServiceModel>
        {
            establishmentBuilder.WithURN("123456").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel()
        }.OrderBy(e => e.EstablishmentName).ToList();

        _comparisonServiceMock.Setup(s => s.GetSavedEstablishments())
            .Returns(establishmentsList.Select(e => e.URN).ToList());

        foreach (var establishment in establishmentsList)
        {
            _establishmentServiceMock.Setup(s => s.GetEstablishmentAsync(establishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishment);
        }

        var urns = establishmentsList.Select(e => e.URN).ToList();
        _establishmentServiceMock.Setup(s => s.GetEstablishmentsAsync(urns, It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentsList.AsEnumerable());

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            },
            TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object)
        };

        controller.TempData.Set(SelectedEstablishmentUrns, urns);


        // Act
        var result = await controller.RemoveConfirm() as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = result.Model as RemoveSchoolsConfirmationViewModel;
        Assert.NotNull(model);
        Assert.Equal(2, model.Schools.Count);
        Assert.Contains(model.Schools, s => s.Urn == "123456");
        Assert.Contains(model.Schools, s => s.Urn == "123457");
    }

    [Fact]
    public void ConfirmRemove_NoEstablishmentInTempData_RedirectsToView()
    {
        // Arrange
        _comparisonServiceMock.Setup(s => s.GetSavedEstablishments()).Returns(new List<string>());
        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object);

        // Act
        var result = controller.ConfirmRemove();

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
    }

    [Fact]
    public void ConfirmRemove_EstablishmentsInTempData_RemovesEstablishmentsFromListAndRedirectsToView()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var establishmentBuilder = new EstablishmentTestBuilder();
        var removeSchoolsModel = new List<RemoveSchoolViewModel>();
        var establishmentsList = new List<EstablishmentServiceModel>
        {
            establishmentBuilder.WithURN("123456").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
            establishmentBuilder.WithURN("123457").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel()
        }.OrderBy(e => e.EstablishmentName).ToList();

        foreach (var establishment in establishmentsList)
        {          
            removeSchoolsModel.Add(new RemoveSchoolViewModel { Urn = establishment.URN, Name = establishment.EstablishmentName });
        }

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            },
            TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object)
        };

        controller.TempData.Set(SelectedSchoolsForRemoval, removeSchoolsModel);

        // Act
        var result = controller.ConfirmRemove();

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        var bannerModel = controller.TempData.Get<BannerViewModel>(BannerModel);
        Assert.NotNull(bannerModel);
        Assert.Equal("Saved schools removed from your compare list", bannerModel.HeaderContent!.Trim());
    }


    [Fact]
    public void ConfirmRemove_EstablishmentInTempData_RemovesEstablishmentFromListAndRedirectsToView()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var establishmentBuilder = new EstablishmentTestBuilder();
        var removeSchoolsModel = new List<RemoveSchoolViewModel>();
        var establishmentsList = new List<EstablishmentServiceModel>
        {
            establishmentBuilder.WithURN("123456").WithFullAddress().WithStatusCode(EstablishmentStatus.Open).BuildServiceModel(),
        }.ToList();

        foreach (var establishment in establishmentsList)
        {
            removeSchoolsModel.Add(new RemoveSchoolViewModel { Urn = establishment.URN, Name = establishment.EstablishmentName });
        }

        var controller = new MySchoolsController(_comparisonServiceMock.Object, _establishmentServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            },
            TempData = new TempDataDictionary(httpContext, new Mock<ITempDataProvider>().Object)
        };

        controller.TempData.Set(SelectedSchoolsForRemoval, removeSchoolsModel);

        // Act
        var result = controller.ConfirmRemove();

        // Assert
        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        var bannerModel = controller.TempData.Get<BannerViewModel>(BannerModel);
        Assert.NotNull(bannerModel);
        Assert.Equal("Saved school removed from your compare list", bannerModel.HeaderContent!.Trim());
    }
}
