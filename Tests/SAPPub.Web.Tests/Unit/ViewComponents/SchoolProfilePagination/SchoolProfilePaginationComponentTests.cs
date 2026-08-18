using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.FeatureManagement;
using SAPPub.Web.Constants;
using SAPPub.Web.ViewComponents.SchoolProfilePagination;

namespace SAPPub.Web.Tests.Unit.ViewComponents.SchoolProfilePagination;

public class SchoolProfilePaginationComponentTests
{
    private static SAPPub.Web.ViewComponents.SchoolProfilePagination.SchoolProfilePagination CreateComponent(
        ISchoolProfilePaginationResolver? resolver = null,
        IFeatureManager? featureManager = null) =>
        new(resolver ?? new SchoolProfilePaginationResolver(), featureManager ?? new FakeFeatureManager(true, true));

    [Fact]
    public async Task InvokeAsync_ReturnsDefaultViewWithResolvedResult()
    {
        var model = new SchoolProfilePaginationModel
        {
            CurrentRoute = RouteConstants.SecondaryAdmissions,
            RouteAttributes = new Dictionary<string, string> { { RouteConstants.URN, "123" } },
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        var component = CreateComponent();

        var result = await component.InvokeAsync(model) as ViewViewComponentResult;

        Assert.NotNull(result);
        Assert.Equal("~/ViewComponents/SchoolProfilePagination/Default.cshtml", result.ViewName);

        var viewModel = Assert.IsType<SchoolProfilePaginationViewModel>(result.ViewData!.Model);
        Assert.NotNull(viewModel.Result.Previous);
        Assert.Equal(RouteConstants.AboutTheSchool, viewModel.Result.Previous!.Route);
        Assert.NotNull(viewModel.Result.Next);
        Assert.Equal(RouteConstants.SecondaryCurriculumAndExtraCurricularActivities, viewModel.Result.Next!.Route);
        Assert.Equal(model.RouteAttributes, viewModel.RouteAttributes);
    }

    [Fact]
    public async Task InvokeAsync_WithNullModel_DoesNotThrow_AndReturnsEmptyResult()
    {
        var component = CreateComponent();

        var result = await component.InvokeAsync(null!) as ViewViewComponentResult;

        Assert.NotNull(result);
        Assert.Equal("~/ViewComponents/SchoolProfilePagination/Default.cshtml", result.ViewName);

        var viewModel = Assert.IsType<SchoolProfilePaginationViewModel>(result.ViewData!.Model);
        Assert.Null(viewModel.Result.Previous);
        Assert.Null(viewModel.Result.Next);
    }

    [Fact]
    public async Task InvokeAsync_DelegatesToResolver_WithFeatureFlagsResolvedCentrally()
    {
        var expected = new SchoolProfilePaginationResult(
            new PaginationLink(RouteConstants.AboutTheSchool, "About the school"),
            null);

        var fakeResolver = new FakeResolver(expected);
        var fakeFeatureManager = new FakeFeatureManager(isPrimaryEnabled: true, is16To19Enabled: false);
        var component = CreateComponent(fakeResolver, fakeFeatureManager);

        var model = new SchoolProfilePaginationModel
        {
            CurrentRoute = RouteConstants.SecondaryAdmissions,
            RouteAttributes = new Dictionary<string, string>(),
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        var result = await component.InvokeAsync(model) as ViewViewComponentResult;

        var viewModel = Assert.IsType<SchoolProfilePaginationViewModel>(result!.ViewData!.Model);
        Assert.Same(expected, viewModel.Result);
        Assert.Equal(model.CurrentRoute, fakeResolver.ReceivedRoute);
        Assert.NotNull(fakeResolver.ReceivedContext);
        Assert.True(fakeResolver.ReceivedContext!.IsPrimaryEnabled);
        Assert.False(fakeResolver.ReceivedContext!.Is16To19Enabled);
    }

    private sealed class FakeResolver : ISchoolProfilePaginationResolver
    {
        private readonly SchoolProfilePaginationResult _result;

        public FakeResolver(SchoolProfilePaginationResult result) => _result = result;

        public string? ReceivedRoute { get; private set; }

        public PaginationContext? ReceivedContext { get; private set; }

        public SchoolProfilePaginationResult Resolve(string currentRoute, PaginationContext context)
        {
            ReceivedRoute = currentRoute;
            ReceivedContext = context;
            return _result;
        }
    }

    private sealed class FakeFeatureManager : IFeatureManager
    {
        private readonly bool _isPrimaryEnabled;
        private readonly bool _is16To19Enabled;

        public FakeFeatureManager(bool isPrimaryEnabled, bool is16To19Enabled)
        {
            _isPrimaryEnabled = isPrimaryEnabled;
            _is16To19Enabled = is16To19Enabled;
        }

        public IAsyncEnumerable<string> GetFeatureNamesAsync() => throw new NotImplementedException();

        public Task<bool> IsEnabledAsync(string feature) => Task.FromResult(feature switch
        {
            Constants.Constants.EnablePrimary => _isPrimaryEnabled,
            Constants.Constants.Enable16to19 => _is16To19Enabled,
            _ => false
        });

        public Task<bool> IsEnabledAsync<TContext>(string feature, TContext context) => IsEnabledAsync(feature);
    }
}
