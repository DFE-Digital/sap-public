using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.KS4.SubjectEntries;
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
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.Destinations;
using SAPPub.Web.Constants;
using SAPPub.Web.Controllers;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;
using SAPPub.Web.Models.SecondarySchool;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers
{
    public class DestinationsControllerTests
    {
        private readonly Mock<ILogger<DestinationsController>> _mockLogger;
        private readonly Mock<IEstablishmentService> _mockEstablishmentService;
        private readonly Mock<IDestinationsService> _mockDestinationsService;
        private readonly DestinationsController _controller;
        private EstablishmentServiceModel _fakeEstablishment;

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

        public DestinationsControllerTests()
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

            _mockLogger = new Mock<ILogger<DestinationsController>>();
            _mockEstablishmentService = new();
            _mockDestinationsService = new();

            _mockEstablishmentService
                .Setup(es => es.GetEstablishmentAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fakeEstablishment);

            var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            _controller = new DestinationsController(_mockLogger.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task Get_Destinations_Info_ReturnsOk()
        {
            var destinationsDetails = new DestinationsDetailsBuilder()
                .WithUrn(_fakeEstablishment.URN)
                .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
                .WithLAName(_fakeEstablishment.LAName)
                .Build();

            _mockDestinationsService
                .Setup(es => es.GetDestinationsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);

            var result = await _controller.KS4(_mockDestinationsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

            string[] expectedAllDestCurrentDataLabels = ["School", $"{_fakeEstablishment.LAName} average", "England average"];
            double?[] expectedAllDestCurrentData =
            [
                destinationsDetails.SchoolAll.CurrentYear,
            destinationsDetails.LocalAuthorityAll.CurrentYear,
            destinationsDetails.EnglandAll.CurrentYear
            ];

            var expectedDataOverTime = new DataOverTimeViewModel
            {
                Labels = ["2020 to 2021", "2021 to 2022", "2022 to 2023"],
                Datasets =
            [
                new DatasetViewModel
            {
                Label = "School",
                Data = [destinationsDetails.SchoolAll.TwoYearsAgo, destinationsDetails.SchoolAll.PreviousYear, destinationsDetails.SchoolAll.CurrentYear],
            },
            new DatasetViewModel
            {
                Label = $"{destinationsDetails.LocalAuthorityName} average",
                Data = [destinationsDetails.LocalAuthorityAll.TwoYearsAgo, destinationsDetails.LocalAuthorityAll.PreviousYear, destinationsDetails.LocalAuthorityAll.CurrentYear],
            },
            new DatasetViewModel
            {
                Label = "England average",
                Data = [destinationsDetails.EnglandAll.TwoYearsAgo, destinationsDetails.EnglandAll.PreviousYear, destinationsDetails.EnglandAll.CurrentYear],
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
                Data = [destinationsDetails.SchoolEducation.CurrentYear, CommonHelper.AddNullable(destinationsDetails.SchoolEmployment.CurrentYear, destinationsDetails.SchoolApprentice.CurrentYear)]
            },
            new DataSeriesViewModel
            {
                Label = $"{destinationsDetails.LocalAuthorityName} average",
                Data = [destinationsDetails.LocalAuthorityEducation.CurrentYear, CommonHelper.AddNullable(destinationsDetails.LocalAuthorityEmployment.CurrentYear, destinationsDetails.LocalAuthorityApprentice.CurrentYear)]
            },
            new DataSeriesViewModel
            {
                Label = "England average",
                Data = [destinationsDetails.EnglandEducation.CurrentYear, CommonHelper.AddNullable(destinationsDetails.EnglandEmployment.CurrentYear, destinationsDetails.EnglandApprentice.CurrentYear)]
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

            Assert.Equal(expectedDataOverTime.Labels, model.AllDestinationsOverTimeData.Labels);
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
            Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        }

        [Fact]
        public async Task Get_Destinations_Info_ResultsNotAvailable_ReturnsOk()
        {
            var destinationsDetails = new DestinationsDetailsBuilder()
                .WithUrn(_fakeEstablishment.URN)
                .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
                .WithLAName(_fakeEstablishment.LAName)
                .BuildResultsNotAvailable();

            _mockDestinationsService
                .Setup(es => es.GetDestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);

            var result = await _controller.KS4(_mockDestinationsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

            string[] expectedAllDestCurrentDataLabels = ["School", $"{_fakeEstablishment.LAName} average", "England average"];
            string[] expectedBreakdownCurrentYearDataLabels = ["Staying in education", "Entering employment and apprenticeships"];

            Assert.NotNull(result);
            Assert.NotNull(result.Model);

            var model = result.Model as DestinationsViewModel;
            Assert.NotNull(model);
            Assert.Equal(_fakeEstablishment.URN, model.URN);
            Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

            Assert.Equal(expectedAllDestCurrentDataLabels, model.AllDestinationsData.Labels);
            Assert.Equal([null, null, null], model.AllDestinationsData.Data);

            Assert.Equal(3, model.AllDestinationsOverTimeData.Datasets.Count);
            Assert.Equal("School", model.AllDestinationsOverTimeData.Datasets[0].Label);
            Assert.Equal([null, null, null], model.AllDestinationsOverTimeData.Datasets[0].Data);

            Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllDestinationsOverTimeData.Datasets[1].Label);
            Assert.Equal([null, null, null], model.AllDestinationsOverTimeData.Datasets[1].Data);

            Assert.Equal("England average", model.AllDestinationsOverTimeData.Datasets[2].Label);
            Assert.Equal(new double?[] { null, null, null }, model.AllDestinationsOverTimeData.Datasets[2].Data);

            Assert.Equal(["2020 to 2021", "2021 to 2022", "2022 to 2023"], model.AllDestinationsOverTimeData.Labels);

            // Breakdown gcse data assert
            Assert.Equal(["Staying in education", "Entering employment and apprenticeships"], model.BreakdownDestinationData.Labels);

            Assert.Equal(3, model.BreakdownDestinationData.Datasets.Count);

            Assert.Equal("School", model.BreakdownDestinationData.Datasets[0].Label);
            Assert.Equal([null, null], model.BreakdownDestinationData.Datasets[0].Data);

            Assert.Equal($"{_fakeEstablishment.LAName} average", model.BreakdownDestinationData.Datasets[1].Label);
            Assert.Equal([null, null], model.BreakdownDestinationData.Datasets[1].Data);

            Assert.Equal("England average", model.BreakdownDestinationData.Datasets[2].Label);
            Assert.Equal([null, null], model.BreakdownDestinationData.Datasets[2].Data);
        }

        [Theory]
        [InlineData("Sheffield", "Sheffield average")]
        [InlineData("Poole Grammar School", "Local council average")]
        public async Task Get_Destinations_Info_LocalCouncilName(string localCouncilName, string expectedCouncilName)
        {
            _fakeEstablishment.LAName = localCouncilName;
            var destinationsDetails = new DestinationsDetailsBuilder()
                 .WithUrn(_fakeEstablishment.URN)
                 .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
                 .WithLAName(_fakeEstablishment.LAName)
                 .Build();

            _mockDestinationsService
                .Setup(es => es.GetDestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);

            var result = await _controller.KS4(_mockDestinationsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

            string[] expectedAllDestCurrentDataLabels = ["School", expectedCouncilName, "England average"];
            string[] expectedDataOvertimeDataLabels = ["School", expectedCouncilName, "England average"];
            string[] expectedBreakdownDataLabels = ["School", expectedCouncilName, "England average"];

            Assert.NotNull(result);
            Assert.NotNull(result.Model);

            var model = result.Model as DestinationsViewModel;
            Assert.NotNull(model);
            Assert.Equal(_fakeEstablishment.URN, model.URN);
            Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

            Assert.Equal(expectedAllDestCurrentDataLabels, model.AllDestinationsData.Labels);

            var actualDataOvertimeDataLabels = model.AllDestinationsOverTimeData.Datasets.Select(s => s.Label).ToArray();
            Assert.Equal(expectedDataOvertimeDataLabels, actualDataOvertimeDataLabels);

            var actualBreakdownDataLabels = model.BreakdownDestinationData.Datasets.Select(s => s.Label).ToArray();
            Assert.Equal(expectedBreakdownDataLabels, actualBreakdownDataLabels);
        }
    }
}
