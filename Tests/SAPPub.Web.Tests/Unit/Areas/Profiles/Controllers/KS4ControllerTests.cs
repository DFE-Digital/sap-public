using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.Performance;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.Performance;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Web.Areas.Profiles.Controllers;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Areas.Profiles.ViewModels.KS4;
using SAPPub.Web.Constants;
using SAPPub.Web.Helpers;
using SAPPub.Web.Models.SecondarySchool;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Controllers;

public class KS4ControllerTests
{
    private readonly Faker _faker = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;
    private readonly Mock<IKS4EstablishmentSubjectEntriesService> _mockEstablishmentSubjectEntriesService = new();
    private readonly Mock<IAcademicPerformanceEnglishAndMathsResultsService> _mockEnglishAndMathsResultsService = new();
    private readonly Mock<IAttainmentAndProgressService> _mockAttainmentAndProgressService = new();
    private readonly Mock<IFeatureManager> _mockFeatureManager = new();
    private readonly KS4Controller _controller;
    private EstablishmentMinimumServiceModel _fakeEstablishment;

    private List<SubjectsEnteredModel> GcseSubjects =
        new()
        {
            new()
            {
                Subject = "English language",
                Qualification = "GCSE",
                TotalNumberOfEntries = "95",
            },
            new()
            {
                Subject = "English literature",
                Qualification = "GCSE",
                TotalNumberOfEntries = "90",
            }
        };

    private List<SubjectsEnteredModel> VocationalSubjects =
        new()
        {
            new()
            {
                Subject = "Sports Studies",
                Qualification = "Vocational",
                TotalNumberOfEntries = "45",
            },
            new()
            {
                Subject = "Engineering Studies",
                Qualification = "Vocational",
                TotalNumberOfEntries = "10",
            }
        };

    private List<SubjectsEnteredModel> OtherSubjects =
    new()
    {
            new()
            {
                Subject = "Additional Maths (FSMQ)",
                Qualification = "FSMQ",
                TotalNumberOfEntries = "45",
            },
            new()
            {
                Subject = "Grade 6 Performing Arts Graded Examination",
                Qualification = "Music Performance: Group",
                TotalNumberOfEntries = "10",
            }
    };

