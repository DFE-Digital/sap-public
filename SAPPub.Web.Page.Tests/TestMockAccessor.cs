using Moq;

namespace SAPPub.Web.Page.Tests;

public class MockAccessor<T> where T : class
{
    private Mock<T> _current = new();

    public void Set(Mock<T> mock) => _current = mock;

    public Mock<T>? Get()
    {
        return _current;
    }

    public Mock<T> GetOrDefault()
    {
        return _current ?? new Mock<T>(); // null fallback because of BackgroundService requesting the Repository at startup
    }

    public void Clear() => _current = null;
}
