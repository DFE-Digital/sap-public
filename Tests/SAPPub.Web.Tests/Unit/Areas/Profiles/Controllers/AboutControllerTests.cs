using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Constants;
using SAPPub.Core.Extensions;
using SAPPub.Web.Areas.Profiles.Models;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class AboutControllerTests : BaseProfilesTests
{
    
    private readonly Mock<ILogger<AboutController>> _mockLogger = new();
    private readonly Mock<IAboutSchoolService> _mockAboutSchoolService = new();
    private readonly AboutController _controller;

    private AboutSchoolModel SchoolDetails()
    {
        return new AboutSchoolModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            AcademyTrust = fakeEstablishment.TrustName,
            Website = fakeEstablishment.Website,
            Telephone = fakeEstablishment.TelephoneNum,
            Address = fakeEstablishment.Address,
            LocalAuthority = fakeEstablishment.LAName,
            LocalAuthorityName = fakeEstablishment.LAName,
            LocalAuthorityWebsite = "www.gov.uk",
            Easting = "50.01",
            Northing = "60.90",
            TypeOfSchool = fakeEstablishment.TypeOfEstablishmentName,
            HeadTeacher = fakeEstablishment.Headteacher,
            AgeRange = fakeEstablishment.AgeRange,
            NumberOfPupils = fakeEstablishment.TotalPupils,
            PupilSex = fakeEstablishment.GenderName,
            ReligiousCharacter = fakeEstablishment.ReligiousCharacterName,
            OfficialSixthFormId = fakeEstablishment.OfficialSixthFormId,
            ResourcedProvisionName = fakeEstablishment.ResourcedProvisionName,
            EstablishmentTypeGroupId = fakeEstablishment.EstablishmentTypeGroupId,
            Status = fakeEstablishment.StatusCode.ToStatus(),
            ClosedDate = fakeEstablishment.ClosedDate.ToDateOnly(),
            OpenReasonId = fakeEstablishment.OpenReasonId,
            OpenDate = fakeEstablishment.OpenDate.ToDateOnly(),
            IsKS2 = true,
            IsKS4 = true
        };
    }

    public AboutControllerTests()
    {
        _controller = new AboutController(_mockLogger.Object, _mockAboutSchoolService.Object);
    }

    [Fact]
    public async Task Get_AboutSchool_Info_ReturnsExpected()
    {
        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(expectedResult.Website, model.SchoolWebsite.Value);
        Assert.Equal(expectedResult.AcademyTrust, model.AcademyTrust.Value);
        Assert.Equal(expectedResult.AcademyTrustUpdatedIn, model.AcademyTrustUpdatedIn.Value);
        Assert.Equal(expectedResult.Telephone, model.Telephone.Value);
        Assert.Equal(expectedResult.LocalAuthority, model.LocalAuthority.Value);
        Assert.Equal(expectedResult.LocalAuthorityName, model.LocalAuthorityCouncilName);
        Assert.Equal(expectedResult.LocalAuthorityWebsite, model.LocalAuthorityWebsite);
        Assert.Equal(expectedResult.TypeOfSchool, model.TypeOfSchool.Value);
        Assert.Equal(expectedResult.HeadTeacher, model.HeadTeacher.Value);
        Assert.Equal(expectedResult.AgeRange, model.AgeRange.Value);
        Assert.Equal("1,117", model.NumberOfPupils.Value);
        Assert.Equal(expectedResult.PupilSex, model.PupilSex.Value);
        Assert.Equal(expectedResult.ReligiousCharacter, model.ReligiousCharacter.Value);
        Assert.Equal(expectedResult.OfficialSixthFormId == "1" ? "Yes" : "No", model.SixthForm.Value);
        Assert.Equal(expectedResult.Status, model.StatusCode);
        Assert.False(model.ClosedDate.IsAvailable);
        Assert.False(model.IsLocalAuthoritySchool);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(expectedResult.Urn, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(TextHelpers.CleanForUrl(expectedResult.SchoolName), model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(expectedResult.OpenReasonId, model.OpenReasonId);
        Assert.Equal(expectedResult.SenTypes, model.SenTypes.Value);
        Assert.Equal("Primary and Secondary", model.EducationPhase);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
    }

    [Fact]
    public async Task Get_AboutSchool_WithPredecessors_ReturnsExpected()
    {
        var expectedResult = SchoolDetails();
        expectedResult.Predecessors =
            [
                new EstablishmentLinkModel
                {
                    Urn = "654321",
                    Name = "Predecessor School 1"
                },
                new EstablishmentLinkModel
                {
                    Urn = "789012",
                    Name = "Predecessor School 2"
                }
            ];

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Collection(model.Predecessors!,
            predecessor1 =>
            {
                Assert.Equal(expectedResult.Predecessors![0].Urn, predecessor1.Urn);
                Assert.Equal(expectedResult.Predecessors[0].Name, predecessor1.Name);
            },
            predecessor2 =>
            {
                Assert.Equal(expectedResult.Predecessors![1].Urn, predecessor2.Urn);
                Assert.Equal(expectedResult.Predecessors[1].Name, predecessor2.Name);
            });
    }

    [Fact]
    public async Task Get_AboutSchool_WithSuccessors_ReturnsExpected()
    {
        var expectedResult = SchoolDetails();
        expectedResult.Successors = [
            new EstablishmentLinkModel
            {
                Urn = "654321",
                Name = "Successor School 1"
            },
            new EstablishmentLinkModel
            {
                Urn = "789012",
                Name = "Successor School 2"
            }
        ];

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Collection(model.Successors!,
            successor1 =>
            {
                Assert.Equal(expectedResult.Successors![0].Urn, successor1.Urn);
                Assert.Equal(expectedResult.Successors[0].Name, successor1.Name);
            },
            successor2 =>
            {
                Assert.Equal(expectedResult.Successors![1].Urn, successor2.Urn);
                Assert.Equal(expectedResult.Successors[1].Name, successor2.Name);
            });
    }

    [Fact]
    public async Task Get_AboutSchool_Info_With_No_Data_ReturnsOk()
    {
        var expectedResult = new AboutSchoolModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName
        };

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);
        
        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(Constants.Constants.NotAvailable, model.SchoolWebsite.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.AcademyTrust.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.AcademyTrustUpdatedIn.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.Telephone.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.LocalAuthority.DisplayText());
        Assert.Equal(expectedResult.LocalAuthorityName, model.LocalAuthorityCouncilName);
        Assert.Equal(expectedResult.LocalAuthorityWebsite, model.LocalAuthorityWebsite);
        Assert.Equal(Constants.Constants.NotAvailable, model.TypeOfSchool.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.HeadTeacher.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.AgeRange.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.NumberOfPupils.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.PupilSex.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.ReligiousCharacter.DisplayText());
        Assert.Equal(Constants.Constants.NotAvailable, model.SixthForm.DisplayText());
        Assert.Equal(expectedResult.Status, model.StatusCode);
        Assert.False(model.ClosedDate.IsAvailable);
        Assert.False(model.IsLocalAuthoritySchool);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(expectedResult.Urn, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(TextHelpers.CleanForUrl(expectedResult.SchoolName), model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(expectedResult.OpenReasonId, model.OpenReasonId);
        Assert.Equal(expectedResult.OpenDate, model.OpenDate);
        Assert.Equal(Constants.Constants.NotRecorded, model.SenTypes.DisplayText(notAvailableText: Constants.Constants.NotRecorded));
    }

    [Theory]
    [InlineData(null, FieldStatus.NotAvailable)]
    [InlineData("", FieldStatus.NotAvailable)]
    [InlineData(" ", FieldStatus.NotAvailable)]
    [InlineData("test", FieldStatus.Available)]
    public async Task Get_AboutSchool_SchoolFeatures_SchoolWebsite(string? website, FieldStatus fieldStatus)
    {
        fakeEstablishment.Website = website!;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

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
        fakeEstablishment.TotalPupils = totalPupils;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.NumberOfPupils.Value);
    }

    [Theory]
    [InlineData(null, "Not recorded")]
    [InlineData("", "Not recorded")]
    [InlineData("Not applicable", "No")]
    [InlineData("Resourced provision", "No")]
    [InlineData("SEN unit", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public async Task Get_AboutSchool_SchoolFeatures_SENUnit(string? resourcedProvisionName, string expectedOutput)
    {
        fakeEstablishment.ResourcedProvisionName = resourcedProvisionName!;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.SenUnit);

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData(null, "Not recorded")]
    [InlineData("", "Not recorded")]
    [InlineData("Not applicable", "No")]
    [InlineData("SEN unit", "No")]
    [InlineData("Resourced provision", "Yes")]
    [InlineData("Resourced provision and SEN unit", "Yes")]
    public async Task Get_AboutSchool_SchoolFeatures_ResourcedUnit(string? resourcedProvisionName, string expectedOutput)
    {
        fakeEstablishment.ResourcedProvisionName = resourcedProvisionName!;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.ResourcedProvision);

        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("2", "No")]
    [InlineData("9", "No")]
    [InlineData("1", "Yes")]
    public async Task Get_AboutSchool_SchoolFeatures_SixthForm_Format(string sixthFormId, string? expectedOutput)
    {
        fakeEstablishment.OfficialSixthFormId = sixthFormId;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedOutput, model.SixthForm.Value);
    }

    [Theory]
    [InlineData("4", true)]
    [InlineData("2", false)]
    [InlineData("9", false)]
    public async Task Get_AboutSchool_SchoolFeatures_IsLocalAuthoritySchool(string establishmentTypeGroupId, bool expectedOutput)
    {
        fakeEstablishment.EstablishmentTypeGroupId = establishmentTypeGroupId;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

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
        fakeEstablishment.StatusCode = statusCode;
        fakeEstablishment.ClosedDate = fakeEstablishment.StatusCode == 2 ? "03-03-2025" : null;

        var expectedResult = SchoolDetails();

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

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

    [Theory]
    [InlineData(10, "2025-03-03", true, "Opened as an academy on 3 March 2025")] // Academy open reason, within 3 years
    [InlineData(1, "2025-03-03", true, "This school opened on 3 March 2025")]    // Other open reason, within 3 years
    [InlineData(0, "2025-03-03", true, "This school opened on 3 March 2025")]    // OpenReasonId 0, within 3 years
    [InlineData(14, "2025-03-03", true, "This school opened on 3 March 2025")]   // OpenReasonId 14, within 3 years
    [InlineData(99, "2025-03-03", true, "This school opened on 3 March 2025")]   // OpenReasonId 99, within 3 years
    [InlineData(10, "2010-01-01", false, null)]                                   // Academy open reason, too old
    [InlineData(1, "2010-01-01", false, null)]                                    // Other open reason, too old
    [InlineData(10, null, false, null)]                                           // Academy open reason, no date
    [InlineData(1, null, false, null)]                                            // Other open reason, no date
    public async Task Get_AboutSchool_RecentlyOpenedSchoolMessage_VariousReasonsAndDates(
    int? openReasonId, string? openDateString, bool expectAvailable, string? expectedMessage)
    {
        // Arrange
        fakeEstablishment.OpenReasonId = openReasonId;
        fakeEstablishment.OpenDate = openDateString ?? null;

        var expectedResult = SchoolDetails();

        expectedResult.OpenReasonId = openReasonId;
        expectedResult.OpenDate = openDateString != null ? DateOnly.Parse(openDateString) : null;

        _mockAboutSchoolService
            .Setup(es => es.GetAboutSchoolDetailsAsync(fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AboutSchool(expectedResult.Urn, expectedResult.SchoolName, CancellationToken.None) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AboutSchoolViewModel;
        Assert.NotNull(model);

        if (expectAvailable)
        {
            Assert.True(model.RecentlyOpenedSchoolMessage.IsAvailable);
            Assert.Equal(expectedMessage, model.RecentlyOpenedSchoolMessage.Value);
        }
        else
        {
            Assert.False(model.RecentlyOpenedSchoolMessage.IsAvailable);
            Assert.True(model.RecentlyOpenedSchoolMessage.IsNotAvailable);
        }
    }
}