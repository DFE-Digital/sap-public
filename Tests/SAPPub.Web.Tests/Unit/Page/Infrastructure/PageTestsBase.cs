using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Helpers;

namespace SAPPub.Web.Tests.Unit.Page.Infrastructure;

public abstract class PageTestsBase : IDisposable // implement IDisposable so can clear the mock accessor after each test
{
    protected readonly WebAppFixture Fixture;
    private readonly List<Action> _clearActions = new();

    protected string BuildUrl(string urn, string schoolName, string pageRoute)
    {
        var encodedSchoolName = TextHelpers.CleanForUrl(schoolName);
        return $"/school/{urn}/{encodedSchoolName}{pageRoute}";
    }

    protected PageTestsBase(WebAppFixture fixture)
    {
        Fixture = fixture;
    }

    protected Mock<T> UseMock<T>() where T : class
    {
        var accessor = Fixture.Factory.Services
            .GetRequiredService<MockAccessor<T>>();

        _clearActions.Add(accessor.Clear);
        return accessor.GetOrCreate();
    }

    public void Dispose()
    {
        foreach (var clear in _clearActions)
            clear();
    }
}
