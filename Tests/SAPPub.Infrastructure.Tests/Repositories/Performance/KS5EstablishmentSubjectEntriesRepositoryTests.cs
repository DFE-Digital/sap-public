using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.Performance;
using System.Net.Http.Headers;

namespace SAPPub.Infrastructure.Tests.Repositories.Performance;

public class KS5EstablishmentSubjectEntriesRepositoryTests
{
    private readonly Mock<IGenericRepository<KS5EstablishmentSubjectEntryRow>> _repo = new();
    private readonly KS5EstablishmentSubjectEntriesRepository _sut;

    public KS5EstablishmentSubjectEntriesRepositoryTests()
    {
        _sut = new KS5EstablishmentSubjectEntriesRepository(_repo.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetSubjectEntriesByUrnAsync_WhenUrnBlank_ReturnsEmpty(string? urn)
    {
        // Arrange/Act
        var result = await _sut.GetSubjectEntriesByUrnAsync(urn!, CancellationToken.None);

        // Assert
        Assert.Empty(result);
        _repo.Verify(a => a.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_WhenNoRows_ReturnsEmpty()
    {
        // Arrange
        var urn = "123456";
        _repo
            .Setup(a => a.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<KS5EstablishmentSubjectEntryRow>());

        // Act
        var result = await _sut.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_OnlyReturnsTotalExamEntriesRows()
    {
        // Arrange
        var urn = "123456";
        _repo
            .Setup(a => a.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                [
                    new() { subject = "Maths", grade = "Total exam entries", entries_count = "10" },
                    new() { subject = "English", grade = "A" }
                ]
            );

        // Act
        var result = await _sut.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        var entry = Assert.Single(result);
        Assert.Equal("10", entry.TotalNumberOfEntries);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_ExcludesAllSubjectsRows()
    {
        // Arrange
        var urn = "123456";
        _repo
            .Setup(a => a.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                [
                    new() { subject = "Maths", grade = "Total exam entries", entries_count = "10" },
                    new() { subject = "All subjects", grade = "A" }
                ]
            );

        // Act
        var result = await _sut.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        var entry = Assert.Single(result);
        Assert.Equal("10", entry.TotalNumberOfEntries);
    }

    [Fact]
    public async Task GetSubjectEntriesByUrnAsync_MapsAllFieldsCorrectly()
    {
        // Arrange
        var urn = "123456";
        var subject = "Sport";
        var qualificationDetailed = "BTEC Sport";
        var qualificationLevel = "3";
        var examCohort = "Applied general";
        var grade = "Total exam entries";
        var entriesCount = "10";

        _repo
            .Setup(a => a.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                [
                    new()
                    {
                        subject = subject,
                        qualification_detailed=qualificationDetailed,
                        qualification_level=qualificationLevel,
                        exam_cohort=examCohort,
                        grade = grade,
                        entries_count = entriesCount
                    }
                ]
            );

        // Act
        var result = await _sut.GetSubjectEntriesByUrnAsync(urn, CancellationToken.None);

        // Assert
        var entry = Assert.Single(result);
        Assert.Equal(subject, entry.Subject);
        Assert.Equal(qualificationDetailed, entry.Qualification);
        Assert.Equal(qualificationLevel, entry.Level);
        Assert.Equal(examCohort, entry.ExamCohort);
        Assert.Equal(entriesCount, entry.TotalNumberOfEntries);
    }
}
