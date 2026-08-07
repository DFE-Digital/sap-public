using Dfe.Analytics.AspNetCore;
using Dfe.Analytics.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Web.Areas.Profiles.Filters;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Filters;

public class DfEAnalyticsAddPhaseTagFilterTests
{
    private const string _urn = "123456";
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IEstablishment> _controllerWithIEstablishment = new();
    private readonly Mock<Event> _eventMock = new Mock<Event>();

    public DfEAnalyticsAddPhaseTagFilterTests()
    {

    }

    private ActionExecutingContext CreateContext(bool controllerImplementsIEstablishment)
    {
        var httpContext = new DefaultHttpContext();
        var webRequestEventFeature = new WebRequestEventFeature(_eventMock.Object);
        httpContext.Features.Set(webRequestEventFeature);

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());
        var context = new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>()
            {
            },
            controller: controllerImplementsIEstablishment ? _controllerWithIEstablishment.Object : new object());
        return context;
    }

    [Fact]
    public async Task UrnNotPresent_ShortcutsToNext()
    {
        var context = CreateContext(controllerImplementsIEstablishment: false);

        var nextCalled = false;

        ActionExecutionDelegate next = () =>
        {
            nextCalled = true;

            return Task.FromResult(
                new ActionExecutedContext(
                    context,
                    [],
                    new object()));
        };

        var filterUnderTest = new DfEAnalyticsAddPhaseTagFilter(_establishmentService.Object);

        // Act
        await filterUnderTest.OnActionExecutionAsync(context, next);

        // Assert
        _establishmentService.Verify(
            x => x.GetEstablishmentAsync(_urn, It.IsAny<CancellationToken>()),
            Times.Never);
        Assert.Empty(_eventMock.Object.Tags);
        Assert.True(nextCalled);
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public async Task IEstablishmentNotImplemented_LoadsEstablishmentFromService(bool isKS2, bool isKS4, bool isKS5)
    {
        // Arrange
        _establishmentService
            .Setup(x => x.GetEstablishmentAsync(_urn, CancellationToken.None))
            .ReturnsAsync(new EstablishmentServiceModel()
            {
                IsKS2 = isKS2,
                IsKS4 = isKS4,
                IsKS5 = isKS5
            });

        var context = CreateContext(controllerImplementsIEstablishment: false);
        context.ActionArguments["urn"] = _urn;

        var nextCalled = false;

        ActionExecutionDelegate next = () =>
        {
            nextCalled = true;

            return Task.FromResult(
                new ActionExecutedContext(
                    context,
                    [],
                    new object()));
        };

        var filterUnderTest = new DfEAnalyticsAddPhaseTagFilter(_establishmentService.Object);

        // Act
        await filterUnderTest.OnActionExecutionAsync(context, next);

        // Assert
        _establishmentService.Verify(
            x => x.GetEstablishmentAsync(_urn, It.IsAny<CancellationToken>()),
            Times.Once);

        if (isKS2)
        {
            Assert.Contains("KS2", _eventMock.Object.Tags);
        }
        if (isKS4)
        {
            Assert.Contains("KS4", _eventMock.Object.Tags);
        }
        if (isKS5)
        {
            Assert.Contains("KS5", _eventMock.Object.Tags);
        }
        Assert.True(nextCalled);
    }

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public async Task IEstablishmentImplemented_UsesEstablishment(bool isKS2, bool isKS4, bool isKS5)
    {
        // Arrange
        _controllerWithIEstablishment
            .SetupGet(x => x.Establishment)
            .Returns(new EstablishmentServiceModel()
            {
                IsKS2 = isKS2,
                IsKS4 = isKS4,
                IsKS5 = isKS5
            });

        var context = CreateContext(controllerImplementsIEstablishment: true);
        context.ActionArguments["urn"] = _urn;

        var nextCalled = false;

        ActionExecutionDelegate next = () =>
        {
            nextCalled = true;

            return Task.FromResult(
                new ActionExecutedContext(
                    context,
                    [],
                    new object()));
        };

        var filterUnderTest = new DfEAnalyticsAddPhaseTagFilter(_establishmentService.Object);

        // Act
        await filterUnderTest.OnActionExecutionAsync(context, next);

        // Assert
        _establishmentService.Verify(
            x => x.GetEstablishmentAsync(_urn, It.IsAny<CancellationToken>()),
            Times.Never);
        if (isKS2)
        {
            Assert.Contains("KS2", _eventMock.Object.Tags);
        }
        if (isKS4)
        {
            Assert.Contains("KS4", _eventMock.Object.Tags);
        }
        if (isKS5)
        {
            Assert.Contains("KS5", _eventMock.Object.Tags);
        }
        Assert.True(nextCalled);
    }
}
