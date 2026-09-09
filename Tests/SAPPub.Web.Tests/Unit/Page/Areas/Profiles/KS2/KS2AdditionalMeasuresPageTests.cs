using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.KS4.AboutSchool;
using SAPPub.Core.Interfaces.Services.Performance;
using SAPPub.Core.ServiceModels;
using SAPPub.Core.ServiceModels.KS4.AboutSchool;
using SAPPub.Core.ServiceModels.Performance;
using SAPPub.Core.Tests.TestBuilders;
using SAPPub.Core.ValueObjects;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;

namespace SAPPub.Web.Tests.Unit.Page.Areas.Profiles.KS2;

[Collection("WebAppCollection")]
public class KS2AdditionalMeasuresPageTests : PageTestsBase
{
    private readonly string _pageRoute = "/primary-performance/additional-measures";
    private readonly string _urn = "149976";
    private readonly string _laName = "Test LA";
    private readonly string _laId = "123";

    private readonly EstablishmentMinimumServiceModel _establishment = new();
    private readonly Mock<IEstablishmentService> _mockEstablishmentService;

    private readonly KS2AdditionalMeasuresModel _ks2AdditionalMeasuresModel;
    private readonly Mock<IKS2AdditionalMeasuresService> _ks2AdditionalMeasuresService = new();
    private readonly Mock<IAboutSchoolService> _aboutSchoolService = new();

    public KS2AdditionalMeasuresPageTests(WebAppFixture fixture) : base(fixture)
    {
        _ks2AdditionalMeasuresService = UseMock<IKS2AdditionalMeasuresService>();
        _mockEstablishmentService = UseMock<IEstablishmentService>();
        _aboutSchoolService = UseMock<IAboutSchoolService>();

        _establishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage2(true)
            .WithLAName(_laName)
            .WithLAId(_laId)
            .BuildServiceModel();

        _ks2AdditionalMeasuresModel = GetKS2AdditionalMeasuresModel();

        _aboutSchoolService
            .Setup(a => a.GetAboutSchoolDetailsAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(new AboutSchoolModel { SchoolName = _establishment.EstablishmentName, NumberOfPupils = "100", Urn = _establishment.URN });

        _mockEstablishmentService
           .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(_establishment);

        _ks2AdditionalMeasuresService
            .Setup(s => s.GetAdditionalMeasures(_urn, _laId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_ks2AdditionalMeasuresModel);
    }

    [Fact]
    public async Task AdditionalMeasures_HasCorrectTitle()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var title = doc.QuerySelector("title");
        Assert.NotNull(title);

        var expectedTitle = $"School149976 - Primary Additional measures - School Profiles - GOV.UK";
        Assert.Contains(expectedTitle, title.TextContent.Trim());
    }

    [Fact]
    public async Task AdditionalMeasures_DisplaysCorrectHeading()
    {
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var heading = doc.QuerySelectorAll("h2");
        Assert.NotNull(heading[2]);
        Assert.Contains("Additional measures", heading[2].TextContent.Trim());
    }

    [Fact]
    public async Task AdditionalMeasures_HasCorrectSubNavigationLinks()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#sub-navigation-academic-performance");
        var links = container?.QuerySelectorAll(".moj-sub-navigation__link");

        Assert.NotNull(links);
        Assert.Equal(4, links.Length);
    }

    [Fact]
    public async Task AdditionalMeasures_ShowsFindOutMoreInformation()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var container = doc.QuerySelector("#details-additional-measures");

