using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.PostcodeLookup;
using SAPPub.Core.ServiceModels.Search;
using SAPPub.Core.Services.Search;
using SearchQuery = SAPPub.Core.ServiceModels.Search.SearchQuery;

namespace SAPPub.Core.Tests.Services.Search;

public class SchoolSearchServiceTests
{
    private readonly Mock<ISchoolSearchIndexReader> _mockIndexReader = new();
    private readonly Mock<IPostcodeLookupService> _mockPostcodeLookupService = new();
    private readonly SchoolSearchDocument searchResult1 = new SchoolSearchDocument(
                    URN: "123456",
                    EstablishmentName: "Test School",
                    Address: "123 Test Street",
                    GenderName: "Mixed",
                    ReligiousCharacterName: "None");
    private readonly SchoolSearchDocument searchResult2 = new SchoolSearchDocument(
                    URN: "223456",
                    EstablishmentName: "A Test School 2",
                    Address: "123 Test Street 2",
                    GenderName: "Girls",
                    ReligiousCharacterName: "Muslim");

    [Fact]
    public async Task SearchAsync_ByName_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchQuery = new SearchQuery() { Name = "test school" };
        _mockIndexReader.Setup(r => r.SearchAsync(
            It.Is<ServiceModels.PostcodeLookup.SearchQuery>(q => q.Name == searchQuery.Name),
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
    public async Task SearchAsync_ByPostcodeLocation_ReturnsExpectedServiceModel()
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
            It.Is<ServiceModels.PostcodeLookup.SearchQuery>(q => q.Latitude == searchLatitude && q.Longitude == searchLongitude && q.Distance == searchQuery.Distance),
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
    public async Task SearchAsync_ByPostcodeLocation_InvalidPostcode_ReturnsExpectedStatus()
    {
        // Arrange
        var searchPostcode = "invalid";
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = 5 };
        _mockPostcodeLookupService.Setup(p => p.GetLatitudeAndLongitudeAsync(searchQuery.Location))
            .ReturnsAsync(new PostcodeResponseModel
            {
                Status = 404,
                Error = "Invalid postcode"
            });

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
            It.Is<ServiceModels.PostcodeLookup.SearchQuery>(q => q.Name == searchQuery.Name),
            It.IsAny<int>()))
            .ReturnsAsync(new SchoolSearchResults(
                Count: 1,
                Results: new List<SchoolSearchDocument> {
                new SchoolSearchDocument(null, null, null, null, null) }));

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