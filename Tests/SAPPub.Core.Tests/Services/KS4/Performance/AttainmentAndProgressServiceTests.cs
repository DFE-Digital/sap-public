using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;

namespace SAPPub.Core.Tests.Services.KS4.Performance;

public class AttainmentAndProgressServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
    private readonly Mock<ILAPerformanceService> _mockLAPerformanceService;
    private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
    private readonly AttainmentAndProgressService _service;

    private readonly EstablishmentMinimumServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
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
            .Setup(r => r.GetEstablishmentMinimumAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel()); // not found

        // Act
        var result = await _service.GetAttainmentAndProgressAsync(urn, CancellationToken.None);

        // Assert - required members are set, but values are empty
        Assert.NotNull(result);
        Assert.Equal(urn, result.Urn);
        Assert.Null(result.SchoolName);

        Assert.False(result.EstablishmentProgress8Score.CurrentYear.HasValue);
        Assert.False(result.LocalAuthorityProgress8Score.CurrentYear.HasValue);
        Assert.False(result.EstablishmentProgress8TotalPupils.CurrentYear.HasValue);
        Assert.False(result.EstablishmentTotalPupils.CurrentYear.HasValue);
    }

    [Fact]
    public async Task AttainmentAndProgressAsync_ShouldReturnData()
    {
        // Arrange
        //var establishmentPerformance = new EstablishmentPerformance
        //{
        //    Id = fakeEstablishment.URN,
        //    Prog8_Tot_Est_Previous_Num = 0.3,
        //    Prog8_Tot_Est_Previous2_Num = 2,
        //    Attainment8_Tot_Est_Current_Num = 40,
        //    Attainment8_Tot_Est_Previous_Num = 50,
        //    Attainment8_Tot_Est_Previous2_Num = 55,
        //    Prog8_TotPup_Est_Previous_Num = 95,
        //    Prog8_TotPup_Est_Previous2_Num = 65,
        //    Pup_Tot_Est_Previous_Num = 100,
        //    Pup_Tot_Est_Previous2_Num = 90
        //};
        var establishmentPerformance = new EstablishmentPerformanceBuilder()
            .WithDisadvantagedMeasures()
            .WithAdditionalMeasures()
            .WithAttainment8()
            .WithProgress8()
            .Build();

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
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        var result = await _service.GetAttainmentAndProgressAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert (common)
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);

        // current year data
        Assert.Equal(establishmentPerformance.Prog8_Tot_Est_Current_Num_Coded, result.EstablishmentProgress8Score.CurrentYear);
        Assert.Equal(lAPerformance.Prog8_Avg_LA_Current_Num_Coded, result.LocalAuthorityProgress8Score.CurrentYear);

        Assert.Equal(establishmentPerformance.Attainment8_Tot_Est_Current_Num_Coded, result.EstablishmentAttainment8Score.CurrentYear);
        Assert.Equal(lAPerformance.Attainment8_Tot_LA_Current_Num_Coded, result.LocalAuthorityAttainment8Score.CurrentYear);
        Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Current_Num_Coded, result.EnglandAttainment8Score.CurrentYear);

        Assert.Equal(establishmentPerformance.Prog8_TotPup_Est_Current_Num_Coded, result.EstablishmentProgress8TotalPupils.CurrentYear);
        Assert.Equal(establishmentPerformance.Pup_Tot_Est_Current_Num_Coded, result.EstablishmentTotalPupils.CurrentYear);

        Assert.False(result.EstablishmentProgress8Score.CurrentYear.HasValue);
        Assert.False(result.LocalAuthorityProgress8Score.CurrentYear.HasValue);
        Assert.False(result.EstablishmentProgress8TotalPupils.CurrentYear.HasValue);

        // previous year data
        Assert.Equal(establishmentPerformance.Prog8_Tot_Est_Previous_Num_Coded, result.EstablishmentProgress8Score.PreviousYear);
        Assert.Equal(lAPerformance.Prog8_Avg_LA_Previous_Num_Coded, result.LocalAuthorityProgress8Score.PreviousYear);

        Assert.Equal(establishmentPerformance.Attainment8_Tot_Est_Previous_Num_Coded, result.EstablishmentAttainment8Score.PreviousYear);
        Assert.Equal(lAPerformance.Attainment8_Tot_LA_Previous_Num_Coded, result.LocalAuthorityAttainment8Score.PreviousYear);
        Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Previous_Num_Coded, result.EnglandAttainment8Score.PreviousYear);

        Assert.Equal(establishmentPerformance.Prog8_TotPup_Est_Previous_Num_Coded, result.EstablishmentProgress8TotalPupils.PreviousYear);
        Assert.Equal(establishmentPerformance.Pup_Tot_Est_Previous_Num_Coded, result.EstablishmentTotalPupils.PreviousYear);

        // previous 2 year data
        Assert.Equal(establishmentPerformance.Prog8_Tot_Est_Previous2_Num_Coded, result.EstablishmentProgress8Score.TwoYearsAgo);
        Assert.Equal(lAPerformance.Prog8_Avg_LA_Previous2_Num_Coded, result.LocalAuthorityProgress8Score.TwoYearsAgo);

        Assert.Equal(establishmentPerformance.Attainment8_Tot_Est_Previous2_Num_Coded, result.EstablishmentAttainment8Score.TwoYearsAgo);
        Assert.Equal(lAPerformance.Attainment8_Tot_LA_Previous2_Num_Coded, result.LocalAuthorityAttainment8Score.TwoYearsAgo);
        Assert.Equal(englandPerformance.Attainment8_Tot_Eng_Previous2_Num_Coded, result.EnglandAttainment8Score.TwoYearsAgo);

        Assert.Equal(establishmentPerformance.Prog8_TotPup_Est_Previous2_Num_Coded, result.EstablishmentProgress8TotalPupils.TwoYearsAgo);
        Assert.Equal(establishmentPerformance.Pup_Tot_Est_Previous2_Num_Coded, result.EstablishmentTotalPupils.TwoYearsAgo);
    }

    [Fact]
    public async Task AttainmentAndProgressAsync_ShouldReturnDisadvantagedData()
    {
        // Arrange
        var establishmentPerformance = new EstablishmentPerformanceBuilder()
            .WithUrn(fakeEstablishment.URN)
            .WithDisadvantagedMeasures()
            .Build();

        var lAPerformance = new LaPerformanceBuilder()
            .WithId(fakeEstablishment.LAId)
            .WithDisadvantagedMeasures()
            .Build();

        var englandPerformance = new EnglandPerformanceBuilder()
            .WithDisadvantagedMeasures()
            .Build();

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockEstablishmentPerformanceService
            .Setup(r => r.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentPerformance);

        _mockLAPerformanceService
            .Setup(r => r.GetLAPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(lAPerformance);

        _mockEnglandPerformanceService
            .Setup(r => r.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetAttainmentAndProgressAsync(fakeEstablishment.URN, CancellationToken.None);
        Assert.Equal(lAPerformance.Attainment8_NDi_LA_Current_Num_Coded, result.LocalAuthorityAttainment8NonDisadvantagedScore);
        Assert.Equal(englandPerformance.Attainment8_NDi_Eng_Current_Num_Coded, result.EnglandAttainment8NonDisadvantagedScore);

        Assert.Equal(establishmentPerformance.Attainment8_Dis_Est_Current_Num_Coded, result.EstablishmentAttainment8DisadvantagedScore.CurrentYear);
        Assert.Equal(lAPerformance.Attainment8_Dis_LA_Current_Num_Coded, result.LocalAuthorityAttainment8DisadvantagedScore.CurrentYear);
        Assert.Equal(englandPerformance.Attainment8_Dis_Eng_Current_Num_Coded, result.EnglandAttainment8DisadvantagedScore.CurrentYear);

        Assert.Equal(establishmentPerformance.Attainment8_Dis_Est_Previous_Num_Coded, result.EstablishmentAttainment8DisadvantagedScore.PreviousYear);
        Assert.Equal(lAPerformance.Attainment8_Dis_LA_Previous_Num_Coded, result.LocalAuthorityAttainment8DisadvantagedScore.PreviousYear);
        Assert.Equal(englandPerformance.Attainment8_Dis_Eng_Previous_Num_Coded, result.EnglandAttainment8DisadvantagedScore.PreviousYear);

        Assert.Equal(establishmentPerformance.Attainment8_Dis_Est_Previous2_Num_Coded, result.EstablishmentAttainment8DisadvantagedScore.TwoYearsAgo);
        Assert.Equal(lAPerformance.Attainment8_Dis_LA_Previous2_Num_Coded, result.LocalAuthorityAttainment8DisadvantagedScore.TwoYearsAgo);
        Assert.Equal(englandPerformance.Attainment8_Dis_Eng_Previous2_Num_Coded, result.EnglandAttainment8DisadvantagedScore.TwoYearsAgo);
    }
}