    public KS4ControllerTests()
    {
        _fakeEstablishment = new EstablishmentMinimumTestBuilder()
            .WithLAName("Sheffield")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .BuildServiceModel();

        _mockEstablishmentService = new();

        _mockEstablishmentService
            .Setup(es => es.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_fakeEstablishment);

        var tempPath = Path.Combine(Path.GetTempPath(), "SAPPubTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);

        _controller = new KS4Controller(_mockEstablishmentService.Object, _mockFeatureManager.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_InvalidYearSelected_ReturnsNotFound()
    {
        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
             _mockAttainmentAndProgressService.Object,
             _fakeEstablishment.URN,
             _fakeEstablishment.EstablishmentName,
             "Invalid-year-selection-string",
             CancellationToken.None) as NotFoundResult;

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current, true)]
    [InlineData(AcademicYearSelection.Previous, false)]
    [InlineData(AcademicYearSelection.Previous2, false)]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_Info_ReturnsExpectedProgressData(AcademicYearSelection academicYearSelection, bool expectedShowProgress8NotAvailableInfo)
    {
        // builder creates a model with no progress 8 data for current year, and with progress 8 data for previous years
        var expectedResult = new AttainmentAndProgressModelBuilder()
            .WithEstablishmentProgress8Data()
            .WithLaProgressData()
            .Build();

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            academicYearSelection.ToRouteSegment()!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(expectedResult.Urn, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(TextHelpers.CleanForUrl(expectedResult.SchoolName!), model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(3, model.AcademicYearsSelectList.Count);
        Assert.Equal(academicYearSelection, model.SelectedAcademicYear);
        Assert.Equal($"Information in this section is for the {academicYearSelection.GetDisplayName()} academic year.", model.AcademicYearInfoParagraph);
        
        Assert.Equal(expectedShowProgress8NotAvailableInfo, model.ShowProgress8NotAvailableInfo);

        Assert.Equal(expectedResult.EstablishmentAttainment8Score.GetValueForYear(academicYearSelection).Value, model.SelectedYearValues.EstablishmentAttainment8Score.Value);
        Assert.Equal(expectedResult.LocalAuthorityAttainment8Score.GetValueForYear(academicYearSelection).Value, model.SelectedYearValues.LocalAuthorityAttainment8Score.Value);
        Assert.Equal(expectedResult.EnglandAttainment8Score.GetValueForYear(academicYearSelection).Value, model.SelectedYearValues.EnglandAttainment8Score.Value);

        if (expectedShowProgress8NotAvailableInfo)
        {
            Assert.False(model.SelectedYearValues.EstablishmentProgress8Score.HasValue);
            Assert.False(model.SelectedYearValues.EstablishmentProgress8CILower.HasValue);
            Assert.False(model.SelectedYearValues.EstablishmentProgress8CIUpper.HasValue);
            Assert.Null(model.SelectedYearValues.EstablishmentProgress8Banding);
            Assert.False(model.SelectedYearValues.LocalAuthorityProgress8Score.HasValue);
            Assert.False(model.SelectedYearValues.EstablishmentProgress8TotalPupils.HasValue);
            Assert.False(model.SelectedYearValues.EstablishmentTotalPupils.HasValue);
        }
        else
        {
            Assert.Equal(expectedResult.EstablishmentProgress8Score.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentProgress8Score);
            Assert.Equal(expectedResult.EstablishmentProgress8CILower.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentProgress8CILower);
            Assert.Equal(expectedResult.EstablishmentProgress8CIUpper.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentProgress8CIUpper);
            Assert.Equal(expectedResult.EstablishmentProgress8Banding.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentProgress8Banding);
            Assert.Equal(expectedResult.LocalAuthorityProgress8Score.GetValueForYear(academicYearSelection), model.SelectedYearValues.LocalAuthorityProgress8Score);
            Assert.Equal(expectedResult.EstablishmentProgress8TotalPupils.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentProgress8TotalPupils);
            Assert.Equal(expectedResult.EstablishmentTotalPupils.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentTotalPupils);
        }
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current)]
    [InlineData(AcademicYearSelection.Previous)]
    [InlineData(AcademicYearSelection.Previous2)]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_ReturnsExpectedSelectedYearAttainment8Data(AcademicYearSelection academicYearSelection)
    {
        var expectedResult = new AttainmentAndProgressModelBuilder()
            .WithAttainment8Data()
            .WithAttainmentNonDisadvantaged8Data()
            .Build();

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            academicYearSelection.ToRouteSegment()!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.EstablishmentAttainment8Score.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentAttainment8Score);
        Assert.Equal(expectedResult.LocalAuthorityAttainment8Score.GetValueForYear(academicYearSelection), model.SelectedYearValues.LocalAuthorityAttainment8Score);
        Assert.Equal(expectedResult.EnglandAttainment8Score.GetValueForYear(academicYearSelection), model.SelectedYearValues.EnglandAttainment8Score);
        Assert.Equal(expectedResult.EstablishmentAttainment8DisadvantagedScore.GetValueForYear(academicYearSelection), model.SelectedYearValues.EstablishmentAttainment8DisadvantagedScore.Value);
        Assert.Equal(expectedResult.EnglandAttainment8DisadvantagedScore.GetValueForYear(academicYearSelection), model.SelectedYearValues.EnglandAttainment8DisadvantagedScore.Value);
        Assert.Equal(expectedResult.EnglandAttainment8NonDisadvantagedScore, model.SelectedYearValues.EnglandAttainment8NonDisadvantagedScore.Value);
        Assert.Equal(expectedResult.LocalAuthorityAttainment8NonDisadvantagedScore, model.SelectedYearValues.LocalAuthorityAttainment8NonDisadvantagedScore.Value);
        Assert.True(model.ShowAttainment8Info);
    }

    [Fact]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_ReturnsExpectedGeneralData()
    {
        var expectedResult = new AttainmentAndProgressModelBuilder()
            .WithAttainmentNonDisadvantaged8Data()
            .WithAttainment8Data()
            .Build();

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            AcademicYearSelection.Current.ToRouteSegment()!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.Urn, model.URN);
        Assert.Equal(expectedResult.SchoolName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(expectedResult.Urn, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(TextHelpers.CleanForUrl(expectedResult.SchoolName!), model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(3, model.AcademicYearsSelectList.Count);
        Assert.Equal(AcademicYearSelection.Current, model.SelectedAcademicYear);
    }

    [Fact]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_NoAttainment8Data_ReturnsExpected()
    {
        var expectedResult = new AttainmentAndProgressModelBuilder()
            .Build();

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            AcademicYearSelection.Current.ToRouteSegment()!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.False(model.ShowAttainment8Info);
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current)]
    [InlineData(AcademicYearSelection.Previous)]
    [InlineData(AcademicYearSelection.Previous2)]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_ReturnsExpectedAttainment8DisadvantagedData(
        AcademicYearSelection academicYearSelection)
    {
        var expectedResult = new AttainmentAndProgressModelBuilder()
            .WithAttainmentNonDisadvantaged8Data()
            .WithAttainment8Data()
            .Build();

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceAttainmentAndProgress(
            _mockAttainmentAndProgressService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            academicYearSelection.ToRouteSegment()!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceAttainmentAndProgressViewModel;
        Assert.NotNull(model);
        Assert.Equal(expectedResult.EstablishmentAttainment8DisadvantagedScore.CurrentYear.ToString(), model.YearValues.CurrentYear.EstablishmentAttainment8DisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.EstablishmentAttainment8DisadvantagedScore.PreviousYear.ToString(), model.YearValues.PreviousYear!.EstablishmentAttainment8DisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.EstablishmentAttainment8DisadvantagedScore.TwoYearsAgo.ToString(), model.YearValues.TwoYearsAgo!.EstablishmentAttainment8DisadvantagedScore.DisplayText());

        Assert.Equal(expectedResult.LocalAuthorityAttainment8DisadvantagedScore.CurrentYear.ToString(), model.YearValues.CurrentYear.LocalAuthorityAttainment8DisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.LocalAuthorityAttainment8DisadvantagedScore.PreviousYear.ToString(), model.YearValues.PreviousYear.LocalAuthorityAttainment8DisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.LocalAuthorityAttainment8DisadvantagedScore.TwoYearsAgo.ToString(), model.YearValues.TwoYearsAgo.LocalAuthorityAttainment8DisadvantagedScore.DisplayText());

        Assert.Equal(expectedResult.EnglandAttainment8DisadvantagedScore.CurrentYear.ToString(), model.YearValues.CurrentYear.EnglandAttainment8DisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.EnglandAttainment8DisadvantagedScore.PreviousYear.ToString(), model.YearValues.PreviousYear.EnglandAttainment8DisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.EnglandAttainment8DisadvantagedScore.TwoYearsAgo.ToString(), model.YearValues.TwoYearsAgo.EnglandAttainment8DisadvantagedScore.DisplayText());

        Assert.Equal(expectedResult.EnglandAttainment8NonDisadvantagedScore.ToString(), model.YearValues.CurrentYear.EnglandAttainment8NonDisadvantagedScore.DisplayText());
        Assert.Equal(expectedResult.LocalAuthorityAttainment8NonDisadvantagedScore.ToString(), model.YearValues.CurrentYear.LocalAuthorityAttainment8NonDisadvantagedScore.DisplayText());
    }

    [Fact]
    public async Task Get_AcademicPerformanceEnglishAndMaths_InvalidGradeSelected_ReturnsNotFound()
    {
        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
             _mockEnglishAndMathsResultsService.Object,
             _fakeEstablishment.URN,
             _fakeEstablishment.EstablishmentName,
             "Invalid-grade-selection-string",
             CancellationToken.None) as NotFoundResult;

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(GcseGradeDataSelection.Grade4AndAbove)]
    [InlineData(GcseGradeDataSelection.Grade5AndAbove)]
    [InlineData(GcseGradeDataSelection.Grade7AndAbove)]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_ReturnsOk(GcseGradeDataSelection grade)
    {
        // enable grade 7 feature flag for this test
        _mockFeatureManager
            .Setup(fm => fm.IsEnabledAsync(Constants.Constants.EnableSecondaryGrade7))
            .ReturnsAsync(true);

        var expectedResult = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(_fakeEstablishment.URN)
            .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
            .WithLaName(_fakeEstablishment.LAName)
            .WithData()
            .Build();

        var gradeName = grade.ToRouteSegment();

        _mockEnglishAndMathsResultsService
            .Setup(s => s.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)grade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            gradeName!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(grade, model.SelectedGrade);
        Assert.Equal(["School", $"{_fakeEstablishment.LAName} average", "England average"], model.AllGcseData.Labels);
        Assert.Equal(
            [
                expectedResult.EstablishmentAll.CurrentYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value,
                expectedResult.EnglandAll.CurrentYear!.Value
            ],
            model.AllGcseData.Data
        );

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);

        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal(
            [
                expectedResult.EstablishmentAll.TwoYearsAgo!.Value,
                expectedResult.EstablishmentAll.PreviousYear!.Value,
                expectedResult.EstablishmentAll.CurrentYear!.Value
            ],
            model.AllGcseOverTimeData.Datasets[0].Data
        );

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal(
            [
                expectedResult.LocalAuthorityAll.TwoYearsAgo!.Value,
                expectedResult.LocalAuthorityAll.PreviousYear!.Value,
                expectedResult.LocalAuthorityAll.CurrentYear!.Value
            ],
            model.AllGcseOverTimeData.Datasets[1].Data
        );

        Assert.Equal("England average", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal(
            [
                expectedResult.EnglandAll.TwoYearsAgo!.Value,
                expectedResult.EnglandAll.PreviousYear!.Value,
                expectedResult.EnglandAll.CurrentYear!.Value],
            model.AllGcseOverTimeData.Datasets[2].Data);


        // Breakdown gcse data assert
        Assert.Equal(["Girls", "Boys"], model.BreakdownGcseData.Labels);

        Assert.Equal(3, model.BreakdownGcseData.Datasets.Count);

        Assert.Equal("School", model.BreakdownGcseData.Datasets[0].Label);
        Assert.Equal([
                expectedResult.EstablishmentGirls.CurrentYear!.Value,
                expectedResult.EstablishmentBoys.CurrentYear!.Value],
            model.BreakdownGcseData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.BreakdownGcseData.Datasets[1].Label);
        Assert.Equal([
                expectedResult.LocalAuthorityGirls.CurrentYear!.Value,
                expectedResult.LocalAuthorityBoys.CurrentYear!.Value],
            model.BreakdownGcseData.Datasets[1].Data);

        Assert.Equal("England average", model.BreakdownGcseData.Datasets[2].Label);
        Assert.Equal([
                expectedResult.EnglandGirls.CurrentYear!.Value,
                expectedResult.EnglandBoys.CurrentYear!.Value],
            model.BreakdownGcseData.Datasets[2].Data);
    }

    [Fact]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_ResultsNotAvailable_ReturnsOk()
    {
        var gradeSelection = GcseGradeDataSelection.Grade4AndAbove;
        var gradeName = gradeSelection.ToRouteSegment();

        EnglishAndMathsResultsModel serviceModel = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(_fakeEstablishment.URN)
            .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
            .WithLaName(_fakeEstablishment.LAName)
            .WithIsKS4(true)
            .Build();

        _mockEnglishAndMathsResultsService
            .Setup(s => s.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)gradeSelection, It.IsAny<CancellationToken>()))
            .ReturnsAsync(serviceModel);

        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            gradeName!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(gradeSelection, model.SelectedGrade);
        Assert.Equal(["School", $"{_fakeEstablishment.LAName} average", "England average"], model.AllGcseData.Labels);
        Assert.Equal([null, null, null], model.AllGcseData.Data);

        Assert.Equal(3, model.AllGcseOverTimeData.Datasets.Count);
        Assert.Equal("School", model.AllGcseOverTimeData.Datasets[0].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.AllGcseOverTimeData.Datasets[1].Label);
        Assert.Equal([null, null, null], model.AllGcseOverTimeData.Datasets[1].Data);

        Assert.Equal("England average", model.AllGcseOverTimeData.Datasets[2].Label);
        Assert.Equal(new double?[] { null, null, null }, model.AllGcseOverTimeData.Datasets[2].Data);

        // Breakdown gcse data assert
        Assert.Equal(["Girls", "Boys"], model.BreakdownGcseData.Labels);

        Assert.Equal(3, model.BreakdownGcseData.Datasets.Count);

        Assert.Equal("School", model.BreakdownGcseData.Datasets[0].Label);
        Assert.Equal([null, null], model.BreakdownGcseData.Datasets[0].Data);

        Assert.Equal($"{_fakeEstablishment.LAName} average", model.BreakdownGcseData.Datasets[1].Label);
        Assert.Equal([null, null], model.BreakdownGcseData.Datasets[1].Data);

        Assert.Equal("England average", model.BreakdownGcseData.Datasets[2].Label);
        Assert.Equal([null, null], model.BreakdownGcseData.Datasets[2].Data);
    }


