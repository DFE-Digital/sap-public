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
    private readonly Mock<IKS2ScaledScoreService> _mockKS2ScaledScoreService = new();
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
            .Setup(a => a.GetAdditionalMeasures(fakeMinimumEstablishment.URN, fakeMinimumEstablishment.LAId, CancellationToken.None))
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

        Assert.Equal(expectedModel.EstablishmentNumPupilsEndOfKS2, model.EstablishmentNumPupilsEndOfKS2.Value);
        Assert.Equal(expectedModel.LANumPupilsEndOfKS2, model.LANumPupilsEndOfKS2.Value);
        Assert.Equal(expectedModel.EnglandNumPupilsEndOfKS2, model.EnglandNumPupilsEndOfKS2.Value);
        Assert.Equal(expectedModel.EstablishmentNumGirlsEndOfKS2, model.EstablishmentNumGirlsEndOfKS2.Value);
        Assert.Equal(expectedModel.EstablishmentNumBoysEndOfKS2, model.EstablishmentNumBoysEndOfKS2.Value);
        Assert.Equal(expectedModel.EstablishmentNumEALEndOfKS2, model.EstablishmentNumEALEndOfKS2.Value);
        Assert.Equal(expectedModel.EstablishmentNumNonMobileEndOfKS2, model.EstablishmentNumNonMobileEndOfKS2.Value);
        Assert.Equal(expectedModel.EstablishmentNumDisadvantagedEndOfKS2, model.EstablishmentNumDisadvantagedEndOfKS2.Value);
        Assert.Equal(expectedModel.LANumDisadvantagedEndOfKS2, model.LANumDisadvantagedEndOfKS2.Value);
        Assert.Equal(expectedModel.EnglandNumDisadvantagedEndOfKS2, model.EnglandNumDisadvantagedEndOfKS2.Value);

        Assert.Equal(expectedModel.LANumNonDisadvantagedEndOfKS2, model.LANumNonDisadvantagedEndOfKS2.Value);
        Assert.Equal(expectedModel.EnglandNumNonDisadvantagedEndOfKS2, model.EnglandNumNonDisadvantagedEndOfKS2.Value);
        Assert.Equal(expectedModel.EstablishmentPupilTotal, model.EstablishmentNumberOfPupils.Value);
        Assert.Equal(expectedModel.EnglandPupilTotal, model.EnglandNumberOfPupils.Value);

        _mockKS2AdditionalMeasuresService
            .Verify(a => a.GetAdditionalMeasures(fakeMinimumEstablishment.URN, fakeMinimumEstablishment.LAId, CancellationToken.None), Times.Once);
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

    [Fact]
    public async Task Get_AcademicPerformanceSubjectScaledScores_ReturnsCorrectData()
    {
        // Arrange
        var expectedModel = GetKS2ScaledScore();
        _mockKS2ScaledScoreService
            .Setup(a => a.GetScaledScoreModel(fakeMinimumEstablishment.URN, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        //Act
        var result = await _controller.AcademicPerformanceSubjectScaledScores(
            _mockKS2ScaledScoreService.Object,
            fakeMinimumEstablishment.URN,
            fakeMinimumEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        // Assert
        Assert.NotNull(result);
        var model = Assert.IsType<AcademicPerformanceSubjectScaledScoresViewModel>(result?.Model);
        Assert.Equal(expectedModel.ReadAverageEstablishment.CurrentYear.Value, model.AllReadOverTimeData!.Datasets[0]!.Data[2]!.Value);
        Assert.Equal(expectedModel.ReadAverageEstablishment.PreviousYear.Value, model.AllReadOverTimeData!.Datasets[0]!.Data[1]!.Value);
        Assert.Equal(expectedModel.ReadAverageEstablishment.TwoYearsAgo.Value, model.AllReadOverTimeData!.Datasets[0]!.Data[0]!.Value);
        Assert.Equal(expectedModel.ReadAverageLA.CurrentYear.Value, model.AllReadOverTimeData!.Datasets[1]!.Data[2]!.Value);
        Assert.Equal(expectedModel.ReadAverageLA.PreviousYear.Value, model.AllReadOverTimeData!.Datasets[1]!.Data[1]!.Value);
        Assert.Equal(expectedModel.ReadAverageLA.TwoYearsAgo.Value, model.AllReadOverTimeData!.Datasets[1]!.Data[0]!.Value);
        Assert.Equal(expectedModel.ReadAverageEngland.CurrentYear.Value, model.AllReadOverTimeData!.Datasets[2]!.Data[2]!.Value);
        Assert.Equal(expectedModel.ReadAverageEngland.PreviousYear.Value, model.AllReadOverTimeData!.Datasets[2]!.Data[1]!.Value);
        Assert.Equal(expectedModel.ReadAverageEngland.TwoYearsAgo.Value, model.AllReadOverTimeData!.Datasets[2]!.Data[0]!.Value);
        Assert.Equal(expectedModel.MathsAverageEstablishment.CurrentYear.Value, model.AllMathsOverTimeData!.Datasets[0]!.Data[2]!.Value);
        Assert.Equal(expectedModel.MathsAverageEstablishment.PreviousYear.Value, model.AllMathsOverTimeData!.Datasets[0]!.Data[1]!.Value);
        Assert.Equal(expectedModel.MathsAverageEstablishment.TwoYearsAgo.Value, model.AllMathsOverTimeData!.Datasets[0]!.Data[0]!.Value);
        Assert.Equal(expectedModel.MathsAverageLA.CurrentYear.Value, model.AllMathsOverTimeData!.Datasets[1]!.Data[2]!.Value);
        Assert.Equal(expectedModel.MathsAverageLA.PreviousYear.Value, model.AllMathsOverTimeData!.Datasets[1]!.Data[1]!.Value);
        Assert.Equal(expectedModel.MathsAverageLA.TwoYearsAgo.Value, model.AllMathsOverTimeData!.Datasets[1]!.Data[0]!.Value);
        Assert.Equal(expectedModel.MathsAverageEngland.CurrentYear.Value, model.AllMathsOverTimeData!.Datasets[2]!.Data[2]!.Value);
        Assert.Equal(expectedModel.MathsAverageEngland.PreviousYear.Value, model.AllMathsOverTimeData!.Datasets[2]!.Data[1]!.Value);
        Assert.Equal(expectedModel.MathsAverageEngland.TwoYearsAgo.Value, model.AllMathsOverTimeData!.Datasets[2]!.Data[0]!.Value);

        Assert.Equal(expectedModel.EstablishmentReadThreeYearAverage.Value, model.ReadThreeYearAverageData.Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityReadThreeYearAverage.Value, model.ReadThreeYearAverageData.Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandReadThreeYearAverage.Value, model.ReadThreeYearAverageData.Data[2]!.Value);
        Assert.Equal(expectedModel.EstablishmentMathsThreeYearAverage.Value, model.MathsThreeYearAverageData.Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityMathsThreeYearAverage.Value, model.MathsThreeYearAverageData.Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandMathsThreeYearAverage.Value, model.MathsThreeYearAverageData.Data[2]!.Value);

        Assert.Equal(expectedModel.GirlsAverageReading, model.GirlsAndBoys.Rows.ToList()[0].AverageReadingScore.Value);
        Assert.Equal(expectedModel.GirlsAverageMaths, model.GirlsAndBoys.Rows.ToList()[0].AverageMathsScore.Value);
        Assert.Equal(expectedModel.BoysAverageReading, model.GirlsAndBoys.Rows.ToList()[1].AverageReadingScore.Value);
        Assert.Equal(expectedModel.BoysAverageMaths, model.GirlsAndBoys.Rows.ToList()[1].AverageMathsScore.Value);
        Assert.Equal(expectedModel.AllPupilsAverageReading, model.GirlsAndBoys.Rows.ToList()[2].AverageReadingScore.Value);
        Assert.Equal(expectedModel.AllPupilsAverageMaths, model.GirlsAndBoys.Rows.ToList()[2].AverageMathsScore.Value);

        Assert.Equal(expectedModel.EALAverageReading, model.EnglishAsAnAdditionalLanguage.Rows.ToList()[0].AverageReadingScore.Value);
        Assert.Equal(expectedModel.EALAverageMaths, model.EnglishAsAnAdditionalLanguage.Rows.ToList()[0].AverageMathsScore.Value);
        Assert.Equal(expectedModel.EALTotalAverageReading, model.EnglishAsAnAdditionalLanguage.Rows.ToList()[1].AverageReadingScore.Value);
        Assert.Equal(expectedModel.EALTotalAverageMaths, model.EnglishAsAnAdditionalLanguage.Rows.ToList()[1].AverageMathsScore.Value);

        Assert.Equal(expectedModel.NonMobileAverageReading, model.NonMobilePupils.Rows.ToList()[0].AverageReadingScore.Value);
        Assert.Equal(expectedModel.NonMobileAverageMaths, model.NonMobilePupils.Rows.ToList()[0].AverageMathsScore.Value);

        Assert.Equal(expectedModel.DisadvantagedAverageReadingEstablishment, model.DisadvantagedPupils.Rows.ToList()[0].AverageReadingScore.Value);
        Assert.Equal(expectedModel.DisadvantagedAverageMathsEstablishment, model.DisadvantagedPupils.Rows.ToList()[0].AverageMathsScore.Value);
        Assert.Equal(expectedModel.DisadvantagedAverageReadingLA, model.DisadvantagedPupils.Rows.ToList()[1].AverageReadingScore.Value);
        Assert.Equal(expectedModel.DisadvantagedAverageMathsLA, model.DisadvantagedPupils.Rows.ToList()[1].AverageMathsScore.Value);
        Assert.Equal(expectedModel.DisadvantagedAverageReadingEngland, model.DisadvantagedPupils.Rows.ToList()[2].AverageReadingScore.Value);
        Assert.Equal(expectedModel.DisadvantagedAverageMathsEngland, model.DisadvantagedPupils.Rows.ToList()[2].AverageMathsScore.Value);

        Assert.Equal(expectedModel.NonDisadvantagedAverageMathsLA, model.NonDisadvantagedPupils.Rows.ToList()[0].AverageMathsScore.Value);
        Assert.Equal(expectedModel.NonDisadvantagedAverageReadingLA, model.NonDisadvantagedPupils.Rows.ToList()[0].AverageReadingScore.Value);
        Assert.Equal(expectedModel.NonDisadvantagedAverageMathsEngland, model.NonDisadvantagedPupils.Rows.ToList()[1].AverageMathsScore.Value);
        Assert.Equal(expectedModel.NonDisadvantagedAverageReadingEngland, model.NonDisadvantagedPupils.Rows.ToList()[1].AverageReadingScore.Value);


        _mockKS2ScaledScoreService
            .Verify(a => a.GetScaledScoreModel(fakeMinimumEstablishment.URN,It.IsAny<CancellationToken>()), Times.Once);
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
            NonMobileMeetingExpectedStandard = GetCodedDouble(38),
            EstablishmentPercentageMeetingOrExceedingThreeYearAverage = GetCodedDouble(39),
            LocalAuthorityPercentageMeetingOrExceedingThreeYearAverage = GetCodedDouble(40),
            EnglandPercentageMeetingOrExceedingThreeYearAverage = GetCodedDouble(41),
            EstablishmentPercentageExceedingThreeYearAverage = GetCodedDouble(42),
            LocalAuthorityPercentageExceedingThreeYearAverage = GetCodedDouble(43),
            EnglandPercentageExceedingThreeYearAverage = GetCodedDouble(44)
        };

    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel() => new()
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

        EstablishmentNumPupilsEndOfKS2 = GetCodedDouble(11),
        LANumPupilsEndOfKS2 = CodedDouble.Empty,
        EnglandNumPupilsEndOfKS2 = CodedDouble.Empty,
        EstablishmentNumGirlsEndOfKS2 = GetCodedDouble(12),
        EstablishmentNumBoysEndOfKS2 = GetCodedDouble(13),
        EstablishmentNumEALEndOfKS2 = GetCodedDouble(14),
        EstablishmentNumNonMobileEndOfKS2 = GetCodedDouble(15),
        EstablishmentNumDisadvantagedEndOfKS2 = GetCodedDouble(16),
        LANumDisadvantagedEndOfKS2 = GetCodedDouble(17),
        EnglandNumDisadvantagedEndOfKS2 = GetCodedDouble(18),
        LANumNonDisadvantagedEndOfKS2 = GetCodedDouble(19),
        EnglandNumNonDisadvantagedEndOfKS2 = GetCodedDouble(20),
        EstablishmentPupilTotal = "20",
        EnglandPupilTotal = CodedDouble.Empty
    };

    private KS2PupilPerformance GetKS2PupilPerformance() => new()
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

    private static KS2ScaledScoreModel GetKS2ScaledScore() => new()
    {

        LAName = "",
        ReadAverageEstablishment = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(1.1), PreviousYear = GetCodedDouble(1.2), TwoYearsAgo = GetCodedDouble(1.3) },
        ReadAverageLA = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(2.1), PreviousYear = GetCodedDouble(2.2), TwoYearsAgo = GetCodedDouble(2.3) },
        ReadAverageEngland = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(3.1), PreviousYear = GetCodedDouble(3.2), TwoYearsAgo = GetCodedDouble(3.3) },
        MathsAverageEstablishment = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(4.1), PreviousYear = GetCodedDouble(4.2), TwoYearsAgo = GetCodedDouble(4.3) },
        MathsAverageLA = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(5.1), PreviousYear = GetCodedDouble(5.2), TwoYearsAgo = GetCodedDouble(5.3) },
        MathsAverageEngland = new RelativeYearValues<CodedDouble> { CurrentYear = GetCodedDouble(6.1), PreviousYear = GetCodedDouble(6.2), TwoYearsAgo = GetCodedDouble(6.3) },

        /* Three year averages */
        EstablishmentReadThreeYearAverage = GetCodedDouble(7),
        LocalAuthorityReadThreeYearAverage = GetCodedDouble(8),
        EnglandReadThreeYearAverage = GetCodedDouble(9),
        EstablishmentMathsThreeYearAverage = GetCodedDouble(10),
        LocalAuthorityMathsThreeYearAverage = GetCodedDouble(11),
        EnglandMathsThreeYearAverage = GetCodedDouble(12),


        /* Girls and boys breakdown */
        GirlsAverageReading = GetCodedDouble(13),
        GirlsAverageMaths = GetCodedDouble(14),
        BoysAverageReading = GetCodedDouble(15),
        BoysAverageMaths = GetCodedDouble(16),
        AllPupilsAverageReading = GetCodedDouble(17),
        AllPupilsAverageMaths = GetCodedDouble(18),

        /* English as an additional language */
        EALAverageReading = GetCodedDouble(19),
        EALAverageMaths = GetCodedDouble(20),
        EALTotalAverageReading = GetCodedDouble(21),
        EALTotalAverageMaths = GetCodedDouble(22),

        /* Non-mobile pupils */
        NonMobileAverageReading = GetCodedDouble(23),
        NonMobileAverageMaths = GetCodedDouble(24),


        /* Disadvantaged pupils */
        DisadvantagedAverageReadingEstablishment = GetCodedDouble(25),
        DisadvantagedAverageMathsEstablishment = GetCodedDouble(26),
        DisadvantagedAverageReadingLA = GetCodedDouble(27),
        DisadvantagedAverageMathsLA = GetCodedDouble(28),
        DisadvantagedAverageReadingEngland = GetCodedDouble(29),
        DisadvantagedAverageMathsEngland = GetCodedDouble(30),

        /* Non-disadvantaged pupils */
        NonDisadvantagedAverageReadingLA = GetCodedDouble(31),
        NonDisadvantagedAverageMathsLA = GetCodedDouble(32),
        NonDisadvantagedAverageReadingEngland = GetCodedDouble(33),
        NonDisadvantagedAverageMathsEngland = GetCodedDouble(34)
    };

}
