using Moq;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Absence;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Core.Services;
using SAPPub.Core.ValueObjects;

namespace SAPPub.Core.Tests.Services;

public class AttendanceServiceTests
{
    private const int EnrolmentsTotal = 1200;
    private const int PersistentAbsenceTotal = 120;

    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentAbsenceService> _mockEstablishmentAbsenceService;
    private readonly Mock<ILAAbsenceService> _mockLAAbsenceService;
    private readonly Mock<IEnglandAbsenceService> _mockEnglandAbsenceService;
    private readonly AttendanceService _service;

    private static readonly (bool isKS2, bool isKS4, bool isSpecialSchool)[] SchoolTypes =
    [
        (false, false, true),
        (false, true, false),
        (true, false, false)
    ];

    private readonly EstablishmentMinimumServiceModel fakeEstablishment = new()
    {
        URN = "123456",
        EstablishmentName = "Test Establishment",
        LAName = "Council",
        LAId = "E09000001"
    };

    private static readonly (double? est, double? la, double? eng)[] AttendanceAbsenceValues =
    [
        (5.55, 10.25, 15.55),
        (10.12, 3.45, 7.35),
        (null, null, null)
    ];

    private static readonly (double? est, double? la, double? eng)[] PersistentAbsenceValues =
    [
        (5.45, 11.25,12.55),
        (10.12, 7.45, 8.35),
        (null, null, null)
    ];

    public static IEnumerable<object[]> AttendanceScenarios => CreateScenarios(AttendanceAbsenceValues);

    public static IEnumerable<object[]> PersistentAbsenceScenarios => CreateScenarios(PersistentAbsenceValues);

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
    public async Task GetAttendanceDetailsAsync_ShouldReturnEmptyModel_WhenEstablishmentNotFound()
    {
        // Arrange
        var urn = "99999";
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(urn, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EstablishmentMinimumServiceModel());

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
    [MemberData(nameof(AttendanceScenarios))]
    public async Task GetAttendenceDetailsAsync_ShouldReturn_AttendanceData_For_SchoolType(
        bool isKS2, bool isKS4,bool isSpecialSchool,
        (double? est, double? la, double? eng) absence)
    {
        // Arrange
        var establishment = CreateEstablishment(isKS2, isKS4, isSpecialSchool);

        var establishmentAbsence = new EstablishmentAbsence
        {
            Id = fakeEstablishment.URN,
            Abs_Tot_Est_Current_Pct_Coded = CreateCoded(absence.est),
            Abs_TotKS2_Est_Current_Pct_Coded = CreateCoded(absence.est),
            Abs_TotSPE_Est_Current_Pct_Coded = CreateCoded(absence.est)
        };

        var lAAbsence = new LAAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Tot_LA_Current_Pct_Coded = CreateCoded(absence.la),
            Abs_TotKS2_LA_Current_Pct_Coded = CreateCoded(absence.la),
            Abs_TotSPE_LA_Current_Pct_Coded = CreateCoded(absence.la),
        };

        var englandAbsence = new EnglandAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Tot_Eng_Current_Pct_Coded = CreateCoded(absence.eng),
            Abs_TotKS2_Eng_Current_Pct_Coded = CreateCoded(absence.eng),
            Abs_TotSPE_Eng_Current_Pct_Coded = CreateCoded(absence.eng),
        };

        SeupMocks(establishment, establishmentAbsence, lAAbsence, englandAbsence);

        // Act
        var result = await _service.GetAttendenceDetailsAsync(establishment.URN, CancellationToken.None);

        // Assert (common)
        AssertCommon(result, establishment);

