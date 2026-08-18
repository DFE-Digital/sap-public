using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS2;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class KS2ControllerTests : BaseProfilesTests
{
    private readonly Mock<IKS2AdditionalMeasuresService> _mockKS2AdditionalMeasuresService = new();
    private readonly Mock<IKS2MeetingOrExceedingStandardsService> _mockKS2MeetingOrExceedingStandardsService = new();
    private readonly KS2Controller _controller;

    public KS2ControllerTests()
    {
        _mockKS2AdditionalMeasuresService = new Mock<IKS2AdditionalMeasuresService>();
        _controller = new() { Establishment = fakeEstablishment };
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
        Assert.Equal(expectedModel.EstablishmentEHCPPopulation, model.EstablishmentEHCPPopulation.Value);
        Assert.Equal(expectedModel.EstablishmentSENSupportPopulation, model.EstablishmentSENSupportPopulation.Value);
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard, model.LAGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard, model.LAGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard, model.EnglandGrammarAtExpectedStandard.Value);
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard, model.EnglandGrammarAtHigherStandard.Value);
        Assert.Equal(expectedModel.EnglandEHCPPopulation, model.EnglandEHCPPopulation.Value);
        Assert.Equal(expectedModel.EnglandSENSupportPopulation, model.EnglandSENSupportPopulation.Value);
       
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

        _mockKS2MeetingOrExceedingStandardsService
            .Verify(a => a.GetMeetingOrExceedingStandardsPercentages(fakeEstablishment.URN, CancellationToken.None), Times.Once);
    }

    private static KS2MeetingOrExceedingStandardsModel GetMeetingOrExceedingStandardsModel()
    {
        return new KS2MeetingOrExceedingStandardsModel
        {
            EstablishmentPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(1, string.Empty, "1"),
                PreviousYear = new CodedDouble(2, string.Empty, "2"),
                TwoYearsAgo = new CodedDouble(3, string.Empty, "3")
            },

            LocalAuthorityPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(4, string.Empty, "4"),
                PreviousYear = new CodedDouble(5, string.Empty, "5"),
                TwoYearsAgo = new CodedDouble(6, string.Empty, "6")
            },
            EnglandPercentageMeetingOrExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(7, string.Empty, "7"),
                PreviousYear = new CodedDouble(8, string.Empty, "8"),
                TwoYearsAgo = new CodedDouble(9, string.Empty, "9")
            },
            EstablishmentPercentageExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(10, string.Empty, "10"),
                PreviousYear = new CodedDouble(11, string.Empty, "11"),
                TwoYearsAgo = new CodedDouble(12, string.Empty, "12")
            },
            LocalAuthorityPercentageExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(13, string.Empty, "13"),
                PreviousYear = new CodedDouble(14, string.Empty, "14"),
                TwoYearsAgo = new CodedDouble(15, string.Empty, "15")
            },

            EnglandPercentageExceeding = new RelativeYearValues<CodedDouble>
            {
                CurrentYear = new CodedDouble(16, string.Empty, "16"),
                PreviousYear = new CodedDouble(17, string.Empty, "17"),
                TwoYearsAgo = new CodedDouble(18, string.Empty, "18")
            }
        };
    }

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel()
    {
        return new KS2AdditionalMeasuresModel
        {
            EstablishmentGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "1"),
            EstablishmentGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "2"),
            EstablishmentEHCPPopulation = new CodedDouble(1, string.Empty, "7"),
            EstablishmentSENSupportPopulation = new CodedDouble(1, string.Empty, "8"),        
            LAGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "3"),
            LAGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "4"),
            EnglandGrammarAtExpectedStandard = new CodedDouble(1, string.Empty, "5"),
            EnglandGrammarAtHigherStandard = new CodedDouble(1, string.Empty, "6"),
            EnglandEHCPPopulation = new CodedDouble(1, string.Empty, "9"),
            EnglandSENSupportPopulation = new CodedDouble(1, string.Empty, "10"),
        };
    }
}
