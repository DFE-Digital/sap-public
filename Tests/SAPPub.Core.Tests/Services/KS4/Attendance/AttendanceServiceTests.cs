using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services.KS4.Attendance;

namespace SAPPub.Core.Tests.Services.KS4.Attendance;

public class AttendanceServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentAbsenceService> _mockEstablishmentAbsenceService;
    private readonly Mock<ILAAbsenceService> _mockLAAbsenceService;
    private readonly Mock<IEnglandAbsenceService> _mockEnglandAbsenceService;
    private readonly AttendanceService _service;

    private readonly EstablishmentServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        PhaseOfEducationName = "Secondary School",
        LAName = "Council",
        LAId = "E09000001"
    };

    public static IEnumerable<object[]> AttendanceData => 
        [
            [(est: (double?)5.55, la: (double?)10.25, eng: (double?)15.55), (est: (double?)94.4, la: (double?)89.8, eng: (double?)84.4)],
            [(est: (double?)10.12, la: (double?)3.45, eng: (double?)7.35), (est: (double?)89.9, la: (double?)96.6, eng: (double?)92.6)],
            [(est: (double?)null, la: (double?)null, eng: (double?)null), (est: (double?)null, la: (double?)null, eng: (double?)null)],
        ];

    public static IEnumerable<object[]> AbsenceData =>
        [
            [(est: (double?)5.45, la: (double?)11.25, eng: (double?)12.55), (est: (double?)5.4, la: (double?)11.2, eng: (double?)12.6)],
            [(est: (double?)10.12, la: (double?)7.45, eng: (double?)8.35), (est: (double?)10.1, la: (double?)7.4, eng: (double?)8.4)],
            [(est: (double?)null, la: (double?)null, eng: (double?)null), (est: (double?)null, la: (double?)null, eng: (double?)null)],
        ];


    public AttendanceServiceTests()
    {
        _mockEstablishmentService = new();
        _mockEstablishmentAbsenceService = new();
        _mockLAAbsenceService = new();
        _mockEnglandAbsenceService = new();

        _service = new AttendanceService(
            _mockEstablishmentService.Object,
            _mockEstablishmentAbsenceService.Object,            
            _mockEnglandAbsenceService.Object,
            _mockLAAbsenceService.Object);
    }

    [Fact]
    public async Task GetAttendenceDetailsAsync_ShouldReturnEmptyModel_WhenEstablishmentNotFound()
    {
        // Arrange
        var urn = "99999";
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentServiceModel()); // not found

        // Act
        var result = await _service.GetAttendenceDetailsAsync(urn, CancellationToken.None);

        // Assert - required members are set, but values are empty
        Assert.NotNull(result);
        Assert.Equal(urn, result.Urn);
        Assert.Null(result.SchoolName);
        Assert.Null(result.LocalAuthority);
        Assert.Null(result.EstablishmentAttendance);
        Assert.Null(result.LocalAuthorityAttendance);
        Assert.Null(result.EnglandAttendance);
    }

    [Theory]
    [MemberData(nameof(AttendanceData))]
    public async Task GetAttendenceDetailsAsync_ShouldReturnData(
        (double? est, double? la, double? eng) absence,
        (double? est, double? la, double? eng) expected)
    {
        // Arrange
        var establishmentAbsence = new EstablishmentAbsence
        {
            Id = fakeEstablishment.URN,
            Abs_Tot_Est_Current_Pct = absence.est
        };

        var lAAbsence = new LAAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Tot_LA_Current_Pct = absence.la
        };

        var englandAbsence = new EnglandAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Tot_Eng_Current_Pct = absence.eng
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockEstablishmentAbsenceService
            .Setup(r => r.GetEstablishmentAbsenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentAbsence);

        _mockLAAbsenceService
            .Setup(r => r.GetLAAbsenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lAAbsence);

        _mockEnglandAbsenceService
            .Setup(r => r.GetEnglandAbsenceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandAbsence);

        // Act
        var result = await _service.GetAttendenceDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert (common)
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthority);

        Assert.Equal(expected.est, result.EstablishmentAttendance);
        Assert.Equal(expected.la, result.LocalAuthorityAttendance);
        Assert.Equal(expected.eng, result.EnglandAttendance);
    }

    [Theory]
    [MemberData(nameof(AbsenceData))]
    public async Task GetAttendenceDetailsAsync_ShouldReturn_Absence_Data(
        (double? est, double? la, double? eng) absence,
        (double? est, double? la, double? eng) expected)
    {
        // Arrange
        var enrolmentsTotal = 1200;
        var absenceTotal = 120;

        var establishmentAbsence = new EstablishmentAbsence
        {
            Id = fakeEstablishment.URN,
            Abs_Persistent_Est_Current_Pct = absence.est,
            Enrolments_Tot_Est_Current_Num = enrolmentsTotal,
            Abs_Persistent_Est_Current_Num = absenceTotal
        };

        var lAAbsence = new LAAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Persistent_LA_Current_Pct = absence.la
        };

        var englandAbsence = new EnglandAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Persistent_Eng_Current_Pct = absence.eng
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeEstablishment);

        _mockEstablishmentAbsenceService
            .Setup(r => r.GetEstablishmentAbsenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentAbsence);

        _mockLAAbsenceService
            .Setup(r => r.GetLAAbsenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lAAbsence);

        _mockEnglandAbsenceService
            .Setup(r => r.GetEnglandAbsenceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandAbsence);

        // Act
        var result = await _service.GetAttendenceDetailsAsync(fakeEstablishment.URN, CancellationToken.None);

        // Assert (common)
        Assert.NotNull(result);
        Assert.Equal(fakeEstablishment.URN, result.Urn);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.SchoolName);
        Assert.Equal(fakeEstablishment.LAName, result.LocalAuthority);

        Assert.Equal(expected.est, result.EstablishmentPersistentAbsence);
        Assert.Equal(expected.la, result.LocalAuthorityPersistentAbsence);
        Assert.Equal(expected.eng, result.EnglandPersistentAbsence);

        Assert.Equal(enrolmentsTotal, result.EstablishmentEnrolmentsTotal);
        Assert.Equal(absenceTotal, result.EstablishmentPersistentAbsenceTotal);
    }
}
