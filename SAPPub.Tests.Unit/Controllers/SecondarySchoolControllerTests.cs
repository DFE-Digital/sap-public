using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Tests.Unit.Controllers;

public class SecondarySchoolControllerTests
{
    private readonly Mock<ILogger<SecondarySchoolController>> _mockLogger;
    private readonly Mock<IEstablishmentService> _mockEstablishment;
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
        OfficialSixthFormId = "Yes",
        ResourcedProvision = "Resourced provision",
    };


    public SecondarySchoolControllerTests()
    {
        _mockLogger = new Mock<ILogger<SecondarySchoolController>>();
        _mockEstablishment = new Mock<IEstablishmentService>();
        _mockEstablishment.Setup(es => es.GetEstablishment(It.IsAny<string>())).Returns(fakeEstablishment);

        // Create a real temp directory
        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new SecondarySchoolController(_mockLogger.Object, _mockEstablishment.Object);

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
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.Website.Should().Be(fakeEstablishment.Website);
        model.AcademyTrust.Should().Be(fakeEstablishment.TrustName);
        model.Telephone.Should().Be(fakeEstablishment.TelephoneNum);
        model.LocalAuthority.Should().Be(fakeEstablishment.LAName);
        model.TypeOfSchool.Should().Be(fakeEstablishment.TypeOfEstablishmentName);
        model.HeadTeacher.Should().Be(fakeEstablishment.Headteacher);
        model.AgeRange.Should().Be(fakeEstablishment.AgeRange);
        model.NumberOfPupils.Should().Be("1,117");
        model.PupilSex.Should().Be(fakeEstablishment.GenderName);
        model.ReligiousCharacter.Should().Be(fakeEstablishment.ReligiousCharacterName);
        model.SixthForm.Should().Be(fakeEstablishment.OfficialSixthFormId);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);

        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
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
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();

        model.NumberOfPupils.Should().Be(expectedOutput);
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
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();
        
        model.SenUnit.Should().Be(expectedOutput);

        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
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
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();

        model.ResourcedProvision.Should().Be(expectedOutput);

        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
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
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AboutSchoolViewModel;
        model.Should().NotBeNull();

        model.SixthForm.Should().Be(expectedOutput);
    }

    [Fact]
    public void Get_Admissions_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Admissions(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AdmissionsViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_Attendance_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Attendance(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AttendanceViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.SchoolWebsite.Should().Be(fakeEstablishment.Website);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_CurriculumAndExtraCurricularActivities_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.CurriculumAndExtraCurricularActivities(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as CurriculumAndExtraCurricularActivitiesViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_AcademicPerformancePupilProgress_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformancePupilProgress(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformancePupilProgressViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_AcademicPerformance_EnglishAndMathsResults_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformanceEnglishAndMathsResults(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.AcademicPerformanceSubjectsEntered(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as AcademicPerformanceSubjectsEnteredViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.CoreSubjects.Should().NotBeNullOrEmpty();
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }

    [Fact]
    public void Get_Destinations_Info_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.Destinations(fakeEstablishment.URN, fakeEstablishment.EstablishmentName) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result.Model.Should().NotBeNull();

        var model = result.Model as DestinationsViewModel;
        model.Should().NotBeNull();
        model.URN.Should().Be(fakeEstablishment.URN);
        model.SchoolName.Should().Be(fakeEstablishment.EstablishmentName);
        model.RouteAttributes.Count.Should().Be(2);
        model.RouteAttributes[RouteConstants.URN].Should().Be(fakeEstablishment.URN);
        model.RouteAttributes[RouteConstants.SchoolName].Should().Be(fakeEstablishment.EstablishmentName);
    }
}
