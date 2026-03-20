using Moq;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Repositories.KS4.SubjectEntries;

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

        [Theory]
        [InlineData(200, 200, 200)]
        [InlineData(200, 100, 100)]
        [InlineData(200, 20, 20)]
        [InlineData(200, 10, 10)]
        [InlineData(200, 5, 5)]
        // this test excludes the pupil count because we've moved from percentages to just number of pupils
        // we did that because some subjects are lumped together and the pupil count is duplicated across them, so the percentage would be misleading
        public async Task UsesTotalExamEntriesRow(int pupilCount, int numberAchieving, int expected)
        {
            var urn = "123";
            _repo.Setup(r => r.ReadManyAsync(
                It.Is<object>(o => o.GetType().GetProperty("Urn")!.GetValue(o)!.ToString() == urn),
                It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() {
                         pupil_count = pupilCount,
                         subject = "Biology",
                         subject_discount_group = "Biology",
                         qualification_type = "GCSE",
                         grade = "Total exam entries",
                         number_achieving = numberAchieving },
                     new()
                     {
                         pupil_count = pupilCount,
                         subject = "Biology",
                         subject_discount_group = "Biology",
                         qualification_type = "GCSE",
                         grade = "4",
                         number_achieving = 1 },
                 });

            var result = await _sut.GetCoreSubjectEntriesByUrnAsync(urn, CancellationToken.None);

            var entry = Assert.Single(result.SubjectEntries);
            AssertPct(expected, entry.SubEntCore_Entr_Est_Current_Num);
        }

        [Theory]
        [InlineData("  biology ", "biology", "biology")]
        [InlineData("  biology ", "  biology  ", "biology")]
        [InlineData("  mathematics ", "maths (Further)", "Mathematics (Further)")]
        [InlineData("  mathematics ", "   maths (Further)", "Mathematics (Further)")]
        public async Task TrimsSubject(string subject, string subjectDetail, string expected)
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = subject, subject_discount_group = subjectDetail, qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
                 });

            var result = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);

            var entry = Assert.Single(result.SubjectEntries);
            Assert.Equal(expected, entry.SubEntCore_Sub_Est_Current_Num); // trimmed subject; case preserved from data
        }

        [Fact]
        public async Task Qualification_UsesTypeFirst_OtherwiseDetailed()
        {
            // subject A uses qualification_type
            // subject B uses qualification_detailed
            // subject C has neither -> null
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "History", subject_discount_group = "History", qualification_type = "GCSE", grade = "Total exam entries", qualification_detailed = "Ignored", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "Geography", subject_discount_group = "Geography", qualification_type = null, grade = "Total exam entries", qualification_detailed = "BTEC", number_achieving = 20 },
                     new() { pupil_count = 100, subject = "Art", subject_discount_group = "Art", qualification_type = null, grade = "Total exam entries", qualification_detailed = "   ", number_achieving = 5 },
                 });

            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Equal(3, add.SubjectEntries.Count);

            var history = add.SubjectEntries.Single(x => x.SubEntAdd_Sub_Est_Current_Num == "History");
            Assert.Equal("GCSE", history.SubEntAdd_Qual_Est_Current_Num);

            var geo = add.SubjectEntries.Single(x => x.SubEntAdd_Sub_Est_Current_Num == "Geography");
            Assert.Equal("BTEC", geo.SubEntAdd_Qual_Est_Current_Num);

            var art = add.SubjectEntries.Single(x => x.SubEntAdd_Sub_Est_Current_Num == "Art");
            Assert.Empty(art.SubEntAdd_Qual_Est_Current_Num!);
        }


        [Fact]
        public async Task SplitsCoreAndAdditionalCorrectly()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "Biology", subject_discount_group = "Biology", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "History", subject_discount_group = "History", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 20 },
                 });

            var core = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);
            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Single(core.SubjectEntries);
            Assert.Single(add.SubjectEntries);

            Assert.Equal("Biology", core.SubjectEntries.Single().SubEntCore_Sub_Est_Current_Num);
            Assert.Equal("History", add.SubjectEntries.Single().SubEntAdd_Sub_Est_Current_Num);
        }

        [Fact]
        public async Task SubjectContainsWhitespace_SplitsCoreAndAdditionalCorrectly()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "  Biology  ", subject_discount_group = "Biology", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
                     new() { pupil_count = 100, subject = "  History  ", subject_discount_group = "History", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 20 },
                 });

            var core = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);
            var add = await _sut.GetAdditionalSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Single(core.SubjectEntries);
            Assert.Single(add.SubjectEntries);

            Assert.Equal("Biology", core.SubjectEntries.Single().SubEntCore_Sub_Est_Current_Num);
            Assert.Equal("History", add.SubjectEntries.Single().SubEntAdd_Sub_Est_Current_Num);
        }

        [Fact]
        public async Task ConvertsMathsToMathematics()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
                     new() { pupil_count = 100, subject = "Mathematics", subject_discount_group = "Additional Maths (Core)", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 10 },
                 });

            var core = await _sut.GetCoreSubjectEntriesByUrnAsync("123", CancellationToken.None);

            Assert.Single(core.SubjectEntries);

            Assert.Equal("Additional Mathematics (Core)", core.SubjectEntries.Single().SubEntCore_Sub_Est_Current_Num);
        }

        [Fact]
        public async Task OrdersBySubjectThenQualification()
        {
            _repo.Setup(r => r.ReadManyAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new List<EstablishmentSubjectEntryRow>
                 {
             new() { pupil_count = 100, subject = "Zoology", subject_discount_group = "Zoology", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 1 },
             new() { pupil_count = 100, subject = "Art", subject_discount_group = "Art", qualification_type = "BTEC", grade = "Total exam entries", number_achieving = 1 },
             new() { pupil_count = 100, subject = "Art", subject_discount_group = "Art", qualification_type = "GCSE", grade = "Total exam entries", number_achieving = 1 },
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
