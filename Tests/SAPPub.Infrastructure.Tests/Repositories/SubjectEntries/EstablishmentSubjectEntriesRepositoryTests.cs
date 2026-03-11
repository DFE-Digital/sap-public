using Moq;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories.KS4.SubjectEntries
{
    public class EstablishmentSubjectEntriesRepositoryTests
    {
        private readonly Mock<IGenericRepository<EstablishmentSubjectEntryRow>> _repo = new();
        private readonly EstablishmentSubjectEntriesRepository _sut;

        public EstablishmentSubjectEntriesRepositoryTests()
        {
            _sut = new EstablishmentSubjectEntriesRepository(_repo.Object);
        }

        [Fact]
        public async Task GetCoreSubjectEntriesByUrnAsync_WhenUrnBlank_ReturnsEmpty_AndDoesNotCallRepo()
        {
            var result = await _sut.GetCoreSubjectEntriesByUrnAsync("   ", CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result.SubjectEntries);

            _repo.Verify(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetAdditionalSubjectEntriesByUrnAsync_WhenNoRows_ReturnsEmpty()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>());

            var result = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Empty(result.SubjectEntries);
        }

        [Fact]
        public async Task WhenCohortIsZeroOrMissing_ReturnsEmpty()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 0, subject = "Mathematics", qualification_type = "GCSE", grade = "9", number_achieving = 10 }
                 });

            var core = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);
            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Empty(core.SubjectEntries);
            Assert.Empty(add.SubjectEntries);
        }

        [Fact]
        public async Task GroupsBySubjectAndQualification_SumsAcrossGrades_AndConvertsToPercentOfCohort()
        {
            // Cohort 200
            // Maths total entries across grades = 50 -> 25%
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 200, subject = "Mathematics", qualification_type = "GCSE", grade = "9", number_achieving = 20 },
                     new() { pupil_count = 200, subject = "Mathematics", qualification_type = "GCSE", grade = "8", number_achieving = 30 },
                 });

            var result = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);

            var entry = Assert.Single(result.SubjectEntries);
            Assert.Equal("Mathematics", entry.SubEntCore_Sub_Est_Current_Num);
            Assert.Equal("GCSE", entry.SubEntCore_Qual_Est_Current_Num);
            AssertPct(25.0, entry.SubEntCore_Entr_Est_Current_Num);
        }

        [Fact]
        public async Task UsesMaxPupilCountAsCohort_Defensively()
        {
            // Max cohort = 100 (even if some rows have smaller/0)
            // English Language total=10 -> 10%
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 0,   subject = "English Language", qualification_type = "GCSE", grade = "9", number_achieving = 5 },
                     new() { pupil_count = 100, subject = "English Language", qualification_type = "GCSE", grade = "8", number_achieving = 5 },
                     new() { pupil_count = 80,  subject = "English Language", qualification_type = "GCSE", grade = "7", number_achieving = 0 },
                 });

            var result = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);

            var entry = Assert.Single(result.SubjectEntries);
            AssertPct(10.0, entry.SubEntCore_Entr_Est_Current_Num);
        }

        [Fact]
        public async Task TrimsSubject_AndTreatsCoreSubjectsCaseInsensitively()
        {
            // cohort=100, maths total=10 -> 10%
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "  mathematics  ", qualification_type = "GCSE", grade = "9", number_achieving = 10 },
                 });

            var result = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);

            var entry = Assert.Single(result.SubjectEntries);
            Assert.Equal("mathematics", entry.SubEntCore_Sub_Est_Current_Num); // trimmed subject; case preserved from data
            AssertPct(10.0, entry.SubEntCore_Entr_Est_Current_Num);
        }

        [Fact]
        public async Task Qualification_UsesType_FirstOtherwiseDetailed_AndNormalisesEmptyToNull()
        {
            // cohort=100
            // subject A uses qualification_type
            // subject B uses qualification_detailed
            // subject C has neither -> null
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "History", qualification_type = "GCSE", qualification_detailed = "Ignored", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "Geography", qualification_type = null, qualification_detailed = "BTEC", number_achieving = 20 },
                     new() { pupil_count = 100, subject = "Art", qualification_type = null, qualification_detailed = "   ", number_achieving = 5 },
                 });

            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Equal(3, add.SubjectEntries.Count);

            var history = add.SubjectEntries.Single(x => x.SubEntAdd_Sub_Est_Current_Num == "History");
            Assert.Equal("GCSE", history.SubEntAdd_Qual_Est_Current_Num);

            var geo = add.SubjectEntries.Single(x => x.SubEntAdd_Sub_Est_Current_Num == "Geography");
            Assert.Equal("BTEC", geo.SubEntAdd_Qual_Est_Current_Num);

            var art = add.SubjectEntries.Single(x => x.SubEntAdd_Sub_Est_Current_Num == "Art");
            Assert.Null(art.SubEntAdd_Qual_Est_Current_Num);
        }

        [Fact]
        public async Task ClampsPercentageTo100_WhenTotalEntriesExceedCohort()
        {
            // cohort=100, totalEntries=150 -> 150% -> clamp to 100
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "Mathematics", qualification_type = "GCSE", grade = "9", number_achieving = 150 }
                 });

            var core = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);

            var entry = Assert.Single(core.SubjectEntries);
            AssertPct(100.0, entry.SubEntCore_Entr_Est_Current_Num);
        }

        [Fact]
        public async Task IgnoresRowsWithNullOrWhitespaceSubject()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = null, qualification_type = "GCSE", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "   ", qualification_type = "GCSE", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "History", qualification_type = "GCSE", number_achieving = 10 },
                 });

            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            var entry = Assert.Single(add.SubjectEntries);
            Assert.Equal("History", entry.SubEntAdd_Sub_Est_Current_Num);
        }

        [Fact]
        public async Task SplitsCoreAndAdditionalCorrectly()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "Mathematics", qualification_type = "GCSE", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "History", qualification_type = "GCSE", number_achieving = 20 },
                 });

            var core = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);
            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Single(core.SubjectEntries);
            Assert.Single(add.SubjectEntries);

            Assert.Equal("Mathematics", core.SubjectEntries.Single().SubEntCore_Sub_Est_Current_Num);
            Assert.Equal("History", add.SubjectEntries.Single().SubEntAdd_Sub_Est_Current_Num);
        }

        [Fact]
        public async Task OrdersBySubjectThenQualification()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
             new() { pupil_count = 100, subject = "Zoology", qualification_type = "GCSE", number_achieving = 1 },
             new() { pupil_count = 100, subject = "Art", qualification_type = "BTEC", number_achieving = 1 },
             new() { pupil_count = 100, subject = "Art", qualification_type = "GCSE", number_achieving = 1 },
                 });

            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Equal(3, add.SubjectEntries.Count);

            Assert.Collection(add.SubjectEntries,
                first =>
                {
                    Assert.Equal("Art", first.SubEntAdd_Sub_Est_Current_Num);
                    Assert.Equal("BTEC", first.SubEntAdd_Qual_Est_Current_Num);
                },
                second =>
                {
                    Assert.Equal("Art", second.SubEntAdd_Sub_Est_Current_Num);
                    Assert.Equal("GCSE", second.SubEntAdd_Qual_Est_Current_Num);
                },
                third =>
                {
                    Assert.Equal("Zoology", third.SubEntAdd_Sub_Est_Current_Num);
                    Assert.Equal("GCSE", third.SubEntAdd_Qual_Est_Current_Num);
                });
        }

        private static void AssertPct(double expected, double? actual, double tolerance = 0.0001)
        {
            Assert.True(actual.HasValue, "Expected a percentage value but was null.");
            Assert.InRange(actual.Value, expected - tolerance, expected + tolerance);
        }

    }
}
