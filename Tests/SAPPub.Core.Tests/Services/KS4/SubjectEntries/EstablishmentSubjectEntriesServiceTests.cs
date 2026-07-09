using Moq;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.Services.KS4.SubjectEntries;
using System.Diagnostics;

namespace SAPPub.Core.Tests.Services.KS4.SubjectEntries;

public class EstablishmentSubjectEntriesServiceTests
{
    private readonly Mock<IEstablishmentSubjectEntriesRepository> _repo = new();
    private readonly EstablishmentSubjectEntriesService _service;

    public EstablishmentSubjectEntriesServiceTests()
    {
        _service = new EstablishmentSubjectEntriesService(_repo.Object);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_calls_gcse_vocational_and_other_once_each()
    {
        // Arrange
        var urn = "123456";

        _repo
            .Setup(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repo
            .Setup(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repo
            .Setup(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _service.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        _repo.Verify(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

   
    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_runs_calls_in_parallel()
    {
        // Arrange
        var urn = "123456";

        _repo
            .Setup(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await Task.Delay(200);
                return [];
            });

        _repo
            .Setup(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await Task.Delay(200);
                return [];
            });

        _repo
            .Setup(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await Task.Delay(200);
                return [];
            });

        var sw = Stopwatch.StartNew();

        // Act
        var _ = await _service.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        sw.Stop();

        // Assert
        // Sequential would be ~400ms. Parallel should be close to ~200-300ms on CI.
        Assert.True(sw.ElapsedMilliseconds < 330, $"Expected parallel execution; took {sw.ElapsedMilliseconds}ms");

        _repo.Verify(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }
}