using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Performance;
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

        var expectedModel = GetKS2MeetingOrExceedingStandardsModel();

        _establishmentService
            .Setup(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance 
            { 
                PTRWM_EXP_Est_Current_Pct_Coded = new CodedDouble(1, string.Empty, "1"), 
                PTRWM_EXP_Est_Previous_Pct_Coded = new CodedDouble(2, string.Empty, "2"),
                PTRWM_EXP_Est_Previous2_Pct_Coded = new CodedDouble(3, string.Empty, "3"),
                PTRWM_HIGH_Est_Current_Pct_Coded = new CodedDouble(4, string.Empty, "4"),
                PTRWM_HIGH_Est_Previous_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                PTRWM_HIGH_Est_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance 
            {
                PTRWM_EXP_LA_Current_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
                PTRWM_EXP_LA_Previous_Pct_Coded = new CodedDouble(8, string.Empty, "8"),
                PTRWM_EXP_LA_Previous2_Pct_Coded = new CodedDouble(9, string.Empty, "9"),
                PTRWM_HIGH_LA_Current_Pct_Coded = new CodedDouble(10, string.Empty, "10"),
                PTRWM_HIGH_LA_Previous_Pct_Coded = new CodedDouble(11, string.Empty, "11"),
                PTRWM_HIGH_LA_Previous2_Pct_Coded = new CodedDouble(12, string.Empty, "12"),
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance 
            {
                PTRWM_EXP_Eng_Current_Pct_Coded = new CodedDouble(13, string.Empty, "13"),
                PTRWM_EXP_Eng_Previous_Pct_Coded = new CodedDouble(14, string.Empty, "14"),
                PTRWM_EXP_Eng_Previous2_Pct_Coded = new CodedDouble(15, string.Empty, "15"),
                PTRWM_HIGH_Eng_Current_Pct_Coded = new CodedDouble(16, string.Empty, "16"),
                PTRWM_HIGH_Eng_Previous_Pct_Coded = new CodedDouble(17, string.Empty, "17"),
                PTRWM_HIGH_Eng_Previous2_Pct_Coded = new CodedDouble(18, string.Empty, "18"),
            });

        // Act
        var result = await _service.GetMeetingOrExceedingStandardsPercentages(urn, CancellationToken.None);

        // Assert
        Assert.Equal(new CodedDouble(1, string.Empty, "1"), result.EstablishmentPercentageMeetingOrExceeding.CurrentYear);
        Assert.Equal(new CodedDouble(2, string.Empty, "2"), result.EstablishmentPercentageMeetingOrExceeding.PreviousYear);
        Assert.Equal(new CodedDouble(3, string.Empty, "3"), result.EstablishmentPercentageMeetingOrExceeding.TwoYearsAgo);
        Assert.Equal(new CodedDouble(7, string.Empty, "7"), result.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear);
        Assert.Equal(new CodedDouble(8, string.Empty, "8"), result.LocalAuthorityPercentageMeetingOrExceeding.PreviousYear);
        Assert.Equal(new CodedDouble(9, string.Empty, "9"), result.LocalAuthorityPercentageMeetingOrExceeding.TwoYearsAgo); 
        Assert.Equal(new CodedDouble(13, string.Empty, "13"), result.EnglandPercentageMeetingOrExceeding.CurrentYear);
        Assert.Equal(new CodedDouble(14, string.Empty, "14"), result.EnglandPercentageMeetingOrExceeding.PreviousYear);
        Assert.Equal(new CodedDouble(15, string.Empty, "15"), result.EnglandPercentageMeetingOrExceeding.TwoYearsAgo);


        Assert.Equal(new CodedDouble(4, string.Empty, "4"), result.EstablishmentPercentageExceeding.CurrentYear);
        Assert.Equal(new CodedDouble(5, string.Empty, "5"), result.EstablishmentPercentageExceeding.PreviousYear);
        Assert.Equal(new CodedDouble(6, string.Empty, "6"), result.EstablishmentPercentageExceeding.TwoYearsAgo);
        Assert.Equal(new CodedDouble(10, string.Empty, "10"), result.LocalAuthorityPercentageExceeding.CurrentYear);
        Assert.Equal(new CodedDouble(11, string.Empty, "11"), result.LocalAuthorityPercentageExceeding.PreviousYear);
        Assert.Equal(new CodedDouble(12, string.Empty, "12"), result.LocalAuthorityPercentageExceeding.TwoYearsAgo);
        Assert.Equal(new CodedDouble(16, string.Empty, "16"), result.EnglandPercentageExceeding.CurrentYear);
        Assert.Equal(new CodedDouble(17, string.Empty, "17"), result.EnglandPercentageExceeding.PreviousYear);
        Assert.Equal(new CodedDouble(18, string.Empty, "18"), result.EnglandPercentageExceeding.TwoYearsAgo);
    }

    private static KS2MeetingOrExceedingStandardsModel GetKS2MeetingOrExceedingStandardsModel()
    {
        return new KS2MeetingOrExceedingStandardsModel
        {
            EstablishmentPercentageMeetingOrExceeding = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3")
            },
            LocalAuthorityPercentageMeetingOrExceeding = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(4, string.Empty, "4"),
                PreviousYear = new CodedDouble(5, string.Empty, "5"),
                TwoYearsAgo = new CodedDouble(6, string.Empty, "6")
            },

            EnglandPercentageMeetingOrExceeding = new Entities.RelativeYearValues<CodedDouble> 
            {
                CurrentYear = new CodedDouble(7, string.Empty, "7"),
                PreviousYear = new CodedDouble(8, string.Empty, "8"),
                TwoYearsAgo = new CodedDouble(9, string.Empty, "9")
            },
            EstablishmentPercentageExceeding = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(10, string.Empty, "10"),
                PreviousYear = new CodedDouble(11, string.Empty, "11"),
                TwoYearsAgo = new CodedDouble(12, string.Empty, "12")
            },
            LocalAuthorityPercentageExceeding = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(13, string.Empty, "13"),
                PreviousYear = new CodedDouble(14, string.Empty, "14"),
                TwoYearsAgo = new CodedDouble(15, string.Empty, "15")
            },

            EnglandPercentageExceeding = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(16, string.Empty, "16"),
                PreviousYear = new CodedDouble(17, string.Empty, "17"),
                TwoYearsAgo = new CodedDouble(18, string.Empty, "18")
            }
        };
    }
}
