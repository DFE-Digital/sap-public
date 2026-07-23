using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.Attendance;
using SAPPub.Web.Constants;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class AttendanceControllerTests
{
    private readonly Mock<IAttendanceService> _mockAttendanceService = new();
    private readonly AttendanceController _controller;
    private EstablishmentServiceModel _fakeEstablishment;

    public AttendanceControllerTests()
    {
        _fakeEstablishment = new EstablishmentTestBuilder()
            .WithTrustName("Trust")
            .WithWebsite("https://www.gov.uk/")
            .WithTelephoneNum("012154896")
            .WithAddressStreet("Street")
            .WithAddressLocality("Locality")
            .WithAddressTown("Town")
            .WithAddressPostcode("Postcode")
            .WithLAName("Sheffield")
            .WithLAGssCode("123")
            .WithTypeOfEstablishmentName("EstablishmentName")
            .WithHeadteacherTitle("Title")
            .WithHeadteacherFirstName("FirstName")
            .WithHeadteacherLastName("LastName")
            .WithAgeRangeLow("11")
            .WithAgeRangeHigh("18")
            .WithTotalPupils("1117")
            .WithGenderName("GenderName")
            .WithReligiousCharacterName("ReligiousCharacter")
            .WithSixthForm(false)
            .WithResourcedProvisionName("Resourced provision")
            .WithEstablishmentTypeGroupId("1")
            .WithStatusCode(1)
            .WithOpenReasonId(10)
            .WithOpenDate()
            .WithSenTypes("VI - Visual Impairment, HI - Hearing Impairment")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new AttendanceController();

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Theory]
    [InlineData(95.5, 97.9, 93.2)]
    [InlineData(null, null, null)]
    public async Task Get_Attendance_Info_ReturnsOk(double? estAttendance, double? laAttendance, double? engAttendance)
    {
        _mockAttendanceService
            .Setup(s => s.GetAttendenceDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel
            {
                Urn = _fakeEstablishment.URN,
                SchoolName = _fakeEstablishment.EstablishmentName,
                LocalAuthority = _fakeEstablishment.LAName,
                EstablishmentAttendance = estAttendance,
                LocalAuthorityAttendance = laAttendance,
                EnglandAttendance = engAttendance,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        var result = await _controller.Attendance(_mockAttendanceService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AttendanceViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.LAName, model.LocalAuthority.Value);

        var expectedEstablishmentAttendence = estAttendance != null ? estAttendance.ToString() : NotAvailable;
        var expectedLAAttendence = laAttendance != null ? laAttendance.ToString() : NotAvailable;
        var expectedEnglandAttendence = engAttendance != null ? engAttendance.ToString() : NotAvailable;

        Assert.Equal(expectedEstablishmentAttendence, model.EstablishmentAttendance.DisplayText());
        Assert.Equal(expectedLAAttendence, model.LocalAuthorityAttendance.DisplayText());
        Assert.Equal(expectedEnglandAttendence, model.EnglandAttendance.DisplayText());

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(5.5, 7.81, 10.2)]
    [InlineData(null, null, null)]
    public async Task Get_Attendance_Absence_Info_ReturnsOk(double? estAbsence, double? laAbsence, double? engAbsence)
    {
        var enrolmentsTotal = 5550;
        var absenceTotal = 110;
        _mockAttendanceService
            .Setup(s => s.GetAttendenceDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel
            {
                Urn = _fakeEstablishment.URN,
                SchoolName = _fakeEstablishment.EstablishmentName,
                LocalAuthority = _fakeEstablishment.LAName,
                EstablishmentPersistentAbsence = estAbsence,
                LocalAuthorityPersistentAbsence = laAbsence,
                EnglandPersistentAbsence = engAbsence,
                EstablishmentEnrolmentsTotal = enrolmentsTotal,
                EstablishmentPersistentAbsenceTotal = absenceTotal,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        var result = await _controller.Attendance(_mockAttendanceService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AttendanceViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.LAName, model.LocalAuthority.Value);

        var expectedEnrolmentsTotal = enrolmentsTotal.ToString();
        var expectedAbsenceTotal = absenceTotal.ToString();

        var expectedEstablishmentAbsence = estAbsence != null ? estAbsence.ToString() : NotAvailable;
        var expectedLAAbsence = laAbsence != null ? laAbsence.ToString() : NotAvailable;
        var expectedEnglandAbsence = engAbsence != null ? engAbsence.ToString() : NotAvailable;

        Assert.Equal(expectedEstablishmentAbsence, model.EstablishmentPersistentAbsence.DisplayText());
        Assert.Equal(expectedLAAbsence, model.LocalAuthorityPersistentAbsence.DisplayText());
        Assert.Equal(expectedEnglandAbsence, model.EnglandPersistentAbsence.DisplayText());

        Assert.Equal(expectedEnrolmentsTotal, model.EstablishmentEnrolmentsTotal.DisplayText());
        Assert.Equal(expectedAbsenceTotal, model.EstablishmentPersistentAbsenceTotal.DisplayText());

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }
}
