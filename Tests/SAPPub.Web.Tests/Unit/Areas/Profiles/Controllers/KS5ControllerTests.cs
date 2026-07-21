using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS5;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class KS5ControllerTests : BaseProfilesTests
{
    private readonly Mock<ILogger<KS5Controller>> _mockLogger = new();
    private readonly Mock<IAdvancedLevelQualificationsService> _mockAdvancedLevelQualificationsService = new();
    private readonly KS5Controller _controller;

    public KS5ControllerTests()
    {
        _controller = new KS5Controller(_mockLogger.Object);
    }

    [Fact]
    public async Task Get_AdvancedLevel_ALevel_Info_ReturnsExpected()
    {
        var qualification = Level3.ALevel;
        var expectedResult = AdvancedLevelDetails(qualification);

        _mockAdvancedLevelQualificationsService
            .Setup(es => es.GetAdvancedLevelQualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AdvancedLevel(
            _mockAdvancedLevelQualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdvancedLevelViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.IsKS5, model.IsKS5);

        Assert.Equal(expectedResult.TotalNoOfStudentCompletedQualification, model.TotalNoOfStudentCompletedQualification.Value);
        Assert.Equal(expectedResult.ProgressScore.Score, model.ProgressScore.Score.Value);
        Assert.Equal(expectedResult.ProgressScore.BandingRating, model.ProgressScore.BandingRating.Value);
        Assert.Equal(expectedResult.ProgressScore.ConfidenceLevelLower, model.ProgressScore.ConfidenceLevelLower.Value);
        Assert.Equal(expectedResult.ProgressScore.ConfidenceLevelUpper, model.ProgressScore.ConfidenceLevelUpper.Value);
        Assert.Equal(expectedResult.ProgressScore.EnglandAverageScore, model.ProgressScore.EnglandAverageScore.Value);

        var expectedProgressBandingDescription = AttainmentHelper.EstablishmentProgress8BandingContextStatement(model.ProgressScore.BandingRating.Value);
        Assert.Equal(expectedProgressBandingDescription.Value, model.ProgressScore.Progress8BandingContextDescription.DisplayText());
    }

    [Fact]
    public async Task Get_AdvancedLevel_ALevel_Info_With_No_Data_ReturnsOk()
    {
        var qualification = Level3.ALevel;
        var expectedResult = new AdvancedLevelQualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            ProgressScore = new ProgressScoreModel()
        };

        _mockAdvancedLevelQualificationsService
            .Setup(es => es.GetAdvancedLevelQualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AdvancedLevel(
            _mockAdvancedLevelQualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AdvancedLevelViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.IsKS5, model.IsKS5);

        Assert.Equal(NotAvailable, model.TotalNoOfStudentCompletedQualification.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.Score.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.BandingRating.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelLower.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelUpper.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.EnglandAverageScore.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.Progress8BandingContextDescription.DisplayText());
    }

    private AdvancedLevelQualificationModel AdvancedLevelDetails(Level3 qualification)
    {
        return new AdvancedLevelQualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            TotalNoOfStudentCompletedQualification = 100,
            ProgressScore = new ProgressScoreModel
            {
                Score = 75.5,
                BandingRating = "Average",
                ConfidenceLevelLower = 1.0,
                ConfidenceLevelUpper = 5.5,
                EnglandAverageScore = 85.1
            }
        };
    }
}
