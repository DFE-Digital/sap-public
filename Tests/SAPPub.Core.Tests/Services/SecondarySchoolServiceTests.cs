using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Destinations;
using SAPPub.Core.Services.KS4;

namespace SAPPub.Core.Tests.Services;

public class SecondarySchoolServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentDestinationsService> _mockEstablishmentDestinationsService;
    private readonly Mock<ILADestinationsService> _mockLADestinationsService;
    private readonly Mock<IEnglandDestinationsService> _mockEnglandDestinationsService;
    private readonly SecondarySchoolService _service;

    private readonly Establishment fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
    };

    public SecondarySchoolServiceTests()
    {
        _mockEstablishmentService = new();
        _mockEstablishmentDestinationsService = new();
        _mockLADestinationsService = new();
        _mockEnglandDestinationsService = new();

        _service = new SecondarySchoolService(
            _mockEstablishmentService.Object,
            _mockEstablishmentDestinationsService.Object,
            _mockLADestinationsService.Object,
            _mockEnglandDestinationsService.Object);
    }

    [Fact]
    public void GetDestinationsDetails_ShouldReturnData()
    {
        // Arrange
        var establishmentDestinations = new EstablishmentDestinations 
        {
            Id = fakeEstablishment.URN,
            AllDest_Tot_Est_Current_Pct = 100,
            AllDest_Tot_Est_Previous_Pct = 90,
            AllDest_Tot_Est_Previous2_Pct = 80
        };

        var lADestinations = new LADestinations
        {
            Id = fakeEstablishment.URN,
            AllDest_Tot_LA_Current_Pct = 70,
            AllDest_Tot_LA_Previous_Pct = 60,
            AllDest_Tot_LA_Previous2_Pct = 80
        };

        var englandDestinations = new EnglandDestinations
        {
            Id = fakeEstablishment.URN,
            AllDest_Tot_Eng_Current_Pct = 50,
            AllDest_Tot_Eng_Previous_Pct = 60,
            AllDest_Tot_Eng_Previous2_Pct = 70
        };

        _mockEstablishmentService.Setup(r => r.GetEstablishment(It.IsAny<string>())).Returns(fakeEstablishment);
        _mockEstablishmentDestinationsService.Setup(r => r.GetEstablishmentDestinations(It.IsAny<string>())).Returns(establishmentDestinations);
        _mockLADestinationsService.Setup(r => r.GetLADestinations(It.IsAny<string>())).Returns(lADestinations);
        _mockEnglandDestinationsService.Setup(r => r.GetEnglandDestinations()).Returns(englandDestinations);

        // Act
        var result = _service.GetDestinationsDetails(It.IsAny<string>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthorityName);

        // Assert school data
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Current_Pct, result.SchoolAll.CurrentYear);
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Previous_Pct, result.SchoolAll.PreviousYear);
        Assert.Equal(establishmentDestinations.AllDest_Tot_Est_Previous2_Pct, result.SchoolAll.TwoYearsAgo);

        // Assert local authority data
        Assert.Equal(lADestinations.AllDest_Tot_LA_Current_Pct, result.LocalAuthorityAll.CurrentYear);
        Assert.Equal(lADestinations.AllDest_Tot_LA_Previous_Pct, result.LocalAuthorityAll.PreviousYear);
        Assert.Equal(lADestinations.AllDest_Tot_LA_Previous2_Pct, result.LocalAuthorityAll.TwoYearsAgo);

        // Assert england data
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Current_Pct, result.EnglandAll.CurrentYear);
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Previous_Pct, result.EnglandAll.PreviousYear);
        Assert.Equal(englandDestinations.AllDest_Tot_Eng_Previous2_Pct, result.EnglandAll.TwoYearsAgo);
    }
}
