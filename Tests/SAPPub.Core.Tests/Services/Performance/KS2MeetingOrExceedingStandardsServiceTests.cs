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
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance 
            {
                PTRWM_EXP_LA_Current_Pct_Coded = new CodedDouble(4, string.Empty, "4"),
                PTRWM_EXP_LA_Previous_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                PTRWM_EXP_LA_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance 
            {
                PTRWM_EXP_Eng_Current_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
                PTRWM_EXP_Eng_Previous_Pct_Coded = new CodedDouble(8, string.Empty, "8"),
                PTRWM_EXP_Eng_Previous2_Pct_Coded = new CodedDouble(9, string.Empty, "9"),
            });

        // Act
        var result = await _service.GetMeetingOrExceedingStandardsPercentages(urn, CancellationToken.None);

        // Assert
        Assert.Equal(expectedModel.EstablishmentPercentage.CurrentYear, result.EstablishmentPercentage.CurrentYear);
        Assert.Equal(expectedModel.EstablishmentPercentage.PreviousYear, result.EstablishmentPercentage.PreviousYear);
        Assert.Equal(expectedModel.EstablishmentPercentage.TwoYearsAgo, result.EstablishmentPercentage.TwoYearsAgo);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.CurrentYear, result.LocalAuthorityPercentage.CurrentYear);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.PreviousYear, result.LocalAuthorityPercentage.PreviousYear);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.TwoYearsAgo, result.LocalAuthorityPercentage.TwoYearsAgo); 
        Assert.Equal(expectedModel.EnglandPercentage.CurrentYear, result.EnglandPercentage.CurrentYear);
        Assert.Equal(expectedModel.EnglandPercentage.PreviousYear, result.EnglandPercentage.PreviousYear);
        Assert.Equal(expectedModel.EnglandPercentage.TwoYearsAgo, result.EnglandPercentage.TwoYearsAgo);
    }

    private static KS2MeetingOrExceedingStandardsModel GetKS2MeetingOrExceedingStandardsModel()
    {
        return new KS2MeetingOrExceedingStandardsModel
        {
            EstablishmentPercentage = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3")
            },
            LocalAuthorityPercentage = new Entities.RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(4, string.Empty, "4"),
                PreviousYear = new CodedDouble(5, string.Empty, "5"),
                TwoYearsAgo = new CodedDouble(6, string.Empty, "6")
            },

            EnglandPercentage = new Entities.RelativeYearValues<CodedDouble> 
            {
                CurrentYear = new CodedDouble(7, string.Empty, "7"),
                PreviousYear = new CodedDouble(8, string.Empty, "8"),
                TwoYearsAgo = new CodedDouble(9, string.Empty, "9")
            }
        };
    }
}
