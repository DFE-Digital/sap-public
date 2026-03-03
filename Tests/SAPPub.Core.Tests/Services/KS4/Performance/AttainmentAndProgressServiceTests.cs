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

        _service = new AttainmentAndProgressService(
            _mockEstablishmentService.Object,
            _mockEstablishmentPerformanceService.Object,
            _mockLAPerformanceService.Object);
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
        };

        var lAPerformance = new LAPerformance
        {
            Id = fakeEstablishment.LAId,
            Prog8_Avg_LA_Previous_Num = 5,
            Prog8_Avg_LA_Previous2_Num = 3
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
        }
        else if (academicYearSelection == AcademicYearSelection.Previous2)
        {
            Assert.Equal(establishmentPerformance.Prog8_Tot_Est_Previous2_Num, result.EstablishmentProgress8Score);
            Assert.Equal(lAPerformance.Prog8_Avg_LA_Previous2_Num, result.LocalAuthorityProgress8Score);
        }
        else
        {
            Assert.Null(result.EstablishmentProgress8Score);
            Assert.Null(result.LocalAuthorityProgress8Score);            
        }
    }
}
