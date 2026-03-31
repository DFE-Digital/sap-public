using Moq;

namespace SAPPub.Web.Page.Tests;

public class MockAccessor<T> where T : class
{
    private readonly AsyncLocal<Mock<T>?> _current = new();

    public void Set(Mock<T> mock) => _current.Value = mock;

    public Mock<T>? Get()
    {
        return _current.Value;
    }

    public Mock<T> GetOrDefault()
    {
        return _current.Value ?? new Mock<T>(); // null fallback because of IHostedService requesting the Repository at startup
    }

    public void Clear() => _current.Value = null;
}
