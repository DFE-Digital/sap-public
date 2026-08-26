using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS2MeetingOrExceedingStandardsServiceTests
{
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IKS2PerformanceRepository> _ks2PerformanceRepository = new();
    private readonly KS2MeetingOrExceedingStandardsService _service;

    public KS2MeetingOrExceedingStandardsServiceTests()
    {
        _service = new KS2MeetingOrExceedingStandardsService(_establishmentService.Object, _ks2PerformanceRepository.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetMeetingOrExceedingStandardsPercentages_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetMeetingOrExceedingStandardsPercentages(urn!, CancellationToken.None));

        Assert.Equal("urn", ex.ParamName);
    }


    [Fact]
    public async Task GetMeetingOrExceedingStandardsPercentages_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetMeetingOrExceedingStandardsPercentages(It.IsAny<string>(), cts.Token));

        // Assert
        _establishmentService.Verify(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetLaPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetMeetingOrExceedingStandardsPercentages_ReturnsAllDataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        _establishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance 
            { 
                PTRWM_EXP_Est_Current_Pct_Coded = GetCodedDouble(1), 
                PTRWM_EXP_Est_Previous_Pct_Coded = GetCodedDouble(2),
                PTRWM_EXP_Est_Previous2_Pct_Coded = GetCodedDouble(3),
                PTRWM_HIGH_Est_Current_Pct_Coded = GetCodedDouble(4),
                PTRWM_HIGH_Est_Previous_Pct_Coded = GetCodedDouble(5),
                PTRWM_HIGH_Est_Previous2_Pct_Coded = GetCodedDouble(6),
                PTRWM_EXP_G_Est_Current_Pct_Coded = GetCodedDouble(19),
                PTRWM_HIGH_G_Est_Current_Pct_Coded = GetCodedDouble(20),
                PTRWM_EXP_B_Est_Current_Pct_Coded = GetCodedDouble(21),
                PTRWM_HIGH_B_Est_Current_Pct_Coded = GetCodedDouble(22),
                PTRWM_EXP_EAL_Est_Current_Pct_Coded = GetCodedDouble(23),
                PTRWM_HIGH_EAL_Est_Current_Pct_Coded = GetCodedDouble(24),
                PTRWM_EXP_MOBN_Est_Current_Pct_Coded = GetCodedDouble(25),
                PTRWM_HIGH_MOBN_Est_Current_Pct_Coded = GetCodedDouble(26),
                PTRWM_EXP_FSM6CLA1A_Est_Current_Pct_Coded = GetCodedDouble(27),
                PTRWM_HIGH_FSM6CLA1A_Est_Current_Pct_Coded = GetCodedDouble(28)
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance 
            {
                PTRWM_EXP_LA_Current_Pct_Coded = GetCodedDouble(7),
                PTRWM_EXP_LA_Previous_Pct_Coded = GetCodedDouble(8),
                PTRWM_EXP_LA_Previous2_Pct_Coded = GetCodedDouble(9),
                PTRWM_HIGH_LA_Current_Pct_Coded = GetCodedDouble(10),
                PTRWM_HIGH_LA_Previous_Pct_Coded = GetCodedDouble(11),
                PTRWM_HIGH_LA_Previous2_Pct_Coded = GetCodedDouble(12),
                PTRWM_EXP_FSM6CLA1A_LA_Current_Pct_Coded = GetCodedDouble(29),
                PTRWM_HIGH_FSM6CLA1A_LA_Current_Pct_Coded = GetCodedDouble(30),
                PTRWM_EXP_NOTFSM6CLA1A_LA_Current_Pct_Coded = GetCodedDouble(31),
                PTRWM_HIGH_NOTFSM6CLA1A_LA_Current_Pct_Coded = GetCodedDouble(32)
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance 
            {
                PTRWM_EXP_Eng_Current_Pct_Coded = GetCodedDouble(13),
                PTRWM_EXP_Eng_Previous_Pct_Coded = GetCodedDouble(14),
                PTRWM_EXP_Eng_Previous2_Pct_Coded = GetCodedDouble(15),
                PTRWM_HIGH_Eng_Current_Pct_Coded = GetCodedDouble(16),
                PTRWM_HIGH_Eng_Previous_Pct_Coded = GetCodedDouble(17),
                PTRWM_HIGH_Eng_Previous2_Pct_Coded = GetCodedDouble(18),
                PTRWM_EXP_FSM6CLA1A_Eng_Current_Pct_Coded = GetCodedDouble(33),
                PTRWM_HIGH_FSM6CLA1A_Eng_Current_Pct_Coded = GetCodedDouble(34),
                PTRWM_EXP_NOTFSM6CLA1A_Eng_Current_Pct_Coded = GetCodedDouble(35),
                PTRWM_HIGH_NOTFSM6CLA1A_Eng_Current_Pct_Coded = GetCodedDouble(36)
            });

        // Act
        var result = await _service.GetMeetingOrExceedingStandardsPercentages(urn, CancellationToken.None);

        // Assert
        Assert.Equal(GetCodedDouble(1), result.EstablishmentPercentageMeetingOrExceeding.CurrentYear);
        Assert.Equal(GetCodedDouble(2), result.EstablishmentPercentageMeetingOrExceeding.PreviousYear);
        Assert.Equal(GetCodedDouble(3), result.EstablishmentPercentageMeetingOrExceeding.TwoYearsAgo);
        Assert.Equal(GetCodedDouble(7), result.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear);
        Assert.Equal(GetCodedDouble(8), result.LocalAuthorityPercentageMeetingOrExceeding.PreviousYear);
        Assert.Equal(GetCodedDouble(9), result.LocalAuthorityPercentageMeetingOrExceeding.TwoYearsAgo); 
        Assert.Equal(GetCodedDouble(13), result.EnglandPercentageMeetingOrExceeding.CurrentYear);
        Assert.Equal(GetCodedDouble(14), result.EnglandPercentageMeetingOrExceeding.PreviousYear);
        Assert.Equal(GetCodedDouble(15), result.EnglandPercentageMeetingOrExceeding.TwoYearsAgo);

        Assert.Equal(GetCodedDouble(4), result.EstablishmentPercentageExceeding.CurrentYear);
        Assert.Equal(GetCodedDouble(5), result.EstablishmentPercentageExceeding.PreviousYear);
        Assert.Equal(GetCodedDouble(6), result.EstablishmentPercentageExceeding.TwoYearsAgo);
        Assert.Equal(GetCodedDouble(10), result.LocalAuthorityPercentageExceeding.CurrentYear);
        Assert.Equal(GetCodedDouble(11), result.LocalAuthorityPercentageExceeding.PreviousYear);
        Assert.Equal(GetCodedDouble(12), result.LocalAuthorityPercentageExceeding.TwoYearsAgo);
        Assert.Equal(GetCodedDouble(16), result.EnglandPercentageExceeding.CurrentYear);
        Assert.Equal(GetCodedDouble(17), result.EnglandPercentageExceeding.PreviousYear);
        Assert.Equal(GetCodedDouble(18), result.EnglandPercentageExceeding.TwoYearsAgo);

        Assert.Equal(GetCodedDouble(19), result.GirlsMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(20), result.GirlsExceedingExpectedStandard);
        Assert.Equal(GetCodedDouble(21), result.BoysMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(22), result.BoysExceedingExpectedStandard);
        Assert.Equal(GetCodedDouble(1), result.AllPupilsMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(4), result.AllPupilsExceedingExpectedStandard);

        Assert.Equal(GetCodedDouble(23), result.EALMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(24), result.EALExceedingExpectedStandard);

        Assert.Equal(GetCodedDouble(25), result.NonMobileMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(26), result.NonMobileExceedingExpectedStandard);
        Assert.Equal(GetCodedDouble(27), result.EstablishmentDisadvantagedMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(28), result.EstablishmentDisadvantagedExceedingExpectedStandard);
        
        Assert.Equal(GetCodedDouble(29), result.LocalAuthorityDisadvantagedMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(30), result.LocalAuthorityDisadvantagedExceedingExpectedStandard);
        Assert.Equal(GetCodedDouble(31), result.LocalAuthorityNonDisadvantagedMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(32), result.LocalAuthorityNonDisadvantagedExceedingExpectedStandard);

        Assert.Equal(GetCodedDouble(33), result.EnglandDisadvantagedMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(34), result.EnglandDisadvantagedExceedingExpectedStandard);
        Assert.Equal(GetCodedDouble(35), result.EnglandNonDisadvantagedMeetingExpectedStandard);
        Assert.Equal(GetCodedDouble(36), result.EnglandNonDisadvantagedExceedingExpectedStandard);


    }

    private static CodedDouble GetCodedDouble(double val)
    {
        return new CodedDouble(val, string.Empty, val.ToString());
    }
}
