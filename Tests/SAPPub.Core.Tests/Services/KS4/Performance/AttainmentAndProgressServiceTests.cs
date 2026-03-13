using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;

namespace SAPPub.Core.Tests.Services.KS4.Performance;

public class AttainmentAndProgressServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
    private readonly Mock<ILAPerformanceService> _mockLAPerformanceService;
    private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
    private readonly AttainmentAndProgressService _service;

    private readonly Establishment fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public AttainmentAndProgressServiceTests()
    {
        _mockEstablishmentService = new();
        _mockEstablishmentPerformanceService = new();
        _mockLAPerformanceService = new();
        _mockEnglandPerformanceService = new();

        _service = new AttainmentAndProgressService(
            _mockEstablishmentService.Object,
            _mockEstablishmentPerformanceService.Object,
            _mockLAPerformanceService.Object,
            _mockEnglandPerformanceService.Object);
    }

    [Fact]
    public async Task GetAttainmentAndProgressAsync_ShouldReturnEmptyModel_WhenEstablishmentNotFound()
    {
        // Arrange
        var urn = "99999";
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Establishment()); // not found

        // Act
        var result = await _service.GetAttainmentAndProgressAsync(urn, AcademicYearSelection.Previous, CancellationToken.None);

        // Assert - required members are set, but values are empty
        Assert.NotNull(result);
        Assert.Equal(urn, result.Urn);
        Assert.Null(result.SchoolName);

        Assert.Null(result.EstablishmentProgress8Score);
        Assert.Null(result.LocalAuthorityProgress8Score);
        Assert.Null(result.EstablishmentProgress8TotalPupils);
        Assert.Null(result.EstablishmentTotalPupils);
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current)]
    [InlineData(AcademicYearSelection.Previous)]
    [InlineData(AcademicYearSelection.Previous2)]
    public async Task AttainmentAndProgressAsync_ShouldReturnData(AcademicYearSelection academicYearSelection)
    {
        // Arrange
        var establishmentPerformance = new EstablishmentPerformance
        {
            Id = fakeEstablishment.URN,
            Prog8_Tot_Est_Previous_Num = 0.3,
            Prog8_Tot_Est_Previous2_Num = 2,
            Attainment8_Tot_Est_Current_Num = 40,
            Attainment8_Tot_Est_Previous_Num = 50,
            Attainment8_Tot_Est_Previous2_Num = 55,
            Prog8_TotPup_Est_Previous_Num = 95,
            Prog8_TotPup_Est_Previous2_Num = 65,
            Pup_Tot_Est_Previous_Num = 100,
            Pup_Tot_Est_Previous2_Num = 90
        };

        var lAPerformance = new LAPerformance
        {
            Id = fakeEstablishment.LAId,
            Prog8_Avg_LA_Previous_Num = 5,
            Prog8_Avg_LA_Previous2_Num = 3,
            Attainment8_Tot_LA_Current_Num = 60,
            Attainment8_Tot_LA_Previous_Num = 50,
            Attainment8_Tot_LA_Previous2_Num = 70,
        };

        var englandPerformance = new EnglandPerformance
        {
            Id = fakeEstablishment.LAId,
            Attainment8_Tot_Eng_Current_Num = 60,
            Attainment8_Tot_Eng_Previous_Num = 70,
            Attainment8_Tot_Eng_Previous2_Num = 40,
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
        var result = await _service.GetAttainmentAndProgressAsync(fakeEstablishment.URN, academicYearSelection, CancellationToken.None);

        // Assert (common)
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        if (academicYearSelection == AcademicYearSelection.Previous)
        {
            Assert.Equal(establishmentPerformance.Prog8_Tot_Est_Previous_Num, result.EstablishmentProgress8Score);
            Assert.Equal(lAPerformance.Prog8_Avg_LA_Previous_Num, result.LocalAuthorityProgress8Score);

            Assert.Equal(establishmentPerformance.Attainment8_Tot_Est_Previous_Num, result.EstablishmentAttainment8Score);
            Assert.Equal(lAPerformance.Attainment8_Tot_LA_Previous_Num, result.LocalAuthorityAttainment8Score);
            Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Previous_Num, result.EnglandAttainment8Score);

            Assert.Equal(establishmentPerformance.Prog8_TotPup_Est_Previous_Num, result.EstablishmentProgress8TotalPupils);
            Assert.Equal(establishmentPerformance.Pup_Tot_Est_Previous_Num, result.EstablishmentTotalPupils);
        }
        else if (academicYearSelection == AcademicYearSelection.Previous2)
        {
            Assert.Equal(establishmentPerformance.Prog8_Tot_Est_Previous2_Num, result.EstablishmentProgress8Score);
            Assert.Equal(lAPerformance.Prog8_Avg_LA_Previous2_Num, result.LocalAuthorityProgress8Score);

            Assert.Equal(establishmentPerformance.Attainment8_Tot_Est_Previous2_Num, result.EstablishmentAttainment8Score);
            Assert.Equal(lAPerformance.Attainment8_Tot_LA_Previous2_Num, result.LocalAuthorityAttainment8Score);
            Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Previous2_Num, result.EnglandAttainment8Score);

            Assert.Equal(establishmentPerformance.Prog8_TotPup_Est_Previous2_Num, result.EstablishmentProgress8TotalPupils);
            Assert.Equal(establishmentPerformance.Pup_Tot_Est_Previous2_Num, result.EstablishmentTotalPupils);
        }
        else
        {
            Assert.Null(result.EstablishmentProgress8Score);
            Assert.Null(result.LocalAuthorityProgress8Score);

            Assert.Equal(establishmentPerformance.Attainment8_Tot_Est_Current_Num, result.EstablishmentAttainment8Score);
            Assert.Equal(lAPerformance.Attainment8_Tot_LA_Current_Num, result.LocalAuthorityAttainment8Score);
            Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Current_Num, result.EnglandAttainment8Score);

            Assert.Null(result.EstablishmentProgress8TotalPupils);
            Assert.Null(result.EstablishmentTotalPupils);
        }
    }
}
