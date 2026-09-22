using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Enums.KS5Qualifications;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS5;
using SAPPub.Web.Helpers;
using static SAPPub.Web.Constants.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class KS5ControllerTests : BaseProfilesTests
{
    private readonly Mock<ILogger<KS5Controller>> _mockLogger = new();
    private readonly Mock<ILevel3QualificationsService> _mockLevel3QualificationsService = new();
    private readonly Mock<ILevel2QualificationsService> _mockLevel2QualificationsService = new();
    private readonly Mock<IEnglishAndMathsQualificationsService> _mockEnglishAndMathsQualificationsService = new();
    private readonly Mock<IKS5EstablishmentSubjectEntriesService> _mockKs5EstablishmentSubjectEntriesService = new();
    private readonly Mock<IEstablishmentService> _establishmentService = new();
    private readonly KS5Controller _controller;

    public KS5ControllerTests()
    {
        _controller = new KS5Controller(_mockLogger.Object);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Get_Level3Qualifications_Info_ReturnsExpected(Level3 qualification)
    {
        var expectedResult = Level3QualificationDetails(qualification);

        _mockLevel3QualificationsService
            .Setup(es => es.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Level3Qualifications(
            _mockLevel3QualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as Level3QualificationViewModel;
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

        var expectedProgressBandingDescription = AttainmentHelper.EstablishmentProgress8BandingContextStatement(model.ProgressScore.BandingRating.Value.Value, ProgressBandingDescriptions.Empty);
        Assert.Equal(expectedProgressBandingDescription.Value, model.ProgressScore.ProgressBandingContextDescription.DisplayText());

        Assert.Equal(expectedResult.AverageResult.Establishment.CurrentYear.Points, model.AverageResult.Establishment.CurrentYear.Points.Value);
        Assert.Equal(expectedResult.AverageResult.Establishment.PreviousYear!.Points, model.AverageResult.Establishment.PreviousYear!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.Establishment.TwoYearsAgo!.Points, model.AverageResult.Establishment.TwoYearsAgo!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.Establishment.CurrentYear.Grade.ToString(), model.AverageResult.Establishment.CurrentYear.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.Establishment.PreviousYear!.Grade.ToString(), model.AverageResult.Establishment.PreviousYear!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.Establishment.TwoYearsAgo!.Grade.ToString(), model.AverageResult.Establishment.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.CurrentYear.Points, model.AverageResult.LocalAuthority.CurrentYear.Points.Value);
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.PreviousYear!.Points, model.AverageResult.LocalAuthority.PreviousYear!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.TwoYearsAgo!.Points, model.AverageResult.LocalAuthority.TwoYearsAgo!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.CurrentYear.Grade.ToString(), model.AverageResult.LocalAuthority.CurrentYear.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.PreviousYear!.Grade.ToString(), model.AverageResult.LocalAuthority.PreviousYear!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.ToString(), model.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.England.CurrentYear.Points, model.AverageResult.England.CurrentYear.Points.Value);
        Assert.Equal(expectedResult.AverageResult.England.PreviousYear!.Points, model.AverageResult.England.PreviousYear!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.England.TwoYearsAgo!.Points, model.AverageResult.England.TwoYearsAgo!.Points.Value);

        Assert.Equal(expectedResult.AverageResult.England.CurrentYear.Grade.ToString(), model.AverageResult.England.CurrentYear.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.England.PreviousYear!.Grade.ToString(), model.AverageResult.England.PreviousYear!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.England.TwoYearsAgo!.Grade.ToString(), model.AverageResult.England.TwoYearsAgo!.Grade.DisplayText());


        if (qualification == Level3.ALevel)
        {
            Assert.Equal(expectedResult.AdditionalData!.TotalNoOfStudentsIncludedInThisMeasure, model.AdditionalData!.TotalNoOfStudentsIncludedInThisMeasure.Value);
            Assert.Equal(expectedResult.AdditionalData!.Establishment.Points, model.AdditionalData!.EstablishmentPoints.Value);
            Assert.Equal(expectedResult.AdditionalData!.Establishment.Grade.ToString(), model.AdditionalData!.EstablishmentGrade.DisplayText());
            Assert.Equal(expectedResult.AdditionalData!.Establishment.Points, model.AdditionalData!.EstablishmentPoints.Value);
            Assert.Equal(expectedResult.AdditionalData!.Establishment.Grade.ToString(), model.AdditionalData!.EstablishmentGrade.DisplayText());
            Assert.Equal(expectedResult.AdditionalData!.Establishment.Points, model.AdditionalData!.EstablishmentPoints.Value);
            Assert.Equal(expectedResult.AdditionalData!.Establishment.Grade.ToString(), model.AdditionalData!.EstablishmentGrade.DisplayText());
        }
        else
        {
            Assert.Null(model.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure.Value);
            Assert.Null(model.AdditionalData?.EstablishmentPoints.Value);
            Assert.Null(model.AdditionalData?.EstablishmentGrade.Value);
            Assert.Null(model.AdditionalData?.EstablishmentPoints.Value);
            Assert.Null(model.AdditionalData?.EstablishmentGrade.Value);
            Assert.Null(model.AdditionalData?.EstablishmentPoints.Value);
            Assert.Null(model.AdditionalData?.EstablishmentGrade.Value);
        }

        if (qualification == Level3.Academic)
        {
            Assert.Equal(expectedResult.AdvancedLevelMathsQualificationData!.SchoolOrCollege, model.AdvancedLevelMathsQualificationData!.SchoolOrCollege.Value);
            Assert.Equal(expectedResult.AdvancedLevelMathsQualificationData!.LocalAuthority, model.AdvancedLevelMathsQualificationData!.LocalAuthority.Value);
            Assert.Equal(expectedResult.AdvancedLevelMathsQualificationData!.England, model.AdvancedLevelMathsQualificationData!.England.Value);
        }
        else
        {
            Assert.Null(model.AdvancedLevelMathsQualificationData?.SchoolOrCollege.Value);
            Assert.Null(model.AdvancedLevelMathsQualificationData?.LocalAuthority.Value);
            Assert.Null(model.AdvancedLevelMathsQualificationData?.England.Value);
        }

        // Assert Disadvantaged and NonDisadvantaged students data

        // Disadvantaged - Establishment
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.NumberOfStudents, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.NumberOfStudents.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.ProgressScore, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ProgressScore.Value);
        Assert.Equal($"{expectedResult.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower.Value} to {expectedResult.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.Result.Points, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Points.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.Result.Grade, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Grade.Value);

        // Disadvantaged - Local Authority
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.NumberOfStudents.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.ProgressScore, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ProgressScore.Value);
        Assert.Equal($"{expectedResult.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.Value} to {expectedResult.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.Result.Points, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Points.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.Result.Grade, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Grade.Value);

        // Disadvantaged - England
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.NumberOfStudents, model.PerformanceGroupsData.DisadvantagedStudents.England.NumberOfStudents.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.ProgressScore, model.PerformanceGroupsData.DisadvantagedStudents.England.ProgressScore.Value);
        Assert.Equal($"{expectedResult.DisadvantagedStudentsData.England.ConfidenceLevelLower.Value} to {expectedResult.DisadvantagedStudentsData.England.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.DisadvantagedStudents.England.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.Result.Points, model.PerformanceGroupsData.DisadvantagedStudents.England.Points.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.Result.Grade, model.PerformanceGroupsData.DisadvantagedStudents.England.Grade.Value);

        // Non-Disadvantaged - Local Authority
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.NumberOfStudents.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ProgressScore.Value);
        Assert.Equal($"{expectedResult.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.Value} to {expectedResult.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.Result.Points, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Points.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Grade.Value);

        // Non-Disadvantaged - England
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.NumberOfStudents, model.PerformanceGroupsData.NonDisadvantagedStudents.England.NumberOfStudents.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.ProgressScore, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ProgressScore.Value);
        Assert.Equal($"{expectedResult.NonDisadvantagedStudentsData.England.ConfidenceLevelLower.Value} to {expectedResult.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.NonDisadvantagedStudents.England.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.Result.Points, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Points.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.Result.Grade, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Grade.Value);
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Get_Level3Qualifications_Info_With_Reason_ReturnsOk(Level3 qualification)
    {
        var expectedResult = new Level3QualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            LAName = fakeEstablishment.LAName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            ProgressScore = new ProgressScoreModel
            {
                Score = new CodedDouble(null, "Not applicable", "z"),
                BandingRating = new CodedString(null, "Not applicable", "z"),
                ConfidenceLevelLower = new CodedDouble(null, "Redacted for confidentiality", "c"),
                ConfidenceLevelUpper = new CodedDouble(null, "Not applicable", "z"),
                EnglandAverageScore = new CodedDouble(null, "Not available", "x"),
            },
            AverageResult = new AverageResultModel
            {
                NumberOfStudents = new RelativeYearValues<CodedDouble>
                {
                    CurrentYear = new CodedDouble(null, "Not applicable", "z"),
                    PreviousYear = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    TwoYearsAgo = new CodedDouble(null, "Not available", "x"),
                },
                Establishment = new RelativeYearValues<PerformanceResult>
                { 
                    CurrentYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z")
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    },
                },
                LocalAuthority = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")                        
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z")                        
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    },
                },
                England = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")                        
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z")
                    },
                }
            },
            AdditionalData = new AdditionalDataModel
            {
                TotalNoOfStudentsIncludedInThisMeasure = new CodedDouble(null, "Not applicable", "z"),
                Establishment = new() { Grade = new CodedString(null, "Not applicable", "z"), Points = new CodedDouble(null, "Not applicable", "z") },
                LocalAuthority = new() { Grade = new CodedString(null, "Redacted for confidentiality", "c"), Points = new CodedDouble(null, "Redacted for confidentiality", "c") },
                England = new() { Grade = new CodedString(null, "Not available", "x"), Points = new CodedDouble(null, "Not available", "x") },
            },
            AdvancedLevelMathsQualificationData = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(null, "Not applicable", "z"),
                LocalAuthority = new CodedDouble(null, "Redacted for confidentiality", "c"),
                England = new CodedDouble(null, "Not available", "x")
            },
            DisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Not applicable", "z"),
                    ProgressScore = new CodedDouble(null, "Not applicable", "z"),
                    ConfidenceLevelLower = new CodedDouble(null, "Not applicable", "z"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Not applicable", "z"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z"),
                    }
                },
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ProgressScore = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelLower = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Not available", "x"),
                    ProgressScore = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelLower = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Not available", "x"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    }
                }
            },
            NonDisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = null,
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Not available", "x"),
                    ProgressScore = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelLower = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Not available", "x"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ProgressScore = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelLower = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    }
                }
            }
        };

        _mockLevel3QualificationsService
            .Setup(es => es.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Level3Qualifications(
            _mockLevel3QualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as Level3QualificationViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal($"{expectedResult.LAName} average", model.LAName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.IsKS5, model.IsKS5);

        Assert.Equal(NotAvailable, model.TotalNoOfStudentCompletedQualification.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.Score.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.BandingRating.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelLower.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelUpper.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.EnglandAverageScore.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ProgressBandingContextDescription.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Points.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Grade.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Points.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Points.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Grade.DisplayText());

        Assert.Equal(NotAvailable, model.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EstablishmentPoints.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EstablishmentGrade.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.LocalAuthorityPoints.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.LocalAuthorityGrade.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EnglandPoints.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EnglandGrade.DisplayText());

        Assert.Equal(NotAvailable, model.AdvancedLevelMathsQualificationData?.SchoolOrCollege.DisplayText());
        Assert.Equal(NotAvailable, model.AdvancedLevelMathsQualificationData?.LocalAuthority.DisplayText());
        Assert.Equal(NotAvailable, model.AdvancedLevelMathsQualificationData?.England.DisplayText());

        // Disadvantaged - Establishment
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Grade.DisplayText());

        // Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Grade.DisplayText());

        // Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Non-Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Grade.DisplayText());
    }

    [Theory]
    [InlineData(Level3.ALevel)]
    [InlineData(Level3.Academic)]
    [InlineData(Level3.AppliedGeneral)]
    [InlineData(Level3.TechLevel)]
    public async Task Get_Level3Qualifications_Info_With_No_Data_ReturnsOk(Level3 qualification)
    {
        var expectedResult = new Level3QualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            LAName = fakeEstablishment.LAName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            ProgressScore = new ProgressScoreModel(),
            AverageResult = new AverageResultModel
            {
                NumberOfStudents = new RelativeYearValues<CodedDouble>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
                Establishment = new RelativeYearValues<PerformanceResult>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
                LocalAuthority = new RelativeYearValues<PerformanceResult>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
                England = new RelativeYearValues<PerformanceResult>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
            },
            AdditionalData = new AdditionalDataModel
            {
                TotalNoOfStudentsIncludedInThisMeasure = CodedDouble.Empty,
                Establishment = new(),
                LocalAuthority = new(),
                England = new(),
            },
            AdvancedLevelMathsQualificationData = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new(),
                LocalAuthority = new(),
                England = new()
            },
            DisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                },
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                }
            },
            NonDisadvantagedStudentsData = new PerformanceSummaryModel
            {
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                }
            }
        };

        _mockLevel3QualificationsService
            .Setup(es => es.GetLevel3QualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Level3Qualifications(
            _mockLevel3QualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as Level3QualificationViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal($"{expectedResult.LAName} average", model.LAName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.IsKS5, model.IsKS5);

        Assert.Equal(NotAvailable, model.TotalNoOfStudentCompletedQualification.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.Score.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.BandingRating.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelLower.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelUpper.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.EnglandAverageScore.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ProgressBandingContextDescription.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Grade.DisplayText());

        Assert.Equal(NotAvailable, model.AdditionalData?.TotalNoOfStudentsIncludedInThisMeasure.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EstablishmentPoints.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EstablishmentGrade.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.LocalAuthorityPoints.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.LocalAuthorityGrade.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EnglandPoints.DisplayText());
        Assert.Equal(NotAvailable, model.AdditionalData?.EnglandGrade.DisplayText());

        Assert.Equal(NotAvailable, model.AdvancedLevelMathsQualificationData?.SchoolOrCollege.DisplayText());
        Assert.Equal(NotAvailable, model.AdvancedLevelMathsQualificationData?.LocalAuthority.DisplayText());
        Assert.Equal(NotAvailable, model.AdvancedLevelMathsQualificationData?.England.DisplayText());

        // Disadvantaged - Establishment
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Grade.DisplayText());

        // Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Grade.DisplayText());

        // Non-Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Non-Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Grade.DisplayText());
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Get_Level2Qualifications_Info_ReturnsExpected(Level2 qualification)
    {
        var expectedResult = Level2QualificationDetails(qualification);

        _mockLevel2QualificationsService
            .Setup(es => es.GetLevel2QualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Level2Qualifications(
            _mockLevel2QualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as Level2QualificationViewModel;
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

        var expectedProgressBandingDescription = AttainmentHelper.EstablishmentProgress8BandingContextStatement(model.ProgressScore.BandingRating.Value.Value, ProgressBandingDescriptions.Empty);
        Assert.Equal(expectedProgressBandingDescription.Value, model.ProgressScore.ProgressBandingContextDescription.DisplayText());

        Assert.Equal(expectedResult.AverageResult.Establishment.CurrentYear.Points, model.AverageResult.Establishment.CurrentYear.Points.Value);
        Assert.Equal(expectedResult.AverageResult.Establishment.PreviousYear!.Points, model.AverageResult.Establishment.PreviousYear!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.Establishment.TwoYearsAgo!.Points, model.AverageResult.Establishment.TwoYearsAgo!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.Establishment.CurrentYear.Grade.ToString(), model.AverageResult.Establishment.CurrentYear.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.Establishment.PreviousYear!.Grade.ToString(), model.AverageResult.Establishment.PreviousYear!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.Establishment.TwoYearsAgo!.Grade.ToString(), model.AverageResult.Establishment.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.CurrentYear.Points, model.AverageResult.LocalAuthority.CurrentYear.Points.Value);
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.PreviousYear!.Points, model.AverageResult.LocalAuthority.PreviousYear!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.TwoYearsAgo!.Points, model.AverageResult.LocalAuthority.TwoYearsAgo!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.CurrentYear.Grade.ToString(), model.AverageResult.LocalAuthority.CurrentYear.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.PreviousYear!.Grade.ToString(), model.AverageResult.LocalAuthority.PreviousYear!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.ToString(), model.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.England.CurrentYear.Points, model.AverageResult.England.CurrentYear.Points.Value);
        Assert.Equal(expectedResult.AverageResult.England.PreviousYear!.Points, model.AverageResult.England.PreviousYear!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.England.TwoYearsAgo!.Points, model.AverageResult.England.TwoYearsAgo!.Points.Value);
        Assert.Equal(expectedResult.AverageResult.England.CurrentYear.Grade.ToString(), model.AverageResult.England.CurrentYear.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.England.PreviousYear!.Grade.ToString(), model.AverageResult.England.PreviousYear!.Grade.DisplayText());
        Assert.Equal(expectedResult.AverageResult.England.TwoYearsAgo!.Grade.ToString(), model.AverageResult.England.TwoYearsAgo!.Grade.DisplayText());

        // Assert Disadvantaged and NonDisadvantaged students data

        // Disadvantaged - Establishment
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.NumberOfStudents, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.NumberOfStudents.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.ProgressScore, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ProgressScore.Value);
        Assert.Equal($"{expectedResult.DisadvantagedStudentsData.Establishment!.ConfidenceLevelLower.Value} to {expectedResult.DisadvantagedStudentsData.Establishment!.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.Result.Points, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Points.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.Establishment!.Result.Grade, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Grade.Value);

        // Disadvantaged - Local Authority
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.NumberOfStudents, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.NumberOfStudents.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.ProgressScore, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ProgressScore.Value);
        Assert.Equal($"{expectedResult.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.Value} to {expectedResult.DisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.Result.Points, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Points.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.LocalAuthority.Result.Grade, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Grade.Value);

        // Disadvantaged - England
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.NumberOfStudents, model.PerformanceGroupsData.DisadvantagedStudents.England.NumberOfStudents.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.ProgressScore, model.PerformanceGroupsData.DisadvantagedStudents.England.ProgressScore.Value);
        Assert.Equal($"{expectedResult.DisadvantagedStudentsData.England.ConfidenceLevelLower.Value} to {expectedResult.DisadvantagedStudentsData.England.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.DisadvantagedStudents.England.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.Result.Points, model.PerformanceGroupsData.DisadvantagedStudents.England.Points.Value);
        Assert.Equal(expectedResult.DisadvantagedStudentsData.England.Result.Grade, model.PerformanceGroupsData.DisadvantagedStudents.England.Grade.Value);

        // Non-Disadvantaged - Local Authority
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.NumberOfStudents, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.NumberOfStudents.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.ProgressScore, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ProgressScore.Value);
        Assert.Equal($"{expectedResult.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelLower.Value} to {expectedResult.NonDisadvantagedStudentsData.LocalAuthority.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.Result.Points, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Points.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.LocalAuthority.Result.Grade, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Grade.Value);

        // Non-Disadvantaged - England
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.NumberOfStudents, model.PerformanceGroupsData.NonDisadvantagedStudents.England.NumberOfStudents.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.ProgressScore, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ProgressScore.Value);
        Assert.Equal($"{expectedResult.NonDisadvantagedStudentsData.England.ConfidenceLevelLower.Value} to {expectedResult.NonDisadvantagedStudentsData.England.ConfidenceLevelUpper.Value}", model.PerformanceGroupsData.NonDisadvantagedStudents.England.ConfidenceInterval.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.Result.Points, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Points.Value);
        Assert.Equal(expectedResult.NonDisadvantagedStudentsData.England.Result.Grade, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Grade.Value);

    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Get_Level2Qualifications_Info_With_Reason_ReturnsOk(Level2 qualification)
    {
        var expectedResult = new Level2QualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            LAName = fakeEstablishment.LAName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            ProgressScore = new ProgressScoreModel
            {
                Score = new CodedDouble(null, "Not applicable", "z"),
                BandingRating = new CodedString(null, "Not applicable", "z"),
                ConfidenceLevelLower = new CodedDouble(null, "Redacted for confidentiality", "c"),
                ConfidenceLevelUpper = new CodedDouble(null, "Not applicable", "z"),
                EnglandAverageScore = new CodedDouble(null, "Not available", "x"),
            },
            AverageResult = new AverageResultModel
            {
                NumberOfStudents = new RelativeYearValues<CodedDouble>
                {
                    CurrentYear = new CodedDouble(null, "Not applicable", "z"),
                    PreviousYear = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    TwoYearsAgo = new CodedDouble(null, "Not available", "x"),
                },
                Establishment = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z")
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    },
                },
                LocalAuthority = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z")
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    },
                },
                England = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z")
                    },
                }
            },
            DisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Not applicable", "z"),
                    ProgressScore = new CodedDouble(null, "Not applicable", "z"),
                    ConfidenceLevelLower = new CodedDouble(null, "Not applicable", "z"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Not applicable", "z"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not applicable", "z"),
                        Points = new CodedDouble(null, "Not applicable", "z"),
                    }
                },
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ProgressScore = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelLower = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Not available", "x"),
                    ProgressScore = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelLower = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Not available", "x"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    }
                }
            },
            NonDisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = null,
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Not available", "x"),
                    ProgressScore = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelLower = new CodedDouble(null, "Not available", "x"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Not available", "x"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Not available", "x"),
                        Points = new CodedDouble(null, "Not available", "x")
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ProgressScore = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelLower = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    ConfidenceLevelUpper = new CodedDouble(null, "Redacted for confidentiality", "c"),
                    Result = new PerformanceResult
                    {
                        Grade = new CodedString(null, "Redacted for confidentiality", "c"),
                        Points = new CodedDouble(null, "Redacted for confidentiality", "c")
                    }
                }
            }
        };

        _mockLevel2QualificationsService
            .Setup(es => es.GetLevel2QualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Level2Qualifications(
            _mockLevel2QualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as Level2QualificationViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal($"{expectedResult.LAName} average", model.LAName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.IsKS5, model.IsKS5);

        Assert.Equal(NotAvailable, model.TotalNoOfStudentCompletedQualification.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.Score.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.BandingRating.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelLower.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelUpper.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.EnglandAverageScore.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ProgressBandingContextDescription.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Grade.DisplayText());

        // Disadvantaged - Establishment
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Grade.DisplayText());

        // Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Grade.DisplayText());

        // Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Non-Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Grade.DisplayText());
    }

    [Theory]
    [InlineData(Level2.TechCert)]
    public async Task Get_Level2Qualifications_Info_With_No_Data_ReturnsOk(Level2 qualification)
    {
        var expectedResult = new Level2QualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            LAName = fakeEstablishment.LAName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            ProgressScore = new ProgressScoreModel(),
            AverageResult = new AverageResultModel
            {
                NumberOfStudents = new RelativeYearValues<CodedDouble>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
                Establishment = new RelativeYearValues<PerformanceResult>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
                LocalAuthority = new RelativeYearValues<PerformanceResult>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
                England = new RelativeYearValues<PerformanceResult>()
                {
                    CurrentYear = new(),
                    PreviousYear = new(),
                    TwoYearsAgo = new()
                },
            },
            DisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                },
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                }
            },
            NonDisadvantagedStudentsData = new PerformanceSummaryModel
            {
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                },
                England = new PerformanceData
                {
                    NumberOfStudents = new(),
                    ProgressScore = new(),
                    ConfidenceLevelUpper = new(),
                    ConfidenceLevelLower = new(),
                    Result = new PerformanceResult()
                }
            }
        };

        _mockLevel2QualificationsService
            .Setup(es => es.GetLevel2QualificationDetailsAsync(fakeEstablishment.URN, qualification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.Level2Qualifications(
            _mockLevel2QualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            qualification,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as Level2QualificationViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal($"{expectedResult.LAName} average", model.LAName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.IsKS5, model.IsKS5);

        Assert.Equal(NotAvailable, model.TotalNoOfStudentCompletedQualification.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.Score.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.BandingRating.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelLower.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ConfidenceLevelUpper.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.EnglandAverageScore.DisplayText());
        Assert.Equal(NotAvailable, model.ProgressScore.ProgressBandingContextDescription.DisplayText());

        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.Establishment.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.LocalAuthority.TwoYearsAgo!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.CurrentYear.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.PreviousYear!.Grade.DisplayText());
        Assert.Equal(NotAvailable, model.AverageResult.England.TwoYearsAgo!.Grade.DisplayText());

        // Disadvantaged - Establishment
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.Establishment!.Grade.DisplayText());

        // Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.DisadvantagedStudents.England.Grade.DisplayText());

        // Non-Disadvantaged - Local Authority
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.LocalAuthority.Grade.DisplayText());

        // Non-Disadvantaged - England
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.NumberOfStudents.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ProgressScore.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.ConfidenceInterval.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Points.DisplayText());
        Assert.Equal(NotAvailable, model.PerformanceGroupsData.NonDisadvantagedStudents.England.Grade.DisplayText());
    }

    [Fact]
    public async Task Get_EnglishAndMaths_ReturnsExpected()
    {
        var expectedResult = GetEnglishMathsQualificationModel();

        _mockEnglishAndMathsQualificationsService
            .Setup(a => a.GetEnglishAndMathsQualificationDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.EnglishAndMaths(
            _mockEnglishAndMathsQualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as EnglishMathsQualificationsViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(expectedResult.IsKS2, model.IsKS2);
        Assert.Equal(expectedResult.IsKS4, model.IsKS4);
        Assert.Equal(expectedResult.LAName, model.LAName);
        Assert.Equal(expectedResult.AverageEnglishProgress!.EnglandAverage, model.AverageEnglishProgress!.EnglandAverage.Value);
        Assert.Equal(expectedResult.AverageEnglishProgress!.LaAverage, model.AverageEnglishProgress!.LaAverage.Value);
        Assert.Equal(expectedResult.AverageEnglishProgress!.NumberOfStudents, model.AverageEnglishProgress!.NumberOfStudents.Value);
        Assert.Equal(expectedResult.AverageEnglishProgress!.SchoolOrCollege, model.AverageEnglishProgress!.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.AverageMathsProgress!.EnglandAverage, model.AverageMathsProgress!.EnglandAverage.Value);
        Assert.Equal(expectedResult.AverageMathsProgress!.LaAverage, model.AverageMathsProgress!.LaAverage.Value);
        Assert.Equal(expectedResult.AverageMathsProgress!.NumberOfStudents, model.AverageMathsProgress!.NumberOfStudents.Value);
        Assert.Equal(expectedResult.AverageMathsProgress!.SchoolOrCollege, model.AverageMathsProgress!.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.EnteredForEnglishQualification!.EnglandAverage, model.EnteredForEnglishQualification!.EnglandAverage.Value);
        Assert.Equal(expectedResult.EnteredForEnglishQualification!.LaAverage, model.EnteredForEnglishQualification!.LaAverage.Value);
        Assert.Equal(expectedResult.EnteredForEnglishQualification!.NumberOfStudents, model.EnteredForEnglishQualification!.NumberOfStudents.Value);
        Assert.Equal(expectedResult.EnteredForEnglishQualification!.SchoolOrCollege, model.EnteredForEnglishQualification!.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.EnteredForMathsQualification!.EnglandAverage, model.EnteredForMathsQualification!.EnglandAverage.Value);
        Assert.Equal(expectedResult.EnteredForMathsQualification!.LaAverage, model.EnteredForMathsQualification!.LaAverage.Value);
        Assert.Equal(expectedResult.EnteredForMathsQualification!.NumberOfStudents, model.EnteredForMathsQualification!.NumberOfStudents.Value);
        Assert.Equal(expectedResult.EnteredForMathsQualification!.SchoolOrCollege, model.EnteredForMathsQualification!.SchoolOrCollege.Value);

        Assert.Equal(expectedResult.NumberOfDisadvantagedStudentsEnglish.SchoolOrCollege, model.NumberOfDisadvantagedStudentsEnglish.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.NumberOfDisadvantagedStudentsEnglish.LocalAuthority, model.NumberOfDisadvantagedStudentsEnglish.LocalAuthority.Value);
        Assert.Equal(expectedResult.NumberOfDisadvantagedStudentsEnglish.England, model.NumberOfDisadvantagedStudentsEnglish.England.Value);

        Assert.Equal(expectedResult.NumberOfDisadvantagedStudentsMaths.SchoolOrCollege, model.NumberOfDisadvantagedStudentsMaths.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.NumberOfDisadvantagedStudentsMaths.LocalAuthority, model.NumberOfDisadvantagedStudentsMaths.LocalAuthority.Value);
        Assert.Equal(expectedResult.NumberOfDisadvantagedStudentsMaths.England, model.NumberOfDisadvantagedStudentsMaths.England.Value);

        Assert.Equal(expectedResult.NumberOfNonDisadvantagedStudentsEnglish.SchoolOrCollege, model.NumberOfNonDisadvantagedStudentsEnglish.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.NumberOfNonDisadvantagedStudentsEnglish.LocalAuthority, model.NumberOfNonDisadvantagedStudentsEnglish.LocalAuthority.Value);
        Assert.Equal(expectedResult.NumberOfNonDisadvantagedStudentsEnglish.England, model.NumberOfNonDisadvantagedStudentsEnglish.England.Value);

        Assert.Equal(expectedResult.NumberOfNonDisadvantagedStudentsMaths.SchoolOrCollege, model.NumberOfNonDisadvantagedStudentsMaths.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.NumberOfNonDisadvantagedStudentsMaths.LocalAuthority, model.NumberOfNonDisadvantagedStudentsMaths.LocalAuthority.Value);
        Assert.Equal(expectedResult.NumberOfNonDisadvantagedStudentsMaths.England, model.NumberOfNonDisadvantagedStudentsMaths.England.Value);

        Assert.Equal(expectedResult.ProgressOfDisadvantagedStudentsEnglish.SchoolOrCollege, model.ProgressOfDisadvantagedStudentsEnglish.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.ProgressOfDisadvantagedStudentsEnglish.LocalAuthority, model.ProgressOfDisadvantagedStudentsEnglish.LocalAuthority.Value);
        Assert.Equal(expectedResult.ProgressOfDisadvantagedStudentsEnglish.England, model.ProgressOfDisadvantagedStudentsEnglish.England.Value);

        Assert.Equal(expectedResult.ProgressOfDisadvantagedStudentsMaths.SchoolOrCollege, model.ProgressOfDisadvantagedStudentsMaths.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.ProgressOfDisadvantagedStudentsMaths.LocalAuthority, model.ProgressOfDisadvantagedStudentsMaths.LocalAuthority.Value);
        Assert.Equal(expectedResult.ProgressOfDisadvantagedStudentsMaths.England, model.ProgressOfDisadvantagedStudentsMaths.England.Value);

        Assert.Equal(expectedResult.ProgressOfNonDisadvantagedStudentsEnglish.SchoolOrCollege, model.ProgressOfNonDisadvantagedStudentsEnglish.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.ProgressOfNonDisadvantagedStudentsEnglish.LocalAuthority, model.ProgressOfNonDisadvantagedStudentsEnglish.LocalAuthority.Value);
        Assert.Equal(expectedResult.ProgressOfNonDisadvantagedStudentsEnglish.England, model.ProgressOfNonDisadvantagedStudentsEnglish.England.Value);

        Assert.Equal(expectedResult.ProgressOfNonDisadvantagedStudentsMaths.SchoolOrCollege, model.ProgressOfNonDisadvantagedStudentsMaths.SchoolOrCollege.Value);
        Assert.Equal(expectedResult.ProgressOfNonDisadvantagedStudentsMaths.LocalAuthority, model.ProgressOfNonDisadvantagedStudentsMaths.LocalAuthority.Value);
        Assert.Equal(expectedResult.ProgressOfNonDisadvantagedStudentsMaths.England, model.ProgressOfNonDisadvantagedStudentsMaths.England.Value);
    }

    [Fact]
    public async Task Get_EnglishAndMaths_NoEstablishmentReturnsErrorView()
    {
        var expectedResult = GetEnglishMathsQualificationModel(null!);

        _mockEnglishAndMathsQualificationsService
            .Setup(a => a.GetEnglishAndMathsQualificationDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.EnglishAndMaths(
            _mockEnglishAndMathsQualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            CancellationToken.None) as ViewResult;

        Assert.Equal("Error", result!.ViewName);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));
    }

    [Fact]
    public async Task Get_EnglishAndMaths_NotKs5ReturnsErrorView()
    {
        var expectedResult = GetEnglishMathsQualificationModel(isKs5: false);

        _mockEnglishAndMathsQualificationsService
            .Setup(a => a.GetEnglishAndMathsQualificationDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.EnglishAndMaths(
            _mockEnglishAndMathsQualificationsService.Object,
            expectedResult.Urn,
            expectedResult.SchoolName,
            CancellationToken.None) as ViewResult;

        Assert.Equal("Error", result!.ViewName);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));
    }


    [Fact]
    public async Task Get_SubjectsEntered_ReturnsExpected()
    {
        var expectedResult = GetSubjectsEnteredList();
        _establishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new EstablishmentMinimumServiceModel
            {
                URN = fakeEstablishment.URN,
                EstablishmentName = fakeEstablishment.EstablishmentName,
                IsKS5 = true
            });

        _mockKs5EstablishmentSubjectEntriesService
            .Setup(a => a.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<QualificationType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.SubjectsEntered(
            _establishmentService.Object,
            _mockKs5EstablishmentSubjectEntriesService.Object,
            QualificationType.AcademicQualifications,
            fakeEstablishment.URN,
            fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var actualResult = result.Model as Ks5SubjectEnteredViewModel;
        Assert.NotNull(actualResult);
        Assert.NotNull(actualResult.Subjects);
        Assert.Equal(actualResult.URN, fakeEstablishment.URN);
        Assert.Equal(actualResult.SchoolName, fakeEstablishment.EstablishmentName);
        Assert.Equal(actualResult.Subjects[0].Subject, expectedResult.First().Subject);
        Assert.Equal(actualResult.Subjects[0].Qualification, expectedResult.First().Qualification);
        Assert.Equal(actualResult.Subjects[0].Level, expectedResult.First().Level);
        Assert.Equal(actualResult.Subjects[0].NumberOfEntries, expectedResult.First().TotalNumberOfEntries?.ToString());

    }

    [Fact]
    public async Task Get_SubjectsEntered_NoEstablishment_ReturnsErrorView()
    {
        _establishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new EstablishmentMinimumServiceModel
            {
                URN = null!,
                EstablishmentName = fakeEstablishment.EstablishmentName
            });

        var result = await _controller.SubjectsEntered(
            _establishmentService.Object,
            _mockKs5EstablishmentSubjectEntriesService.Object,
            QualificationType.AcademicQualifications,
            fakeEstablishment.URN,
            fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.Equal("Error", result!.ViewName);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));

        _establishmentService
            .Verify(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

        _mockKs5EstablishmentSubjectEntriesService
            .Verify(a => a.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<QualificationType>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Get_SubjectsEntered_NotKs5ReturnsErrorView()
    {
        _establishmentService
            .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new EstablishmentMinimumServiceModel
            {
                URN = fakeEstablishment.URN,
                EstablishmentName = fakeEstablishment.EstablishmentName
            });
        var result = await _controller.SubjectsEntered(
            _establishmentService.Object,
            _mockKs5EstablishmentSubjectEntriesService.Object,
            QualificationType.AcademicQualifications,
            fakeEstablishment.URN,
            fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.Equal("Error", result!.ViewName);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));

        _establishmentService
            .Verify(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

        _mockKs5EstablishmentSubjectEntriesService
            .Verify(a => a.GetSubjectEntriesByUrnAsync(It.IsAny<string>(), It.IsAny<QualificationType>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void Get_SubjectsEnteredRedirect_RedirectsCorrectly()
    {
        // Arrange
        var qualType = QualificationType.AcademicQualifications;

        // Act
        var result = _controller
            .SubjectsEnteredRedirect(fakeEstablishment.URN, fakeEstablishment.EstablishmentName, qualType) as RedirectToActionResult;


        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.RouteValues);
        Assert.Equal("SubjectsEntered", result.ActionName);
        Assert.Equal(fakeEstablishment.URN, result.RouteValues["urn"]);
        Assert.Equal(fakeEstablishment.EstablishmentName, result.RouteValues["schoolName"]);
        Assert.Equal(QualificationType.AcademicQualifications.ToString().ToLower(), result.RouteValues["qualification"]);
    }

    private IEnumerable<SubjectsEnteredModel> GetSubjectsEnteredList() => [
            new ()
            {
                Subject = "Business Studies",
                ExamCohort = "20",
                Level = "3",
                TotalNumberOfEntries = "55",
                Qualification = "BTEC"
            }
        ];

    private EnglishMathsQualificationModel GetEnglishMathsQualificationModel(string urn = "", bool isKs5 = true)
    {
        return new EnglishMathsQualificationModel
        {
            Urn = urn == "" ? fakeEstablishment.URN : urn,
            SchoolName = fakeEstablishment.EstablishmentName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = isKs5,
            LAName = "Test LA",
            AverageEnglishProgress = new EnglishMathsScoreModel
            {
                NumberOfStudents = new CodedDouble(1, string.Empty, "1"),
                SchoolOrCollege = new CodedDouble(2, string.Empty, "2"),
                LaAverage = new CodedDouble(3, string.Empty, "3"),
                EnglandAverage = new CodedDouble(4, string.Empty, "4")
            },
            AverageMathsProgress = new EnglishMathsScoreModel
            {
                NumberOfStudents = new CodedDouble(5, string.Empty, "5"),
                SchoolOrCollege = new CodedDouble(6, string.Empty, "6"),
                LaAverage = new CodedDouble(7, string.Empty, "7"),
                EnglandAverage = new CodedDouble(8, string.Empty, "8")
            },
            EnteredForEnglishQualification = new EnglishMathsScoreModel
            {
                NumberOfStudents = new CodedDouble(9, string.Empty, "9"),
                SchoolOrCollege = new CodedDouble(10, string.Empty, "10"),
                LaAverage = new CodedDouble(11, string.Empty, "11"),
                EnglandAverage = new CodedDouble(12, string.Empty, "12")
            },
            EnteredForMathsQualification = new EnglishMathsScoreModel
            {
                NumberOfStudents = new CodedDouble(13, string.Empty, "13"),
                SchoolOrCollege = new CodedDouble(14, string.Empty, "14"),
                LaAverage = new CodedDouble(15, string.Empty, "15"),
                EnglandAverage = new CodedDouble(16, string.Empty, "16")
            },
            NumberOfDisadvantagedStudentsEnglish = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(17, "", "17"),
                LocalAuthority = new CodedDouble(18, "", "18"),
                England = new CodedDouble(19, "", "19")
            },
            NumberOfDisadvantagedStudentsMaths = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(20, "", "20"),
                LocalAuthority = new CodedDouble(21, "", "21"),
                England = new CodedDouble(22, "", "22")
            },
            NumberOfNonDisadvantagedStudentsEnglish = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(0, "", "0"),
                LocalAuthority = new CodedDouble(23, "", "23"),
                England = new CodedDouble(24, "", "24")
            },
            NumberOfNonDisadvantagedStudentsMaths = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(0, "", "0"),
                LocalAuthority = new CodedDouble(25, "", "25"),
                England = new CodedDouble(26, "", "26")
            },
            ProgressOfDisadvantagedStudentsEnglish = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(27, "", "27"),
                LocalAuthority = new CodedDouble(28, "", "28"),
                England = new CodedDouble(29, "", "29")
            },
            ProgressOfDisadvantagedStudentsMaths = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(30, "", "30"),
                LocalAuthority = new CodedDouble(31, "", "31"),
                England = new CodedDouble(32, "", "32")
            },
            ProgressOfNonDisadvantagedStudentsEnglish = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(0, "", "0"),
                LocalAuthority = new CodedDouble(33, "", "33"),
                England = new CodedDouble(34, "", "34")
            },
            ProgressOfNonDisadvantagedStudentsMaths = new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = new CodedDouble(0, "", "0"),
                LocalAuthority = new CodedDouble(35, "", "35"),
                England = new CodedDouble(36, "", "36")
            }
        };
    }

    private Level3QualificationModel Level3QualificationDetails(Level3 qualification)
    {
        var isALevelQual = qualification == Level3.ALevel;
        var isAcademicQual = qualification == Level3.Academic;

        return new Level3QualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            LAName = fakeEstablishment.LAName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            TotalNoOfStudentCompletedQualification = GetCodedDouble(100),
            ProgressScore = new ProgressScoreModel
            {
                Score = GetCodedDouble(75.55),
                BandingRating = GetCodedString("Average"),
                ConfidenceLevelLower = GetCodedDouble(1.0),
                ConfidenceLevelUpper = GetCodedDouble(5.5),
                EnglandAverageScore = GetCodedDouble(85.11)
            },
            AverageResult = new AverageResultModel
            {
                NumberOfStudents = new RelativeYearValues<CodedDouble> 
                { 
                    CurrentYear = GetCodedDouble(100),
                    PreviousYear = GetCodedDouble(120),
                    TwoYearsAgo = GetCodedDouble(150)
                },
                Establishment = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult 
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(21.45)
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(71.22)
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(50.36)
                    }
                },
                LocalAuthority = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(31.54)
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(75.32)
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(50.15)
                    }
                },
                England = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = GetCodedString("D"),
                        Points = GetCodedDouble(51.75)
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(83.79)
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(35.79)
                    }
                },
            },
            AdditionalData = isALevelQual ? new AdditionalDataModel
            {
                TotalNoOfStudentsIncludedInThisMeasure = GetCodedDouble(100),
                Establishment = new() { Grade = GetCodedString("A"), Points = GetCodedDouble(19.52) },
                LocalAuthority = new() { Grade = GetCodedString("B"), Points = GetCodedDouble(29.53) },
                England = new() { Grade = GetCodedString("B"), Points = GetCodedDouble(31.75) },
            } : null,
            AdvancedLevelMathsQualificationData = isAcademicQual ? new SimpleCodedDoubleTableModel
            {
                SchoolOrCollege = GetCodedDouble(95.12),
                LocalAuthority = GetCodedDouble(82.45),
                England = GetCodedDouble(79.37),
            } : null,
            DisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(255),
                    ProgressScore = GetCodedDouble(85.23),
                    ConfidenceLevelLower = GetCodedDouble(1.0),
                    ConfidenceLevelUpper = GetCodedDouble(5.5),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(85.25)
                    }
                },
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(450),
                    ProgressScore = GetCodedDouble(78.32),
                    ConfidenceLevelLower = GetCodedDouble(0.5),
                    ConfidenceLevelUpper = GetCodedDouble(3.4),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(51.25)
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(805),
                    ProgressScore = GetCodedDouble(77.31),
                    ConfidenceLevelLower = GetCodedDouble(1.2),
                    ConfidenceLevelUpper = GetCodedDouble(4.9),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(65.12)
                    }
                }
            },
            NonDisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = null,
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(500),
                    ProgressScore = GetCodedDouble(81.56),
                    ConfidenceLevelLower = GetCodedDouble(1.1),
                    ConfidenceLevelUpper = GetCodedDouble(4.3),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(81.69)
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(700),
                    ProgressScore = GetCodedDouble(81.59),
                    ConfidenceLevelLower = GetCodedDouble(0.2),
                    ConfidenceLevelUpper = GetCodedDouble(2.5),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(69.15)
                    }
                }
            }
        };
    }

    private Level2QualificationModel Level2QualificationDetails(Level2 qualification)
    {
        return new Level2QualificationModel
        {
            Urn = fakeEstablishment.URN,
            SchoolName = fakeEstablishment.EstablishmentName,
            LAName = fakeEstablishment.LAName,
            IsKS2 = true,
            IsKS4 = true,
            IsKS5 = true,
            QualificationType = qualification,
            TotalNoOfStudentCompletedQualification = GetCodedDouble(120),
            ProgressScore = new ProgressScoreModel
            {
                Score = GetCodedDouble(83.37),
                BandingRating = GetCodedString("Average"),
                ConfidenceLevelLower = GetCodedDouble(0.3),
                ConfidenceLevelUpper = GetCodedDouble(4.2),
                EnglandAverageScore = GetCodedDouble(71.59)
            },
            AverageResult = new AverageResultModel
            {
                NumberOfStudents = new RelativeYearValues<CodedDouble>
                {
                    CurrentYear = GetCodedDouble(100),
                    PreviousYear = GetCodedDouble(120),
                    TwoYearsAgo = GetCodedDouble(150)
                },
                Establishment = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(74.33)
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(31.22)
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(55.17)
                    }
                },
                LocalAuthority = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(52.32)
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(72.25)
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(35.77)
                    }
                },
                England = new RelativeYearValues<PerformanceResult>
                {
                    CurrentYear = new PerformanceResult
                    {
                        Grade = GetCodedString("D"),
                        Points = GetCodedDouble(55.47)
                    },
                    PreviousYear = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(48.85)
                    },
                    TwoYearsAgo = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(33.64)
                    }
                },
            },            
            DisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(150),
                    ProgressScore = GetCodedDouble(81.66),
                    ConfidenceLevelLower = GetCodedDouble(2.0),
                    ConfidenceLevelUpper = GetCodedDouble(5.0),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(83.59)
                    }
                },
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(240),
                    ProgressScore = GetCodedDouble(51.54),
                    ConfidenceLevelLower = GetCodedDouble(1.0),
                    ConfidenceLevelUpper = GetCodedDouble(3.0),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("C"),
                        Points = GetCodedDouble(50.76)
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(1200),
                    ProgressScore = GetCodedDouble(72.56),
                    ConfidenceLevelLower = GetCodedDouble(1.1),
                    ConfidenceLevelUpper = GetCodedDouble(4.5),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(66.79)
                    }
                }
            },
            NonDisadvantagedStudentsData = new PerformanceSummaryModel
            {
                Establishment = null,
                LocalAuthority = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(450),
                    ProgressScore = GetCodedDouble(80.79),
                    ConfidenceLevelLower = GetCodedDouble(1.2),
                    ConfidenceLevelUpper = GetCodedDouble(4.3),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("A"),
                        Points = GetCodedDouble(85.42)
                    }
                },
                England = new PerformanceData
                {
                    NumberOfStudents = GetCodedDouble(550),
                    ProgressScore = GetCodedDouble(87.46),
                    ConfidenceLevelLower = GetCodedDouble(0.4),
                    ConfidenceLevelUpper = GetCodedDouble(2.9),
                    Result = new PerformanceResult
                    {
                        Grade = GetCodedString("B"),
                        Points = GetCodedDouble(67.23)
                    }
                }
            }
        };
    }
}