    [Theory]
    [InlineData("Sheffield", "Sheffield average")]
    [InlineData("Poole Grammar School", "Local council average")]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_LocalCouncilName(string localCouncilName, string expectedCouncilName)
    {
        _fakeEstablishment.LAName = localCouncilName;
        var grade = GcseGradeDataSelection.Grade4AndAbove;
        var gradeName = grade.ToRouteSegment();
        var expectedResult = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(_fakeEstablishment.URN)
            .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
            .WithLaName(_fakeEstablishment.LAName)
            .Build();

        _mockEnglishAndMathsResultsService
            .Setup(s => s.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)grade, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            gradeName!,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

        string[] expectedAllGcseDataLabels = ["School", expectedCouncilName, "England average"];
        string[] expectedAllGcseOverTimeDataLabels = ["School", expectedCouncilName, "England average"];
        string[] expectedBreakdownGcseDataLabels = ["School", expectedCouncilName, "England average"];


        Assert.Equal(expectedAllGcseDataLabels, model.AllGcseData.Labels);

        var actualAllGcseOverTimeDataLabels = model.AllGcseOverTimeData.Datasets.Select(s => s.Label).ToArray();
        Assert.Equal(expectedAllGcseOverTimeDataLabels, actualAllGcseOverTimeDataLabels);

        var actualBreakdownGcseDataLabels = model.BreakdownGcseData.Datasets.Select(s => s.Label).ToArray();
        Assert.Equal(actualBreakdownGcseDataLabels, expectedBreakdownGcseDataLabels);
    }

