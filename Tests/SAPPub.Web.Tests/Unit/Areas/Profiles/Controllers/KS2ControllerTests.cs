using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS2;
using SAPPub.Web.Models.Config;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class KS2ControllerTests : BaseProfilesTests
{
    private readonly string primaryschoolAccountabilityLinkUrl = "https://test.com";
    private readonly bool primarySchoolAccountabilityLinkNewTab = true;
    private readonly Mock<IKS2AdditionalMeasuresService> _mockKS2AdditionalMeasuresService = new();
    private readonly Mock<IKS2PupilProgressService> _mockKS2PupilProgressService = new();
    private readonly Mock<IKS2MeetingOrExceedingStandardsService> _mockKS2MeetingOrExceedingStandardsService = new();
    private readonly KS2Controller _controller;

    public KS2ControllerTests()
    {
        var opts = Options.Create(new UrlLinksOptions
        {
            PrimarySchoolAccountability = new UrlLinkOptions { Url = primaryschoolAccountabilityLinkUrl, NewTab = primarySchoolAccountabilityLinkNewTab }
        });

        _mockKS2AdditionalMeasuresService = new Mock<IKS2AdditionalMeasuresService>();
        _controller = new(opts) { Establishment = fakeMinimumEstablishment };
    }

    [Fact]
    public void AcademicPerformancePupilProgress_RedirectsAsExpected()
    {
        // Arrange
        AcademicYearSelection selectedAcademicYear = AcademicYearSelection.Previous;

        // Act
        var result = _controller.AcademicPerformancePupilProgress(
            fakeEstablishment.URN,
            fakeEstablishment.EstablishmentName,
            selectedAcademicYear) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("AcademicPerformancePupilProgress", result.ActionName);
        Assert.Equal(fakeEstablishment.URN, result?.RouteValues!["urn"]);
        Assert.Equal(fakeEstablishment.EstablishmentName, result?.RouteValues!["schoolName"]);
        Assert.Equal("previous", result?.RouteValues!["selectedAcademicYearName"]);
    }

    [Fact]
    public async Task Get_AcademicPerformancePupilProgress_ReturnsNotFoundWhenIncorrectAcademicYear()
    {
        //Act
        var result = await _controller.AcademicPerformancePupilProgress(
            _mockKS2PupilProgressService.Object,
            fakeEstablishment.URN,
            fakeEstablishment.EstablishmentName,
            "randomyear",
            CancellationToken.None) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Get_AcademicPerformancePupilProgress_ReturnsCorrectData()
    {
        // Arrange
        var expectedModel = GetKS2PupilPerformance();
        _mockKS2PupilProgressService
            .Setup(a => a.GetPupilProgressAsync(fakeMinimumEstablishment.URN, AcademicYearSelection.Previous2, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        //Act
        var result = await _controller.AcademicPerformancePupilProgress(
            _mockKS2PupilProgressService.Object,
            fakeMinimumEstablishment.URN,
            fakeMinimumEstablishment.EstablishmentName,
            AcademicYearSelection.Previous2.ToString().ToLower(),
            CancellationToken.None) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<AcademicPerformancePupilProgressViewModel>(result?.Model);
        Assert.Equal(expectedModel.Urn, model.URN);
        Assert.Equal(primaryschoolAccountabilityLinkUrl, model.PrimarySchoolAccountabilityLinkUrl);
        Assert.Equal(primarySchoolAccountabilityLinkNewTab, model.PrimarySchoolAccountabilityLinkNewTab);
        Assert.Equal($"Information in this section is for the 2022 to 2023 academic year.", model.AcademicYearInfoParagraph);
        Assert.False(model.ShowDataNotAvailableInfo);
        Assert.True(model.ShowReadingScore);
        Assert.True(model.ShowWritingScore);
        Assert.True(model.ShowMathsScore);
        Assert.Equal(expectedModel.EstablishmentReadingScore, model.EstablishmentReadingScore.Score);
        Assert.Equal(expectedModel.EstablishmentReadingConfidenceLower, model.EstablishmentReadingScore.ConfidenceLevelLower);
        Assert.Equal(expectedModel.EstablishmentReadingConfidenceUpper, model.EstablishmentReadingScore.ConfidenceLevelUpper);
        Assert.Equal(expectedModel.EstablishmentReadingDescription, model.EstablishmentReadingScore.BandingRating);
        Assert.Equal(expectedModel.EstablishmentWritingScore, model.EstablishmentWritingScore.Score);
        Assert.Equal(expectedModel.EstablishmentWritingConfidenceLower, model.EstablishmentWritingScore.ConfidenceLevelLower);
        Assert.Equal(expectedModel.EstablishmentWritingConfidenceUpper, model.EstablishmentWritingScore.ConfidenceLevelUpper);
        Assert.Equal(expectedModel.EstablishmentWritingDescription, model.EstablishmentWritingScore.BandingRating);
        Assert.Equal(expectedModel.EstablishmentMathsScore, model.EstablishmentMathsScore.Score);
        Assert.Equal(expectedModel.EstablishmentMathsConfidenceLower, model.EstablishmentMathsScore.ConfidenceLevelLower);
        Assert.Equal(expectedModel.EstablishmentMathsConfidenceUpper, model.EstablishmentMathsScore.ConfidenceLevelUpper);
        Assert.Equal(expectedModel.EstablishmentMathsDescription, model.EstablishmentMathsScore.BandingRating);
        Assert.Equal(expectedModel.LaReadingScore, model.LaReadingAverage);
        Assert.Equal(expectedModel.LaWritingScore, model.LaWritingAverage);
        Assert.Equal(expectedModel.LaMathsScore, model.LaMathsAverage);
        Assert.Equal(AcademicYearSelection.Previous2, model.SelectedAcademicYear);

        _mockKS2PupilProgressService
            .Verify(a => a.GetPupilProgressAsync(fakeMinimumEstablishment.URN, AcademicYearSelection.Previous2, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_InvalidYearSelected_ReturnsNotFound()
    {
        // Arrange
        var expectedModel = GetKS2AdditionalMeasuresModel();

        _mockKS2AdditionalMeasuresService
            .Setup(a => a.GetAdditionalMeasures(fakeMinimumEstablishment.URN, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        // Act
        var result = await _controller.AcademicPerformanceAdditionalMeasures(
             _mockKS2AdditionalMeasuresService.Object,
             fakeMinimumEstablishment.URN,
             fakeMinimumEstablishment.EstablishmentName,
             CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsType<AcademicPerformanceAdditionalMeasuresViewModel>(result?.Model);
        Assert.Equal(fakeMinimumEstablishment.URN, model.URN);
        Assert.True(model.IsKS2);
        Assert.Equal(expectedModel.EstablishmentGrammarAtExpectedStandard, model.EstablishmentGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.EstablishmentGrammarAtHigherStandard, model.EstablishmentGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.EstablishmentEHCPPopulation, model.EstablishmentEHCPPopulation.Value);
        Assert.Equal(expectedModel.EstablishmentSENSupportPopulation, model.EstablishmentSENSupportPopulation.Value);
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard, model.LAGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard, model.LAGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard, model.EnglandGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard, model.EnglandGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.EnglandEHCPPopulation, model.EnglandEHCPPopulation.Value);
        Assert.Equal(expectedModel.EnglandSENSupportPopulation, model.EnglandSENSupportPopulation.Value);

        _mockKS2AdditionalMeasuresService
            .Verify(a => a.GetAdditionalMeasures(fakeMinimumEstablishment.URN, CancellationToken.None), Times.Once);
    }


    [Fact]
    public async Task Get_AcademicPerformanceMeetingOrExceedingStandards_ReturnsValidViewModel()
    {
        // Arrange
        var expectedModel = GetKS2MeetingOrExceedingStandardsModel();

        _mockKS2MeetingOrExceedingStandardsService
            .Setup(a => a.GetMeetingOrExceedingStandardsPercentages(fakeMinimumEstablishment.URN, fakeMinimumEstablishment.LAId, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        // Act
        var result = await _controller.AcademicPerformanceMeetingOrExceedingStandards(
             _mockKS2MeetingOrExceedingStandardsService.Object,
             fakeMinimumEstablishment.URN,
             fakeMinimumEstablishment.EstablishmentName,
             CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsType<AcademicPerformanceMeetingOrExceedingStandardsViewModel>(result?.Model);
        Assert.Equal(fakeMinimumEstablishment.URN, model.URN);
        Assert.True(model.IsKS2);
        Assert.Equal(expectedModel.EstablishmentPercentageMeetingOrExceeding.CurrentYear.Value, model.AllMeetingExceedingStandardsData!.Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear.Value, model.AllMeetingExceedingStandardsData!.Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageMeetingOrExceeding.CurrentYear.Value, model.AllMeetingExceedingStandardsData!.Data[2]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageMeetingOrExceeding.TwoYearsAgo.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[0].Data[0]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageMeetingOrExceeding.PreviousYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[0].Data[1]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageMeetingOrExceeding.CurrentYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[0].Data[2]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageMeetingOrExceeding.TwoYearsAgo.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[1].Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageMeetingOrExceeding.PreviousYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[1].Data[1]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageMeetingOrExceeding.CurrentYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[1].Data[2]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageMeetingOrExceeding.TwoYearsAgo.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[2].Data[0]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageMeetingOrExceeding.PreviousYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[2].Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageMeetingOrExceeding.CurrentYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[2].Data[2]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageExceeding.CurrentYear.Value, model.AllExceedingStandardsData!.Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageExceeding.CurrentYear.Value, model.AllExceedingStandardsData!.Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageExceeding.CurrentYear.Value, model.AllExceedingStandardsData!.Data[2]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageExceeding.TwoYearsAgo.Value, model.AllExceedingStandardsOverTimeData!.Datasets[0].Data[0]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageExceeding.PreviousYear.Value, model.AllExceedingStandardsOverTimeData!.Datasets[0].Data[1]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentageExceeding.CurrentYear.Value, model.AllExceedingStandardsOverTimeData!.Datasets[0].Data[2]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageExceeding.TwoYearsAgo.Value, model.AllExceedingStandardsOverTimeData!.Datasets[1].Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageExceeding.PreviousYear.Value, model.AllExceedingStandardsOverTimeData!.Datasets[1].Data[1]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentageExceeding.CurrentYear.Value, model.AllExceedingStandardsOverTimeData!.Datasets[1].Data[2]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageExceeding.TwoYearsAgo.Value, model.AllExceedingStandardsOverTimeData!.Datasets[2].Data[0]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageExceeding.PreviousYear.Value, model.AllExceedingStandardsOverTimeData!.Datasets[2].Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandPercentageExceeding.CurrentYear.Value, model.AllExceedingStandardsOverTimeData!.Datasets[2].Data[2]!.Value);

        Assert.Equal(expectedModel.GirlsMeetingExpectedStandard.Value.ToString(), model.GirlsAndBoys.Rows.First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.GirlsExceedingExpectedStandard.Value.ToString(), model.GirlsAndBoys.Rows.First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.BoysMeetingExpectedStandard.Value.ToString(), model.GirlsAndBoys.Rows.Skip(1).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.BoysExceedingExpectedStandard.Value.ToString(), model.GirlsAndBoys.Rows.Skip(1).First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.AllPupilsMeetingExpectedStandard.Value.ToString(), model.GirlsAndBoys.Rows.Skip(2).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.AllPupilsExceedingExpectedStandard.Value.ToString(), model.GirlsAndBoys.Rows.Skip(2).First().ExceedingStandard.Value.ToString());

        Assert.Equal(expectedModel.EALMeetingExpectedStandard.Value.ToString(), model.EnglishAsAnAdditionalLanguage.Rows.First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.EALExceedingExpectedStandard.Value.ToString(), model.EnglishAsAnAdditionalLanguage.Rows.First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.AllPupilsMeetingExpectedStandard.Value.ToString(), model.EnglishAsAnAdditionalLanguage.Rows.Skip(1).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.AllPupilsExceedingExpectedStandard.Value.ToString(), model.EnglishAsAnAdditionalLanguage.Rows.Skip(1).First().ExceedingStandard.Value.ToString());

        Assert.Equal(expectedModel.NonMobileMeetingExpectedStandard.Value.ToString(), model.NonMobilePupils.Rows.First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.NonMobileExceedingExpectedStandard.Value.ToString(), model.NonMobilePupils.Rows.First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.AllPupilsMeetingExpectedStandard.Value.ToString(), model.NonMobilePupils.Rows.Skip(1).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.AllPupilsExceedingExpectedStandard.Value.ToString(), model.NonMobilePupils.Rows.Skip(1).First().ExceedingStandard.Value.ToString());

        Assert.Equal(expectedModel.EstablishmentDisadvantagedMeetingExpectedStandard.Value.ToString(), model.DisadvantagedPupils.Rows.First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.EstablishmentDisadvantagedExceedingExpectedStandard.Value.ToString(), model.DisadvantagedPupils.Rows.First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.LocalAuthorityDisadvantagedMeetingExpectedStandard.Value.ToString(), model.DisadvantagedPupils.Rows.Skip(1).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.LocalAuthorityDisadvantagedExceedingExpectedStandard.Value.ToString(), model.DisadvantagedPupils.Rows.Skip(1).First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.EnglandDisadvantagedMeetingExpectedStandard.Value.ToString(), model.DisadvantagedPupils.Rows.Skip(2).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.EnglandDisadvantagedExceedingExpectedStandard.Value.ToString(), model.DisadvantagedPupils.Rows.Skip(2).First().ExceedingStandard.Value.ToString());

        Assert.Equal(expectedModel.LocalAuthorityNonDisadvantagedMeetingExpectedStandard.Value.ToString(), model.NonDisadvantagedPupils.Rows.First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.LocalAuthorityNonDisadvantagedExceedingExpectedStandard.Value.ToString(), model.NonDisadvantagedPupils.Rows.First().ExceedingStandard.Value.ToString());
        Assert.Equal(expectedModel.EnglandNonDisadvantagedMeetingExpectedStandard.Value.ToString(), model.NonDisadvantagedPupils.Rows.Skip(1).First().MeetingStandard.Value.ToString());
        Assert.Equal(expectedModel.EnglandNonDisadvantagedExceedingExpectedStandard.Value.ToString(), model.NonDisadvantagedPupils.Rows.Skip(1).First().ExceedingStandard.Value.ToString());


        _mockKS2MeetingOrExceedingStandardsService
            .Verify(a => a.GetMeetingOrExceedingStandardsPercentages(fakeMinimumEstablishment.URN, fakeMinimumEstablishment.LAId, CancellationToken.None), Times.Once);
    }

    private static KS2MeetingOrExceedingStandardsModel GetKS2MeetingOrExceedingStandardsModel()
    {
        return new KS2MeetingOrExceedingStandardsModel
        {
            EstablishmentPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = GetCodedDouble(1),
                PreviousYear = GetCodedDouble(2),
                TwoYearsAgo = GetCodedDouble(3)
            },
            LocalAuthorityPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = GetCodedDouble(4),
                PreviousYear = GetCodedDouble(5),
                TwoYearsAgo = GetCodedDouble(6)
            },

            EnglandPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = GetCodedDouble(7),
                PreviousYear = GetCodedDouble(8),
                TwoYearsAgo = GetCodedDouble(9)
            },
            EstablishmentPercentageExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = GetCodedDouble(10),
                PreviousYear = GetCodedDouble(11),
                TwoYearsAgo = GetCodedDouble(12)
            },
            LocalAuthorityPercentageExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = GetCodedDouble(13),
                PreviousYear = GetCodedDouble(14),
                TwoYearsAgo = GetCodedDouble(15)
            },

            EnglandPercentageExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = GetCodedDouble(16),
                PreviousYear = GetCodedDouble(17),
                TwoYearsAgo = GetCodedDouble(18)
            },
            AllPupilsExceedingExpectedStandard = GetCodedDouble(19),
            AllPupilsMeetingExpectedStandard = GetCodedDouble(20),
            BoysExceedingExpectedStandard = GetCodedDouble(21),
            BoysMeetingExpectedStandard = GetCodedDouble(22),
            EALExceedingExpectedStandard = GetCodedDouble(23),
            EALMeetingExpectedStandard = GetCodedDouble(24),
            EnglandDisadvantagedExceedingExpectedStandard = GetCodedDouble(25),
            EnglandDisadvantagedMeetingExpectedStandard = GetCodedDouble(26),
            EnglandNonDisadvantagedExceedingExpectedStandard = GetCodedDouble(27),
            EnglandNonDisadvantagedMeetingExpectedStandard = GetCodedDouble(28),
            EstablishmentDisadvantagedExceedingExpectedStandard = GetCodedDouble(29),
            EstablishmentDisadvantagedMeetingExpectedStandard = GetCodedDouble(30),
            GirlsExceedingExpectedStandard = GetCodedDouble(31),
            GirlsMeetingExpectedStandard = GetCodedDouble(32),
            LocalAuthorityDisadvantagedExceedingExpectedStandard = GetCodedDouble(33),
            LocalAuthorityDisadvantagedMeetingExpectedStandard = GetCodedDouble(34),
            LocalAuthorityNonDisadvantagedExceedingExpectedStandard = GetCodedDouble(35),
            LocalAuthorityNonDisadvantagedMeetingExpectedStandard = GetCodedDouble(36),
            NonMobileExceedingExpectedStandard = GetCodedDouble(37),
            NonMobileMeetingExpectedStandard = GetCodedDouble(38)
        };

    }

    private static CodedDouble GetCodedDouble(double val)
    {
        return new CodedDouble(val, string.Empty, val.ToString());
    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel()
    {
        return new KS2AdditionalMeasuresModel
        {
            EstablishmentGrammarAtExpectedStandard = GetCodedDouble(1),
            EstablishmentGrammarAtHigherStandard = GetCodedDouble(2),
            EstablishmentEHCPPopulation = GetCodedDouble(7),
            EstablishmentSENSupportPopulation = GetCodedDouble(8),
            LAGrammarAtExpectedStandard = GetCodedDouble(3),
            LAGrammarAtHigherStandard = GetCodedDouble(4),
            EnglandGrammarAtExpectedStandard = GetCodedDouble(5),
            EnglandGrammarAtHigherStandard = GetCodedDouble(6),
            EnglandEHCPPopulation = GetCodedDouble(9),
            EnglandSENSupportPopulation = GetCodedDouble(10),
        };
    }

    private KS2PupilPerformance GetKS2PupilPerformance()
    {
        return new KS2PupilPerformance
        {
            Urn = fakeMinimumEstablishment.URN,
            EstablishmentReadingScore = GetCodedDouble(1),
            EstablishmentReadingDescription = new CodedString("2", "", ""),
            EstablishmentReadingConfidenceUpper = GetCodedDouble(3),
            EstablishmentReadingConfidenceLower = GetCodedDouble(4),
            LaReadingScore = GetCodedDouble(5),
            EstablishmentWritingScore = GetCodedDouble(6),
            EstablishmentWritingDescription = new CodedString("7", "", ""),
            EstablishmentWritingConfidenceUpper = GetCodedDouble(8),
            EstablishmentWritingConfidenceLower = GetCodedDouble(9),
            LaWritingScore = GetCodedDouble(10),
            EstablishmentMathsScore = GetCodedDouble(11),
            EstablishmentMathsDescription = new CodedString("12", "", ""),
            EstablishmentMathsConfidenceUpper = GetCodedDouble(13),
            EstablishmentMathsConfidenceLower = GetCodedDouble(14),
            LaMathsScore = GetCodedDouble(15),
        };
    }
}
