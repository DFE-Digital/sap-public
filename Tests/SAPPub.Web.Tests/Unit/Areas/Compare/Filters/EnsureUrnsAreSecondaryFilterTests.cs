using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Compare.Filters;

namespace SAPPub.Web.Tests.Unit.Areas.Compare.Filters;

public class EnsureUrnsAreSecondaryFilterTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService = new();
    private readonly EstablishmentServiceModel establishment1IsKS4;
    private readonly EstablishmentServiceModel establishment2IsKS4;
    private readonly EstablishmentServiceModel establishment3IsNotKS4;
    private readonly EstablishmentServiceModel establishment4IsNotKS4;

    private DefaultHttpContext httpContext = new();
    private ActionContext actionContext;
    private ActionExecutedContext executedContext;
    private SecondaryComparisonQueryValidationFilter filter;

    public EnsureUrnsAreSecondaryFilterTests()
    {
        establishment1IsKS4 = new EstablishmentTestBuilder().WithIsKeyStage4(true).BuildServiceModel();
        establishment2IsKS4 = new EstablishmentTestBuilder().WithIsKeyStage4(true).BuildServiceModel();
        establishment3IsNotKS4 = new EstablishmentTestBuilder().WithIsKeyStage4(false).BuildServiceModel();
        establishment4IsNotKS4 = new EstablishmentTestBuilder().WithIsKeyStage4(false).BuildServiceModel();

        _mockEstablishmentService.Setup(x => x.GetEstablishmentAsync(establishment1IsKS4.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment1IsKS4);
        _mockEstablishmentService.Setup(x => x.GetEstablishmentAsync(establishment2IsKS4.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment2IsKS4);
        _mockEstablishmentService.Setup(x => x.GetEstablishmentAsync(establishment3IsNotKS4.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment3IsNotKS4);
        _mockEstablishmentService.Setup(x => x.GetEstablishmentAsync(establishment4IsNotKS4.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment4IsNotKS4);

        filter = new SecondaryComparisonQueryValidationFilter(_mockEstablishmentService.Object);

        actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor()
        );

        executedContext = new ActionExecutedContext(
                actionContext,
                [],
                controller: null!
            );
    }

    [Fact]
    public async Task Filter_ContextHasValidUrns_ReturnsUrnsAndAddsEstablishmentsToContext()
    {
        // Arrange
        var actionArguments = new Dictionary<string, object?>
            {
                { "urns", new List<string> { establishment1IsKS4.URN, establishment2IsKS4.URN } }
            };

        var context = new ActionExecutingContext(
            actionContext,
            [],
            actionArguments,
            null!);

        var nextCalled = false;
        Task<ActionExecutedContext> next()
        {
            nextCalled = true;
            return Task.FromResult(executedContext);
        }

        // Act
        await filter.OnActionExecutionAsync(context, next);

        // Assert
        var resultUrns = context.ActionArguments["urns"] as List<string>;
        Assert.NotNull(resultUrns);
        Assert.Equal(2, resultUrns.Count);
        Assert.Collection(resultUrns,
            urn => Assert.Equal(establishment1IsKS4.URN, urn),
            urn => Assert.Equal(establishment2IsKS4.URN, urn)
        );

        var establishments = context.HttpContext.Items["Establishments"] as List<EstablishmentServiceModel>;

        Assert.NotNull(establishments);

        Assert.Collection(establishments,
            est => Assert.Equal(establishment1IsKS4.URN, est.URN),
            est => Assert.Equal(establishment2IsKS4.URN, est.URN)
        );
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task Filter_ContextHasValidAndInvalidUrns_ReturnsOnlyValidUrns()
    {
        // Arrange
        var actionArguments = new Dictionary<string, object?>
            {
                { "urns", new List<string> { establishment1IsKS4.URN, establishment2IsKS4.URN, establishment3IsNotKS4.URN } }
            };

        var context = new ActionExecutingContext(
            actionContext,
            [],
            actionArguments,
            null!);

        var nextCalled = false;
        Task<ActionExecutedContext> next()
        {
            nextCalled = true;
            return Task.FromResult(executedContext);
        }

        // Act
        await filter.OnActionExecutionAsync(context, next);

        // Assert
        var resultUrns = context.ActionArguments["urns"] as List<string>;
        Assert.NotNull(resultUrns);
        Assert.Equal(2, resultUrns.Count);
        Assert.Collection(resultUrns,
            urn => Assert.Equal(establishment1IsKS4.URN, urn),
            urn => Assert.Equal(establishment2IsKS4.URN, urn)
        );
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task Filter_ContextHasLessThanTwoValidUrns_ReturnNotFoundResult()
    {
        // Arrange
        var actionArguments = new Dictionary<string, object?>
            {
                { "urns", new List<string> { establishment2IsKS4.URN, establishment3IsNotKS4.URN, establishment4IsNotKS4.URN } }
            };

        var context = new ActionExecutingContext(
            actionContext,
            [],
            actionArguments,
            null!);

        var nextCalled = false;
        Task<ActionExecutedContext> next()
        {
            nextCalled = true;
            return Task.FromResult(executedContext);
        }
        ;

        // Act
        await filter.OnActionExecutionAsync(context, next);

        // Assert
        var result = context.Result as NotFoundResult;
        Assert.NotNull(result);
        Assert.False(nextCalled);
    }
}
