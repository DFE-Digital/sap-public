using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Core.Tests.Services.KS4.Destinations
{
    public class EnglishAndMathsResultsServiceTests
    {
        private readonly Mock<IEstablishmentService> _mockEstablishmentService;
        private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
        private readonly Mock<ILAPerformanceService> _mockLAPerformanceService;
        private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
        private readonly EnglishAndMathsResultsService _service;

        private readonly Establishment fakeEstablishment = new()
        {
            URN = "123456",
            EstablishmentName = "Test Establishment",
            PhaseOfEducationName = "Secondary School",
            LAName = "Council",
            LAId = "E09000001"
        };

        public EnglishAndMathsResultsServiceTests()
        {
            _mockEstablishmentService = new();
            _mockEstablishmentPerformanceService = new();
            _mockLAPerformanceService = new();
            _mockEnglandPerformanceService = new();

            _service = new EnglishAndMathsResultsService(
                _mockEstablishmentService.Object,
                _mockEstablishmentPerformanceService.Object,
                _mockLAPerformanceService.Object,
                _mockEnglandPerformanceService.Object);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async Task GetEnglishAndMathsResultsAsync_ShouldReturnData(int selectedGrade)
        {
            // Arrange
            var establishmentPerformance = new EstablishmentPerformance
            {
                Id = fakeEstablishment.URN,
                EngMaths49_Tot_Est_Current_Pct = 100,
                EngMaths59_Tot_Est_Current_Pct = 90,
                EngMaths49_Boy_Est_Current_Pct = 80,
                EngMaths59_Boy_Est_Current_Pct = 70,
                EngMaths49_Grl_Est_Current_Pct = 90,
                EngMaths59_Grl_Est_Current_Pct = 80,
                EngMaths49_Tot_Est_Previous_Pct = 70,
                EngMaths59_Tot_Est_Previous_Pct = 80,
                EngMaths49_Tot_Est_Previous2_Pct = 60,
                EngMaths59_Tot_Est_Previous2_Pct = 90,
            };

            var lAPerformance = new LAPerformance
            {
                Id = fakeEstablishment.LAId,
                EngMaths49_Tot_LA_Current_Pct = 50,
                EngMaths59_Tot_LA_Current_Pct = 80,
                EngMaths49_Boy_LA_Current_Pct = 70,
                EngMaths59_Boy_LA_Current_Pct = 80,
                EngMaths49_Grl_LA_Current_Pct = 80,
                EngMaths59_Grl_LA_Current_Pct = 60,
                EngMaths49_Tot_LA_Previous_Pct = 80,
                EngMaths59_Tot_LA_Previous_Pct = 50,
                EngMaths49_Tot_LA_Previous2_Pct = 80,
                EngMaths59_Tot_LA_Previous2_Pct = 70,
            };

            var englandPerformance = new EnglandPerformance
            {
                Id = "National",
                EngMaths49_Tot_Eng_Current_Pct = 70,
                EngMaths59_Tot_Eng_Current_Pct = 90,
                EngMaths49_Boy_Eng_Current_Pct = 90,
                EngMaths59_Boy_Eng_Current_Pct = 70,
                EngMaths49_Grl_Eng_Current_Pct = 50,
                EngMaths59_Grl_Eng_Current_Pct = 70,
                EngMaths49_Tot_Eng_Previous_Pct = 70,
                EngMaths59_Tot_Eng_Previous_Pct = 90,
                EngMaths49_Tot_Eng_Previous2_Pct = 70,
                EngMaths59_Tot_Eng_Previous2_Pct = 90,
            };

            _mockEstablishmentService
                .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeEstablishment);

            _mockEstablishmentPerformanceService
                .Setup(r => r.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(establishmentPerformance);

            _mockLAPerformanceService
                .Setup(r => r.GetLAPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(lAPerformance);

            _mockEnglandPerformanceService
                .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(englandPerformance);

            // Act
            var result = await _service.GetEnglishAndMathsResultsAsync(fakeEstablishment.URN, selectedGrade, CancellationToken.None);

            // Assert (common)
            Assert.NotNull(result);
            Assert.Equal(fakeEstablishment.URN, result.Urn);
            Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
            Assert.Equal(fakeEstablishment.LAName, result.LAName);

            if (selectedGrade == 4)
            {
                Assert.Equal(establishmentPerformance.EngMaths49_Tot_Est_Current_Pct, result.EstablishmentAll.CurrentYear);
                Assert.Equal(establishmentPerformance.EngMaths49_Tot_Est_Previous_Pct, result.EstablishmentAll.PreviousYear);
                Assert.Equal(establishmentPerformance.EngMaths49_Tot_Est_Previous2_Pct, result.EstablishmentAll.TwoYearsAgo);

                Assert.Equal(lAPerformance.EngMaths49_Tot_LA_Current_Pct, result.LocalAuthorityAll.CurrentYear);
                Assert.Equal(lAPerformance.EngMaths49_Tot_LA_Previous_Pct, result.LocalAuthorityAll.PreviousYear);
                Assert.Equal(lAPerformance.EngMaths49_Tot_LA_Previous2_Pct, result.LocalAuthorityAll.TwoYearsAgo);

                Assert.Equal(englandPerformance.EngMaths49_Tot_Eng_Current_Pct, result.EnglandAll.CurrentYear);
                Assert.Equal(englandPerformance.EngMaths49_Tot_Eng_Previous_Pct, result.EnglandAll.PreviousYear);
                Assert.Equal(englandPerformance.EngMaths49_Tot_Eng_Previous2_Pct, result.EnglandAll.TwoYearsAgo);

                Assert.Equal(establishmentPerformance.EngMaths49_Boy_Est_Current_Pct, result.EstablishmentBoys.CurrentYear);
                Assert.Equal(establishmentPerformance.EngMaths49_Grl_Est_Current_Pct, result.EstablishmentGirls.CurrentYear);

                Assert.Equal(lAPerformance.EngMaths49_Boy_LA_Current_Pct, result.LocalAuthorityBoys.CurrentYear);
                Assert.Equal(lAPerformance.EngMaths49_Grl_LA_Current_Pct, result.LocalAuthorityGirls.CurrentYear);

                Assert.Equal(englandPerformance.EngMaths49_Boy_Eng_Current_Pct, result.EnglandBoys.CurrentYear);
                Assert.Equal(englandPerformance.EngMaths49_Grl_Eng_Current_Pct, result.EnglandGirls.CurrentYear);
            }
            else if (selectedGrade == 5)
            {
                Assert.Equal(establishmentPerformance.EngMaths59_Tot_Est_Current_Pct, result.EstablishmentAll.CurrentYear);
                Assert.Equal(establishmentPerformance.EngMaths59_Tot_Est_Previous_Pct, result.EstablishmentAll.PreviousYear);
                Assert.Equal(establishmentPerformance.EngMaths59_Tot_Est_Previous2_Pct, result.EstablishmentAll.TwoYearsAgo);

                Assert.Equal(lAPerformance.EngMaths59_Tot_LA_Current_Pct, result.LocalAuthorityAll.CurrentYear);
                Assert.Equal(lAPerformance.EngMaths59_Tot_LA_Previous_Pct, result.LocalAuthorityAll.PreviousYear);
                Assert.Equal(lAPerformance.EngMaths59_Tot_LA_Previous2_Pct, result.LocalAuthorityAll.TwoYearsAgo);

                Assert.Equal(englandPerformance.EngMaths59_Tot_Eng_Current_Pct, result.EnglandAll.CurrentYear);
                Assert.Equal(englandPerformance.EngMaths59_Tot_Eng_Previous_Pct, result.EnglandAll.PreviousYear);
                Assert.Equal(englandPerformance.EngMaths59_Tot_Eng_Previous2_Pct, result.EnglandAll.TwoYearsAgo);

                Assert.Equal(establishmentPerformance.EngMaths59_Boy_Est_Current_Pct, result.EstablishmentBoys.CurrentYear);
                Assert.Equal(establishmentPerformance.EngMaths59_Grl_Est_Current_Pct, result.EstablishmentGirls.CurrentYear);

                Assert.Equal(lAPerformance.EngMaths59_Boy_LA_Current_Pct, result.LocalAuthorityBoys.CurrentYear);
                Assert.Equal(lAPerformance.EngMaths59_Grl_LA_Current_Pct, result.LocalAuthorityGirls.CurrentYear);

                Assert.Equal(englandPerformance.EngMaths59_Boy_Eng_Current_Pct, result.EnglandBoys.CurrentYear);
                Assert.Equal(englandPerformance.EngMaths59_Grl_Eng_Current_Pct, result.EnglandGirls.CurrentYear);
            }
            else
            {
                Assert.Null(result.EstablishmentAll.CurrentYear);
                Assert.Null(result.EstablishmentAll.PreviousYear);
                Assert.Null(result.EstablishmentAll.TwoYearsAgo);

                Assert.Null(result.LocalAuthorityAll.CurrentYear);
                Assert.Null(result.LocalAuthorityAll.PreviousYear);
                Assert.Null(result.LocalAuthorityAll.TwoYearsAgo);

                Assert.Null(result.EnglandAll.CurrentYear);
                Assert.Null(result.EnglandAll.PreviousYear);
                Assert.Null(result.EnglandAll.TwoYearsAgo);

                Assert.Null(result.EstablishmentBoys.CurrentYear);
                Assert.Null(result.EstablishmentGirls.CurrentYear);

                Assert.Null(result.LocalAuthorityBoys.CurrentYear);
                Assert.Null(result.LocalAuthorityGirls.CurrentYear);

                Assert.Null(result.EnglandBoys.CurrentYear);
                Assert.Null(result.EnglandGirls.CurrentYear);
            }
        }

        [Fact]
        public async Task GetEnglishAndMathsResultsAsync_ShouldReturnEmptyModel_WhenEstablishmentNotFound()
        {
            // Arrange
            var urn = "99999";
            _mockEstablishmentService
                .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Establishment()); // not found

            // Act
            var result = await _service.GetEnglishAndMathsResultsAsync(urn, 4, CancellationToken.None);

            // Assert - required members are set, but values are empty
            Assert.NotNull(result);
            Assert.Equal(urn, result.Urn);
            Assert.Equal(string.Empty, result.SchoolName);
            Assert.Null(result.LAName);

            Assert.Null(result.EstablishmentAll.CurrentYear);
            Assert.Null(result.LocalAuthorityAll.CurrentYear);
            Assert.Null(result.EnglandAll.CurrentYear);
        }
    }
}
