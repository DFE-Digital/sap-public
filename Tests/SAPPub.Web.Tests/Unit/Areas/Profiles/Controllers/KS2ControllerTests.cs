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
        _controller = new(opts) { Establishment = fakeEstablishment };
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
            .Setup(a => a.GetPupilProgressAsync(fakeEstablishment.URN, AcademicYearSelection.Previous2, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        //Act
        var result = await _controller.AcademicPerformancePupilProgress(
            _mockKS2PupilProgressService.Object,
            fakeEstablishment.URN,
            fakeEstablishment.EstablishmentName,
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
            .Verify(a => a.GetPupilProgressAsync(fakeEstablishment.URN, AcademicYearSelection.Previous2, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_InvalidYearSelected_ReturnsNotFound()
    {
        // Arrange
        var expectedModel = GetKS2AdditionalMeasuresModel();

        _mockKS2AdditionalMeasuresService
            .Setup(a => a.GetAdditionalMeasures(fakeEstablishment.URN, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        // Act
        var result = await _controller.AcademicPerformanceAdditionalMeasures(
             _mockKS2AdditionalMeasuresService.Object,
             fakeEstablishment.URN,
             fakeEstablishment.EstablishmentName,
             CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsType<AcademicPerformanceAdditionalMeasuresViewModel>(result?.Model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.True(model.IsKS2);
        Assert.Equal(expectedModel.EstablishmentGrammarAtExpectedStandard, model.EstablishmentGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.EstablishmentGrammarAtHigherStandard, model.EstablishmentGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard, model.LAGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard, model.LAGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard, model.EnglandGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard, model.EnglandGrammarAtHigherStandard.Value);

        _mockKS2AdditionalMeasuresService
            .Verify(a => a.GetAdditionalMeasures(fakeEstablishment.URN, CancellationToken.None), Times.Once);
    }


    [Fact]
    public async Task Get_AcademicPerformanceMeetingOrExceedingStandards_ReturnsValidViewModel()
    {
        // Arrange
        var expectedModel = GetMeetingOrExceedingStandardsModel();

        _mockKS2MeetingOrExceedingStandardsService
            .Setup(a => a.GetMeetingOrExceedingStandardsPercentages(fakeEstablishment.URN, CancellationToken.None))
            .ReturnsAsync(expectedModel);

        // Act
        var result = await _controller.AcademicPerformanceMeetingOrExceedingStandards(
             _mockKS2MeetingOrExceedingStandardsService.Object,
             fakeEstablishment.URN,
             fakeEstablishment.EstablishmentName,
             CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        var model = Assert.IsType<AcademicPerformanceMeetingOrExceedingStandardsViewModel>(result?.Model);
        Assert.Equal(fakeEstablishment.URN, model.URN);
        Assert.True(model.IsKS2);
        Assert.Equal(expectedModel.EstablishmentPercentage.CurrentYear.Value, model.AllMeetingExceedingStandardsData!.Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.CurrentYear.Value, model.AllMeetingExceedingStandardsData!.Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandPercentage.CurrentYear.Value, model.AllMeetingExceedingStandardsData!.Data[2]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentage.TwoYearsAgo.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[0].Data[0]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentage.PreviousYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[0].Data[1]!.Value);
        Assert.Equal(expectedModel.EstablishmentPercentage.CurrentYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[0].Data[2]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.TwoYearsAgo.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[1].Data[0]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.PreviousYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[1].Data[1]!.Value);
        Assert.Equal(expectedModel.LocalAuthorityPercentage.CurrentYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[1].Data[2]!.Value);
        Assert.Equal(expectedModel.EnglandPercentage.TwoYearsAgo.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[2].Data[0]!.Value);
        Assert.Equal(expectedModel.EnglandPercentage.PreviousYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[2].Data[1]!.Value);
        Assert.Equal(expectedModel.EnglandPercentage.CurrentYear.Value, model.AllMeetingExceedingStandardsOverTimeData!.Datasets[2].Data[2]!.Value);

        _mockKS2MeetingOrExceedingStandardsService
            .Verify(a => a.GetMeetingOrExceedingStandardsPercentages(fakeEstablishment.URN, CancellationToken.None), Times.Once);
    }

    private static KS2MeetingOrExceedingStandardsModel GetMeetingOrExceedingStandardsModel()
    {
        return new KS2MeetingOrExceedingStandardsModel
        {
            EstablishmentPercentage = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3")
            },

            LocalAuthorityPercentage = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(4, string.Empty, "4"),
                PreviousYear = new CodedDouble(5, string.Empty, "5"),
                TwoYearsAgo = new CodedDouble(6, string.Empty, "6")
            },
            EnglandPercentage = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(7, string.Empty, "7"),
                PreviousYear = new CodedDouble(8, string.Empty, "8"),
                TwoYearsAgo = new CodedDouble(9, string.Empty, "9")
            }
        };
    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel()
    {
        return new KS2AdditionalMeasuresModel
        {
            EstablishmentGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "1"),
            EstablishmentGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "2"),
            LAGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "3"),
            LAGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "4"),
            EnglandGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "5"),
            EnglandGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "6"),
        };
    }

    private KS2PupilPerformance GetKS2PupilPerformance()
    {
        return new KS2PupilPerformance
        {
            Urn = fakeEstablishment.URN,
            EstablishmentReadingScore = new CodedDouble(1, "", ""),
            EstablishmentReadingDescription = new CodedString("2", "", ""),
            EstablishmentReadingConfidenceUpper = new CodedDouble(3, "", ""),
            EstablishmentReadingConfidenceLower = new CodedDouble(4, "", ""),
            LaReadingScore = new CodedDouble(5, "", ""),
            EstablishmentWritingScore = new CodedDouble(6, "", ""),
            EstablishmentWritingDescription = new CodedString("7", "", ""),
            EstablishmentWritingConfidenceUpper = new CodedDouble(8, "", ""),
            EstablishmentWritingConfidenceLower = new CodedDouble(9, "", ""),
            LaWritingScore = new CodedDouble(10, "", ""),
            EstablishmentMathsScore = new CodedDouble(11, "", ""),
            EstablishmentMathsDescription = new CodedString("12", "", ""),
            EstablishmentMathsConfidenceUpper = new CodedDouble(13, "", ""),
            EstablishmentMathsConfidenceLower = new CodedDouble(14, "", ""),
            LaMathsScore = new CodedDouble(15, "", ""),
        };
    }
}
