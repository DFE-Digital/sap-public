using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.Destinations;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4;
using SAPPub.Core.Interfaces.Services.KS4.Admissions;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.Admissions;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.Tests.TestBuilders;
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
    private readonly Mock<IAdmissionsService> _mockAdmissionsService = new();
    private readonly SecondarySchoolController _controller;
    private Establishment _fakeEstablishment;

    private List<EstablishmentCoreSubjectEntries.SubjectEntry> CoreSubjects =
                new()
                {
                    new () {
                        SubEntCore_Sub_Est_Current_Num = "English language",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 95.04,
                    },
                    new () {
                        SubEntCore_Sub_Est_Current_Num = "English literature",
                        SubEntCore_Qual_Est_Current_Num = "GCSE",
                        SubEntCore_Entr_Est_Current_Num = 90.15,
                    }
                };

    private List<EstablishmentAdditionalSubjectEntries.SubjectEntry> AdditionalSubjects =
                new()
                {
                    new () {
                        SubEntAdd_Sub_Est_Current_Num = "Geography",
                        SubEntAdd_Qual_Est_Current_Num = "GCSE",
                        SubEntAdd_Entr_Est_Current_Num = 45.45,
                    },
                    new () {
                        SubEntAdd_Sub_Est_Current_Num = "Music",
                        SubEntAdd_Qual_Est_Current_Num = "GCSE",
                        SubEntAdd_Entr_Est_Current_Num = 10.12,
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
        EstablishmentAll = new RelativeYearValues<double?>()
        { 
            CurrentYear = 60,
            PreviousYear = 80,
            TwoYearsAgo = 60
        },
        LocalAuthorityAll = new RelativeYearValues<double?>()
        {
            CurrentYear = 80,
            PreviousYear = 70,
            TwoYearsAgo = 80
        },
        EnglandAll = new RelativeYearValues<double?>()
        {
            CurrentYear = 70,
            PreviousYear = 70,
            TwoYearsAgo = 80
        },

        EstablishmentBoys = new RelativeYearValues<double?>()
        {
            CurrentYear = 50
        },
        LocalAuthorityBoys = new RelativeYearValues<double?>()
        {
            CurrentYear = 70,
        },
        EnglandBoys = new RelativeYearValues<double?>()
        {
            CurrentYear = 60,
        },
        EstablishmentGirls = new RelativeYearValues<double?>()
        {
            CurrentYear = 80
        },
        LocalAuthorityGirls = new RelativeYearValues<double?>()
        {
            CurrentYear = 70,
        },
        EnglandGirls = new RelativeYearValues<double?>()
        {
            CurrentYear = 90,
        },
    };

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
            .WithResourcedProvision("Resourced provision")
            .Build();

        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();
        _mockEstablishmentService = new();
        _mockDestinationsService = new();
        _mockEstablishmentService.Setup(es => es.GetEstablishment(It.IsAny<string>())).Returns(_fakeEstablishment);

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockLogger.Object, _mockEstablishmentService.Object, _mockDestinationsService.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public void Get_AboutSchool_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AboutSchool(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.Website);
        Assert.Equal(_fakeEstablishment.TrustName, model.AcademyTrust);
        Assert.Equal(_fakeEstablishment.TelephoneNum, model.Telephone);
        Assert.Equal(_fakeEstablishment.LAName, model.LocalAuthority);
        Assert.Equal(_fakeEstablishment.TypeOfEstablishmentName, model.TypeOfSchool);
        Assert.Equal(_fakeEstablishment.Headteacher, model.HeadTeacher);
        Assert.Equal(_fakeEstablishment.AgeRange, model.AgeRange);
        Assert.Equal("1,117", model.NumberOfPupils);
        Assert.Equal(_fakeEstablishment.GenderName, model.PupilSex);
        Assert.Equal(_fakeEstablishment.ReligiousCharacterName, model.ReligiousCharacter);
        Assert.Equal(_fakeEstablishment.OfficialSixthFormId, model.SixthForm);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);

    }

    [Theory]
    [InlineData("100", "100")]
    [InlineData("1117", "1,117")]
    [InlineData("50000", "50,000")]
    [InlineData("2,500", "2,500")]
    [InlineData("Test", "Test")]
    public void Get_AboutSchool_SchoolFeatures_NumberOfPupils_Format(string totalPupils, string expectedOutput)
    {
        // Arrange
        _fakeEstablishment.TotalPupils = totalPupils;

        // Act
        var result = _controller.AboutSchool(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.NumberOfPupils);
    }

    [Theory]
    [InlineData("", "No")]
    [InlineData("Not applicable", "No")]
    [InlineData("SEN unit", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public void Get_AboutSchool_SchoolFeatures_SENUnit(string resourcedProvision, string expectedOutput)
    {
        // Arrange
        _fakeEstablishment.ResourcedProvision = resourcedProvision;

        // Act
        var result = _controller.AboutSchool(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
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
    [InlineData("Resourced provision", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public void Get_AboutSchool_SchoolFeatures_ResourcedUnit(string resourcedProvision, string expectedOutput)
    {
        // Arrange
        _fakeEstablishment.ResourcedProvision = resourcedProvision;

        // Act
        var result = _controller.AboutSchool(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
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
    public void Get_AboutSchool_SchoolFeatures_SixthForm_Format(string sixthFormId, string expectedOutput)
    {
        // Arrange
        _fakeEstablishment.OfficialSixthFormId = sixthFormId;

        // Act
        var result = _controller.AboutSchool(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);

        Assert.Equal(expectedOutput, model.SixthForm);
    }

    [Fact]
    public async Task Get_Admissions_Info_ReturnsExpectedViewModel()
    {
        // Arrange
        var lASchoolAdmissionsUrl = "https://www.example.com/school-admissions";
        var laName = "Example Local Authority";

        _mockAdmissionsService.Setup(s => s.GetAdmissionsDetailsAsync(_fakeEstablishment.URN)).ReturnsAsync(new AdmissionsServiceModel(
            LAName: laName,
            LASchoolAdmissionsUrl: lASchoolAdmissionsUrl
        ));

        // Act
        var result = await _controller.Admissions(_mockAdmissionsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(lASchoolAdmissionsUrl, model.LASecondarySchoolAdmissionsLinkUrl);
        Assert.Equal(laName, model.LAName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_Attendance_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Attendance(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AttendanceViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(_fakeEstablishment.Website, model.SchoolWebsite);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.CurriculumAndExtraCurricularActivities(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_AcademicPerformancePupilProgress_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformancePupilProgress(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformancePupilProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(GcseGradeDataSelection.Grade4AndAbove)]
    [InlineData(GcseGradeDataSelection.Grade5AndAbove)]
    public void Get_AcademicPerformance_EnglishAndMathsResults_ReturnsOk(GcseGradeDataSelection grade)
    {
        // Arrange
        var expectedResult = EnglishAndMathsResults(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, _fakeEstablishment.LAName);
        _mockEnglishAndMathsResultsService.Setup(s => s.GetEnglishAndMathsResults(_fakeEstablishment.URN, (int)grade))
            .Returns(expectedResult);

        // Act
        var result = _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName, grade) as ViewResult;

        // Assert
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
            new double[] {
                expectedResult.EstablishmentAll.CurrentYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value,
                expectedResult.EnglandAll.CurrentYear!.Value },
            model.AllGcseData.Data
        );

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);

        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal([
                expectedResult.EstablishmentAll.TwoYearsAgo!.Value,
                expectedResult.EstablishmentAll.PreviousYear!.Value,
                expectedResult.EstablishmentAll.CurrentYear!.Value],
            model.AllGcseOverTimeData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal([
                expectedResult.LocalAuthorityAll.TwoYearsAgo!.Value,
                expectedResult.LocalAuthorityAll.PreviousYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value],
            model.AllGcseOverTimeData.Datasets[1].Data);

        Assert.Equal("England average", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal([
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
    public void Get_AcademicPerformance_EnglishAndMathsResults_ResultsNotAvailable_Substitutes0AndReturnsOk()
    {
        // Arrange
        var gradeSelection = GcseGradeDataSelection.Grade4AndAbove;
        EnglishAndMathsResultsModel serviceModel = new()
        {            
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            LAName = _fakeEstablishment.LAName,
            EstablishmentAll = new RelativeYearValues<double?>()
            {
                CurrentYear = null
            },
            LocalAuthorityAll = new RelativeYearValues<double?>()
            {
                CurrentYear = null
            },
            EnglandAll = new RelativeYearValues<double?>()
            {
                CurrentYear = null,
            },
            EstablishmentBoys = new RelativeYearValues<double?>()
            {
                CurrentYear = null
            },
            LocalAuthorityBoys = new RelativeYearValues<double?>()
            {
                CurrentYear = null,
            },
            EnglandBoys = new RelativeYearValues<double?>()
            {
                CurrentYear = null,
            },
            EstablishmentGirls = new RelativeYearValues<double?>()
            {
                CurrentYear = null
            },
            LocalAuthorityGirls = new RelativeYearValues<double?>()
            {
                CurrentYear = null,
            },
            EnglandGirls = new RelativeYearValues<double?>()
            {
                CurrentYear = null,
            },
        };

        _mockEnglishAndMathsResultsService.Setup(s => s.GetEnglishAndMathsResults(_fakeEstablishment.URN, (int)gradeSelection))
            .Returns(serviceModel);

        // Act
        var result = _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName, gradeSelection) as ViewResult;

        // Assert
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
        Assert.Equal([0, 0, 0 ], model.AllGcseData.Data);

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);

        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal([0, 0, 0 ], model.AllGcseOverTimeData.Datasets[0].Data);

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
    public void Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        // Arrange
        _mockEstablishmentSubjectEntriesService.Setup(s => s.GetSubjectEntriesByUrn(_fakeEstablishment.URN))
            .Returns((new() { SubjectEntries = CoreSubjects }, new() { SubjectEntries = AdditionalSubjects }));

        // Act
        var result = _controller.AcademicPerformanceSubjectsEntered(_mockEstablishmentSubjectEntriesService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
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
           CoreSubjects.Select(c => $"{c.SubEntCore_Entr_Est_Current_Num:F1}%").OrderBy(s => s),
           model.CoreSubjects.Select(s => s.PercentageOfPupilsEntered).OrderBy(s => s)
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
           AdditionalSubjects.Select(c => $"{c.SubEntAdd_Entr_Est_Current_Num:F1}%").OrderBy(s => s),
           model.AdditionalSubjects.Select(s => s.PercentageOfPupilsEntered).OrderBy(s => s)
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
    public void Get_Destinations_Info_ReturnsOk()
    {
        // Arrange
        var destinationsDetails = new DestinationsDetails
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            LocalAuthorityName = _fakeEstablishment.LAName,
            SchoolAll = new RelativeYearValues<double?>
            {
                CurrentYear = 10,
                PreviousYear = 20,
                TwoYearsAgo = 30,
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = 30,
                PreviousYear = 40,
                TwoYearsAgo = 50,
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
                PreviousYear = 80,
                TwoYearsAgo = 30,
            },
            SchoolEducation = new RelativeYearValues<double?>
            {
                CurrentYear = 20,
                PreviousYear = 10,
                TwoYearsAgo = 40,
            },
            LocalAuthorityEducation = new RelativeYearValues<double?>
            {
                CurrentYear = 40,
                PreviousYear = 50,
                TwoYearsAgo = 70,
            },
            EnglandEducation = new RelativeYearValues<double?>
            {
                CurrentYear = 60,
                PreviousYear = 70,
                TwoYearsAgo = 20,
            },
            SchoolEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = 50,
                PreviousYear = 70,
                TwoYearsAgo = 30,
            },
            LocalAuthorityEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = 60,
                PreviousYear = 90,
                TwoYearsAgo = 50,
            },
            EnglandEmployment = new RelativeYearValues<double?>
            {
                CurrentYear = 40,
                PreviousYear = 60,
                TwoYearsAgo = 50,
            },
            SchoolApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = 40,
                PreviousYear = 50,
                TwoYearsAgo = 70,
            },
            LocalAuthorityApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = 20,
                PreviousYear = 70,
                TwoYearsAgo = 50,
            },
            EnglandApprentice = new RelativeYearValues<double?>
            {
                CurrentYear = 50,
                PreviousYear = 60,
                TwoYearsAgo = 40,
            },
        };

        _mockDestinationsService.Setup(es => es.GetDestinationsDetails(It.IsAny<string>())).Returns(destinationsDetails);

        // Act
        var result = _controller.Destinations(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName) as ViewResult;

        string[] expectedAllDestCurrentDataLabels = ["School", $"{_fakeEstablishment.LAName} average", "England average"];
        double[] expectedAllDestCurrentData = [destinationsDetails.SchoolAll.CurrentYear ?? 0, destinationsDetails.LocalAuthorityAll.CurrentYear ?? 0, destinationsDetails.EnglandAll.CurrentYear ?? 0];
        var expectedDataOverTime = new DataOverTimeViewModel
        {
            Labels = [],
            Datasets = [
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
                    new DataSeriesViewModel {
                        Label = "School",
                        Data = [destinationsDetails.SchoolEducation.CurrentYear ?? 0, (destinationsDetails.SchoolEmployment.CurrentYear ?? 0 + destinationsDetails.SchoolApprentice.CurrentYear ?? 0)]
                    },
                    new DataSeriesViewModel {
                        Label = $"{destinationsDetails.LocalAuthorityName} average",
                        Data = [destinationsDetails.LocalAuthorityEducation.CurrentYear ?? 0, (destinationsDetails.LocalAuthorityEmployment.CurrentYear ?? 0 + destinationsDetails.LocalAuthorityApprentice.CurrentYear ?? 0)]
                    },
                    new DataSeriesViewModel {
                        Label = "England average",
                        Data = [destinationsDetails.EnglandEducation.CurrentYear ?? 0, (destinationsDetails.EnglandEmployment.CurrentYear ?? 0 + destinationsDetails.EnglandApprentice.CurrentYear ?? 0)]
                    },
                ],
        };


        // Assert
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