        Assert.Equal(GetAsAttendance(absence.est), result.EstablishmentAttendance.Value);
        Assert.Equal(GetAsAttendance(absence.la), result.LocalAuthorityAttendance.Value);
        Assert.Equal(GetAsAttendance(absence.eng), result.EnglandAttendance.Value);
        Assert.Equal(isKS2, result.IsKS2);
        Assert.Equal(isKS4, result.IsKS4);
    }


    [Theory]
    [MemberData(nameof(PersistentAbsenceScenarios))]
    public async Task GetAttendanceDetailsAsync_ShouldReturn_PersisentAbsenceData_For_SchoolTyp(
        bool isKS2, bool isKS4, bool isSpecialSchool,
        (double? est, double? la, double? eng) absence)
    {
        // Arrange
        var establishment = CreateEstablishment(isKS2, isKS4, isSpecialSchool);

        fakeEstablishment.IsKS4 = true;

        var establishmentAbsence = new EstablishmentAbsence
        {
            Id = fakeEstablishment.URN,
            Enrolments_Tot_Est_Current_Num_Coded = CreateCoded(EnrolmentsTotal),
            Abs_Persistent_Est_Current_Pct_Coded = CreateCoded(absence.est),
            Abs_PersistentKS2_Est_Current_Pct_Coded = CreateCoded(absence.est),
            Abs_PersistentSPE_Est_Current_Pct_Coded = CreateCoded(absence.est),
            Abs_Persistent_Est_Current_Num_Coded = CreateCoded(PersistentAbsenceTotal),
            Abs_PersistentKS2_Est_Current_Num_Coded = CreateCoded(PersistentAbsenceTotal),
            Abs_PersistentSPE_Est_Current_Num_Coded = CreateCoded(PersistentAbsenceTotal)
        };

        var lAAbsence = new LAAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Persistent_LA_Current_Pct_Coded = CreateCoded(absence.la),
            Abs_PersistentKS2_LA_Current_Pct_Coded = CreateCoded(absence.la),
            Abs_PersistentSPE_LA_Current_Pct_Coded = CreateCoded(absence.la),
        };

        var englandAbsence = new EnglandAbsence
        {
            Id = fakeEstablishment.LAId,
            Abs_Persistent_Eng_Current_Pct_Coded = CreateCoded(absence.eng),
            Abs_PersistentKS2_Eng_Current_Pct_Coded = CreateCoded(absence.eng),
            Abs_PersistentSPE_Eng_Current_Pct_Coded = CreateCoded(absence.eng),
        };

        SeupMocks(establishment, establishmentAbsence, lAAbsence, englandAbsence);

        // Act
        var result = await _service.GetAttendenceDetailsAsync(establishment.URN, CancellationToken.None);

        // Assert (common)
        AssertCommon(result, establishment);

        Assert.Equal(absence.est, result.EstablishmentPersistentAbsence.Value);
        Assert.Equal(absence.la, result.LocalAuthorityPersistentAbsence.Value);
        Assert.Equal(absence.eng, result.EnglandPersistentAbsence.Value);
        Assert.Equal(EnrolmentsTotal, result.EstablishmentEnrolmentsTotal.Value);
        Assert.Equal(PersistentAbsenceTotal, result.EstablishmentPersistentAbsenceTotal.Value);
        Assert.Equal(isKS2, result.IsKS2);
        Assert.Equal(isKS4, result.IsKS4);
    }

    private void SeupMocks(
        EstablishmentMinimumServiceModel establishment, 
        EstablishmentAbsence establishmentAbsence, 
        LAAbsence lAAbsence, 
        EnglandAbsence englandAbsence)
    {
        _mockEstablishmentService
            .Setup(r => r.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishment);

        _mockEstablishmentAbsenceService
            .Setup(r => r.GetEstablishmentAbsenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(establishmentAbsence);

        _mockLAAbsenceService
            .Setup(r => r.GetLAAbsenceAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(lAAbsence);

        _mockEnglandAbsenceService
            .Setup(r => r.GetEnglandAbsenceAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(englandAbsence);
    }

    private static EstablishmentMinimumServiceModel CreateEstablishment(bool isKS2, bool isKS4, bool isSpecialSchool)
    {
        return new EstablishmentMinimumServiceModel
        {
            URN = "123456",
            EstablishmentName = "School",
            LAName = "TEST LA",
            LAId= "123",
            IsKS2 = isKS2,
            IsKS4 = isKS4,
            TypeOfEstablishment = isSpecialSchool ? TypeOfEstablishment.CommunitySchool : TypeOfEstablishment.CommunitySchool
        };
    }

    private static CodedDouble CreateCoded(double? value)
    {
        return new CodedDouble(value, string.Empty, value?.ToString() ?? string.Empty);
    }

    private static CodedDouble CreateCoded(int value)
    {
        return new CodedDouble(value, string.Empty, value.ToString());
    }

    private static IEnumerable<object[]> CreateScenarios((double? est, double? la, double? eng)[] vals)
    {
        foreach (var schoolType in SchoolTypes)
        {
            foreach (var value in vals)
            {
                yield return [schoolType.isKS2, schoolType.isKS4, schoolType.isSpecialSchool, value];
            }
        }
    }   

    private static void AssertCommon(AttendanceModel result, EstablishmentMinimumServiceModel establishment)
    {
        Assert.NotNull(result);
        Assert.Equal(establishment.URN, result.Urn);
        Assert.Equal(establishment.EstablishmentName, result.SchoolName);
        Assert.Equal(establishment.LAName, result.LocalAuthority);
    }

    private static double? GetAsAttendance(double? absence)
    {
        return absence.HasValue ? 100 - absence.Value : null;
    }
}
