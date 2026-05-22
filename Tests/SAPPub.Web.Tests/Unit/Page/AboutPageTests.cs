using Moq;
using SAPPub.Core.Enums;
using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page;

// Share the WebAppFixture across tests in this class so that we only start the web app once for all tests in this collection
// and run tests sequentially to avoid issues with shared state in the mock accessor.`
// The Mock accessor is cleared after each test, but if tests run in parallel, they could interfere with each other.
[Collection("WebAppCollection")]
public class AboutPageTests : PageTestsBase
{
    private static string _pageRoute = "/secondary/about";
    private readonly Mock<IAboutSchoolService> _about;

    public AboutPageTests(WebAppFixture fixture) : base(fixture)
    {
        // access the mocks needed by the controller endpoint being used
        _about = UseMock<IAboutSchoolService>();
    }

    [Theory]
    [InlineData("Fake academy trust name")]
    [InlineData(null)]
    public async Task AboutPage_ShowsSchoolDetailsSummary(string? academyTrustName)
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            AcademyTrust = academyTrustName,
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.SchoolName, doc.GetRowContentByIdAndKey("school-details-summary", "Name"));
        if (academyTrustName is not null)
        {
            Assert.Contains(aboutSchoolModel.AcademyTrust!, doc.GetRowContentByIdAndKey("school-details-summary", "Academy Trust"));
        }
        else
        {
            Assert.Null(doc.GetRowContentByIdAndKey("school-details-summary", "Academy Trust"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AboutPage_ClosedSchool_ShowsSchoolClosedInfo(bool isClosed)
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            ClosedDate = isClosed ? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) : null,
            Status = isClosed ? EstablishmentStatus.Closed : EstablishmentStatus.Open,
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.SchoolName, doc.GetRowContentByIdAndKey("school-details-summary", "Name"));
        if (isClosed)
        {
            Assert.NotNull(doc.QuerySelector("[data-testid='school-closed-custom-card']"));
        }
        else
        {
            Assert.Null(doc.QuerySelector("[data-testid='school-closed-custom-card']"));
        }
    }

    [Fact]
    public async Task AboutPage_SchoolOpenedInLast3YearsAndHasPredecessors_ShowsSchoolPredecessorInfo()
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            Status = EstablishmentStatus.Open,
            OpenDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-440)),
            Predecessors = new List<EstablishmentLinkModel>()
            {
                new EstablishmentLinkModel()
                {
                    Urn = "123456",
                    Name = "Predecessor School 1"
                },
                new EstablishmentLinkModel()
                {
                    Urn = "123457",
                    Name = "Predecessor School 2"
                }
            }
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                aboutSchoolModel.Urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.SchoolName, doc.GetRowContentByIdAndKey("school-details-summary", "Name"));
        var predecessorInfo = doc.QuerySelector("[data-testid='school-predecessors-custom-card']");
        Assert.NotNull(predecessorInfo);
        Assert.Contains("Predecessor School 1", predecessorInfo.TextContent);
        Assert.Contains("Predecessor School 2", predecessorInfo.TextContent);
    }

    [Fact]
    public async Task AboutPage_SchoolOpenedInLast3YearsButHasNoPredecessors_NoPredecessorInfoShown()
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            Status = EstablishmentStatus.Open,
            OpenDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-440))
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                aboutSchoolModel.Urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.SchoolName, doc.GetRowContentByIdAndKey("school-details-summary", "Name"));
        var predecessorInfo = doc.QuerySelector("[data-testid='school-predecessors-custom-card']");
        Assert.Null(predecessorInfo);
    }

    [Fact]
    public async Task AboutPage_SchoolClosedAndHasSuccessors_ShowsSchoolSuccessorInfo()
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            Status = EstablishmentStatus.Closed,
            Successors = new List<EstablishmentLinkModel>()
            {
                new EstablishmentLinkModel()
                {
                    Urn = "123456",
                    Name = "Successor School 1"
                },
                new EstablishmentLinkModel()
                {
                    Urn = "123457",
                    Name = "Successor School 2"
                }
            }
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                aboutSchoolModel.Urn,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.SchoolName, doc.GetRowContentByIdAndKey("school-details-summary", "Name"));
        var successorInfo = doc.QuerySelector("[data-testid='school-closed-custom-card']");
        Assert.NotNull(successorInfo);
        Assert.Contains("Successor School 1", successorInfo.TextContent);
        Assert.Contains("Successor School 2", successorInfo.TextContent);
    }

    [Fact]
    public async Task AboutPage_ShowsSchoolLocationSummary()
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            Address = "Some address",
            LocalAuthority = "Bury"
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.Address, doc.GetRowContentByIdAndKey("school-location-summary", "Address"));
        Assert.Contains(aboutSchoolModel.LocalAuthority, doc.GetRowContentByIdAndKey("school-location-summary", "Local council"));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    public async Task AboutPage_ShowsSchoolFeatures(string officialSixthFormId)
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
            AgeRange = "11-16",
            HeadTeacher = _faker.Name.FullName(),
            NumberOfPupils = _faker.Random.Number(100, 2000).ToString(),
            PupilSex = "Mixed",
            ReligiousCharacter = "Church of England",
            OfficialSixthFormId = officialSixthFormId,
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(aboutSchoolModel.HeadTeacher, doc.GetRowContentByIdAndKey("school-features-summary", "Headteacher"));
        Assert.Contains(aboutSchoolModel.AgeRange, doc.GetRowContentByIdAndKey("school-features-summary", "Age range"));
        Assert.Contains(aboutSchoolModel.NumberOfPupils.ToInt()?.ToString("N0")!, doc.GetRowContentByIdAndKey("school-features-summary", "Number of pupils"));
        Assert.Contains(aboutSchoolModel.PupilSex, doc.GetRowContentByIdAndKey("school-features-summary", "Pupil sex"));
        Assert.Contains(aboutSchoolModel.ReligiousCharacter, doc.GetRowContentByIdAndKey("school-features-summary", "Religious character"));
        Assert.Contains(aboutSchoolModel.OfficialSixthFormId == "1" ? "Yes" : "No", doc.GetRowContentByIdAndKey("school-features-summary", "Sixth form"));
    }

    [Fact]
    public async Task AboutPage_ValuesNotAvailable_ShowsNotAvailableContent()
    {
        // Arrange
        var notAvailableContent = "Not available";
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143034",
            SchoolName = "St David's Church of England Academy",
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(notAvailableContent, doc.GetRowContentByIdAndKey("school-features-summary", "Headteacher"));
        Assert.Contains(notAvailableContent, doc.GetRowContentByIdAndKey("school-features-summary", "Age range"));
        Assert.Contains(notAvailableContent, doc.GetRowContentByIdAndKey("school-features-summary", "Number of pupils"));
        Assert.Contains(notAvailableContent, doc.GetRowContentByIdAndKey("school-features-summary", "Pupil sex"));
        Assert.Contains(notAvailableContent, doc.GetRowContentByIdAndKey("school-features-summary", "Religious character"));
        Assert.Contains(notAvailableContent, doc.GetRowContentByIdAndKey("school-features-summary", "Sixth form"));
    }

    [Theory]
    [InlineData("SEN unit", true, false)]
    [InlineData("Resourced provision and SEN unit", true, true)]
    [InlineData("Resourced provision", false, true)]
    public async Task AboutPage_ShowsCorrectResourcedProvisionAndSENUnitDetails(string ResourcedProvisionName, bool hasSENUnit, bool hasResourcedProvision)
    {
        // Arrange
        var aboutSchoolModel = new AboutSchoolModel()
        {
            Urn = "143031",
            SchoolName = "St David's Church of England School",
            ResourcedProvisionName = ResourcedProvisionName
        };
        _about
            .Setup(service => service.GetAboutSchoolDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(aboutSchoolModel);

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(aboutSchoolModel.Urn, aboutSchoolModel.SchoolName, _pageRoute));

        // Assert
        Assert.Contains(hasSENUnit ? "Yes" : "No", doc.GetRowContentByIdAndKey("school-features-summary", "SEN unit"));
        Assert.Contains(hasResourcedProvision ? "Yes" : "No", doc.GetRowContentByIdAndKey("school-features-summary", "Resourced provision"));
    }
}
