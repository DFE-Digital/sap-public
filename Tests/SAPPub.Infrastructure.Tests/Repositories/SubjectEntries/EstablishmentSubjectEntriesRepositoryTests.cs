using Moq;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;

namespace SAPPub.Infrastructure.Tests.Repositories.SubjectEntries;
public class EstablishmentSubjectEntriesRepositoryTests
{
    private readonly Mock<IGenericRepository<EstablishmentSubjectEntryRow>> _repo = new();
    private readonly EstablishmentSubjectEntriesRepository _sut;

    public EstablishmentSubjectEntriesRepositoryTests()
    {
        _sut = new EstablishmentSubjectEntriesRepository(_repo.Object);
    }

    [Fact]
    public async Task GetGcseSubjectEntriesByUrnAsync_WhenUrnBlank_ReturnsEmpty_AndDoesNotCallRepo()
    {
        var result = await _sut.GetGcseSubjectEntriesByUrnAsync("   ", CancellationToken.None);

        Assert.NotNull(result);

        _repo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetVocationalSubjectEntriesByUrnAsync_WhenNoRows_ReturnsEmpty()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentSubjectEntryRow>());

        var result = await _sut.GetVocationalAwardSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOtherSubjectEntriesByUrnAsync_WhenNoRows_ReturnsEmpty()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentSubjectEntryRow>());

        var result = await _sut.GetOtherSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGcseSubjectEntriesByUrnAsync_OnlyReturnsTotalExamEntriesRows()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
            {
                new() { subject_discount_group = "Mathematics", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
                new() { subject_discount_group = "Mathematics", qualification_type = "GCSE", grade = "9", number_achieving = 1 }
            });

        var result = await _sut.GetGcseSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result?.Count());
    }

    [Fact]
    public async Task GetVocationalSubjectEntriesByUrnAsync_OnlyReturnsTotalExamEntriesRows()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
            {
                new() { subject_discount_group = "Sports Studies", qualification_type = "Vocational", grade = "Total exam entries", number_achieving = 10 },
                new() { subject_discount_group = "Sports Studies", qualification_type = "Vocational", grade = "9", number_achieving = 1 }
            });

        var result = await _sut.GetVocationalAwardSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result?.Count());
    }

    [Fact]
    public async Task GetOtherSubjectEntriesByUrnAsync_OnlyReturnsTotalExamEntriesRows()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
            {
                new() { subject_discount_group = "Music Performance: Group", qualification_type = "Grade 6 Music or Dance", grade = "Total exam entries", number_achieving = 10 },
                new() { subject_discount_group = "Music Performance: Group", qualification_type = "Grade 6 Music or Dance", grade = "9", number_achieving = 1 }
            });

        var result = await _sut.GetOtherSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result?.Count());
    }

    [Theory]
    [InlineData("  biology ", "biology", "biology")]
    [InlineData("  biology ", "  biology  ", "biology")]
    [InlineData("  mathematics ", "maths (Further)", "Mathematics (Further)")]
    [InlineData("  mathematics ", "   maths (Further)", "Mathematics (Further)")]
    public async Task TrimsSubject(string subject, string subjectDetail, string expected)
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
               new() { pupil_count = 100, subject = subject, subject_discount_group = subjectDetail, qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
            ]);

        var result = await _sut.GetGcseSubjectEntriesByUrnAsync("123", CancellationToken.None);

        var entry = Assert.Single(result);
        Assert.Equal(expected, entry.Subject); // trimmed subject; case preserved from data
    }

    [Fact]
    public async Task SplitsGroupsCorrectly()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new() { pupil_count = 100, subject = "Biology", subject_discount_group = "Biology", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
                new() { pupil_count = 100, subject = "Sports", subject_discount_group = "Sports Studies", qualification_type = "Vocational", grade = "Total exam entries", number_achieving = 20 },
                new() { pupil_count = 100, subject = "Music", subject_discount_group = "Music Performance: Group", qualification_type = "Grade 6 Performing Arts Graded Examination", grade = "Total exam entries", number_achieving = 20 },
            ]);

        var gcse = await _sut.GetGcseSubjectEntriesByUrnAsync("123", CancellationToken.None);
        var vocational = await _sut.GetVocationalAwardSubjectEntriesByUrnAsync("123", CancellationToken.None);
        var other = await _sut.GetOtherSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.Single(gcse);
        Assert.Single(vocational);
        Assert.Single(other);

        Assert.Equal("Biology", gcse.Single().Subject);
        Assert.Equal("Sports Studies", vocational.Single().Subject);
        Assert.Equal("Music Performance: Group", other.Single().Subject);
    }


    [Fact]
    public async Task ConvertsMathsToMathematics()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
            {
                new() { pupil_count = 100, subject = "Mathematics", subject_discount_group = "Additional Maths (Core)", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
            });

        var core = await _sut.GetGcseSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.Single(core);

        Assert.Equal("Additional Mathematics (Core)", core.Single().Subject);
    }

    [Fact]
    public async Task OrdersBySubjectThenQualification()
    {
        _repo
            .Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new() { pupil_count = 100, subject = "Mathematics", subject_discount_group = "Additional Maths (FSMQ)", qualification_type = "FSMQ", grade = "Total exam entries", number_achieving = 1 },
                new() { pupil_count = 100, subject = "Music", subject_discount_group = "Music Performance: Group", qualification_type = "Grade 8 Performing Arts Graded Examination", grade = "Total exam entries", number_achieving = 2 },
                new() { pupil_count = 100, subject = "Music", subject_discount_group = "Music Performance: Group", qualification_type = "Grade 6 Performing Arts Graded Examination", grade = "Total exam entries", number_achieving = 2 },
                new() { pupil_count = 100, subject = "Music", subject_discount_group = "Music Performance: Group", qualification_type = "Grade 7 Performing Arts Graded Examination", grade = "Total exam entries", number_achieving = 4 },
            ]);

        var otherSubjects = await _sut.GetOtherSubjectEntriesByUrnAsync("123", CancellationToken.None);

        Assert.Equal(4, otherSubjects.Count());

        Assert.Collection(otherSubjects,
            first =>
            {
                Assert.Equal("Additional Maths (FSMQ)", first.Subject);
                Assert.Equal("FSMQ", first.Qualification);
            },
            second =>
            {
                Assert.Equal("Music Performance: Group", second.Subject);
                Assert.Equal("Grade 6 Performing Arts Graded Examination", second.Qualification);
            },
            third =>
            {
                Assert.Equal("Music Performance: Group", third.Subject);
                Assert.Equal("Grade 7 Performing Arts Graded Examination", third.Qualification);
            },
            fourth =>
            {
                Assert.Equal("Music Performance: Group", fourth.Subject);
                Assert.Equal("Grade 8 Performing Arts Graded Examination", fourth.Qualification);
            });
    }
}
