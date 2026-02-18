using Moq;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.Services.KS4.SubjectEntries;
using System.Diagnostics;
using Xunit;

namespace SAPPub.Core.Tests.Services.KS4.SubjectEntries;

public class EstablishmentSubjectEntriesServiceTests
{
    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_calls_core_and_additional_once_each()
    {
        // Arrange
        var urn = "123456";

        var repo = new Mock<IEstablishmentSubjectEntriesRepository>(MockBehavior.Strict);

        repo.Setup(r => r.GetCoreSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentCoreSubjectEntries());

        repo.Setup(r => r.GetAdditionalSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentAdditionalSubjectEntries());

        var sut = new EstablishmentSubjectEntriesService(repo.Object); 

        // Act
        var result = await sut.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Core);
        Assert.NotNull(result.Additional);

        repo.Verify(r => r.GetCoreSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.GetAdditionalSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_runs_calls_in_parallel()
    {
        // Arrange
        var urn = "123456";

        var repo = new Mock<IEstablishmentSubjectEntriesRepository>();

        repo.Setup(r => r.GetCoreSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await Task.Delay(200);
                return new EstablishmentCoreSubjectEntries();
            });

        repo.Setup(r => r.GetAdditionalSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await Task.Delay(200);
                return new EstablishmentAdditionalSubjectEntries();
            });

        var sut = new EstablishmentSubjectEntriesService(repo.Object); 

        var sw = Stopwatch.StartNew();

        // Act
        var _ = await sut.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None); 

        sw.Stop();

        // Assert
        // Sequential would be ~400ms. Parallel should be close to ~200-300ms on CI.
        Assert.True(sw.ElapsedMilliseconds < 330, $"Expected parallel execution; took {sw.ElapsedMilliseconds}ms");

        repo.Verify(r => r.GetCoreSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(r => r.GetAdditionalSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }
}
