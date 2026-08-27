using Npgsql;
using SAPPub.Infrastructure.Repositories.Overview;

namespace SAPPub.Infrastructure.Tests.Repositories.Overview;

public class OverviewRepositoryTests
{
    private static NpgsqlDataSource CreateSafeDataSource()
    {
        return NpgsqlDataSource.Create(
            "Host=127.0.0.1;Port=1;Username=x;Password=x;Database=x;" +
            "Timeout=1;Command Timeout=1");
    }

    [Fact]
    public void Constructor_WithNullDataSource_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new OverviewRepository(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public async Task GetOverviewAsync_WithInvalidUrn_ReturnsNull(string? urn)
    {
        var sut = new OverviewRepository(CreateSafeDataSource());

        var result = await sut.GetOverviewAsync(
            urn!,
            CancellationToken.None);

        Assert.Null(result);
    }
}