    [Fact]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_Grade7_FeatureFlagNotEnabled_ReturnsNotFound()
    {
        // Act
        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            GcseGradeDataSelection.Grade7AndAbove.ToRouteSegment()!,
            CancellationToken.None);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Get_AcademicPerformance_EnglishAndMathsResults_Disadvantaged_ReturnsExpectedData()
    {
        // Arrange
        var expectedResult = new EnglishAndMathsResultsModelBuilder()
            .WithUrn(_fakeEstablishment.URN)
            .WithEstablishmentName(_fakeEstablishment.EstablishmentName)
            .WithLaName(_fakeEstablishment.LAName)
            .WithData()
            .Build();
        _mockEnglishAndMathsResultsService.Setup(x => x.GetEnglishAndMathsResultsAsync(_fakeEstablishment.URN, (int)GcseGradeDataSelection.Grade5AndAbove, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AcademicPerformanceEnglishAndMathsResults(
            _mockEnglishAndMathsResultsService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            GcseGradeDataSelection.Grade5AndAbove.ToRouteSegment()!,
            CancellationToken.None);

        // Assert
        Assert.IsType<ViewResult>(result);

        var viewModel = (result as ViewResult)!.Model as AcademicPerformanceEnglishAndMathsResultsViewModel;
        Assert.NotNull(viewModel);

        Assert.Equal("School", viewModel.BreakdownDisadvantaged.Datasets[0].Label);
        Assert.Equal(
            expectedResult.EstablishmentDisadvantaged.CurrentYear,
            viewModel.BreakdownDisadvantaged.Datasets[0].Data.Single()
        );
        Assert.Equal(
            expectedResult.LocalAuthorityDisadvantaged.CurrentYear,
            viewModel.BreakdownDisadvantaged.Datasets[1].Data.Single()
        );
        Assert.Equal(
            expectedResult.EnglandDisadvantaged.CurrentYear,
            viewModel.BreakdownDisadvantaged.Datasets[2].Data.Single()
        );

        Assert.Equal(new[] { "Percentage who achieved Grade 5 and above in English and maths" }, viewModel.BreakdownDisadvantaged.Labels);

        Assert.Equal(
            expectedResult.LocalAuthorityNonDisadvantaged.CurrentYear,
            viewModel.BreakdownNonDisadvantaged.Datasets[0].Data.Single()
        );
        Assert.Equal(
            expectedResult.EnglandNonDisadvantaged.CurrentYear,
            viewModel.BreakdownNonDisadvantaged.Datasets[1].Data.Single()
        );
    }

    [Fact]
    public async Task Get_AcademicPerformance_SubjectsEntered_ReturnsOk()
    {
        _mockEstablishmentSubjectEntriesService
            .Setup(s => s.GetSubjectEntriesByUrnAsync(_fakeEstablishment.URN, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GcseSubjects, VocationalSubjects, OtherSubjects));

        var result = await _controller.AcademicPerformanceSubjectsEntered(
            _mockEstablishmentSubjectEntriesService.Object,
            _fakeEstablishment.URN,
            _fakeEstablishment.EstablishmentName,
            CancellationToken.None) as ViewResult;

        Assert.NotNull(result);
        Assert.NotNull(result.Model);

        var model = result.Model as AcademicPerformanceSubjectsEnteredViewModel;
        Assert.NotNull(model);
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);

        Assert.NotNull(model);
        Assert.Equal(
            GcseSubjects.Select(c => c.Subject).OrderBy(s => s),
            model?.GcseSubjects?.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           GcseSubjects.Select(c => $"{c.TotalNumberOfEntries:F0}").OrderBy(s => s),
           model?.GcseSubjects?.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            GcseSubjects.Select(c => c.Qualification).OrderBy(s => s),
            model?.GcseSubjects?.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.NotNull(model?.VocationalSubjects);
        Assert.Equal(
            VocationalSubjects.Select(c => c.Subject).OrderBy(s => s),
            model?.VocationalSubjects?.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           VocationalSubjects.Select(c => $"{c.TotalNumberOfEntries:F0}").OrderBy(s => s),
           model?.VocationalSubjects?.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            VocationalSubjects.Select(c => c.Qualification).OrderBy(s => s),
            model?.VocationalSubjects?.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.NotNull(model?.OtherSubjects);
        Assert.Equal(
            OtherSubjects.Select(c => c.Subject).OrderBy(s => s),
            model?.OtherSubjects?.Select(s => s.Subject).OrderBy(s => s)
        );
        Assert.Equal(
           OtherSubjects.Select(c => $"{c.TotalNumberOfEntries:F0}").OrderBy(s => s),
           model?.OtherSubjects?.Select(s => s.NumberOfEntries).OrderBy(s => s)
        );
        Assert.Equal(
            OtherSubjects.Select(c => c.Qualification).OrderBy(s => s),
            model?.OtherSubjects?.Select(s => s.Qualification).OrderBy(s => s)
        );

        Assert.Equal(2, model?.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model!.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
    }
}
