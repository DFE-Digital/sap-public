using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Enums;
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

    private EnglishAndMathsResultsModel EnglishAndMathsResults(
        string urn = "123456",
        string establishmentName = "School Name",
        string laName = "Sheffield") => new()
        {
            Urn = urn,
            SchoolName = establishmentName,
            LAName = laName,
            EstablishmentAll = new RelativeYearValues<double?>
            {
                CurrentYear = 60,
                PreviousYear = 80,
                TwoYearsAgo = 60
            },
            LocalAuthorityAll = new RelativeYearValues<double?>
            {
                CurrentYear = 80,
                PreviousYear = 70,
                TwoYearsAgo = 80
            },
            EnglandAll = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
                PreviousYear = 70,
                TwoYearsAgo = 80
            },
            EstablishmentBoys = new RelativeYearValues<double?>
            {
                CurrentYear = 50
            },
            LocalAuthorityBoys = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
            },
            EnglandBoys = new RelativeYearValues<double?>
            {
                CurrentYear = 60,
            },
            EstablishmentGirls = new RelativeYearValues<double?>
            {
                CurrentYear = 80
            },
            LocalAuthorityGirls = new RelativeYearValues<double?>
            {
                CurrentYear = 70,
            },
            EnglandGirls = new RelativeYearValues<double?>
            {
                CurrentYear = 90,
            },
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
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
    public async Task Get_AcademicPerformanceAttainmentAndProgress_Info_ReturnsOk(AcademicYearSelection academicYearSelection, bool expectedShowProgress8NotAvailableInfo)
    {
        var expectedResult = new AttainmentAndProgressModel
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            EstablishmentProgress8Score = expectedShowProgress8NotAvailableInfo ? null : 0.9,
            EstablishmentProgress8CILower = expectedShowProgress8NotAvailableInfo ? null : -0.2,
            EstablishmentProgress8CIUpper = expectedShowProgress8NotAvailableInfo ? null : 1.2,
            EstablishmentProgress8Banding = expectedShowProgress8NotAvailableInfo ? null : "Average",
            LocalAuthorityProgress8Score = expectedShowProgress8NotAvailableInfo ? null : 1.5,
            EstablishmentAttainment8Score = 70,
            LocalAuthorityAttainment8Score = 80,
            EnglandAttainment8Score = 50,
            EstablishmentProgress8TotalPupils = expectedShowProgress8NotAvailableInfo ? null : 65,
            EstablishmentTotalPupils = expectedShowProgress8NotAvailableInfo ? null : 95,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, academicYearSelection, It.IsAny<CancellationToken>()))
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
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(3, model.AcademicYearsSelectList.Count);
        Assert.Equal(academicYearSelection, model.SelectedAcademicYear);
        Assert.Equal($"Information in this section is for the {academicYearSelection.GetDisplayName()} academic year.", model.AcademicYearInfoParagraph);
        Assert.Equal(expectedShowProgress8NotAvailableInfo, model.ShowProgress8NotAvailableInfo);

        Assert.Equal(expectedResult.EstablishmentAttainment8Score, model.EstablishmentAttainment8Score);
        Assert.Equal(expectedResult.LocalAuthorityAttainment8Score, model.LocalAuthorityAttainment8Score);
        Assert.Equal(expectedResult.EnglandAttainment8Score, model.EnglandAttainment8Score);

        if (expectedShowProgress8NotAvailableInfo)
        {
            Assert.Null(model.EstablishmentProgress8Score);
            Assert.Null(model.EstablishmentProgress8CILower);
            Assert.Null(model.EstablishmentProgress8CIUpper);
            Assert.Null(model.EstablishmentProgress8Banding);
            Assert.Null(model.LocalAuthorityProgress8Score);
            Assert.Null(model.EstablishmentProgress8TotalPupils);
            Assert.Null(model.EstablishmentTotalPupils);
        }
        else
        {
            Assert.Equal(expectedResult.EstablishmentProgress8Score, model.EstablishmentProgress8Score);
            Assert.Equal(expectedResult.EstablishmentProgress8CILower, model.EstablishmentProgress8CILower);
            Assert.Equal(expectedResult.EstablishmentProgress8CIUpper, model.EstablishmentProgress8CIUpper);
            Assert.Equal(expectedResult.EstablishmentProgress8Banding, model.EstablishmentProgress8Banding);
            Assert.Equal(expectedResult.LocalAuthorityProgress8Score, model.LocalAuthorityProgress8Score);
            Assert.Equal(expectedResult.EstablishmentProgress8TotalPupils, model.EstablishmentProgress8TotalPupils);
            Assert.Equal(expectedResult.EstablishmentTotalPupils, model.EstablishmentTotalPupils);
        }
    }

    [Theory]
    [InlineData(AcademicYearSelection.Current, true)]
    [InlineData(AcademicYearSelection.Previous, false)]
    [InlineData(AcademicYearSelection.Previous2, false)]
    public async Task Get_AcademicPerformanceAttainmentAndProgress_Display_Attainment8_Data(
        AcademicYearSelection academicYearSelection,
        bool expectedShowAttainment8Info)
    {
        var expectedResult = new AttainmentAndProgressModel
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            EstablishmentProgress8Score = 0.9,
            LocalAuthorityProgress8Score = 1.5,
            EstablishmentAttainment8Score = expectedShowAttainment8Info ? 70 : null,
            LocalAuthorityAttainment8Score = 80,
            EnglandAttainment8Score = 50,
            EstablishmentProgress8TotalPupils = 65,
            EstablishmentTotalPupils = 95,
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

        _mockAttainmentAndProgressService
            .Setup(s => s.GetAttainmentAndProgressAsync(_fakeEstablishment.URN, academicYearSelection, It.IsAny<CancellationToken>()))
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
        Assert.Equal(_fakeEstablishment.URN, model.URN);
        Assert.Equal(_fakeEstablishment.EstablishmentName, model.SchoolName);
        Assert.Equal(2, model.RouteAttributes.Count);
        Assert.Equal(_fakeEstablishment.URN, model.RouteAttributes[RouteConstants.URN]);
        Assert.Equal(_fakeEstablishment.EstablishmentNameClean, model.RouteAttributes[RouteConstants.SchoolName]);
        Assert.Equal(3, model.AcademicYearsSelectList.Count);
        Assert.Equal(academicYearSelection, model.SelectedAcademicYear);
        Assert.Equal(expectedShowAttainment8Info, model.ShowAttainment8Info);
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
            .Setup(fm => fm.IsEnabledAsync(Constants.Constants.EnableSecondaryYear2526Publication))
            .ReturnsAsync(true);

        var expectedResult = EnglishAndMathsResults(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, _fakeEstablishment.LAName);
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

        EnglishAndMathsResultsModel serviceModel = new()
        {
            Urn = _fakeEstablishment.URN,
            SchoolName = _fakeEstablishment.EstablishmentName,
            LAName = _fakeEstablishment.LAName,
            EstablishmentAll = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityAll = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandAll = new RelativeYearValues<double?> { CurrentYear = null },
            EstablishmentBoys = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityBoys = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandBoys = new RelativeYearValues<double?> { CurrentYear = null },
            EstablishmentGirls = new RelativeYearValues<double?> { CurrentYear = null },
            LocalAuthorityGirls = new RelativeYearValues<double?> { CurrentYear = null },
            EnglandGirls = new RelativeYearValues<double?> { CurrentYear = null },
            IsKS2 = false,
            IsKS4 = true,
            IsKS5 = false
        };

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
        var expectedResult = EnglishAndMathsResults(_fakeEstablishment.URN, _fakeEstablishment.EstablishmentName, _fakeEstablishment.LAName);

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
