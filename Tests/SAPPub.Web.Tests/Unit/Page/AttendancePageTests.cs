using Moq;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Core.ServiceModels.KS4.Attendance;
using SAPPub.Web.Tests.UI.Helpers;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Page;

[Collection("WebAppCollection")]
public class AttendancePageTests : PageTestsBase
{
    private static string _pageRoute = "/attendance";
    private readonly Mock<IAttendanceService> _serviceMock;

    public AttendancePageTests(WebAppFixture fixture) : base(fixture)
    {
        _serviceMock = UseMock<IAttendanceService>();
    }

    [Fact]
    public async Task AttendancePage_HasCorrectTitle()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var title = doc.Title;
        Assert.Contains("Loreto High School Chorlton - Secondary Attendance - School Profiles - GOV.UK", title);
    }

    [Fact]
    public async Task AttendancePage_DisplaysMainHeading()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Assert
        var heading = doc.QuerySelector("h1")?.TextContent.Trim();
        Assert.Equal("Attendance", heading);
    }

    [Fact]
    public async Task AttendancePage_Displays_SchoolName_Caption()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));
        var schoolNameCaptionElement = doc.GetElementById("school-name-caption");
        var schoolNameCaption = schoolNameCaptionElement?.TextContent;

        // Assert
        Assert.Equal("Loreto High School Chorlton", schoolNameCaption);
    }

    [Fact]
    public async Task AttendancePage_Displays_VerticalNavigation()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentAttendance = null,
                LocalAuthorityAttendance = null,
                EnglandAttendance = null,
                IsKS2 = false,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var pageUrl = BuildUrl(urn, establishmentName, _pageRoute);
        var doc = await Fixture.BrowseToPage(pageUrl);

        var nav = new VerticalNavigationAssertHelper(doc);

        nav.ShouldBeVisibleAsync();
        nav.ShouldHaveItemsCountAsync(6);
        nav.ShouldHaveOneActiveItemAsync();
        nav.ShouldHaveActiveHrefAsync(pageUrl);
    }

    [Fact]
    public async Task AttendancePage_Displays_AttendancePolicy_Summary()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                Website = "https://website.com",
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));
        var summaryCard = doc.GetElementById("attendance-policy-summary");

        var contactSchoolInfo = summaryCard?.QuerySelector("[data-testid='contact-school-info']");
        var schoolWebsiteLink = summaryCard?.QuerySelector("[data-testid='school-website-link']");
        var commissionerWebsiteLink = summaryCard?.QuerySelector("[data-testid='commissioner-website-link']");
        var schoolWebsiteHref = schoolWebsiteLink?.GetAttribute("href");
        var schoolWebsiteText = schoolWebsiteLink?.TextContent;
        var commissionerWebsiteHref = commissionerWebsiteLink?.GetAttribute("href");
        var commissionerWebsiteText = commissionerWebsiteLink?.TextContent;


        // Assert
        Assert.NotNull(summaryCard);
        Assert.Null(contactSchoolInfo);
        Assert.NotNull(schoolWebsiteLink);
        Assert.NotNull(commissionerWebsiteLink);

        Assert.NotNull(schoolWebsiteHref);
        Assert.NotNull(schoolWebsiteText);
        Assert.NotNull(commissionerWebsiteHref);
        Assert.NotNull(commissionerWebsiteText);
    }

    [Fact]
    public async Task AttendancePage_DisplaysPagination()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        // Act
        var attendancePagination = doc.GetElementById("attendance-pagination");

        // Act
        var previousPaginationLink = doc.QuerySelector("#attendance-pagination .govuk-pagination__prev a");
        var nextPaginationLink = doc.QuerySelector("#attendance-pagination .govuk-pagination__next a");

        var previousPaginationText = previousPaginationLink?.TextContent;
        var nextPaginationText = nextPaginationLink?.TextContent;

        // Assert
        Assert.NotNull(attendancePagination);
        Assert.Equal("Curriculum and extra-curricular activities", previousPaginationText?.Trim());
        Assert.Equal("Secondary academic performance: Progress and attainment", nextPaginationText?.Trim());
    }

    [Fact]
    public async Task AttendancePage_Displays_ContactSchoolText()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        var contactSchoolInfo = doc.QuerySelector("[data-testid='contact-school-info']");

        // Assert
        Assert.NotNull(contactSchoolInfo);
    }

    [Fact]
    public async Task AttendancePage_Displays_AttendanceRate_Summary()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));
        var summaryCard = doc.GetElementById("details-attendance-rate");

        // Assert
        Assert.NotNull(summaryCard);
    }

    [Fact]
    public async Task AttendancePage_Displays_Attendance_Table()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                LocalAuthority = "Sheffield",
                EstablishmentAttendance = 50.5,
                LocalAuthorityAttendance = 70.9,
                EnglandAttendance = 65.7,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        Assert.Contains("Sheffield", doc.GetTableHeaderContentByIdAndIndex("attendance-table", 2, 0));
        Assert.Contains("50.5%", doc.GetTableCellContentByIdAndIndex("attendance-table", 1, 0));
        Assert.Contains("50.5%", doc.GetTableCellContentByIdAndIndex("attendance-table", 1, 1));
        Assert.Contains("70.9%", doc.GetTableCellContentByIdAndIndex("attendance-table", 2, 0));
        Assert.Contains("70.9%", doc.GetTableCellContentByIdAndIndex("attendance-table", 2, 1));
        Assert.Contains("65.7%", doc.GetTableCellContentByIdAndIndex("attendance-table", 3, 0));
        Assert.Contains("65.7%", doc.GetTableCellContentByIdAndIndex("attendance-table", 3, 1));
    }

    [Fact]
    public async Task AttendancePage_Displays_Attendance_Table_With_NotAvailable()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentAttendance = null,
                LocalAuthorityAttendance = null,
                EnglandAttendance = null,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        Assert.Equal(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 1, 0));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 1, 1));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 2, 0));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 2, 1));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 3, 0));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 3, 1));
    }

    [Fact]
    public async Task AttendancePage_Displays_PersistentAbsence_Table()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        var enrolmentsTotal = 1200;
        var absenceTotal = 200;

        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                LocalAuthority = "Sheffield",
                EstablishmentPersistentAbsence = 10.3,
                LocalAuthorityPersistentAbsence = 5.9,
                EnglandPersistentAbsence = 6.7,
                EstablishmentEnrolmentsTotal = enrolmentsTotal,
                EstablishmentPersistentAbsenceTotal = absenceTotal,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        Assert.Contains("Sheffield", doc.GetTableHeaderContentByIdAndIndex("persistent-absence-table", 2, 0));
        Assert.Contains("10.3%", doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 1, 0));
        Assert.Contains("10.3%", doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 1, 1));
        Assert.Contains("5.9%", doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 2, 0));
        Assert.Contains("5.9%", doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 2, 1));
        Assert.Contains("6.7%", doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 3, 0));
        Assert.Contains("6.7%", doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 3, 1));

        var expectedEnrolmentsTotal = enrolmentsTotal.ToString("N0");
        var expectedAbsenceTotal = absenceTotal.ToString("N0");

        Assert.Contains(expectedEnrolmentsTotal, doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 1, 1));
        Assert.Contains(expectedAbsenceTotal, doc.GetTableCellContentByIdAndIndex("persistent-absence-table", 1, 1));
    }

    [Fact]
    public async Task AttendancePage_Displays_PersistentAbsence_Table_With_NotAvailable()
    {
        // Arrange
        var urn = "143034";
        var establishmentName = "Loreto High School Chorlton";
        _serviceMock
            .Setup(service => service.GetAttendenceDetailsAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AttendanceModel()
            {
                Urn = urn,
                SchoolName = establishmentName,
                EstablishmentAttendance = null,
                LocalAuthorityAttendance = null,
                EnglandAttendance = null,
                IsKS2 = true,
                IsKS4 = true,
                IsKS5 = false
            });

        // Act
        var doc = await Fixture.BrowseToPage(BuildUrl(urn, establishmentName, _pageRoute));

        Assert.Equal(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 1, 0));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 1, 1));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 2, 0));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 2, 1));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 3, 0));
        Assert.Contains(NotAvailable, doc.GetTableCellContentByIdAndIndex("attendance-table", 3, 1));
    }
}
