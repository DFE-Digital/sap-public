using Moq;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public class MockAccessor<T> where T : class
{
    private Mock<T>? _current;

    public void Set(Mock<T> mock) => _current = mock;

    public Mock<T>? Get() => _current;

    public Mock<T> GetOrCreate()
    {
        _current ??= new Mock<T>();
        return _current;
    }

    public T Object => GetOrCreate().Object;

    public void Clear() => _current = null;
}
