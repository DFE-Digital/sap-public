using Moq;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Performance;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Services.Performance;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services.Performance;

public class KS2AdditionalMeasuresServiceTests : ServiceTestBase
{
    private readonly string _testLaId = "123";
    private readonly Mock<IAboutSchoolService> _aboutSchoolService = new();
    private readonly Mock<IKS2PerformanceRepository> _ks2PerformanceRepository = new();
    private readonly KS2AdditionalMeasuresService _service;

    public KS2AdditionalMeasuresServiceTests()
    {
        _service = new KS2AdditionalMeasuresService(_ks2PerformanceRepository.Object, _aboutSchoolService.Object);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task GetAdditionalMeasures_ThrowsForInvalidUrn(string? urn)
    {
        var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() =>
            _service.GetAdditionalMeasures(urn!, _testLaId, CancellationToken.None));

        Assert.Equal("urn", ex.ParamName);
    }


    [Fact]
    public async Task GetAdditionalMeasures_ThrowsWhenCancellationAlreadyRequested()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            _service.GetAdditionalMeasures("123456", _testLaId, cts.Token));

        // Assert
        _aboutSchoolService.Verify(a => a.GetAboutSchoolDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEstablishmentPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetLaPerformanceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _ks2PerformanceRepository.Verify(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()), Times.Never);

    }

    [Fact]
    public async Task GetAdditionalMeasures_ReturnsAllDataCorrectly()
    {
        // Arrange
        var urn = "123456";
        var numPupils = "20";

        var expectedModel = GetKS2AdditionalMeasuresModel();

        _aboutSchoolService
            .Setup(a => a.GetAboutSchoolDetailsAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AboutSchoolModel { Urn = urn, SchoolName = "test", NumberOfPupils = numPupils });

        _ks2PerformanceRepository
            .Setup(a => a.GetEstablishmentPerformanceAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EstablishmentPerformance
            {
                PTGPS_EXP_Est_Current_Pct_Coded = GetCodedDouble(1),
                PTGPS_HIGH_Est_Current_Pct_Coded = GetCodedDouble(2),
                PSENELE_Est_Current_Pct_Coded = GetCodedDouble(3),
                PSENELK_Est_Current_Pct_Coded = GetCodedDouble(4),
                TELIG_Est_Current_Num_Coded = GetCodedDouble(11),
                GELIG_Est_Current_Num_Coded = GetCodedDouble(12),
                BELIG_Est_Current_Num_Coded = GetCodedDouble(13),
                TEALGRP2_Est_Current_Num_Coded = GetCodedDouble(14),
                TMOBN_Est_Current_Num_Coded = GetCodedDouble(15),
                TFSM6CLA1A_Est_Current_Num_Coded = GetCodedDouble(16)
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetLaPerformanceAsync(_testLaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2LAPerformance
            {
                PTGPS_EXP_LA_Current_Pct_Coded = GetCodedDouble(5),
                PTGPS_HIGH_LA_Current_Pct_Coded = GetCodedDouble(6),
                TFSM6CLA1A_LA_Current_Num_Coded = GetCodedDouble(17),
                TNOTFSM6CLA1A_LA_Current_Num_Coded = GetCodedDouble(19)
            });

        _ks2PerformanceRepository
            .Setup(a => a.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new KS2EnglandPerformance
            {
                PTGPS_EXP_Eng_Current_Pct_Coded = GetCodedDouble(7),
                PTGPS_HIGH_Eng_Current_Pct_Coded = GetCodedDouble(8),
                PSENELE_Eng_Current_Pct_Coded = GetCodedDouble(9),
                PSENELK_Eng_Current_Pct_Coded = GetCodedDouble(10),
                TFSM6CLA1A_Eng_Current_Num_Coded = GetCodedDouble(18),
                TNOTFSM6CLA1A_Eng_Current_Num_Coded = GetCodedDouble(20)
            });

        // Act
        var result = await _service.GetAdditionalMeasures(urn, _testLaId, CancellationToken.None);

        // Assert
        Assert.Equal(expectedModel.EstablishmentGrammarAtExpectedStandard, result.EstablishmentGrammarAtExpectedStandard);
        Assert.Equal(expectedModel.EstablishmentGrammarAtHigherStandard, result.EstablishmentGrammarAtHigherStandard);
        Assert.Equal(expectedModel.EstablishmentEHCPPopulation, result.EstablishmentEHCPPopulation);
        Assert.Equal(expectedModel.EstablishmentSENSupportPopulation, result.EstablishmentSENSupportPopulation);
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard, result.LAGrammarAtExpectedStandard);
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard, result.LAGrammarAtHigherStandard);
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard, result.EnglandGrammarAtExpectedStandard);
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard, result.EnglandGrammarAtHigherStandard);
        Assert.Equal(expectedModel.EnglandEHCPPopulation, result.EnglandEHCPPopulation);
        Assert.Equal(expectedModel.EnglandSENSupportPopulation, result.EnglandSENSupportPopulation);

        Assert.Equal(expectedModel.EstablishmentNumPupilsEndOfKS2, result.EstablishmentNumPupilsEndOfKS2);
        Assert.Equal(expectedModel.LANumPupilsEndOfKS2, result.LANumPupilsEndOfKS2);
        Assert.Equal(expectedModel.EnglandNumPupilsEndOfKS2, result.EnglandNumPupilsEndOfKS2);

        Assert.Equal(expectedModel.EstablishmentNumGirlsEndOfKS2, result.EstablishmentNumGirlsEndOfKS2);
        Assert.Equal(expectedModel.EstablishmentNumBoysEndOfKS2, result.EstablishmentNumBoysEndOfKS2);
        Assert.Equal(expectedModel.EstablishmentNumEALEndOfKS2, result.EstablishmentNumEALEndOfKS2);
        Assert.Equal(expectedModel.EstablishmentNumNonMobileEndOfKS2, result.EstablishmentNumNonMobileEndOfKS2);
        Assert.Equal(expectedModel.EstablishmentNumDisadvantagedEndOfKS2, result.EstablishmentNumDisadvantagedEndOfKS2);
        Assert.Equal(expectedModel.LANumDisadvantagedEndOfKS2, result.LANumDisadvantagedEndOfKS2);

        Assert.Equal(expectedModel.EnglandNumDisadvantagedEndOfKS2, result.EnglandNumDisadvantagedEndOfKS2);
        Assert.Equal(expectedModel.LANumDisadvantagedEndOfKS2, result.LANumDisadvantagedEndOfKS2);

        Assert.Equal(expectedModel.EnglandNumNonDisadvantagedEndOfKS2, result.EnglandNumNonDisadvantagedEndOfKS2);
        Assert.Equal(expectedModel.EstablishmentPupilTotal, result.EstablishmentPupilTotal);
        Assert.Equal(expectedModel.EnglandPupilTotal, result.EnglandPupilTotal);
    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel() =>
        new()
        {
            EstablishmentGrammarAtExpectedStandard = GetCodedDouble(1),
            EstablishmentGrammarAtHigherStandard = GetCodedDouble(2),
            EstablishmentEHCPPopulation = GetCodedDouble(3),
            EstablishmentSENSupportPopulation = GetCodedDouble(4),
            LAGrammarAtExpectedStandard = GetCodedDouble(5),
            LAGrammarAtHigherStandard = GetCodedDouble(6),
            EnglandGrammarAtExpectedStandard = GetCodedDouble(7),
            EnglandGrammarAtHigherStandard = GetCodedDouble(8),
            EnglandEHCPPopulation = GetCodedDouble(9),
            EnglandSENSupportPopulation = GetCodedDouble(10),

            EstablishmentNumPupilsEndOfKS2 = GetCodedDouble(11),
            LANumPupilsEndOfKS2 = CodedDouble.Empty,
            EnglandNumPupilsEndOfKS2 = CodedDouble.Empty,
            EstablishmentNumGirlsEndOfKS2 = GetCodedDouble(12),
            EstablishmentNumBoysEndOfKS2 = GetCodedDouble(13),
            EstablishmentNumEALEndOfKS2 = GetCodedDouble(14),
            EstablishmentNumNonMobileEndOfKS2 = GetCodedDouble(15),
            EstablishmentNumDisadvantagedEndOfKS2 = GetCodedDouble(16),
            LANumDisadvantagedEndOfKS2 = GetCodedDouble(17),
            EnglandNumDisadvantagedEndOfKS2 = GetCodedDouble(18),
            LANumNonDisadvantagedEndOfKS2 = GetCodedDouble(19),
            EnglandNumNonDisadvantagedEndOfKS2 = GetCodedDouble(20),
            EstablishmentPupilTotal = "20",
            EnglandPupilTotal = CodedDouble.Empty
        };
}
