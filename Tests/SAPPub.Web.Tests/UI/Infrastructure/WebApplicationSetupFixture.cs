using Microsoft.Extensions.Configuration;
using SAPPub.Web.Tests.UI.Helpers;

namespace SAPPub.Web.Tests.UI.Infrastructure
{
    public class WebApplicationSetupFixture : IAsyncLifetime
    {
        private readonly bool _enableOverview;
        private TestWebApplicationFactory? _factory;

        public WebApplicationSetupFixture()
            : this(true)
        {
        }

        protected WebApplicationSetupFixture(
            bool enableOverview)
        {
            _enableOverview = enableOverview;
        }

        public string BaseUrl { get; private set; } = null!;
        public IConfiguration Configuration { get; private set; } = null!;

        public IServiceProvider Services => _factory?.Services ?? throw new InvalidOperationException("Test Server not started");

        public Task InitializeAsync()
        {
            _factory =
                new TestWebApplicationFactory(
                    _enableOverview);

            if (_factory.Server == null)
            {
                throw new InvalidOperationException(
                    "Test Server not started");
            }

            BaseUrl =
                _factory.ClientOptions.BaseAddress.ToString();

            Configuration =
                _factory.Configuration;

            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await AccessibilityReportHelper.FlushReportAsync();

            if (_factory != null)
            {
                await _factory.DisposeAsync();
            }
        }
    }
}
