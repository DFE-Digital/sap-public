using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Repositories.SubjectEntries;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Services.Performance;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS5EstablishmentSubjectEntriesServiceTests
{
    private readonly Mock<IKS5EstablishmentSubjectEntriesRepository> _repo = new();
    private readonly IKS5EstablishmentSubjectEntriesService _service;

    private static readonly List<SubjectsEnteredModel> AllSubjects =
    [
        new() { Subject = "Maths",      ExamCohort = "A level" },
        new() { Subject = "History",    ExamCohort = "Other academic" },
        new() { Subject = "Sport",      ExamCohort = "Applied general" },
        new() { Subject = "IT",         ExamCohort = "Tech level" },
        new() { Subject = "Health",     ExamCohort = "Technical certificate" }
    ];

    public KS5EstablishmentSubjectEntriesServiceTests()
    {
        _service = new KS5EstablishmentSubjectEntriesService(_repo.Object);
    }

    [Fact]
    public void Constructor_throws_when_repository_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => new KS4EstablishmentSubjectEntriesService(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetSubjectEntriesByUrnAsync_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetSubjectEntriesByUrnAsync(urn!, QualificationType.AllQualifications, CancellationToken.None));
        Assert.Equal("urn", ex.ParamName);
    }


    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), QualificationType.AllQualifications, cts.Token));

        // Assert
        _repo.Verify(a => a.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(QualificationType.AllQualifications)]
    [InlineData(null)]
    public async Task GetSubjectEntriesByUrnAsync_ReturnsAllSubjects_WhenAllQualificationsOrNull(QualificationType? qualificationType)
    {
        // Arrange
        _repo
            .Setup(A => A.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AllSubjects);

        // Act
        var result = (await _service.GetSubjectEntriesByUrnAsync("123456", qualificationType, CancellationToken.None)).ToList();

        // Assert
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_ReturnsAllSubjects_WhenAcademicQualificationsRequested()
    {
        // Arrange
        _repo
            .Setup(A => A.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AllSubjects);

        // Act
        var result = (await _service.GetSubjectEntriesByUrnAsync("123456", QualificationType.AcademicQualifications, CancellationToken.None)).ToList();


        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, a => Assert.Contains(a.ExamCohort, new[] { "A level", "Other academic" }));
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_ReturnsAllSubjects_WhenVocationalAndTechnicalQualificationsRequested()
    {
        // Arrange
        _repo
            .Setup(A => A.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AllSubjects);

        // Act
        var result = (await _service.GetSubjectEntriesByUrnAsync("123456", QualificationType.VocationalAndTechnicalQualifications, CancellationToken.None)).ToList();


        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, a => Assert.Contains(a.ExamCohort, new[] { "Applied general", "Tech level", "Technical certificate" }));
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_ReturnsEmpty_WhenRepoReturnsNoData()
    {
        // Arrange
        _repo
            .Setup(A => A.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = (await _service.GetSubjectEntriesByUrnAsync("123456", QualificationType.AllQualifications, CancellationToken.None)).ToList();


        // Assert
        Assert.Empty(result);

    }
}