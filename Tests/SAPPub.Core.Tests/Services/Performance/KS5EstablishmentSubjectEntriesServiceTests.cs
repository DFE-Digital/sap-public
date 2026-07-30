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

    public KS5EstablishmentSubjectEntriesServiceTests()
    {
        _service = new KS5EstablishmentSubjectEntriesService(_repo.Object);
    }

    [Fact]
    public async Task Constructor_throws_when_repository_is_null()
    {
        Assert.Throws<ArgumentNullException>(() => new KS4EstablishmentSubjectEntriesService(null!));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetSubjectEntriesByUrnAsync_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => _service.GetSubjectEntriesByUrnAsync(urn!, QualificationType.AllQualifications, CancellationToken.None));
        Assert.Equal("urn", ex.ParamName);
    }


    [Theory]
    [InlineData(QualificationType.AllQualifications)]

    public async Task GetSubjectEntriesByUrnAsync_ReturnsRequiredData(QualificationType? qualificationType)
    {
        // Arrange
        var subjectsEnteredList = new List<SubjectsEnteredModel>
        {
            new() { ExamCohort = "10" },
        };


        _repo
            .Setup(a => a.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subjectsEnteredList);


        // Act


        // Assert
    }
}