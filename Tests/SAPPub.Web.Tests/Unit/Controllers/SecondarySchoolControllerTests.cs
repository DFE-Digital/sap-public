using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Constants;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class SecondarySchoolControllerTests
{
    private readonly Mock<ILogger<SecondarySchoolController>> _mockLogger;
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IDestinationsService> _mockDestinationsService;
    private readonly Mock<IEstablishmentSubjectEntriesService> _mockEstablishmentSubjectEntriesService = new();
    private readonly Mock<IAcademicPerformanceEnglishAndMathsResultsService> _mockEnglishAndMathsResultsService = new();
    private readonly Mock<IAttainmentAndProgressService> _mockAttainmentAndProgressService = new();
    private readonly Mock<IAdmissionsService> _mockAdmissionsService = new();
    private readonly Mock<IAboutSchoolService> _mockAboutSchoolService = new();
    private readonly SecondarySchoolController _controller;
    private Establishment _fakeEstablishment;

    private List<EstablishmentCoreSubjectEntries.SubjectEntry> CoreSubjects =
        new()
        {
            new()
            {
                SubEntCore_Sub_Est_Current_Num = "English language",
                SubEntCore_Qual_Est_Current_Num = "GCSE",
                SubEntCore_Entr_Est_Current_Num = 95,
            },
            new()
            {
                SubEntCore_Sub_Est_Current_Num = "English literature",
                SubEntCore_Qual_Est_Current_Num = "GCSE",
                SubEntCore_Entr_Est_Current_Num = 90,
            }
        };

    private List<EstablishmentAdditionalSubjectEntries.SubjectEntry> AdditionalSubjects =
        new()
        {
            new()
            {
                SubEntAdd_Sub_Est_Current_Num = "Geography",
                SubEntAdd_Qual_Est_Current_Num = "GCSE",
                SubEntAdd_Entr_Est_Current_Num = 45,
            },
            new()
            {
                SubEntAdd_Sub_Est_Current_Num = "Music",
                SubEntAdd_Qual_Est_Current_Num = "GCSE",
                SubEntAdd_Entr_Est_Current_Num = 10,
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
            StatusCode = _fakeEstablishment.StatusCode,
            ClosedDate = _fakeEstablishment.ClosedDate.ToDateOnly()
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
            .WithOfficialSixthFormId("No")
            .WithResourcedProvisionName("Resourced provision")
            .WithEstablishmentTypeGroupId("1")
            .WithStatusCode(1)
            .Build();

        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();
        _mockEstablishmentService = new();
        _mockDestinationsService = new();

        _mockEstablishmentService
            .Setup(es => es.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fakeEstablishment);

        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockLogger.Object, _mockEstablishmentService.Object, _mockDestinationsService.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Get_AboutSchool_Info_ReturnsOk()
    {
        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(expectedResult.Website, model.SchoolWebsite.Value);
        Assert.Equal(expectedResult.AcademyTrust, model.AcademyTrust.Value);
        Assert.Equal(expectedResult.AcademyTrustUpdatedIn, model.AcademyTrustUpdatedIn.Value);
        Assert.Equal(expectedResult.Telephone, model.Telephone);
        Assert.Equal(expectedResult.LocalAuthority, model.LocalAuthority);
        Assert.Equal(expectedResult.LocalAuthorityName, model.LocalAuthorityCouncilName);
        Assert.Equal(expectedResult.LocalAuthorityWebsite, model.LocalAuthorityWebsite);
        Assert.Equal(expectedResult.TypeOfSchool, model.TypeOfSchool);
        Assert.Equal(expectedResult.HeadTeacher, model.HeadTeacher);
        Assert.Equal(expectedResult.AgeRange, model.AgeRange);
        Assert.Equal("1,117", model.NumberOfPupils);
        Assert.Equal(expectedResult.PupilSex, model.PupilSex);
        Assert.Equal(expectedResult.ReligiousCharacter, model.ReligiousCharacter);
        Assert.Equal(expectedResult.OfficialSixthFormId, model.SixthForm);
        Assert.Equal(expectedResult.StatusCode, model.StatusCode);
        Assert.False(model.ClosedDate.IsAvailable);
        Assert.False(model.IsLocalAuthoritySchool);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(expectedResult.Urn, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(expectedResult.SchoolName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(null, FieldStatus.NotAvailable)]
    [InlineData("", FieldStatus.NotAvailable)]
    [InlineData(" ", FieldStatus.NotAvailable)]
    [InlineData("test", FieldStatus.Available)]
    public async Task Get_AboutSchool_SchoolFeatures_SchoolWebsite(string? website, FieldStatus fieldStatus)
    {
        _fakeEstablishment.Website = website!;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
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
    [InlineData("100", "100")]
    [InlineData("1117", "1,117")]
    [InlineData("50000", "50,000")]
    [InlineData("2,500", "2,500")]
    [InlineData("Test", "Test")]
    public async Task Get_AboutSchool_SchoolFeatures_NumberOfPupils_Format(string totalPupils, string expectedOutput)
    {
        _fakeEstablishment.TotalPupils = totalPupils;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.NumberOfPupils);
    }

    [Theory]
    [InlineData("", "No")]
    [InlineData("Not applicable", "No")]
    [InlineData("Resourced provision", "No")]
    [InlineData("SEN unit", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public async Task Get_AboutSchool_SchoolFeatures_SENUnit(string resourcedProvisionName, string expectedOutput)
    {
        _fakeEstablishment.ResourcedProvisionName = resourcedProvisionName;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.SenUnit);

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData("", "No")]
    [InlineData("Not applicable", "No")]
    [InlineData("SEN unit", "No")]
    [InlineData("Resourced provision", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public async Task Get_AboutSchool_SchoolFeatures_ResourcedUnit(string resourcedProvisionName, string expectedOutput)
    {
        _fakeEstablishment.ResourcedProvisionName = resourcedProvisionName;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.ResourcedProvision);

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData("", "No")]
    [InlineData("2", "No")]
    [InlineData("9", "No")]
    [InlineData("1", "Yes")]
    public async Task Get_AboutSchool_SchoolFeatures_SixthForm_Format(string sixthFormId, string expectedOutput)
    {
        _fakeEstablishment.OfficialSixthFormId = sixthFormId;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.SixthForm);
    }

    [Theory]
    [InlineData("4", true)]
    [InlineData("2", false)]
    [InlineData("9", false)]
    public async Task Get_AboutSchool_SchoolFeatures_IsLocalAuthoritySchool(string establishmentTypeGroupId, bool expectedOutput)
    {
        _fakeEstablishment.EstablishmentTypeGroupId = establishmentTypeGroupId;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.IsLocalAuthoritySchool);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    public async Task Get_AboutSchool_SchoolFeatures_SchoolClosed(int? statusCode, bool expectedOutput)
    {
        _fakeEstablishment.StatusCode = statusCode;
        _fakeEstablishment.ClosedDate = _fakeEstablishment.StatusCode == 2 ? "03-03-2025" : null;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(
            _mockAboutSchoolService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.IsSchoolClosed);

        if (model.IsSchoolClosed)
        {
            Assert.False(model.ClosedDate.IsNotAvailable);
            Assert.True(model.ClosedDate.IsAvailable);
            Assert.Equal("03 March 2025", model.ClosedDate.DisplayText(d => d.ToString("dd MMMM yyyy")));
        }
        else
        {
            Assert.False(model.ClosedDate.IsAvailable);
            Assert.True(model.ClosedDate.IsNotAvailable);
        }
    }

    [Fact]
    public async Task Get_Admissions_Info_ReturnsExpectedViewModel()
    {
        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService
            .Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AdmissionsServiceModel(
                SchoolWebsite: _fakeEstablishment.Website,
                LAName: laName,
                LASchoolAdmissionsUrl: lASchoolAdmissionsUrl
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
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
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
                SchoolWebsite: _fakeEstablishment.Website,
                LAName: laName,
                LASchoolAdmissionsUrl: lASchoolAdmissionsUrl
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
    }

    [Fact]
    public async Task Get_Attendance_Info_ReturnsOk()
    {
        var result = await _controller.Attendance(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AttendanceViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.SchoolWebsite.Value);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
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
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
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
            LocalAuthorityProgress8Score = expectedShowProgress8NotAvailableInfo ? null : 1.5,
            EstablishmentAttainment8Score = 70,
            LocalAuthorityAttainment8Score = 80,
            EnglandAttainment8Score = 50,
            EstablishmentProgress8TotalPupils = expectedShowProgress8NotAvailableInfo ? null : 65,
            EstablishmentTotalPupils = expectedShowProgress8NotAvailableInfo ? null : 95
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
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
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
            Assert.Null(model.LocalAuthorityProgress8Score);
            Assert.Null(model.EstablishmentProgress8TotalPupils);
            Assert.Null(model.EstablishmentTotalPupils);
        }
        else
        {
            Assert.Equal(expectedResult.EstablishmentProgress8Score, model.EstablishmentProgress8Score);
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
            EstablishmentTotalPupils = 95
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
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
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
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(grade, model.SelectedGrade);
        Assert.Equal(["School", $"{_fakeEstablishment.LAName} average", "England average"], model.AllGcseData.Labels);
        Assert.Equal(
            new double[]
            {
                expectedResult.EstablishmentAll.CurrentYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value,
                expectedResult.EnglandAll.CurrentYear!.Value
            },
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
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_ResultsNotAvailable_Substitutes0AndReturnsOk()
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
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(gradeSelection, model.SelectedGrade);
        Assert.Equal(["School", $"{_fakeEstablishment.LAName} average", "England average"], model.AllGcseData.Labels);
        Assert.Equal([0, 0, 0], model.AllGcseData.Data);

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);
        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal([0, 0, 0], model.AllGcseOverTimeData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal([0, 0, 0], model.AllGcseOverTimeData.Datasets[1].Data);

        Assert.Equal("England average", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal(new double[] { 0, 0, 0 }, model.AllGcseOverTimeData.Datasets[2].Data);

        // Breakdown gcse data assert
        Assert.Equal(["Girls", "Boys"], model.BreakdownGcseData.Labels);

        Assert.Equal(3, model.BreakdownGcseData.Datasets.Count);

        Assert.Equal("School", model.BreakdownGcseData.Datasets[0].Label);
        Assert.Equal([0, 0], model.BreakdownGcseData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.BreakdownGcseData.Datasets[1].Label);
        Assert.Equal([0, 0], model.BreakdownGcseData.Datasets[1].Data);

        Assert.Equal("England average", model.BreakdownGcseData.Datasets[2].Label);
        Assert.Equal([0, 0], model.BreakdownGcseData.Datasets[2].Data);
    }

    [Fact]
    public async Task Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        _mockEstablishmentSubjectEntriesService
            .Setup(s => s.GetSubjectEntriesByUrnAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new() { SubjectEntries = CoreSubjects }, new() { SubjectEntries = AdditionalSubjects }));

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

        Assert.NotNull(model.CoreSubjects);
        Assert.Equal(
            CoreSubjects.Select(c => c.SubEntCore_Sub_Est_Current_Num).OrderBy(s => s),
            model.CoreSubjects.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           CoreSubjects.Select(c => $"{c.SubEntCore_Entr_Est_Current_Num:F0}").OrderBy(s => s),
           model.CoreSubjects.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            CoreSubjects.Select(c => c.SubEntCore_Qual_Est_Current_Num).OrderBy(s => s),
            model.CoreSubjects.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.NotNull(model.AdditionalSubjects);
        Assert.Equal(
            AdditionalSubjects.Select(c => c.SubEntAdd_Sub_Est_Current_Num).OrderBy(s => s),
            model.AdditionalSubjects.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           AdditionalSubjects.Select(c => $"{c.SubEntAdd_Entr_Est_Current_Num:F0}").OrderBy(s => s),
           model.AdditionalSubjects.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            AdditionalSubjects.Select(c => c.SubEntAdd_Qual_Est_Current_Num).OrderBy(s => s),
            model.AdditionalSubjects.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public async Task Get_Destinations_Info_ReturnsOk()
    {
        var destinationsDetails = new DestinationsDetails
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            LocalAuthorityName = _fakeEstablishment.LAName,
            SchoolAll = new RelativeYearValues<double?> { CurrentYear = 10, PreviousYear = 20, TwoYearsAgo = 30 },
            LocalAuthorityAll = new RelativeYearValues<double?> { CurrentYear = 30, PreviousYear = 40, TwoYearsAgo = 50 },
            EnglandAll = new RelativeYearValues<double?> { CurrentYear = 70, PreviousYear = 80, TwoYearsAgo = 30 },
            SchoolEducation = new RelativeYearValues<double?> { CurrentYear = 20, PreviousYear = 10, TwoYearsAgo = 40 },
            LocalAuthorityEducation = new RelativeYearValues<double?> { CurrentYear = 40, PreviousYear = 50, TwoYearsAgo = 70 },
            EnglandEducation = new RelativeYearValues<double?> { CurrentYear = 60, PreviousYear = 70, TwoYearsAgo = 20 },
            SchoolEmployment = new RelativeYearValues<double?> { CurrentYear = 50, PreviousYear = 70, TwoYearsAgo = 30 },
            LocalAuthorityEmployment = new RelativeYearValues<double?> { CurrentYear = 60, PreviousYear = 90, TwoYearsAgo = 50 },
            EnglandEmployment = new RelativeYearValues<double?> { CurrentYear = 40, PreviousYear = 60, TwoYearsAgo = 50 },
            SchoolApprentice = new RelativeYearValues<double?> { CurrentYear = 40, PreviousYear = 50, TwoYearsAgo = 70 },
            LocalAuthorityApprentice = new RelativeYearValues<double?> { CurrentYear = 20, PreviousYear = 70, TwoYearsAgo = 50 },
            EnglandApprentice = new RelativeYearValues<double?> { CurrentYear = 50, PreviousYear = 60, TwoYearsAgo = 40 },
        };

        _mockDestinationsService
            .Setup(es => es.GetDestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(destinationsDetails);

        var result = await _controller.Destinations(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

        string[] expectedAllDestCurrentDataLabels = ["School", $"{_fakeEstablishment.LAName} average", "England average"];
        double[] expectedAllDestCurrentData =
        [
            destinationsDetails.SchoolAll.CurrentYear ?? 0,
            destinationsDetails.LocalAuthorityAll.CurrentYear ?? 0,
            destinationsDetails.EnglandAll.CurrentYear ?? 0
        ];

        var expectedDataOverTime = new DataOverTimeViewModel
        {
            Labels = [],
            Datasets =
            [
                new DatasetViewModel
                {
                    Label = "School",
                    Data = [destinationsDetails.SchoolAll.TwoYearsAgo ?? 0, destinationsDetails.SchoolAll.PreviousYear ?? 0, destinationsDetails.SchoolAll.CurrentYear ?? 0],
                },
                new DatasetViewModel
                {
                    Label = $"{destinationsDetails.LocalAuthorityName} average",
                    Data = [destinationsDetails.LocalAuthorityAll.TwoYearsAgo ?? 0, destinationsDetails.LocalAuthorityAll.PreviousYear ?? 0, destinationsDetails.LocalAuthorityAll.CurrentYear ?? 0],
                },
                new DatasetViewModel
                {
                    Label = "England average",
                    Data = [destinationsDetails.EnglandAll.TwoYearsAgo ?? 0, destinationsDetails.EnglandAll.PreviousYear ?? 0, destinationsDetails.EnglandAll.CurrentYear ?? 0],
                },
            ],
        };

        string[] expectedBreakdownCurrentYearDataLabels = ["Staying in education", "Entering employment and apprenticeships"];

        var expectedBreakdownCurrentYearData = new SeriesViewModel
        {
            Labels = ["Staying in education", "Entering employment and apprenticeships"],
            Datasets =
            [
                new DataSeriesViewModel
                {
                    Label = "School",
                    Data = [destinationsDetails.SchoolEducation.CurrentYear ?? 0, (destinationsDetails.SchoolEmployment.CurrentYear ?? 0 + destinationsDetails.SchoolApprentice.CurrentYear ?? 0)]
                },
                new DataSeriesViewModel
                {
                    Label = $"{destinationsDetails.LocalAuthorityName} average",
                    Data = [destinationsDetails.LocalAuthorityEducation.CurrentYear ?? 0, (destinationsDetails.LocalAuthorityEmployment.CurrentYear ?? 0 + destinationsDetails.LocalAuthorityApprentice.CurrentYear ?? 0)]
                },
                new DataSeriesViewModel
                {
                    Label = "England average",
                    Data = [destinationsDetails.EnglandEducation.CurrentYear ?? 0, (destinationsDetails.EnglandEmployment.CurrentYear ?? 0 + destinationsDetails.EnglandApprentice.CurrentYear ?? 0)]
                },
            ],
        };

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as DestinationsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

        Assert.Equal(expectedAllDestCurrentDataLabels, model.AllDestinationsData.Labels);
        Assert.Equal(expectedAllDestCurrentData, model.AllDestinationsData.Data);

        foreach (var expectedDataset in expectedDataOverTime.Datasets)
        {
            var actualDatset = model.AllDestinationsOverTimeData.Datasets.FirstOrDefault(s => s.Label == expectedDataset.Label);
            Assert.NotNull(actualDatset);
            Assert.Equal(expectedDataset.Label, actualDatset.Label);
            Assert.Equal(expectedDataset.Data, actualDatset.Data);
        }

        Assert.Equal(expectedBreakdownCurrentYearDataLabels, model.BreakdownDestinationData.Labels);
        foreach (var expectedDataset in expectedBreakdownCurrentYearData.Datasets)
        {
            var actualDatset = model.BreakdownDestinationData.Datasets.FirstOrDefault(s => s.Label == expectedDataset.Label);
            Assert.NotNull(actualDatset);
            Assert.Equal(expectedDataset.Label, actualDatset.Label);
            Assert.Equal(expectedDataset.Data, actualDatset.Data);
        }

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }
}
