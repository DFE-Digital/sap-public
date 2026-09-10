using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services;

public class DestinationsServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKS4DestinationsRepository> _mockKs4DestinationsRepo;
    private readonly Mock<IKS5DestinationsRepository> _mockKs5DestinationsRepo;
    private readonly DestinationsService _service;

    private readonly EstablishmentMinimumServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        LAName = "Council",
        LAId = "E09000001"
    };

    public DestinationsServiceTests()
    {
        _mockEstablishmentService = new();
        _mockKs4DestinationsRepo = new();
        _mockKs5DestinationsRepo = new();

        _service = new DestinationsService(
            _mockEstablishmentService.Object,
            _mockKs4DestinationsRepo.Object,
            _mockKs5DestinationsRepo.Object);
    }

    [Fact]
    public async Task GetKS4DestinationsDetailsAsync_ShouldReturnData()
    {
        // Arrange
        var establishmentDestinations = new KS4EstablishmentDestinations
        {
            Id = fakeEstablishment.URN,
            AllDest_Tot_Est_Current_Pct_Coded = new CodedDouble(100, "", "100"),
            AllDest_Tot_Est_Previous_Pct_Coded = new CodedDouble(90, "", "90"),
            AllDest_Tot_Est_Previous2_Pct_Coded = new CodedDouble(80, "", "80"),

            Education_Tot_Est_Current_Pct_Coded = new CodedDouble(50, "", "50"),

            Employment_Tot_Est_Current_Pct_Coded = new CodedDouble(90, "", "90"),

            Apprentice_Tot_Est_Current_Pct_Coded = new CodedDouble(50, "", "50"),
        };

        var lADestinations = new KS4LADestinations
        {
            Id = fakeEstablishment.LAId,
            AllDest_Tot_LA_Current_Pct_Coded = new CodedDouble(70, "", "70"),
            AllDest_Tot_LA_Previous_Pct_Coded = new CodedDouble(60, "", "60"),
            AllDest_Tot_LA_Previous2_Pct_Coded = new CodedDouble(80, "", "80"),

            Education_Tot_LA_Current_Pct_Coded = new CodedDouble(40, "", "40"),

            Employment_Tot_LA_Current_Pct_Coded = new CodedDouble(80, "", "80"),

            Apprentice_Tot_LA_Current_Pct_Coded = new CodedDouble(65, "", "65")
        };

        var englandDestinations = new KS4EnglandDestinations
        {
            Id = "National",
            AllDest_Tot_Eng_Current_Pct_Coded = new CodedDouble(50, "", "50"),
            AllDest_Tot_Eng_Previous_Pct_Coded = new CodedDouble(60, "", "60"),
            AllDest_Tot_Eng_Previous2_Pct_Coded = new CodedDouble(70, "", "70"),

            Education_Tot_Eng_Current_Pct_Coded = new CodedDouble(60, "", "60"),

            Employment_Tot_Eng_Current_Pct_Coded = new CodedDouble(70, "", "70"),

            Apprentice_Tot_Eng_Current_Pct_Coded = new CodedDouble(45, "", "45")
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockKs4DestinationsRepo
            .Setup(r => r.GetEstablishmentDestinationsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentDestinations);

        _mockKs4DestinationsRepo
            .Setup(r => r.GetLADestinationsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lADestinations);

        _mockKs4DestinationsRepo
            .Setup(r => r.GetEnglandDestinationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandDestinations);

        // Act
        var result = await _service.GetKS4DestinationsDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthorityName);

        // Assert school data
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Current_Pct_Coded.Value, result.SchoolAll.CurrentYear.Value);
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Previous_Pct_Coded.Value, result.SchoolAll.PreviousYear.Value);
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Previous2_Pct_Coded.Value, result.SchoolAll.TwoYearsAgo.Value);

        Assert.Equal(establishmentDestinations.Education_Tot_Est_Current_Pct_Coded.Value, result.SchoolEducation.CurrentYear.Value);
        Assert.Equal(establishmentDestinations.Employment_Tot_Est_Current_Pct_Coded.Value, result.SchoolEmployment.CurrentYear.Value);
        Assert.Equal(establishmentDestinations.Apprentice_Tot_Est_Current_Pct_Coded.Value, result.SchoolApprentice.CurrentYear.Value);

        // Assert local authority data
        Assert.Equal(lADestinations.AllDest_Tot_LA_Current_Pct_Coded.Value, result.LocalAuthorityAll.CurrentYear.Value);
        Assert.Equal(lADestinations.AllDest_Tot_LA_Previous_Pct_Coded.Value, result.LocalAuthorityAll.PreviousYear.Value);
        Assert.Equal(lADestinations.AllDest_Tot_LA_Previous2_Pct_Coded.Value, result.LocalAuthorityAll.TwoYearsAgo.Value);

        Assert.Equal(lADestinations.Education_Tot_LA_Current_Pct_Coded.Value, result.LocalAuthorityEducation.CurrentYear.Value);
        Assert.Equal(lADestinations.Employment_Tot_LA_Current_Pct_Coded.Value, result.LocalAuthorityEmployment.CurrentYear.Value);
        Assert.Equal(lADestinations.Apprentice_Tot_LA_Current_Pct_Coded.Value, result.LocalAuthorityApprentice.CurrentYear.Value);

        // Assert england data
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Current_Pct_Coded.Value, result.EnglandAll.CurrentYear.Value);
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Previous_Pct_Coded.Value, result.EnglandAll.PreviousYear.Value);
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Previous2_Pct_Coded.Value, result.EnglandAll.TwoYearsAgo.Value);

        Assert.Equal(englandDestinations.Education_Tot_Eng_Current_Pct_Coded.Value, result.EnglandEducation.CurrentYear.Value);
        Assert.Equal(englandDestinations.Employment_Tot_Eng_Current_Pct_Coded.Value, result.EnglandEmployment.CurrentYear.Value);
        Assert.Equal(englandDestinations.Apprentice_Tot_Eng_Current_Pct_Coded.Value, result.EnglandApprentice.CurrentYear.Value);
    }

    [Fact]
    public async Task GetKS4DestinationsDetailsAsync_ReturnsEmptyWhenNoEstablishmentFound()
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel());

        // Act
        var result = await _service.GetKS4DestinationsDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.Equal(string.Empty, result.Urn);
        Assert.Equal(string.Empty, result.SchoolName);
        Assert.Equal(string.Empty, result.LocalAuthorityName);
        var expectedEmptyYears = new RelativeYearValues<CodedDouble>
        {
            CurrentYear = CodedDouble.Empty,
            PreviousYear = CodedDouble.Empty,
            TwoYearsAgo = CodedDouble.Empty
        };

        Assert.Equal(expectedEmptyYears, result.SchoolAll);
        Assert.Equal(expectedEmptyYears, result.LocalAuthorityAll);
        Assert.Equal(expectedEmptyYears, result.EnglandAll);
        Assert.Equal(expectedEmptyYears, result.SchoolEducation);
        Assert.Equal(expectedEmptyYears, result.LocalAuthorityEducation);
        Assert.Equal(expectedEmptyYears, result.EnglandEducation);
        Assert.Equal(expectedEmptyYears, result.SchoolEmployment);
        Assert.Equal(expectedEmptyYears, result.LocalAuthorityEmployment);
        Assert.Equal(expectedEmptyYears, result.EnglandEmployment);
        Assert.Equal(expectedEmptyYears, result.SchoolApprentice);
        Assert.Equal(expectedEmptyYears, result.LocalAuthorityApprentice);
        Assert.Equal(expectedEmptyYears, result.EnglandApprentice);
        Assert.False(result.IsKS2);
        Assert.False(result.IsKS4);
        Assert.False(result.IsKS5);
    }



    [Fact]
    public async Task GetKS5DestinationsDetailsAsync_ShouldReturnData()
    {
        // Arrange
        var establishmentDestinations = new KS5EstablishmentDestinations
        {
            Id = fakeEstablishment.URN,
            TOT_COHORT_Est_Current_Num = 1002,
            TOT_OVERALLPER_Est_Current_Pct = 88
        };

        var lADestinations = new KS5LADestinations
        {
            Id = fakeEstablishment.LAId,
            TOT_OVERALLPER_LA_Current_Pct = 77
        };

        var englandDestinations = new KS5EnglandDestinations
        {
            TOT_OVERALLPER_Eng_Current_Pct = 66
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockKs5DestinationsRepo
            .Setup(r => r.GetEstablishmentDestinationsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentDestinations);

        _mockKs5DestinationsRepo
            .Setup(r => r.GetLADestinationsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lADestinations);

        _mockKs5DestinationsRepo
            .Setup(r => r.GetEnglandDestinationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandDestinations);

        // Act
        var result = await _service.GetKS5DestinationsDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthorityName);

        Assert.Equal(establishmentDestinations.TOT_COHORT_Est_Current_Num, result.EstablishmentTotalCohortFor);
        Assert.Equal(establishmentDestinations.TOT_OVERALLPER_Est_Current_Pct, result.EstablishmentTotalOverall);
        Assert.Equal(lADestinations.TOT_OVERALLPER_LA_Current_Pct, result.LATotalOverall);
        Assert.Equal(englandDestinations.TOT_OVERALLPER_Eng_Current_Pct, result.EnglandOverall);

    }

    [Fact]
    public async Task GetKS5DestinationsDetailsAsync_ReturnsEmptyWhenNoEstablishmentFound()
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel());

        // Act
        var result = await _service.GetKS5DestinationsDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.Equal(string.Empty, result.Urn);
        Assert.Equal(string.Empty, result.SchoolName);
        Assert.Equal(string.Empty, result.LocalAuthorityName);
        Assert.False(result.IsKS2);
        Assert.False(result.IsKS4);
        Assert.False(result.IsKS5);
    }
}