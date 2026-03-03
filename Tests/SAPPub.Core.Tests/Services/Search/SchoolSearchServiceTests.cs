using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.PostcodeSearch;
using SAPPub.Core.ServiceModels.Search;
using SAPPub.Core.Services.Search;
using SearchQuery = SAPPub.Core.ServiceModels.Search.SearchQuery;

namespace SAPPub.Core.Tests.Services.Search;

public class SchoolSearchServiceTests
{
    private readonly Mock<ISchoolSearchIndexReader> _mockIndexReader = new();
    private readonly Mock<IPostcodeLookupService> _mockPostcodeLookupService = new();
    private readonly SchoolSearchDocument searchResult1 = new SchoolSearchDocument()
    {
        URN = "123456",
        EstablishmentName = "Test School",
        Address = "123 Test Street",
        GenderName = "Mixed",
        ReligiousCharacterName = "None"
    }
    ;
    private readonly SchoolSearchDocument searchResult2 = new SchoolSearchDocument()
    {
        URN = "223456",
        EstablishmentName = "A Test School 2",
        Address = "123 Test Street 2",
        GenderName = "Girls",
        ReligiousCharacterName = "Muslim"
    };

    [Fact]
    public async Task SearchAsync_ByName_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchQuery = new SearchQuery() { Name = "test school" };
        _mockIndexReader.Setup(r => r.SearchAsync(
            It.Is<ServiceModels.PostcodeSearch.SearchQuery>(q => q.Name == searchQuery.Name),
            It.IsAny<int>()))
            .ReturnsAsync(new SchoolSearchResults(
                Count: 2,
                Results: new List<SchoolSearchDocument> { searchResult1, searchResult2 }));

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object, _mockPostcodeLookupService.Object);
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(2, result.Count);
        Assert.Collection(result.SchoolSearchResults,
            item =>
            {
                Assert.Equal(searchResult1.URN, item.URN);
                Assert.Equal(searchResult1.EstablishmentName, item.EstablishmentName);
                Assert.Equal(searchResult1.Address, item.Address);
                Assert.Equal(searchResult1.GenderName, item.GenderName);
                Assert.Equal(searchResult1.ReligiousCharacterName, item.ReligiousCharacterName);
            },
            item =>
            {
                Assert.Equal(searchResult2.URN, item.URN);
                Assert.Equal(searchResult2.EstablishmentName, item.EstablishmentName);
                Assert.Equal(searchResult2.Address, item.Address);
                Assert.Equal(searchResult2.GenderName, item.GenderName);
                Assert.Equal(searchResult2.ReligiousCharacterName, item.ReligiousCharacterName);
            });
    }

    [Fact]
    public async Task SearchAsync_ByValidPostcode_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchPostcode = "NE1 8QH";
        var searchLatitude = 54.9783f;
        var searchLongitude = -1.6174f;
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = 5 };
        _mockPostcodeLookupService.Setup(p => p.GetLatitudeAndLongitudeAsync(searchQuery.Location))
            .ReturnsAsync(new PostcodeResponseModel
            {
                Status = 200,
                Result = new PostcodeResultModel { Latitude = searchLatitude, Longitude = searchLongitude, Postcode = searchQuery.Location }
            });
        _mockIndexReader.Setup(r => r.SearchAsync(
            It.Is<ServiceModels.PostcodeSearch.SearchQuery>(q => q.Latitude == searchLatitude && q.Longitude == searchLongitude && q.Distance == searchQuery.Distance),
            It.IsAny<int>()))
            .ReturnsAsync(new SchoolSearchResults(
                Count: 2,
                Results: new List<SchoolSearchDocument> { searchResult1, searchResult2 }));

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object, _mockPostcodeLookupService.Object);
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(2, result.Count);
        Assert.Collection(result.SchoolSearchResults,
            item =>
            {
                Assert.Equal(searchResult1.URN, item.URN);
                Assert.Equal(searchResult1.EstablishmentName, item.EstablishmentName);
                Assert.Equal(searchResult1.Address, item.Address);
                Assert.Equal(searchResult1.GenderName, item.GenderName);
                Assert.Equal(searchResult1.ReligiousCharacterName, item.ReligiousCharacterName);
            },
            item =>
            {
                Assert.Equal(searchResult2.URN, item.URN);
                Assert.Equal(searchResult2.EstablishmentName, item.EstablishmentName);
                Assert.Equal(searchResult2.Address, item.Address);
                Assert.Equal(searchResult2.GenderName, item.GenderName);
                Assert.Equal(searchResult2.ReligiousCharacterName, item.ReligiousCharacterName);
            });
    }

    [Theory]
    [InlineData(1, new string[] { "123456" })]
    [InlineData(3, new string[] { "123456", "223456" })]
    public async Task SearchAsync_ByValidPostcodeAndDistance_ReturnsExpectedServiceModel(int distance, string[] expectedUrns)
    {
        // Arrange
        var searchPostcode = "NE1 8QH";
        var searchLatitude = 54.9783f;
        var searchLongitude = -1.6174f;
        var latLonWithin1mile = (54.97750081131553f, -1.6004802554195086f);
        var latLonWithin3Miles = (54.993756258737676f, -1.5913472019452886f);
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = distance };
        _mockPostcodeLookupService.Setup(p => p.GetLatitudeAndLongitudeAsync(searchQuery.Location))
            .ReturnsAsync(new PostcodeResponseModel
            {
                Status = 200,
                Result = new PostcodeResultModel { Latitude = searchLatitude, Longitude = searchLongitude, Postcode = searchQuery.Location }
            });
        _mockIndexReader.Setup(r => r.SearchAsync(
            It.Is<ServiceModels.PostcodeSearch.SearchQuery>(q => q.Latitude == searchLatitude && q.Longitude == searchLongitude && q.Distance == searchQuery.Distance),
            It.IsAny<int>()))
            .ReturnsAsync(new SchoolSearchResults(
                Count: 2,
                Results: new List<SchoolSearchDocument> {
                    searchResult1 with { Latitude = latLonWithin1mile.Item1, Longitude = latLonWithin1mile.Item2 },
                    searchResult2 with { Latitude = latLonWithin3Miles.Item1, Longitude = latLonWithin3Miles.Item2 } }));

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object, _mockPostcodeLookupService.Object);
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(expectedUrns.Length, result.SchoolSearchResults.Count);
        var resultUrns = result.SchoolSearchResults.Select(r => r.URN).ToArray();
        Assert.Equal(expectedUrns, resultUrns);
    }

    [Fact]
    public async Task SearchAsync_ByPostcodeLocationNotRecognised_ErrorReturnedByService_ReturnsExpectedStatus()
    {
        // Arrange
        var searchPostcode = "NE2 3RR";
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = 5 };
        _mockPostcodeLookupService.Setup(p => p.GetLatitudeAndLongitudeAsync(searchQuery.Location))
            .ReturnsAsync(new PostcodeResponseModel
            {
                Status = 404,
                Error = "Postcode not found"
            });

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object, _mockPostcodeLookupService.Object);
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.PostcodeNotFound, result.Status);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("NE1")]
    [InlineData("123")]
    [InlineData("!@#")]
    public async Task SearchAsync_ByInvalidPostcodeLocation_PostcodeFormatInvalid_ReturnsExpectedStatus(string invalidPostcode)
    {
        // Arrange
        var searchPostcode = invalidPostcode;
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = 5 };

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object, _mockPostcodeLookupService.Object);
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.InvalidPostcode, result.Status);
    }

    [Fact]
    public async Task SearchAsync_ResultsFieldsNull_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchQuery = new SearchQuery() { Name = "test school" };
        _mockIndexReader.Setup(r => r.SearchAsync(
            It.Is<ServiceModels.PostcodeSearch.SearchQuery>(q => q.Name == searchQuery.Name),
            It.IsAny<int>()))
            .ReturnsAsync(new SchoolSearchResults(
                Count: 1,
                Results: new List<SchoolSearchDocument> {
                new SchoolSearchDocument() }));

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object, _mockPostcodeLookupService.Object);
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(1, result.Count);
        var singleResult = Assert.Single(result.SchoolSearchResults);
        Assert.Null(singleResult.URN);
        Assert.Null(singleResult.EstablishmentName);
        Assert.Null(singleResult.Address);
        Assert.Null(singleResult.GenderName);
        Assert.Null(singleResult.ReligiousCharacterName);
    }
}