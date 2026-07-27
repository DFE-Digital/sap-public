using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Interfaces.Repositories.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services;

namespace SAPPub.Core.Tests.Services;

public class DestinationsServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKS4DestinationsRepository> _mockKs4DestinationsRepo;
    private readonly Mock<IKS5DestinationsRepository> _mockKs5DestinationsRepo;
    private readonly DestinationsService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
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
            AllDest_Tot_Est_Current_Pct = 100,
            AllDest_Tot_Est_Previous_Pct = 90,
            AllDest_Tot_Est_Previous2_Pct = 80,

            Education_Tot_Est_Current_Pct = 50,

            Employment_Tot_Est_Current_Pct = 90,

            Apprentice_Tot_Est_Current_Pct = 50,
        };

        var lADestinations = new KS4LADestinations
        {
            Id = fakeEstablishment.LAId,
            AllDest_Tot_LA_Current_Pct = 70,
            AllDest_Tot_LA_Previous_Pct = 60,
            AllDest_Tot_LA_Previous2_Pct = 80,

            Education_Tot_LA_Current_Pct = 40,

            Employment_Tot_LA_Current_Pct = 80,

            Apprentice_Tot_LA_Current_Pct = 65
        };

        var englandDestinations = new KS4EnglandDestinations
        {
            Id = "National",
            AllDest_Tot_Eng_Current_Pct = 50,
            AllDest_Tot_Eng_Previous_Pct = 60,
            AllDest_Tot_Eng_Previous2_Pct = 70,

            Education_Tot_Eng_Current_Pct = 60,

            Employment_Tot_Eng_Current_Pct = 70,

            Apprentice_Tot_Eng_Current_Pct = 45
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Current_Pct, result.SchoolAll.CurrentYear);
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Previous_Pct, result.SchoolAll.PreviousYear);
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Previous2_Pct, result.SchoolAll.TwoYearsAgo);

        Assert.Equal(establishmentDestinations.Education_Tot_Est_Current_Pct, result.SchoolEducation.CurrentYear);
        Assert.Equal(establishmentDestinations.Employment_Tot_Est_Current_Pct, result.SchoolEmployment.CurrentYear);
        Assert.Equal(establishmentDestinations.Apprentice_Tot_Est_Current_Pct, result.SchoolApprentice.CurrentYear);

        // Assert local authority data
        Assert.Equal(lADestinations.AllDest_Tot_LA_Current_Pct, result.LocalAuthorityAll.CurrentYear);
        Assert.Equal(lADestinations.AllDest_Tot_LA_Previous_Pct, result.LocalAuthorityAll.PreviousYear);
        Assert.Equal(lADestinations.AllDest_Tot_LA_Previous2_Pct, result.LocalAuthorityAll.TwoYearsAgo);

        Assert.Equal(lADestinations.Education_Tot_LA_Current_Pct, result.LocalAuthorityEducation.CurrentYear);
        Assert.Equal(lADestinations.Employment_Tot_LA_Current_Pct, result.LocalAuthorityEmployment.CurrentYear);
        Assert.Equal(lADestinations.Apprentice_Tot_LA_Current_Pct, result.LocalAuthorityApprentice.CurrentYear);

        // Assert england data
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Current_Pct, result.EnglandAll.CurrentYear);
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Previous_Pct, result.EnglandAll.PreviousYear);
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Previous2_Pct, result.EnglandAll.TwoYearsAgo);

        Assert.Equal(englandDestinations.Education_Tot_Eng_Current_Pct, result.EnglandEducation.CurrentYear);
        Assert.Equal(englandDestinations.Employment_Tot_Eng_Current_Pct, result.EnglandEmployment.CurrentYear);
        Assert.Equal(englandDestinations.Apprentice_Tot_Eng_Current_Pct, result.EnglandApprentice.CurrentYear);
    }

    [Fact]
    public async Task GetKS4DestinationsDetailsAsync_ReturnsEmptyWhenNoEstablishmentFound()
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel());

        // Act
        var result = await _service.GetKS4DestinationsDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert
        Assert.Equal(string.Empty, result.Urn);
        Assert.Equal(string.Empty, result.SchoolName);
        Assert.Equal(string.Empty, result.LocalAuthorityName);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.SchoolAll);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.LocalAuthorityAll);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.EnglandAll);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.SchoolEducation);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.LocalAuthorityEducation);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.EnglandEducation);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.SchoolEmployment);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.LocalAuthorityEmployment);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.EnglandEmployment);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.SchoolApprentice);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.LocalAuthorityApprentice);
        Assert.Equal(new RelativeYearValues<double?>() { CurrentYear = null }, result.EnglandApprentice);
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
            TOT_OVERALLPER_LA_Current_Num = 77
        };

        var englandDestinations = new KS5EnglandDestinations
        {
            TOT_OVERALLPER_Eng_Current_Pct = 66
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        Assert.Equal(lADestinations.TOT_OVERALLPER_LA_Current_Num, result.LATotalOverall);
        Assert.Equal(englandDestinations.TOT_OVERALLPER_Eng_Current_Pct, result.EnglandOverall);

    }

    [Fact]
    public async Task GetKS5DestinationsDetailsAsync_ReturnsEmptyWhenNoEstablishmentFound()
    {
        // Arrange
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel());

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