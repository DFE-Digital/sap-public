using Bogus;
using Moq;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Services.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;

namespace SAPPub.Core.Tests.Services.KS4.Performance;

public class AdditionalMeasuresServiceTests
{
    private readonly Faker _faker = new Faker("en_GB");
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentPerformanceService> _mockEstablishmentPerformanceService;
    private readonly Mock<ILAPerformanceService> _mockLAPerformanceService;
    private readonly Mock<IEnglandPerformanceService> _mockEnglandPerformanceService;
    private readonly AdditionalMeasuresService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public AdditionalMeasuresServiceTests()
    {
        _mockEstablishmentService = new();
        _mockEstablishmentPerformanceService = new();
        _mockLAPerformanceService = new();
        _mockEnglandPerformanceService = new();

        _service = new AdditionalMeasuresService(
            _mockEstablishmentService.Object,
            _mockEstablishmentPerformanceService.Object,
            _mockLAPerformanceService.Object,
            _mockEnglandPerformanceService.Object);
    }

    [Fact]
    public async Task GetAsync_AllDataAvailable_ReturnsExpectedModel()
    {
        // Arrange
        var establishmentPerformance = new EstablishmentPerformanceBuilder().WithAdditionalMeasures().Build();
        var laPerformance = new LaPerformanceBuilder().WithAdditionalMeasures().Build();
        var englandPerformance = new EnglandPerformanceBuilder().WithAdditionalMeasures().Build();

        _mockEstablishmentService.Setup(s => s.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);
        _mockEstablishmentPerformanceService.Setup(s => s.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentPerformance);
        _mockLAPerformanceService.Setup(s => s.GetLAPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laPerformance);
        _mockEnglandPerformanceService.Setup(s => s.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        AssertEstablishmentAdditionalMeasuresData(establishmentPerformance, result.EstablishmentCurrentYear);
        AssertLaAdditionalMeasuresData(laPerformance, result.LocalAuthorityCurrentYear);
        AssertEnglandAdditionalMeasuresData(englandPerformance, result.EnglandCurrentYear);
    }

    [Fact]
    public async Task GetAsync_EstablishmentPerformanceDataNotAvailable_ReturnsExpectedModel()
    {
        // Arrange
        var laPerformance = new LaPerformanceBuilder().WithAdditionalMeasures().Build();
        var englandPerformance = new EnglandPerformanceBuilder().WithAdditionalMeasures().Build();

        _mockEstablishmentService.Setup(s => s.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);
        _mockEstablishmentPerformanceService.Setup(s => s.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentPerformance());
        _mockLAPerformanceService.Setup(s => s.GetLAPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laPerformance);
        _mockEnglandPerformanceService.Setup(s => s.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        AssertNullAdditionalMeasuresData(result.EstablishmentCurrentYear);
        AssertLaAdditionalMeasuresData(laPerformance, result.LocalAuthorityCurrentYear);
        AssertEnglandAdditionalMeasuresData(englandPerformance, result.EnglandCurrentYear);
    }

    [Fact]
    public async Task GetAsync_EstablishmentLaIdNotAvailable_ReturnsExpectedModel()
    {
        // Arrange
        var establishmentPerformance = new EstablishmentPerformanceBuilder().WithAdditionalMeasures().Build();
        var englandPerformance = new EnglandPerformanceBuilder().WithAdditionalMeasures().Build();

        _mockEstablishmentService.Setup(s => s.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);
        _mockEstablishmentPerformanceService.Setup(s => s.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentPerformance);
        _mockLAPerformanceService.Setup(s => s.GetLAPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new LAPerformance());
        _mockEnglandPerformanceService.Setup(s => s.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandPerformance);

        // Act
        var result = await _service.GetAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        AssertEstablishmentAdditionalMeasuresData(establishmentPerformance, result.EstablishmentCurrentYear);
        AssertNullAdditionalMeasuresData(result.LocalAuthorityCurrentYear);
        AssertEnglandAdditionalMeasuresData(englandPerformance, result.EnglandCurrentYear);
    }

    [Fact]
    public async Task GetAsync_EnglandPerformanceDataNotAvailable_ReturnsExpectedModel()
    {
        // Arrange
        var establishmentPerformance = new EstablishmentPerformanceBuilder().WithAdditionalMeasures().Build();
        var laPerformance = new LaPerformanceBuilder().WithAdditionalMeasures().Build();

        _mockEstablishmentService.Setup(s => s.GetEstablishmentAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);
        _mockEstablishmentPerformanceService.Setup(s => s.GetEstablishmentPerformanceAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentPerformance);
        _mockLAPerformanceService.Setup(s => s.GetLAPerformanceAsync(fakeEstablishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(laPerformance);
        _mockEnglandPerformanceService.Setup(s => s.GetEnglandPerformanceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EnglandPerformance());

        // Act
        var result = await _service.GetAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        AssertEstablishmentAdditionalMeasuresData(establishmentPerformance, result.EstablishmentCurrentYear);
        AssertLaAdditionalMeasuresData(laPerformance, result.LocalAuthorityCurrentYear);
        AssertNullAdditionalMeasuresData(result.EnglandCurrentYear);
    }

    private void AssertEstablishmentAdditionalMeasuresData(EstablishmentPerformance establishmentPerformance, AdditionalMeasures result)
    {
        Assert.Equal(establishmentPerformance.AnyQual_Tot_Est_Current_Pct_Coded.Value, result.PercentAchievingAtLeastOneQualification.Value);
        Assert.Equal(establishmentPerformance.TripSci_Tot_Est_Current_Pct_Coded.Value, result.PercentEnteredForTripleScience.Value);
        Assert.Equal(establishmentPerformance.More1FL_Tot_Est_Current_Pct_Coded.Value, result.PercentEnteredMoreThanOneForeignLanguage.Value);
        Assert.Equal(establishmentPerformance.ExamEntriesGSCE_Tot_Est_Current_Num_Coded.Value, result.AverageGCSEExamEntriesPerPupil.Value);
        Assert.Equal(establishmentPerformance.ExamEntriesKS4_Tot_Est_Current_Num_Coded.Value, result.AverageAllKS4QualificationsExamEntriesPerPupil.Value);
    }

    private void AssertLaAdditionalMeasuresData(LAPerformance laPerformance, AdditionalMeasures result)
    {
        Assert.Equal(laPerformance.AnyQual_Tot_LA_Current_Pct_Coded.Value, result.PercentAchievingAtLeastOneQualification.Value);
        Assert.Equal(laPerformance.TripSci_Tot_LA_Current_Pct_Coded.Value, result.PercentEnteredForTripleScience.Value);
        Assert.Equal(laPerformance.More1FL_Tot_LA_Current_Pct_Coded.Value, result.PercentEnteredMoreThanOneForeignLanguage.Value);
        Assert.Equal(laPerformance.ExamEntriesGSCE_Tot_LA_Current_Num_Coded.Value, result.AverageGCSEExamEntriesPerPupil.Value);
        Assert.Equal(laPerformance.ExamEntriesKS4_Tot_LA_Current_Num_Coded.Value, result.AverageAllKS4QualificationsExamEntriesPerPupil.Value);
    }

    private void AssertEnglandAdditionalMeasuresData(EnglandPerformance englandPerformance, AdditionalMeasures result)
    {
        Assert.Equal(englandPerformance.AnyQual_Tot_Eng_Current_Pct_Coded.Value, result.PercentAchievingAtLeastOneQualification.Value);
        Assert.Equal(englandPerformance.TripSci_Tot_Eng_Current_Pct_Coded.Value, result.PercentEnteredForTripleScience.Value);
        Assert.Equal(englandPerformance.More1FL_Tot_Eng_Current_Pct_Coded.Value, result.PercentEnteredMoreThanOneForeignLanguage.Value);
        Assert.Equal(englandPerformance.ExamEntriesGSCE_Tot_Eng_Current_Num_Coded.Value, result.AverageGCSEExamEntriesPerPupil.Value);
        Assert.Equal(englandPerformance.ExamEntriesKS4_Tot_Eng_Current_Num_Coded.Value, result.AverageAllKS4QualificationsExamEntriesPerPupil.Value);
    }

    private void AssertNullAdditionalMeasuresData(AdditionalMeasures result)
    {
        Assert.Null(result.PercentAchievingAtLeastOneQualification.Value);
        Assert.Null(result.PercentEnteredForTripleScience.Value);
        Assert.Null(result.PercentEnteredMoreThanOneForeignLanguage.Value);
        Assert.Null(result.AverageGCSEExamEntriesPerPupil.Value);
        Assert.Null(result.AverageAllKS4QualificationsExamEntriesPerPupil.Value);
    }
}
