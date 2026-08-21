using Moq;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.Services;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services;

public class AttendanceServiceTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentAbsenceService> _mockEstablishmentAbsenceService;
    private readonly Mock<ILAAbsenceService> _mockLAAbsenceService;
    private readonly Mock<IEnglandAbsenceService> _mockEnglandAbsenceService;
    private readonly AttendanceService _service;

    private readonly EstablishmentMinimumServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        LAName = "Council",
        LAId = "E09000001"
    };

    public static IEnumerable<object[]> AttendanceData => 
        [
            [(est: (double?)5.55, la: (double?)10.25, eng: (double?)15.55), (est: (double?)94.45, la: (double?)89.75, eng: (double?)84.45)],
            [(est: (double?)10.12, la: (double?)3.45, eng: (double?)7.35), (est: (double?)89.88, la: (double?)96.55, eng: (double?)92.65)],
            [(est: (double?)null, la: (double?)null, eng: (double?)null), (est: (double?)null, la: (double?)null, eng: (double?)null)],
        ];

    public static IEnumerable<object[]> AbsenceData =>
        [
            [(est: (double?)5.45, la: (double?)11.25, eng: (double?)12.55), (est: (double?)5.45, la: (double?)11.25, eng: (double?)12.55)],
            [(est: (double?)10.12, la: (double?)7.45, eng: (double?)8.35), (est: (double?)10.12, la: (double?)7.45, eng: (double?)8.35)],
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
            .Setup(r => r.GetEstablishmentMinimumAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel()); // not found

        // Act
        var result = await _service.GetAttendenceDetailsAsync(urn, CancellationToken.None);

        // Assert - required members are set, but values are empty
        Assert.NotNull(result);
        Assert.Equal(urn, result.Urn);
        Assert.Null(result.SchoolName);
        Assert.Null(result.LocalAuthority);
        Assert.False(result.EstablishmentAttendance.HasValue);
        Assert.False(result.LocalAuthorityAttendance.HasValue);
        Assert.False(result.EnglandAttendance.HasValue);
    }

    [Theory]
    [MemberData(nameof(AttendanceData))]
    public async Task GetAttendenceDetailsAsync_ShouldReturn_KS4_Attendance_Data(
        (double? est, double? la, double? eng) absence,
        (double? est, double? la, double? eng) expected)
    {
        // Arrange
        fakeEstablishment.IsKS4 = true;

        var establishmentAbsence = new EstablishmentAbsence
        {
            Id = fakeEstablishment.URN,
            Abs_Tot_Est_Current_Pct_Coded = new CodedDouble(absence.est, string.Empty, absence.est.ToString()!)
        };

        var lAAbsence = new LAAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Tot_LA_Current_Pct_Coded = new CodedDouble(absence.la, string.Empty, absence.la.ToString()!)
        };

        var englandAbsence = new EnglandAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Tot_Eng_Current_Pct_Coded = new CodedDouble(absence.eng, string.Empty, absence.eng.ToString()!)
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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

        Assert.Equal(expected.est, result.EstablishmentAttendance.Value);
        Assert.Equal(expected.la, result.LocalAuthorityAttendance.Value);
        Assert.Equal(expected.eng, result.EnglandAttendance.Value);
    }

    [Theory]
    [MemberData(nameof(AbsenceData))]
    public async Task GetAttendenceDetailsAsync_ShouldReturn_KS4_Absence_Data(
        (double? est, double? la, double? eng) absence,
        (double? est, double? la, double? eng) expected)
    {
        // Arrange
        var enrolmentsTotal = 1200;
        var absenceTotal = 120;

        fakeEstablishment.IsKS4 = true;

        var establishmentAbsence = new EstablishmentAbsence
        {
            Id = fakeEstablishment.URN,
            Abs_Persistent_Est_Current_Pct_Coded = new CodedDouble(absence.est, "", absence.eng.ToString()!),
            Enrolments_Tot_Est_Current_Num_Coded = new CodedDouble(enrolmentsTotal, "", enrolmentsTotal.ToString()),
            Abs_Persistent_Est_Current_Num_Coded = new CodedDouble(absenceTotal, "", absenceTotal.ToString())
        };

        var lAAbsence = new LAAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Persistent_LA_Current_Pct_Coded = new CodedDouble(absence.la, "", absence.la.ToString()!),
        };

        var englandAbsence = new EnglandAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Persistent_Eng_Current_Pct_Coded = new CodedDouble(absence.eng, "", absence.eng.ToString()!),
        };

        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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

        Assert.Equal(expected.est, result.EstablishmentPersistentAbsence.Value);
        Assert.Equal(expected.la, result.LocalAuthorityPersistentAbsence.Value);
        Assert.Equal(expected.eng, result.EnglandPersistentAbsence.Value);

        Assert.Equal(enrolmentsTotal, result.EstablishmentEnrolmentsTotal.Value);
        Assert.Equal(absenceTotal, result.EstablishmentPersistentAbsenceTotal.Value);
    }
}