        Assert.NotNull(container);
    }

    [Fact]
    public async Task AdditionalMeasures_Displays_BreakdownOfNumbers()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();

        // Act
        var doc = await Fixture.BrowseToPage(url);

        Assert.Contains("Pupil group", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 0, 0));
        Assert.Contains("Grammar, punctuation, spelling at the expected standard", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 0, 1));
        Assert.Contains("Grammar, punctuation, spelling at the higher standard", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 0, 2));

        // Assert
        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 1, 0));
        Assert.Equal(expectedModel.EstablishmentGrammarAtExpectedStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 1, 0));
        Assert.Equal(expectedModel.EstablishmentGrammarAtHigherStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 1, 1));

        Assert.Equal($"{_laName} average", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 2, 0));
        Assert.Equal(expectedModel.LAGrammarAtExpectedStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 2, 0));
        Assert.Equal(expectedModel.LAGrammarAtHigherStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 2, 1));

        Assert.Equal("England average", doc.GetTableHeaderContentByIdAndIndex("additional-measures-breakdown-table", 3, 0));
        Assert.Equal(expectedModel.EnglandGrammarAtExpectedStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 3, 0));
        Assert.Equal(expectedModel.EnglandGrammarAtHigherStandard!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex("additional-measures-breakdown-table", 3, 1));

    }

    [Fact]
    public async Task AdditionalMeasures_ShowsPupilPopulationAccordion()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();
        var ehcpTableSelector = "ehcp-population-table";
        var senTableSelector = "sen-population-table";

        // Act
        var doc = await Fixture.BrowseToPage(url);
        var accordion = doc.QuerySelector("#pupil-population-accordion");
        var ehcpSection = doc.QuerySelector("#pupils-with-ehcp-section");
        var senSupportSection = doc.QuerySelector("#pupils-with-sen-support-section");
        var ehcpTable = doc.QuerySelector($"#{ehcpTableSelector}");
        var senTable = doc.QuerySelector($"#{senTableSelector}");

        //Assert
        Assert.NotNull(accordion);
        Assert.NotNull(ehcpSection);
        Assert.NotNull(senSupportSection);
        Assert.NotNull(ehcpTable);
        Assert.NotNull(senTable);

        Assert.Equal(string.Empty, doc.GetTableHeaderContentByIdAndIndex(ehcpTableSelector, 0, 0));
        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex(ehcpTableSelector, 0, 1));
        Assert.Equal("England mainstream schools", doc.GetTableHeaderContentByIdAndIndex(ehcpTableSelector, 0, 2));
        Assert.Equal("Pupils with EHCPs", doc.GetTableHeaderContentByIdAndIndex(ehcpTableSelector, 1, 0));
        Assert.Equal(expectedModel.EstablishmentEHCPPopulation!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex(ehcpTableSelector, 1, 0));
        Assert.Equal(expectedModel.EnglandEHCPPopulation!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex(ehcpTableSelector, 1, 1));

        Assert.Equal(string.Empty, doc.GetTableHeaderContentByIdAndIndex(senTableSelector, 0, 0));
        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex(senTableSelector, 0, 1));
        Assert.Equal("England mainstream schools", doc.GetTableHeaderContentByIdAndIndex(senTableSelector, 0, 2));
        Assert.Equal("Pupils with SEN support", doc.GetTableHeaderContentByIdAndIndex(senTableSelector, 1, 0));
        Assert.Equal(expectedModel.EstablishmentSENSupportPopulation!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex(senTableSelector, 1, 0));
        Assert.Equal(expectedModel.EnglandSENSupportPopulation!.Value.ToString() + "%", doc.GetTableCellContentByIdAndIndex(senTableSelector, 1, 1));

    }

    [Fact]
    public async Task AdditionalMeasures_PupilPopulationAccordion_DataNotAvailable_ShowsTableValuesAsNotAvailable()
    {
        // Arrange
        var modelWithNoData = new KS2AdditionalMeasuresModel
        {
            EnglandGrammarAtExpectedStandard = GetCodedDouble(1),
            EnglandGrammarAtHigherStandard = GetCodedDouble(2),
            EstablishmentGrammarAtExpectedStandard = GetCodedDouble(3),
            EstablishmentGrammarAtHigherStandard = GetCodedDouble(4),
            LAGrammarAtExpectedStandard = GetCodedDouble(7),
            LAGrammarAtHigherStandard = GetCodedDouble(8),
            EstablishmentEHCPPopulation = CodedDouble.Empty,
            EnglandEHCPPopulation = new CodedDouble(null, "Redacted for confidentiality", "c"),
            EstablishmentSENSupportPopulation = new CodedDouble(null, "Not applicable", "z"),
            EnglandSENSupportPopulation = new CodedDouble(null, "Not available", "x"),

            EstablishmentNumPupilsEndOfKS2 = GetCodedDouble(10),
            LANumPupilsEndOfKS2 = GetCodedDouble(11),
            EnglandNumPupilsEndOfKS2 = GetCodedDouble(12),
            EstablishmentNumGirlsEndOfKS2 = GetCodedDouble(13),
            EstablishmentNumBoysEndOfKS2 = GetCodedDouble(14),
            EstablishmentNumEALEndOfKS2 = GetCodedDouble(15),
            EstablishmentNumNonMobileEndOfKS2 = GetCodedDouble(16),
            EstablishmentNumDisadvantagedEndOfKS2 = GetCodedDouble(17),

            LANumDisadvantagedEndOfKS2 = GetCodedDouble(18),
            EnglandNumDisadvantagedEndOfKS2 = GetCodedDouble(19),
            LANumNonDisadvantagedEndOfKS2 = GetCodedDouble(20),
            EnglandNumNonDisadvantagedEndOfKS2 = GetCodedDouble(21),


            EstablishmentPupilTotal = "22",
            EnglandPupilTotal = GetCodedDouble(23)

        };

        _ks2AdditionalMeasuresService
            .Setup(s => s.GetAdditionalMeasures(_urn, _establishment.LAId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(modelWithNoData);

        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("ehcp-population-table", 1, 0));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("ehcp-population-table", 1, 1));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("sen-population-table", 1, 0));
        Assert.Contains("Not available", doc.GetTableCellContentByIdAndIndex("sen-population-table", 1, 1));
    }

    [Fact]
    public async Task AdditionalMeasuresPage_DisplaysBottomPagination_WithCorrectDestinations()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/primary-performance/subject-scaled-scores", previousLink.GetAttribute("href"));

        Assert.Null(nextLink);
    }

    [Fact]
    public async Task AdditionalMeasuresPage_DisplaysBottomPagination_WithCorrectDestinations_WhenMultiplePhases()
    {
        // Arrange
        var multiPhaseEstablishment = new EstablishmentMinimumTestBuilder()
            .WithURN(_urn)
            .WithEstablishmentName($"School{_urn}")
            .WithIsKeyStage2(true)
            .WithIsKeyStage4(true)
            .WithLAName(_laName)
            .WithLAId(_laId)
            .BuildServiceModel();


        _mockEstablishmentService
           .Setup(a => a.GetEstablishmentMinimumAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(multiPhaseEstablishment);

        var url = BuildUrl(multiPhaseEstablishment.URN, multiPhaseEstablishment.EstablishmentName, _pageRoute);

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var pagination = doc.QuerySelector("nav.govuk-pagination");
        Assert.NotNull(pagination);

        var previousLink = pagination.QuerySelector(".govuk-pagination__prev a");
        var nextLink = pagination.QuerySelector(".govuk-pagination__next a");

        Assert.NotNull(previousLink);
        Assert.Contains("/primary-performance/subject-scaled-scores", previousLink.GetAttribute("href"));

        Assert.NotNull(nextLink);
        Assert.Contains("/secondary-performance/progress-attainment", nextLink.GetAttribute("href"));
    }

    [Fact]
    public async Task AdditionalMeasures_DisplaysNumberOfPupilsAtEndOfKS2()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();
        var eoKS2TableSelector = "pupils-eoks2-table";

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var table = doc.QuerySelector($"#{eoKS2TableSelector}");
        Assert.NotNull(table);

        Assert.Equal("Pupil group", doc.GetTableHeaderContentByIdAndIndex(eoKS2TableSelector, 0, 0));
        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex(eoKS2TableSelector, 0, 1));
        Assert.Equal(_laName, doc.GetTableHeaderContentByIdAndIndex(eoKS2TableSelector, 0, 2));
        Assert.Equal("England", doc.GetTableHeaderContentByIdAndIndex(eoKS2TableSelector, 0, 3));

        Assert.Equal("Number of pupils at the end of KS2", doc.GetTableHeaderContentByIdAndIndex(eoKS2TableSelector, 1, 0));
        Assert.Equal(expectedModel.EstablishmentNumPupilsEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(eoKS2TableSelector, 1, 0));
        Assert.Equal(expectedModel.LANumPupilsEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(eoKS2TableSelector, 1, 1));
        Assert.Equal(expectedModel.EnglandNumPupilsEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(eoKS2TableSelector, 1, 2));
    }

    [Fact]
    public async Task AdditionalMeasures_ShowsPupilCharacteristicsBreakdown()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();
        var detailsSelector = "pupils-by-characteristics";
        var tableSelector = "ks2-population-breakdown-table";

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var details = doc.QuerySelector($"#{detailsSelector}");
        var table = doc.QuerySelector($"#{tableSelector}");
        Assert.NotNull(details);
        Assert.NotNull(table);

        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 0, 1));
        Assert.Equal("Girls", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 1, 0));
        Assert.Equal(expectedModel.EstablishmentNumGirlsEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelector, 1, 0));
        Assert.Equal("Boys", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 2, 0));
        Assert.Equal(expectedModel.EstablishmentNumBoysEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelector, 2, 0));
        Assert.Equal("English as an additional language (EAL)", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 3, 0));
        Assert.Equal(expectedModel.EstablishmentNumEALEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelector, 3, 0));
        Assert.Equal("Non-mobile pupils", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 4, 0));
        Assert.Equal(expectedModel.EstablishmentNumNonMobileEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelector, 4, 0));
    }

    [Fact]
    public async Task AdditionalMeasures_ShowsDisadvantagedAndNonDisadvantangedPupilTables()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();
        var disadvantagedSectionSelector = "disadvantaged-pupils-section";
        var disadvantagedTableSelector = "disadvantaged-pupils-population-table";
        var nonDisadvantagedDetailsSelector = "details-compare-non-disadvantaged-pupils-explained";
        var nonDisadvantagedTableSelector = "non-disadvantaged-pupils-compare-table";

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var disadvantagedSection = doc.QuerySelector($"#{disadvantagedSectionSelector}");
        var disadvantagedTable = doc.QuerySelector($"#{disadvantagedTableSelector}");
        var nonDisadvantagedDetails = doc.QuerySelector($"#{nonDisadvantagedDetailsSelector}");
        var nonDisadvantagedTable = doc.QuerySelector($"#{nonDisadvantagedTableSelector}");

        Assert.NotNull(disadvantagedSection);
        Assert.NotNull(disadvantagedTable);
        Assert.NotNull(nonDisadvantagedDetails);
        Assert.NotNull(nonDisadvantagedTable);

        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex(disadvantagedTableSelector, 0, 1));
        Assert.Equal(_laName, doc.GetTableHeaderContentByIdAndIndex(disadvantagedTableSelector, 0, 2));
        Assert.Equal("England", doc.GetTableHeaderContentByIdAndIndex(disadvantagedTableSelector, 0, 3));
        Assert.Equal(expectedModel.EstablishmentNumDisadvantagedEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(disadvantagedTableSelector, 1, 0));
        Assert.Equal(expectedModel.LANumDisadvantagedEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(disadvantagedTableSelector, 1, 1));
        Assert.Equal(expectedModel.EnglandNumDisadvantagedEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(disadvantagedTableSelector, 1, 2));

        Assert.Equal(_laName, doc.GetTableHeaderContentByIdAndIndex(nonDisadvantagedTableSelector, 0, 1));
        Assert.Equal("England", doc.GetTableHeaderContentByIdAndIndex(nonDisadvantagedTableSelector, 0, 2));
        Assert.Equal(expectedModel.LANumNonDisadvantagedEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(nonDisadvantagedTableSelector, 1, 0));
        Assert.Equal(expectedModel.EnglandNumNonDisadvantagedEndOfKS2!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(nonDisadvantagedTableSelector, 1, 1));

    }

    [Fact]
    public async Task AdditionalMeasures_DisplaysWholeSchoolPupilPopulationTable()
    {
        // Arrange
        var url = BuildUrl(_establishment.URN, _establishment.EstablishmentName, _pageRoute);
        var expectedModel = GetKS2AdditionalMeasuresModel();
        var tableSelector = "whole-school-population-table";

        // Act
        var doc = await Fixture.BrowseToPage(url);

        // Assert
        var table = doc.QuerySelector($"#{tableSelector}");

        Assert.NotNull(table);
        
        Assert.Equal("Pupil group", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 0, 0));
        Assert.Equal("School", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 0, 1));
        Assert.Equal("England mainstream schools", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 0, 2));

        Assert.Equal("Number of pupils on roll", doc.GetTableHeaderContentByIdAndIndex(tableSelector, 1, 0));
        Assert.Equal(expectedModel.EstablishmentPupilTotal, doc.GetTableCellContentByIdAndIndex(tableSelector, 1, 0));
        Assert.Equal(expectedModel.EnglandPupilTotal!.Value.ToString(), doc.GetTableCellContentByIdAndIndex(tableSelector, 1, 1));
    }


    private static CodedDouble GetCodedDouble(double val) => new(val, string.Empty, val.ToString());

    private static KS2AdditionalMeasuresModel GetKS2AdditionalMeasuresModel()
    {
        return new KS2AdditionalMeasuresModel
        {
            EnglandGrammarAtExpectedStandard = GetCodedDouble(1),
            EnglandGrammarAtHigherStandard = GetCodedDouble(2),
            EnglandEHCPPopulation = GetCodedDouble(3),
            EnglandSENSupportPopulation = GetCodedDouble(4),
            EstablishmentGrammarAtExpectedStandard = GetCodedDouble(3),
            EstablishmentGrammarAtHigherStandard = GetCodedDouble(4),
            EstablishmentEHCPPopulation = GetCodedDouble(5),
            EstablishmentSENSupportPopulation = GetCodedDouble(6),
            LAGrammarAtExpectedStandard = GetCodedDouble(7),
            LAGrammarAtHigherStandard = GetCodedDouble(8),
            EstablishmentNumPupilsEndOfKS2 = GetCodedDouble(10),
            LANumPupilsEndOfKS2 = GetCodedDouble(11),
            EnglandNumPupilsEndOfKS2 = GetCodedDouble(12),
            EstablishmentNumGirlsEndOfKS2 = GetCodedDouble(13),
            EstablishmentNumBoysEndOfKS2 = GetCodedDouble(14),
            EstablishmentNumEALEndOfKS2 = GetCodedDouble(15),
            EstablishmentNumNonMobileEndOfKS2 = GetCodedDouble(16),
            EstablishmentNumDisadvantagedEndOfKS2 = GetCodedDouble(17),

            LANumDisadvantagedEndOfKS2 = GetCodedDouble(18),
            EnglandNumDisadvantagedEndOfKS2 = GetCodedDouble(19),
            LANumNonDisadvantagedEndOfKS2 = GetCodedDouble(20),
            EnglandNumNonDisadvantagedEndOfKS2 = GetCodedDouble(21),
            EstablishmentPupilTotal = "22",
            EnglandPupilTotal = GetCodedDouble(23)
        };
    }
}
