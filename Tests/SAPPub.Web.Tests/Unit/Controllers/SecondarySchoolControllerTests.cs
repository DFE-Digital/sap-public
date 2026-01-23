using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.SubjectEntries;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.KS4.SubjectEntries;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;
using static SAPPub.Web.Models.SecondarySchool.AcademicPerformanceEnglishAndMathsResultsViewModel;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class SecondarySchoolControllerTests
{
    private readonly Mock<ILogger<SecondarySchoolController>> _mockLogger;
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IEstablishmentSubjectEntriesService> _mockEstablishmentSubjectEntriesService = new();
    private readonly Mock<IAcademicPerformanceEnglishAndMathsResultsService> _mockEnglishAndMathsResultsService = new();
    private readonly SecondarySchoolController _controller;

    private readonly Establishment fakeEstablishment = new()
    {
        URN = "1",
        EstablishmentName = "Test School",
        TrustName = "Trust",
        Website = "https://www.gov.uk/",
        TelephoneNum = "012154896",
        AddressStreet = "Street",
        AddressLocality = "Locality",
        AddressTown = "Town",
        AddressPostcode = "Postcode",
        LAName = "LocalAuthority",
        TypeOfEstablishmentName = "EstablishmentName",
        HeadteacherTitle = "Title",
        HeadteacherFirstName = "FirstName",
        HeadteacherLastName = "LastName",
        AgeRangeLow = "11",
        AgeRangeHigh = "18",
        TotalPupils = "1117",
        GenderName = "GenderName",
        ReligiousCharacterName = "ReligiousCharacter",
        OfficialSixthFormId = "No",
        ResourcedProvision = "Resourced provision",
    };

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

    // CML TODO move all these
    private EnglishAndMathsResultsServiceModel EnglishAndMathsResults = new()
    {
        EnglandAverage = 55,
        LocalAuthorityAverage = 65,
        EstablishmentResult = 75,
        LAName = "Sheffield"
    };

    public SecondarySchoolControllerTests()
    {
        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();
        _mockEstablishmentService = new();
        _mockEstablishmentService.Setup(es => es.GetEstablishment(fakeEstablishment.URN)).Returns(fakeEstablishment);

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockLogger.Object, _mockEstablishmentService.Object);

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
        var result = _controller.AboutSchool(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(fakeEstablishment.Website, model.Website);
        Assert.Equal(fakeEstablishment.TrustName, model.AcademyTrust);
        Assert.Equal(fakeEstablishment.TelephoneNum, model.Telephone);
        Assert.Equal(fakeEstablishment.LAName, model.LocalAuthority);
        Assert.Equal(fakeEstablishment.TypeOfEstablishmentName, model.TypeOfSchool);
        Assert.Equal(fakeEstablishment.Headteacher, model.HeadTeacher);
        Assert.Equal(fakeEstablishment.AgeRange, model.AgeRange);
        Assert.Equal("1,117", model.NumberOfPupils);
        Assert.Equal(fakeEstablishment.GenderName, model.PupilSex);
        Assert.Equal(fakeEstablishment.ReligiousCharacterName, model.ReligiousCharacter);
        Assert.Equal(fakeEstablishment.OfficialSixthFormId, model.SixthForm);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);

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
        fakeEstablishment.TotalPupils = totalPupils;

        // Act
        var result = _controller.AboutSchool(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

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
        fakeEstablishment.ResourcedProvision = resourcedProvision;

        // Act
        var result = _controller.AboutSchool(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.SenUnit);

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData("", "No")]
    [InlineData("Not applicable", "No")]
    [InlineData("Resourced provision", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public void Get_AboutSchool_SchoolFeatures_ResourcedUnit(string resourcedProvision, string expectedOutput)
    {
        // Arrange
        fakeEstablishment.ResourcedProvision = resourcedProvision;

        // Act
        var result = _controller.AboutSchool(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);

        Assert.Equal(expectedOutput, model.ResourcedProvision);

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData("", "No")]
    [InlineData("2", "No")]
    [InlineData("9", "No")]
    [InlineData("1", "Yes")]
    public void Get_AboutSchool_SchoolFeatures_SixthForm_Format(string sixthFormId, string expectedOutput)
    {
        // Arrange
        fakeEstablishment.OfficialSixthFormId = sixthFormId;

        // Act
        var result = _controller.AboutSchool(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);

        Assert.Equal(expectedOutput, model.SixthForm);
    }

    [Fact]
    public void Get_Admissions_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Admissions(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdmissionsViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_Attendance_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Attendance(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AttendanceViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(fakeEstablishment.Website, model.SchoolWebsite);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.CurriculumAndExtraCurricularActivities(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_AcademicPerformancePupilProgress_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformancePupilProgress(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformancePupilProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(GcseGradeDataSelection.Grade4AndAbove)]
    [InlineData(GcseGradeDataSelection.Grade5AndAbove)]
    public void Get_AcademicPerformance_EnglishAndMathsResults_ReturnsOk(GcseGradeDataSelection grade)
    {
        // Arrange
        _mockEnglishAndMathsResultsService.Setup(s => s.ResultsOfSpecifiedGradeAndAbove(fakeEstablishment.URN, (int)grade))
            .Returns(EnglishAndMathsResults);

        // Act
        var result = _controller.AcademicPerformanceEnglishAndMathsResults(_mockEnglishAndMathsResultsService.Object, fakeEstablishment.URN, fakeEstablishment.EstablishmentName, grade) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(grade, model.SelectedGrade);
        Assert.Contains($"Grade {grade} and above", model.GcseChartData.ChartTitle);
        Assert.Equal(
            new List<string> { "School", "Local Authority Average", "England Average" },
            model.GcseChartData.Lables
        );
        Assert.Equal(
            new double[] {
                EnglishAndMathsResults.EstablishmentResult!.Value,
                EnglishAndMathsResults.LocalAuthorityAverage!.Value,
                EnglishAndMathsResults.EnglandAverage!.Value },
            model.GcseChartData.GcseData
        );
    }

    [Fact]
    public void Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        // Arrange
        _mockEstablishmentSubjectEntriesService.Setup(s => s.GetSubjectEntriesByUrn(fakeEstablishment.URN))
            .Returns((new() { SubjectEntries = CoreSubjects }, new() { SubjectEntries = AdditionalSubjects }));

        // Act
        var result = _controller.AcademicPerformanceSubjectsEntered(_mockEstablishmentSubjectEntriesService.Object, fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceSubjectsEnteredViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
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
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Fact]
    public void Get_Destinations_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Destinations(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as DestinationsViewModel;
        Assert.NotNull(model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentName, model.RouteAttributes[RouteConstants.SchoolName]);
    }
}
