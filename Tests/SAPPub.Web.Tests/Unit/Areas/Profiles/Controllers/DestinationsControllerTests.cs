using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Destinations;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.Destinations;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.Charts;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers
{
    public class DestinationsControllerTests
    {
        private readonly Mock<ILogger<DestinationsController>> _mockLogger;
        private readonly Mock<IEstablishmentService> _mockEstablishmentService;
        private readonly Mock<IDestinationsService> _mockDestinationsService;
        private readonly DestinationsController _controller;
        private EstablishmentServiceModel _fakeEstablishment;

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
        public async Task Get_KS4Destinations_Info_ReturnsOk()
        {
            var destinationsDetails = new KS4DestinationsDetailsBuilder()
                .WithUrn(_fakeEstablishment.URN)
                .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
                .WithLAName(_fakeEstablishment.LAName)
                .WithKS4(true)
                .Build();

            _mockDestinationsService
                .Setup(es => es.GetKS4DestinationsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
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

            var model = result.Model as KS4DestinationsViewModel;
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
        public async Task Get_KS4Destinations_Info_ResultsNotAvailable_ReturnsOk()
        {
            var destinationsDetails = new KS4DestinationsDetailsBuilder()
                .WithUrn(_fakeEstablishment.URN)
                .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
                .WithLAName(_fakeEstablishment.LAName)
                .WithKS4(true)
                .BuildResultsNotAvailable();

            _mockDestinationsService
                .Setup(es => es.GetKS4DestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);

            var result = await _controller.KS4(_mockDestinationsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

            string[] expectedAllDestCurrentDataLabels = ["School", $"{_fakeEstablishment.LAName} average", "England average"];
            string[] expectedBreakdownCurrentYearDataLabels = ["Staying in education", "Entering employment and apprenticeships"];

            Assert.NotNull(result);
            Assert.NotNull(result.Model);

            var model = result.Model as KS4DestinationsViewModel;
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
        public async Task Get_KS4Destinations_Info_LocalCouncilName(string localCouncilName, string expectedCouncilName)
        {
            _fakeEstablishment.LAName = localCouncilName;
            var destinationsDetails = new KS4DestinationsDetailsBuilder()
                 .WithUrn(_fakeEstablishment.URN)
                 .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
                 .WithLAName(_fakeEstablishment.LAName)
                 .WithKS4(true)
                 .Build();

            _mockDestinationsService
                .Setup(es => es.GetKS4DestinationsDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);

            var result = await _controller.KS4(_mockDestinationsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

            string[] expectedAllDestCurrentDataLabels = ["School", expectedCouncilName, "England average"];
            string[] expectedDataOvertimeDataLabels = ["School", expectedCouncilName, "England average"];
            string[] expectedBreakdownDataLabels = ["School", expectedCouncilName, "England average"];

            Assert.NotNull(result);
            Assert.NotNull(result.Model);

            var model = result.Model as KS4DestinationsViewModel;
            Assert.NotNull(model);
            Assert.Equal(_fakeEstablishment.URN, model.URN);
            Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

            Assert.Equal(expectedAllDestCurrentDataLabels, model.AllDestinationsData.Labels);

            var actualDataOvertimeDataLabels = model.AllDestinationsOverTimeData.Datasets.Select(s => s.Label).ToArray();
            Assert.Equal(expectedDataOvertimeDataLabels, actualDataOvertimeDataLabels);

            var actualBreakdownDataLabels = model.BreakdownDestinationData.Datasets.Select(s => s.Label).ToArray();
            Assert.Equal(expectedBreakdownDataLabels, actualBreakdownDataLabels);
        }

        [Fact]
        public async Task Get_KS5Destinations_Info_ReturnsOk()
        {
            var destinationsDetails = new KS5DestinationsDetails
            {
                Urn = _fakeEstablishment.URN,
                LocalAuthorityName = _fakeEstablishment.LAName,
                SchoolName = _fakeEstablishment.EstablishmentName,
                IsKS2 = false,
                IsKS4 = false,
                IsKS5 = false,
                EstablishmentTotalOverall = 88,
                LATotalOverall = 77,
                EnglandOverall = 66
            };

            double?[] expectedAllDestData =
            [
                destinationsDetails.EstablishmentTotalOverall = 88,
                destinationsDetails.LATotalOverall = 66,
                destinationsDetails.EnglandOverall = 77,
            ];

            _mockDestinationsService
                .Setup(es => es.GetKS5DestinationsDetailsAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
                .ReturnsAsync(destinationsDetails);

            var result = await _controller.KS5(_mockDestinationsService.Object, _fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, CancellationToken.None) as ViewResult;

            string[] expectedAllDestDataLabels = ["School or College", $"{_fakeEstablishment.LAName} average", "England average"];


            Assert.NotNull(result);
            Assert.NotNull(result.Model);

            var model = result.Model as KS5DestinationsViewModel;
            Assert.NotNull(model);
            Assert.Equal(_fakeEstablishment.URN, model.URN);
            Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

            Assert.Equal(expectedAllDestDataLabels, model.AllDestinationsData.Labels);
            Assert.Equal(expectedAllDestData, model.AllDestinationsData.Data);

            Assert.Equal(2, model.RouteAttributes.Count);
            Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
            Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        }
    }
}