using SAPPub.Core.Entities;
using SAPPub.Core.ServiceModels.PostcodeLookup;
using SAPPub.Infrastructure.LuceneSearch;

namespace SAPPub.Infrastructure.Tests.LuceneSearch;

public class LuceneIndexReaderTests
{
    private readonly LuceneSchoolSearchIndexWriter _writer;
    private readonly LuceneSchoolSearchIndexReader _sut;

    private Establishment FakeEstablishmentOne = new()
    {
        URN = "1",
        UKPRN = "2",
        LAId = "3",
        EstablishmentNumber = "4",
        EstablishmentName = "Fake School One"
    };
    private Establishment FakeEstablishmentTwo = new()
    {
        URN = "10",
        UKPRN = "20",
        LAId = "30",
        EstablishmentNumber = "40",
        EstablishmentName = "Fake School Two"
    };
    private Establishment FakeEstablishmentThree = new()
    {
        URN = "15",
        UKPRN = "25",
        LAId = "35",
        EstablishmentNumber = "45",
        EstablishmentName = "Saint Fake School Three"
    };

    private const string SearchPostcode = "NE1 8QH";
    private const float SearchLat = 54.979671f;
    private const float SearchLon = -1.611639f;
    private List<Establishment> Within1MileOfPostcode => new()
    {
        new Establishment
        {
            URN = "108493",
            EstablishmentName = "Christ Church CofE Primary School",
            Easting = "425532",
            Northing = "564589",
        },
        new Establishment
        {
            URN = "148271",
            EstablishmentName = "St Catherine's Catholic Primary School",
            Easting = "426042",
            Northing = "565547"
        }
    };

    private List<Establishment> Between1And3MilesOfPostcode => new()
    {
        new Establishment
        {
            URN = "146752",
            EstablishmentName = "Jesmond Park Academy",
            Easting = "426403",
            Northing = "566700"
        },
        new Establishment
        {
            URN = "144271",
            EstablishmentName = "Benfield School",
            Easting = "428294",
            Northing = "566235"
        }
    };

    public LuceneIndexReaderTests()
    {
        var ctx = new LuceneIndexContext();
        _writer = new LuceneSchoolSearchIndexWriter(ctx);
        var tokeniser = new LuceneTokeniser(ctx);
        _sut = new LuceneSchoolSearchIndexReader(ctx, tokeniser);
    }

    [Fact]
    public async Task SearchAsync_Finds_Items_With_Abbreviation_Expansion_And_Tokenization()
    {
        // Arrange: build an index with two schools
        _writer.AddToIndex([
            FakeEstablishmentOne,
            FakeEstablishmentTwo,
            FakeEstablishmentThree
        ]);

        // Act: search using the abbreviation 'St' that should expand to 'saint'
        var results = await _sut.SearchAsync(new SearchQuery() { Name = "St Fake Three" });

        // Assert
        Assert.NotNull(results);
        Assert.NotEmpty(results.Results);
        Assert.Equal(FakeEstablishmentThree.URN, results.Results.First().URN);
        Assert.Contains(FakeEstablishmentThree.EstablishmentName, results.Results.First().EstablishmentName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public async Task SearchAsync_ReturnsEmpty_ForNullOrWhiteSpace(string? input)
    {
        _writer.AddToIndex([
            FakeEstablishmentOne,
            FakeEstablishmentTwo
        ]);

        var results = await _sut.SearchAsync(new SearchQuery() { Name = input! });

        Assert.Empty(results.Results);
    }

    [Fact]
    public async Task SearchAsync_PartialWords_returns_AllMatches()
    {
        const string Input = "fa";

        _writer.AddToIndex([
            FakeEstablishmentOne,
            FakeEstablishmentTwo,
        ]);

        var result = await _sut.SearchAsync(new SearchQuery() { Name = Input });

        Assert.Equal(2, result.Count);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(3, 4)]
    public async Task SearchAsync_LocationAndDistance_ReturnsResults(int distance, int expectedCount)
    {
        var establishmentsInIndex = Between1And3MilesOfPostcode.Concat(this.Within1MileOfPostcode).ToList();
        _writer.AddToIndex(establishmentsInIndex);

        var result = await _sut.SearchAsync(new SearchQuery() { Distance = distance, Latitude = SearchLat, Longitude = SearchLon });

        Assert.Equal(expectedCount, result.Count);
    }

    [Theory]
    [InlineData("School", 1, 2)]
    [InlineData("School", 3, 3)]
    [InlineData("Academy", 3, 1)]
    [InlineData("Academy", 1, 0)]
    [InlineData("non-present name", 1, 0)]
    public async Task SearchAsync_NameAndLocation_ReturnsResults(string searchName, int distance, int expectedCount)
    {
        var establishmentsInIndex = Between1And3MilesOfPostcode.Concat(this.Within1MileOfPostcode).ToList();
        _writer.AddToIndex(establishmentsInIndex);

        var result = await _sut.SearchAsync(new SearchQuery() { Distance = distance, Latitude = SearchLat, Longitude = SearchLon, Name = searchName });

        Assert.Equal(expectedCount, result.Count);
    }
}
