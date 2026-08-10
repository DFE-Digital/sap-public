namespace SAPPub.Integration.Tests;

public class WebApplicationSetupFixture : IAsyncLifetime
{
    private TestWebApplicationFactory? _factory;

    public string BaseUrl { get; private set; } = null!;

    public Task InitializeAsync()
    {
        _factory = new TestWebApplicationFactory();

        if (_factory.Server == null) throw new InvalidOperationException("Test Server not started");

        BaseUrl = _factory.ClientOptions.BaseAddress.ToString();

        return Task.CompletedTask;
    }

    public bool IsHeaded()
    {
        return _factory?
            .GetAppConfiguration()
            .GetSection("Playwright")["Headed"] is string headed && bool.TryParse(headed, out var isHeaded) && isHeaded;
    }

    public async Task DisposeAsync()
    {
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }
}
