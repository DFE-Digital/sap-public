using Moq;
using SAPPub.Core.Interfaces.Repositories.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.Performance;
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
    public async Task Constructor_throws_when_repository_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => new EstablishmentSubjectEntriesService(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetSubjectEntriesByUrnAsync_throws_for_invalid_urn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => _service.GetSubjectEntriesByUrnAsync(urn!, CancellationToken.None));
        Assert.Equal("urn", ex.ParamName);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_throws_when_cancellation_is_already_requestsed()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        var urn = "123456";

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _service.GetSubjectEntriesByUrnAsync(urn, cts.Token));

        // Assert
        _repo.Verify(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Never);
        _repo.Verify(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Never);
        _repo.Verify(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_returns_values_from_all_repository_calls()
    {
        // Arrange
        var urn = "123456";
        var gcse = new List<SubjectsEntered> { new() { Subject = "Maths" } };
        var vocational = new List<SubjectsEntered> { new() { Subject = "Engineering" } };
        var other = new List<SubjectsEntered> { new() { Subject = "Music" } };


        _repo
            .Setup(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(gcse);

        _repo
            .Setup(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(vocational);

        _repo
            .Setup(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(other);

        // Act
        var result = await _service.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);


        // Assert
        Assert.Same(gcse, result.Gcse);
        Assert.Same(vocational, result.Vocational);
        Assert.Same(other, result.Other);

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
        Assert.True(sw.ElapsedMilliseconds < 550, $"Expected parallel execution; took {sw.ElapsedMilliseconds}ms");

        _repo.Verify(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        _repo.Verify(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_when_multiple_repository_calls_fail_then_throws()
    {
        // Arrange
        var urn = "123456";
        var gcseExc = new InvalidOperationException("gcse failure");
        var otherExc = new InvalidOperationException("other failure");

        _repo
            .Setup(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IEnumerable<SubjectsEntered>>(gcseExc));

        _repo
            .Setup(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repo
            .Setup(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IEnumerable<SubjectsEntered>>(otherExc));

        // Act
        var ex = await Record.ExceptionAsync(() => _service.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None));

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<InvalidOperationException>(ex);
        Assert.True(ex.Message == gcseExc.Message || ex.Message == otherExc.Message, $"Unexpected exception message: {ex.Message}");
    }

    /// <summary>
    /// Verifies the service truly awaits on otherTask in Task.WhenAll
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_waits_for_other_task_before_faulting()
    {
        // Arrange
        var urn = "123456";
        var expected = new InvalidOperationException("gcse failed");

        // Keep the "other" task pending so we can prove Task.WhenAll includes it.
        var otherTaskCompletionSource = new TaskCompletionSource<IEnumerable<SubjectsEntered>>(TaskCreationOptions.RunContinuationsAsynchronously);

        _repo
            .Setup(r => r.GetGcseSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IEnumerable<SubjectsEntered>>(expected));

        _repo
            .Setup(r => r.GetVocationalAwardSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repo
            .Setup(r => r.GetOtherSubjectEntriesByUrnAsync(urn, It.IsAny<CancellationToken>()))
            .Returns(otherTaskCompletionSource.Task);

        // Act
        var result = _service.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        // If the service omitted "other" task from the WhenAll, this would already be completed/faulted (because its currently "pending")
        Assert.False(result.IsCompleted);
        otherTaskCompletionSource.SetResult([]);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => result);
        Assert.Equal(expected.Message, ex.Message);
    }
}