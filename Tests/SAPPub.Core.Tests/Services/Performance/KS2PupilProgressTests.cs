using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS2PupilProgressTests
{
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly Mock<IKS2PerformanceRepository> _ks2PerformanceRepository = new();
    private readonly KS2PupilProgressService _service;

    public KS2PupilProgressTests()
    {
        _service = new KS2PupilProgressService(_establishmentService.Object, _ks2PerformanceRepository.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetPupilProgressAsync_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetPupilProgressAsync(urn!, It.IsAny<AcademicYearSelection>(), CancellationToken.None));

        Assert.Equal("urn", ex.ParamName);
    }

    [Fact]
    public async Task GetPupilProgressAsync_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetPupilProgressAsync(It.IsAny<string>(), It.IsAny<AcademicYearSelection>(), cts.Token));

        // Assert
        _establishmentService.Verify(a => a.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetLaPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetPupilProgressAsync_ReturnsAll_Previous2_DataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        _establishmentService
            .Setup(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance
            {
                READPROG_Est_Previous2_Num_Coded = new CodedDouble(1, string.Empty, "1"),
                READPROG_DESCR_Est_Previous2_Num_Coded = new CodedString("1", string.Empty, "1"),
                READPROG_UPPER_Est_Previous2_Num_Coded = new CodedDouble(3, string.Empty, "3"),
                READPROG_LOWER_Est_Previous2_Num_Coded = new CodedDouble(4, string.Empty, "4"),
                WRITPROG_Est_Previous2_Num_Coded = new CodedDouble(5, string.Empty, "5"),
                WRITPROG_DESCR_Est_Previous2_Num_Coded = new CodedString("1", string.Empty, "1"),
                WRITPROG_UPPER_Est_Previous2_Num_Coded = new CodedDouble(7, string.Empty, "7"),
                WRITPROG_LOWER_Est_Previous2_Num_Coded = new CodedDouble(8, string.Empty, "8"),
                MATPROG_Est_Previous2_Num_Coded = new CodedDouble(9, string.Empty, "9"),
                MATPROG_DESCR_Est_Previous2_Num_Coded = new CodedString("1", string.Empty, "1"),
                MATPROG_UPPER_Est_Previous2_Num_Coded = new CodedDouble(11, string.Empty, "11"),
                MATPROG_LOWER_Est_Previous2_Num_Coded = new CodedDouble(12, string.Empty, "12")
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance
            {
                READPROG_LA_Previous2_Num_Coded = new CodedDouble(30, string.Empty, "30"),
                WRITPROG_LA_Previous2_Num_Coded = new CodedDouble(31, string.Empty, "31"),
                MATPROG_LA_Previous2_Num_Coded = new CodedDouble(32, string.Empty, "32")
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance
            {
                ProgBand_Read_Band1_Eng_Previous2_Desc = new CodedString("reading well above average reason", string.Empty, "reading well above average reason"),
                ProgBand_Writ_Band1_Eng_Previous2_Desc = new CodedString("writing well above average reason", string.Empty, "writing well above average reason"),
                ProgBand_Math_Band1_Eng_Previous2_Desc = new CodedString("maths well above average reason", string.Empty, "maths well above average reason"),
                ProgBand_Read_Band1_Eng_Previous2_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                ProgBand_Read_Band2_Eng_Previous2_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                ProgBand_Read_Band3_Eng_Previous2_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                ProgBand_Read_Band4_Eng_Previous2_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                ProgBand_Read_Band5_Eng_Previous2_Pct_Coded = new CodedDouble(5, string.Empty, "5"),
                ProgBand_Writ_Band1_Eng_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
                ProgBand_Writ_Band2_Eng_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
                ProgBand_Writ_Band3_Eng_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
                ProgBand_Writ_Band4_Eng_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
                ProgBand_Writ_Band5_Eng_Previous2_Pct_Coded = new CodedDouble(6, string.Empty, "6"),
                ProgBand_Math_Band1_Eng_Previous2_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
                ProgBand_Math_Band2_Eng_Previous2_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
                ProgBand_Math_Band3_Eng_Previous2_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
                ProgBand_Math_Band4_Eng_Previous2_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
                ProgBand_Math_Band5_Eng_Previous2_Pct_Coded = new CodedDouble(7, string.Empty, "7"),
            });

        // Act
        var result = await _service.GetPupilProgressAsync(urn, AcademicYearSelection.Previous2, CancellationToken.None);

        // Assert
        Assert.Equal(urn, result.Urn);
        Assert.Equal(1, result.EstablishmentReadingScore!.Value);
        Assert.Equal("1", result.EstablishmentReadingDescription!.Value);
        Assert.Equal(3, result.EstablishmentReadingConfidenceUpper!.Value);
        Assert.Equal(4, result.EstablishmentReadingConfidenceLower!.Value);
        Assert.Equal(5, result.EstablishmentWritingScore!.Value);
        Assert.Equal("1", result.EstablishmentWritingDescription!.Value);
        Assert.Equal(7, result.EstablishmentWritingConfidenceUpper!.Value);
        Assert.Equal(8, result.EstablishmentWritingConfidenceLower!.Value);
        Assert.Equal(9, result.EstablishmentMathsScore!.Value);
        Assert.Equal("1", result.EstablishmentMathsDescription!.Value);
        Assert.Equal(11, result.EstablishmentMathsConfidenceUpper!.Value);
        Assert.Equal(12, result.EstablishmentMathsConfidenceLower!.Value);

        Assert.Equal(30, result.LaReadingScore!.Value);
        Assert.Equal(31, result.LaWritingScore!.Value);
        Assert.Equal(32, result.LaMathsScore!.Value);

        Assert.Equal("reading well above average reason", result.EstablishmentReadingContextDescription.Value);
        Assert.Equal("writing well above average reason", result.EstablishmentWritingContextDescription.Value);
        Assert.Equal("maths well above average reason", result.EstablishmentMathsContextDescription.Value);

        Assert.Equal(5, result.ReadingProgressBandingPercentages.Length);
        Assert.All(
            result.ReadingProgressBandingPercentages,
            x => Assert.Equal(5, x.Value));
        Assert.Equal(5, result.WritingProgressBandingPercentages.Length);
        Assert.All(
            result.WritingProgressBandingPercentages,
            x => Assert.Equal(6, x.Value));
        Assert.Equal(5, result.MathsProgressBandingPercentages.Length);
        Assert.All(
            result.MathsProgressBandingPercentages,
            x => Assert.Equal(7, x.Value));

        _establishmentService
            .Verify(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()), Times.Once);

        _ks2PerformanceRepository
            .Verify(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()), Times.Once);

        _ks2PerformanceRepository
            .Verify(a => a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetDisadvantagedPupilProgressAsync_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetDisadvantagedPupilProgressAsync(urn!, It.IsAny<CancellationToken>()));

        Assert.Equal("urn", ex.ParamName);
    }

    [Fact]
    public async Task GetDisadvantagedPupilProgressAsync_ReturnsUrnOnly_WhenEstablishmentNotFound()
    {
        // Arrange
        var urn = "123456";

        _establishmentService
            .Setup(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel());
        
        // Act
        var result = await _service.GetDisadvantagedPupilProgressAsync(urn, CancellationToken.None);
        
        // Assert
        Assert.Equal(urn, result.Urn);
        Assert.Null(result.LocalAuthorityReadingScore);
        Assert.Null(result.EnglandAverageReadingScore);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetDisadvantagedPupilProgressAsync_ReturnsAll_DataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        SetupAllPerformanceData(urn, laId);

        // Act
        var result = await _service.GetDisadvantagedPupilProgressAsync(urn, CancellationToken.None);

        // Assert
        Assert.Equal(urn, result.Urn);
        Assert.Equal(5, result.EstablishmentReadingScore.Value);
        Assert.Equal(6, result.EstablishmentWritingScore.Value);
        Assert.Equal(7, result.EstablishmentMathsScore.Value);
        Assert.Equal(11, result.LocalAuthorityReadingScore!.Value.Value);
        Assert.Equal(12, result.LocalAuthorityWritingScore!.Value.Value);
        Assert.Equal(13, result.LocalAuthorityMathsScore!.Value.Value);
        Assert.Equal(17, result.EnglandAverageReadingScore!.Value.Value);
        Assert.Equal(18, result.EnglandAverageWritingScore!.Value.Value);
        Assert.Equal(19, result.EnglandAverageMathsScore!.Value.Value);
    }

    [Fact]
    public async Task GetPupilProgressAsync_Bandings_ReturnsAllPupilsScores()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        SetupAllPerformanceData(urn, laId);

        // Act  
        var result = await _service.GetPupilProgressAsync(urn, CancellationToken.None);

        // Assert
        Assert.Equal(urn, result.Urn);
        Assert.Equal(1, result.EstablishmentReadingScore.Value);
        Assert.Equal(2, result.EstablishmentWritingScore.Value);
        Assert.Equal(3, result.EstablishmentMathsScore.Value);
        Assert.Equal("4", result.EstablishmentReadingProgressBanding!.Value.Value);
        Assert.Equal(8, result.LocalAuthorityReadingScore!.Value.Value);
        Assert.Equal(9, result.LocalAuthorityWritingScore!.Value.Value);
        Assert.Equal(10, result.LocalAuthorityMathsScore!.Value.Value);
        Assert.Equal(14, result.EnglandAverageReadingScore!.Value.Value);
        Assert.Equal(15, result.EnglandAverageWritingScore!.Value.Value);
        Assert.Equal(16, result.EnglandAverageMathsScore!.Value.Value);

    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetGirlBoyPupilProgressAsync_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetGirlBoyPupilProgressAsync(urn!, It.IsAny<CancellationToken>()));

        Assert.Equal("urn", ex.ParamName);
    }

    [Fact]
    public async Task GetGirlBoyPupilProgressAsync_ReturnsEmptyGirlsAndBoys_WhenEstablishmentNotFound()
    {
        // Arrange
        var urn = "123456";

        _establishmentService
            .Setup(a => a.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel());

        // Act
        var result = await _service.GetGirlBoyPupilProgressAsync(urn, CancellationToken.None);

        // Assert
        Assert.Equal(["Girls", "Boys"], result.Select(a=>a.CharacteristicName));
        Assert.All(result, a => 
        {
            Assert.Equal(urn, a.Urn);
            Assert.Null(a.LocalAuthorityReadingScore);
            Assert.Null(a.EnglandAverageReadingScore);

        });
        
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetGirlBoyPupilProgressAsync_ReturnsAll_DataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var laId = "TST123";

        SetupAllPerformanceData(urn, laId);

        // Act
        var result = await _service.GetGirlBoyPupilProgressAsync(urn, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Length);
        var girls = result[0];
        var boys = result[1];   
        Assert.Equal("Girls", girls.CharacteristicName);
        Assert.Equal(urn, girls.Urn);
        Assert.Equal(20, girls.EstablishmentReadingScore.Value);
        Assert.Equal(21, girls.EstablishmentWritingScore.Value);
        Assert.Equal(22, girls.EstablishmentMathsScore.Value);
        Assert.Equal(26, girls.LocalAuthorityReadingScore!.Value.Value);
        Assert.Equal(27, girls.LocalAuthorityWritingScore!.Value.Value);
        Assert.Equal(28, girls.LocalAuthorityMathsScore!.Value.Value);
        Assert.Equal(32, girls.EnglandAverageReadingScore!.Value.Value);
        Assert.Equal(33, girls.EnglandAverageWritingScore!.Value.Value);
        Assert.Equal(34, girls.EnglandAverageMathsScore!.Value.Value);

        Assert.Equal("Boys", boys.CharacteristicName);
        Assert.Equal(urn, boys.Urn);
        Assert.Equal(23, boys.EstablishmentReadingScore.Value);
        Assert.Equal(24, boys.EstablishmentWritingScore.Value);
        Assert.Equal(25, boys.EstablishmentMathsScore.Value);
        Assert.Equal(29, boys.LocalAuthorityReadingScore!.Value.Value);
        Assert.Equal(30, boys.LocalAuthorityWritingScore!.Value.Value);
        Assert.Equal(31, boys.LocalAuthorityMathsScore!.Value.Value);
        Assert.Equal(35, boys.EnglandAverageReadingScore!.Value.Value);
        Assert.Equal(36, boys.EnglandAverageWritingScore!.Value.Value);
        Assert.Equal(37, boys.EnglandAverageMathsScore!.Value.Value);
    }


    private void SetupAllPerformanceData(string urn, string laId)
    {
        _establishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel { URN = urn, LAId = laId });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance
            {
                READPROG_Est_Current_Num_Coded = GetCodedDouble(1),
                WRITPROG_Est_Current_Num_Coded = GetCodedDouble(2),
                MATPROG_Est_Current_Num_Coded = GetCodedDouble(3),
                READPROG_DESCR_Est_Current_Num_Coded = new CodedString("4", string.Empty, "4"),
                READPROG_FSM6CLA1A_Est_Current_Num_Coded = GetCodedDouble(5),
                WRITPROG_FSM6CLA1A_Est_Current_Num_Coded = GetCodedDouble(6),
                MATPROG_FSM6CLA1A_Est_Current_Num_Coded = GetCodedDouble(7),
                READPROG_G_Est_Current_Num_Coded = GetCodedDouble(20),
                WRITPROG_G_Est_Current_Num_Coded = GetCodedDouble(21),
                MATPROG_G_Est_Current_Num_Coded = GetCodedDouble(22),
                READPROG_B_Est_Current_Num_Coded = GetCodedDouble(23),
                WRITPROG_B_Est_Current_Num_Coded = GetCodedDouble(24),
                MATPROG_B_Est_Current_Num_Coded = GetCodedDouble(25),
            });

        _ks2PerformanceRepository
            .Setup(a=>a.GetLaPerformanceAsync(laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance
            {
                READPROG_LA_Current_Num_Coded = GetCodedDouble(8),
                WRITPROG_LA_Current_Num_Coded = GetCodedDouble(9),
                MATPROG_LA_Current_Num_Coded = GetCodedDouble(10),
                READPROG_FSM6CLA1A_LA_Current_Num_Coded = GetCodedDouble(11),
                WRITPROG_FSM6CLA1A_LA_Current_Num_Coded = GetCodedDouble(12),
                MATPROG_FSM6CLA1A_LA_Current_Num_Coded = GetCodedDouble(13),
                READPROG_G_LA_Current_Num_Coded = GetCodedDouble(26),
                WRITPROG_G_LA_Current_Num_Coded = GetCodedDouble(27),
                MATPROG_G_LA_Current_Num_Coded = GetCodedDouble(28),
                READPROG_B_LA_Current_Num_Coded = GetCodedDouble(29),
                WRITPROG_B_LA_Current_Num_Coded = GetCodedDouble(30),
                MATPROG_B_LA_Current_Num_Coded = GetCodedDouble(31),
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance
            {
                READPROG_Eng_Current_Num_Coded = GetCodedDouble(14),
                WRITPROG_Eng_Current_Num_Coded = GetCodedDouble(15),
                MATPROG_Eng_Current_Num_Coded = GetCodedDouble(16),
                READPROG_FSM6CLA1A_Eng_Current_Num_Coded = GetCodedDouble(17),
                WRITPROG_FSM6CLA1A_Eng_Current_Num_Coded = GetCodedDouble(18),
                MATPROG_FSM6CLA1A_Eng_Current_Num_Coded = GetCodedDouble(19),
                READPROG_G_Eng_Current_Num_Coded = GetCodedDouble(32),
                WRITPROG_G_Eng_Current_Num_Coded = GetCodedDouble(33),
                MATPROG_G_Eng_Current_Num_Coded = GetCodedDouble(34),
                READPROG_B_Eng_Current_Num_Coded = GetCodedDouble(35),
                WRITPROG_B_Eng_Current_Num_Coded = GetCodedDouble(36),
                MATPROG_B_Eng_Current_Num_Coded = GetCodedDouble(37),
            });

    }

    private static CodedDouble GetCodedDouble(double? val)
    {
        return new CodedDouble(val!, string.Empty, val is null ? null! : val.ToString()!);
    }

}