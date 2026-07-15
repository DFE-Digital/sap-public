using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Constants;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class SecondarySchoolControllerTests
{
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IDestinationsService> _mockDestinationsService;
    private readonly Mock<IEstablishmentSubjectEntriesService> _mockEstablishmentSubjectEntriesService = new();
    private readonly Mock<IAcademicPerformanceEnglishAndMathsResultsService> _mockEnglishAndMathsResultsService = new();
    private readonly Mock<IAttainmentAndProgressService> _mockAttainmentAndProgressService = new();
    private readonly Mock<IAdmissionsService> _mockAdmissionsService = new();
    private readonly Mock<IAttendanceService> _mockAttendanceService = new();
    private readonly Mock<IAdditionalMeasuresService> _mockAdditionalMeasuresService = new();
    private readonly SecondarySchoolController _controller;
    private EstablishmentServiceModel _fakeEstablishment;

    private List<SubjectsEntered> GcseSubjects =
        new()
        {
            new()
            {
                Subject = "English language",
                Qualification = "GCSE",
                TotalNumberOfEntries = 95,
            },
            new()
            {
                Subject = "English literature",
                Qualification = "GCSE",
                TotalNumberOfEntries = 90,
            }
        };

    private List<SubjectsEntered> VocationalSubjects =
        new()
        {
            new()
            {
                Subject = "Sports Studies",
                Qualification = "Vocational",
                TotalNumberOfEntries = 45,
            },
            new()
            {
                Subject = "Engineering Studies",
                Qualification = "Vocational",
                TotalNumberOfEntries = 10,
            }
        };

    private List<SubjectsEntered> OtherSubjects =
    new()
    {
            new()
            {
                Subject = "Additional Maths (FSMQ)",
                Qualification = "FSMQ",
                TotalNumberOfEntries = 45,
            },
            new()
            {
                Subject = "Grade 6 Performing Arts Graded Examination",
                Qualification = "Music Performance: Group",
                TotalNumberOfEntries = 10,
            }
    };

    private EnglishAndMathsResultsModel EnglishAndMathsResults(
        string urn = "123456",
        string establishmentName = "School Name",
        string laName = "Sheffield") => new()
        {
            Urn = urn,
            SchoolName = establishmentName,
            LAName = laName,
            EstablishmentAll = new RelativeYearValues<double?>
            {
                CurrentYear = 60,
                PreviousYear = 80,
                TwoYearsAgo = 60
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = 80,
                PreviousYear = 70,
                TwoYearsAgo = 80
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
                PreviousYear = 70,
                TwoYearsAgo = 80
            },
            EstablishmentBoys = new RelativeYearValues<double?>
            {
                CurrentYear = 50
            },
            LocalAuthorityBoys = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
            },
            EnglandBoys = new RelativeYearValues<double?>
            {
                CurrentYear = 60,
            },
            EstablishmentGirls = new RelativeYearValues<double?>
            {
                CurrentYear = 80
            },
            LocalAuthorityGirls = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
            },
            EnglandGirls = new RelativeYearValues<double?>
            {
                CurrentYear = 90,
            },
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

    private AboutSchoolModel SchoolDetails()
    {
        return new AboutSchoolModel
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            AcademyTrust = _fakeEstablishment.TrustName,
            Website = _fakeEstablishment.Website,
            Telephone = _fakeEstablishment.TelephoneNum,
            Address = _fakeEstablishment.Address,
            LocalAuthority = _fakeEstablishment.LAName,
            LocalAuthorityName = _fakeEstablishment.LAName,
            LocalAuthorityWebsite = "www.gov.uk",
            Easting = "50.01",
            Northing = "60.90",
            TypeOfSchool = _fakeEstablishment.TypeOfEstablishmentName,
            HeadTeacher = _fakeEstablishment.Headteacher,
            AgeRange = _fakeEstablishment.AgeRange,
            NumberOfPupils = _fakeEstablishment.TotalPupils,
            PupilSex = _fakeEstablishment.GenderName,
            ReligiousCharacter = _fakeEstablishment.ReligiousCharacterName,
            OfficialSixthFormId = _fakeEstablishment.OfficialSixthFormId,
            ResourcedProvisionName = _fakeEstablishment.ResourcedProvisionName,
            EstablishmentTypeGroupId = _fakeEstablishment.EstablishmentTypeGroupId,
            Status = _fakeEstablishment.StatusCode.ToStatus(),
            ClosedDate = _fakeEstablishment.ClosedDate.ToDateOnly(),
            OpenReasonId = _fakeEstablishment.OpenReasonId,
            OpenDate = _fakeEstablishment.OpenDate.ToDateOnly(),
            IsKS2 = true,
            IsKS4 = true
        };
    }

    public SecondarySchoolControllerTests()
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

        _mockEstablishmentService = new();
        _mockDestinationsService = new();

        _mockEstablishmentService
            .Setup(es => es.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fakeEstablishment);

        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockEstablishmentService.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Get_Admissions_Info_ReturnsExpectedViewModel()
    {
        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel(
                SchoolName: _fakeEstablishment.EstablishmentName,
                SchoolWebsite: _fakeEstablishment.Website,
                LAName: laName,
                LASchoolAdmissionsUrl: lASchoolAdmissionsUrl,
                EstablishmentStatus: EstablishmentStatus.Open,
                IsKS2: false,
                IsKS4: true,
                IsKS5: false

            ));

        var result = await _controller.Admissions(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.SchoolWebsite.Value);
        Assert.Equal(lASchoolAdmissionsUrl, model.LASecondarySchoolAdmissionsLinkUrl);
        Assert.Equal(laName, model.LAName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.False(model.IsSchoolClosed);
    }

    [Theory]
    [InlineData(null, FieldStatus.NotAvailable)]
    [InlineData("", FieldStatus.NotAvailable)]
    [InlineData(" ", FieldStatus.NotAvailable)]
    [InlineData("test", FieldStatus.Available)]
    public async Task Get_Admissions_Info_SchoolWebsite(string? website, FieldStatus fieldStatus)
    {
        _fakeEstablishment.Website = website!;

        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel(
                SchoolName: _fakeEstablishment.EstablishmentName,
                SchoolWebsite: _fakeEstablishment.Website,
                LAName: laName,
                LASchoolAdmissionsUrl: lASchoolAdmissionsUrl,
                EstablishmentStatus: EstablishmentStatus.Open,
                IsKS2: false,
                IsKS4: true,
                IsKS5: false
            ));

        var result = await _controller.Admissions(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;

        Assert.NotNull(model);
        Assert.Equal(fieldStatus, model.SchoolWebsite.Status);

        if (fieldStatus == FieldStatus.Available)
        {
            Assert.False(model.SchoolWebsite.IsNotAvailable);
            Assert.True(model.SchoolWebsite.IsAvailable);
            Assert.Equal(website, model.SchoolWebsite.Value);
            Assert.Equal(website, model.SchoolWebsite.DisplayText());
        }
        else
        {
            Assert.False(model.SchoolWebsite.IsAvailable);
            Assert.True(model.SchoolWebsite.IsNotAvailable);
            Assert.Equal("Not available", model.SchoolWebsite.DisplayText());
        }

        Assert.False(model.IsSchoolClosed);
    }

    [Theory]
    [InlineData(EstablishmentStatus.Open, false)]
    [InlineData(EstablishmentStatus.Closed, true)]
    public async Task Get_Admissions_Info_IsSchoolClosed(EstablishmentStatus? statusCode, bool expectedResult)
    {
        _fakeEstablishment.StatusCode = statusCode.ToStatusCode();

        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel(
                SchoolName: _fakeEstablishment.EstablishmentName,
                SchoolWebsite: _fakeEstablishment.Website,
                LAName: laName,
                LASchoolAdmissionsUrl: lASchoolAdmissionsUrl,
                EstablishmentStatus: statusCode,
                IsKS2: false,
                IsKS4: true,
                IsKS5: false
            ));

        var result = await _controller.Admissions(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;

        Assert.NotNull(model);
        Assert.Equal(expectedResult, model.IsSchoolClosed);
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

    [Fact]
    public async Task Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        var result = await _controller.CurriculumAndExtraCurricularActivities(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.SchoolWebsite.Value);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(null, FieldStatus.NotAvailable)]
    [InlineData("", FieldStatus.NotAvailable)]
    [InlineData(" ", FieldStatus.NotAvailable)]
    [InlineData("test", FieldStatus.Available)]
    public async Task Get_CurriculumAndExtraCurricularActivities_Info_SchoolWebsite(string? website, FieldStatus fieldStatus)
    {
        _fakeEstablishment.Website = website!;

        var result = await _controller.CurriculumAndExtraCurricularActivities(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;

        Assert.NotNull(model);
        Assert.Equal(fieldStatus, model.SchoolWebsite.Status);

        if (fieldStatus == FieldStatus.Available)
        {
            Assert.False(model.SchoolWebsite.IsNotAvailable);
            Assert.True(model.SchoolWebsite.IsAvailable);
            Assert.Equal(website, model.SchoolWebsite.Value);
            Assert.Equal(website, model.SchoolWebsite.DisplayText());
        }
        else
        {
            Assert.False(model.SchoolWebsite.IsAvailable);
            Assert.True(model.SchoolWebsite.IsNotAvailable);
            Assert.Equal("Not available", model.SchoolWebsite.DisplayText());
        }
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current, true)]
    [InlineData(AcademicYearSelection.Previous, false)]
    [InlineData(AcademicYearSelection.Previous2, false)]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_Info_ReturnsOk(AcademicYearSelection academicYearSelection, bool expectedShowProgress8NotAvailableInfo)
    {
        var expectedResult = new AttainmentAndProgressModel
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            EstablishmentProgress8Score = expectedShowProgress8NotAvailableInfo ? null : 0.9,
            EstablishmentProgress8CILower = expectedShowProgress8NotAvailableInfo ? null : -0.2,
            EstablishmentProgress8CIUpper = expectedShowProgress8NotAvailableInfo ? null : 1.2,
            EstablishmentProgress8Banding = expectedShowProgress8NotAvailableInfo ? null : "Average",
            LocalAuthorityProgress8Score = expectedShowProgress8NotAvailableInfo ? null : 1.5,
            EstablishmentAttainment8Score = 70,
            LocalAuthorityAttainment8Score = 80,
            EnglandAttainment8Score = 50,
            EstablishmentProgress8TotalPupils = expectedShowProgress8NotAvailableInfo ? null : 65,
            EstablishmentTotalPupils = expectedShowProgress8NotAvailableInfo ? null : 95,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, academicYearSelection, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            academicYearSelection,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(3, model.AcademicYearsSelectList.Count);
        Assert.Equal(academicYearSelection, model.SelectedAcademicYear);
        Assert.Equal($"Information in this section is for the {academicYearSelection.GetDisplayName()} academic year.", model.AcademicYearInfoParagraph);
        Assert.Equal(expectedShowProgress8NotAvailableInfo, model.ShowProgress8NotAvailableInfo);

        Assert.Equal(expectedResult.EstablishmentAttainment8Score, model.EstablishmentAttainment8Score);
        Assert.Equal(expectedResult.LocalAuthorityAttainment8Score, model.LocalAuthorityAttainment8Score);
        Assert.Equal(expectedResult.EnglandAttainment8Score, model.EnglandAttainment8Score);

        if (expectedShowProgress8NotAvailableInfo)
        {
            Assert.Null(model.EstablishmentProgress8Score);
            Assert.Null(model.EstablishmentProgress8CILower);
            Assert.Null(model.EstablishmentProgress8CIUpper);
            Assert.Null(model.EstablishmentProgress8Banding);
            Assert.Null(model.LocalAuthorityProgress8Score);
            Assert.Null(model.EstablishmentProgress8TotalPupils);
            Assert.Null(model.EstablishmentTotalPupils);
        }
        else
        {
            Assert.Equal(expectedResult.EstablishmentProgress8Score, model.EstablishmentProgress8Score);
            Assert.Equal(expectedResult.EstablishmentProgress8CILower, model.EstablishmentProgress8CILower);
            Assert.Equal(expectedResult.EstablishmentProgress8CIUpper, model.EstablishmentProgress8CIUpper);
            Assert.Equal(expectedResult.EstablishmentProgress8Banding, model.EstablishmentProgress8Banding);
            Assert.Equal(expectedResult.LocalAuthorityProgress8Score, model.LocalAuthorityProgress8Score);
            Assert.Equal(expectedResult.EstablishmentProgress8TotalPupils, model.EstablishmentProgress8TotalPupils);
            Assert.Equal(expectedResult.EstablishmentTotalPupils, model.EstablishmentTotalPupils);
        }
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current, true)]
    [InlineData(AcademicYearSelection.Previous, false)]
    [InlineData(AcademicYearSelection.Previous2, false)]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_Display_Attainment8_Data(
        AcademicYearSelection academicYearSelection,
        bool expectedShowAttainment8Info)
    {
        var expectedResult = new AttainmentAndProgressModel
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            EstablishmentProgress8Score = 0.9,
            LocalAuthorityProgress8Score = 1.5,
            EstablishmentAttainment8Score = expectedShowAttainment8Info ? 70 : null,
            LocalAuthorityAttainment8Score = 80,
            EnglandAttainment8Score = 50,
            EstablishmentProgress8TotalPupils = 65,
            EstablishmentTotalPupils = 95,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, academicYearSelection, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            academicYearSelection,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(3, model.AcademicYearsSelectList.Count);
        Assert.Equal(academicYearSelection, model.SelectedAcademicYear);
        Assert.Equal(expectedShowAttainment8Info, model.ShowAttainment8Info);
    }

    [Theory]
    [InlineData(GcseGradeDataSelection.Grade4AndAbove)]
    [InlineData(GcseGradeDataSelection.Grade5AndAbove)]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_ReturnsOk(GcseGradeDataSelection grade)
    {
        var expectedResult = EnglishAndMathsResults(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, _fakeEstablishment.LAName);

        _mockEnglishAndMathsResultsService
            .Setup(s => s.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)grade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            grade,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(grade, model.SelectedGrade);
        Assert.Equal(["School", $"{_fakeEstablishment.LAName} average", "England average"], model.AllGcseData.Labels);
        Assert.Equal(
            [
                expectedResult.EstablishmentAll.CurrentYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value,
                expectedResult.EnglandAll.CurrentYear!.Value
            ],
            model.AllGcseData.Data
        );

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);

        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal(
            [
                expectedResult.EstablishmentAll.TwoYearsAgo!.Value,
                expectedResult.EstablishmentAll.PreviousYear!.Value,
                expectedResult.EstablishmentAll.CurrentYear!.Value
            ],
            model.AllGcseOverTimeData.Datasets[0].Data
        );

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal(
            [
                expectedResult.LocalAuthorityAll.TwoYearsAgo!.Value,
                expectedResult.LocalAuthorityAll.PreviousYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value
            ],
            model.AllGcseOverTimeData.Datasets[1].Data
        );

        Assert.Equal("England average", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal(
            [
                expectedResult.EnglandAll.TwoYearsAgo!.Value,
                expectedResult.EnglandAll.PreviousYear!.Value,
                expectedResult.EnglandAll.CurrentYear!.Value],
            model.AllGcseOverTimeData.Datasets[2].Data);


        // Breakdown gcse data assert
        Assert.Equal(["Girls", "Boys"], model.BreakdownGcseData.Labels);

        Assert.Equal(3, model.BreakdownGcseData.Datasets.Count);

        Assert.Equal("School", model.BreakdownGcseData.Datasets[0].Label);
        Assert.Equal([
                expectedResult.EstablishmentGirls.CurrentYear!.Value,
                expectedResult.EstablishmentBoys.CurrentYear!.Value],
            model.BreakdownGcseData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.BreakdownGcseData.Datasets[1].Label);
        Assert.Equal([
                expectedResult.LocalAuthorityGirls.CurrentYear!.Value,
                expectedResult.LocalAuthorityBoys.CurrentYear!.Value],
            model.BreakdownGcseData.Datasets[1].Data);

        Assert.Equal("England average", model.BreakdownGcseData.Datasets[2].Label);
        Assert.Equal([
                expectedResult.EnglandGirls.CurrentYear!.Value,
                expectedResult.EnglandBoys.CurrentYear!.Value],
            model.BreakdownGcseData.Datasets[2].Data);
    }

    [Fact]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_ResultsNotAvailable_ReturnsOk()
    {
        var gradeSelection = GcseGradeDataSelection.Grade4AndAbove;

        EnglishAndMathsResultsModel serviceModel = new()
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            LAName = _fakeEstablishment.LAName,
            EstablishmentAll = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityAll = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandAll = new RelativeYearValues<double?> { CurrentYear = null },
            EstablishmentBoys = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityBoys = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandBoys = new RelativeYearValues<double?> { CurrentYear = null },
            EstablishmentGirls = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityGirls = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandGirls = new RelativeYearValues<double?> { CurrentYear = null },
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        _mockEnglishAndMathsResultsService
            .Setup(s => s.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)gradeSelection, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceModel);

        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            gradeSelection,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(gradeSelection, model.SelectedGrade);
        Assert.Equal(["School", $"{_fakeEstablishment.LAName} average", "England average"], model.AllGcseData.Labels);
        Assert.Equal([null, null, null], model.AllGcseData.Data);

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);
        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[1].Data);

        Assert.Equal("England average", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal(new double?[] { null, null, null }, model.AllGcseOverTimeData.Datasets[2].Data);

        // Breakdown gcse data assert
        Assert.Equal(["Girls", "Boys"], model.BreakdownGcseData.Labels);

        Assert.Equal(3, model.BreakdownGcseData.Datasets.Count);

        Assert.Equal("School", model.BreakdownGcseData.Datasets[0].Label);
        Assert.Equal([null, null], model.BreakdownGcseData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.BreakdownGcseData.Datasets[1].Label);
        Assert.Equal([null, null], model.BreakdownGcseData.Datasets[1].Data);

        Assert.Equal("England average", model.BreakdownGcseData.Datasets[2].Label);
        Assert.Equal([null, null], model.BreakdownGcseData.Datasets[2].Data);
    }


    [Theory]
    [InlineData("Sheffield", "Sheffield average")]
    [InlineData("Poole Grammar School", "Local council average")]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_LocalCouncilName(string localCouncilName, string expectedCouncilName)
    {
        _fakeEstablishment.LAName = localCouncilName;
        var grade = GcseGradeDataSelection.Grade4AndAbove;
        var expectedResult = EnglishAndMathsResults(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, _fakeEstablishment.LAName);

        _mockEnglishAndMathsResultsService
            .Setup(s => s.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)grade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            grade,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

        string[] expectedAllGcseDataLabels = ["School", expectedCouncilName, "England average"];
        string[] expectedAllGcseOverTimeDataLabels = ["School", expectedCouncilName, "England average"];
        string[] expectedBreakdownGcseDataLabels = ["School", expectedCouncilName, "England average"];


        Assert.Equal(expectedAllGcseDataLabels, model.AllGcseData.Labels);

        var actualAllGcseOverTimeDataLabels = model.AllGcseOverTimeData.Datasets.Select(s => s.Label).ToArray();
        Assert.Equal(expectedAllGcseOverTimeDataLabels, actualAllGcseOverTimeDataLabels);

        var actualBreakdownGcseDataLabels = model.BreakdownGcseData.Datasets.Select(s => s.Label).ToArray();
        Assert.Equal(actualBreakdownGcseDataLabels, expectedBreakdownGcseDataLabels);
    }

    [Fact]
    public async Task Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        _mockEstablishmentSubjectEntriesService
            .Setup(s => s.GetSubjectEntriesByUrnAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GcseSubjects, VocationalSubjects, OtherSubjects));

        var result = await _controller.AcademicPerformanceSubjectsEntered(
            _mockEstablishmentSubjectEntriesService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceSubjectsEnteredViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

        Assert.NotNull(model);
        Assert.Equal(
            GcseSubjects.Select(c => c.Subject).OrderBy(s => s),
            model?.GcseSubjects?.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           GcseSubjects.Select(c => $"{c.TotalNumberOfEntries:F0}").OrderBy(s => s),
           model?.GcseSubjects?.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            GcseSubjects.Select(c => c.Qualification).OrderBy(s => s),
            model?.GcseSubjects?.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.NotNull(model?.VocationalSubjects);
        Assert.Equal(
            VocationalSubjects.Select(c => c.Subject).OrderBy(s => s),
            model?.VocationalSubjects?.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           VocationalSubjects.Select(c => $"{c.TotalNumberOfEntries:F0}").OrderBy(s => s),
           model?.VocationalSubjects?.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            VocationalSubjects.Select(c => c.Qualification).OrderBy(s => s),
            model?.VocationalSubjects?.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.NotNull(model?.OtherSubjects);
        Assert.Equal(
            OtherSubjects.Select(c => c.Subject).OrderBy(s => s),
            model?.OtherSubjects?.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           OtherSubjects.Select(c => $"{c.TotalNumberOfEntries:F0}").OrderBy(s => s),
           model?.OtherSubjects?.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            OtherSubjects.Select(c => c.Qualification).OrderBy(s => s),
            model?.OtherSubjects?.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.Equal(2, model?.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model!.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public async Task Get_AdditionalMeasures_ReturnsExpected()
    {
        // Arrange
        var additionalMeasures = new AdditionalMeasuresModel()
        {
            EnglandCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            LocalAuthorityCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build(),
            EstablishmentCurrentYear = new AdditionalMeasuresBuilder().WithAutoPopulatedValues().Build()
        };

        _mockAdditionalMeasuresService
            .Setup(s => s.GetAsync(_fakeEstablishment.URN, It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(additionalMeasures);

        // Act
        var result = await _controller.AcademicPerformanceAdditionalMeasures(
            _mockAdditionalMeasuresService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAdditionalMeasuresViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        AssertAdditionalMeasuresData(additionalMeasures.EstablishmentCurrentYear, model.MeasuresInTableFormat.Select(d => d.EstablishmentCurrentYear).ToArray());
        AssertAdditionalMeasuresData(additionalMeasures.LocalAuthorityCurrentYear, model.MeasuresInTableFormat.Select(d => d.LocalAuthorityCurrentYear).ToArray());
        AssertAdditionalMeasuresData(additionalMeasures.EnglandCurrentYear, model.MeasuresInTableFormat.Select(d => d.EnglandCurrentYear).ToArray());

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    private void AssertAdditionalMeasuresData(AdditionalMeasures expected, DisplayField<CodedDouble>[] performance)
    {
        Assert.Equal(expected.PercentAchievingAtLeastOneQualification.Value, performance[0].Value.Value);
        Assert.Equal(expected.PercentEnteredForTripleScience.Value, performance[1].Value.Value);
        Assert.Equal(expected.PercentEnteredMoreThanOneForeignLanguage.Value, performance[2].Value.Value);
        Assert.Equal(expected.AverageGCSEExamEntriesPerPupil.Value, performance[3].Value.Value);
        Assert.Equal(expected.AverageAllKS4QualificationsExamEntriesPerPupil.Value, performance[4].Value.Value);
    }
}
