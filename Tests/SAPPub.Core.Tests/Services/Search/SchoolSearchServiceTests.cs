using Moq;
using SAPPub.Core.Entities.SchoolSearch;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.Services.Search;

namespace SAPPub.Core.Tests.Services.Search;

public class SchoolSearchServiceTests
{
    private readonly Mock<ISchoolSearchIndexReader> _mockIndexReader = new();
    private readonly SchoolSearchResult searchResult1 = new SchoolSearchResult(URN: "123456",
                    EstablishmentName: "Test School",
                    Address: "123 Test Street",
                    GenderName: "Mixed",
                    ReligiousCharacterName: "None");
    private readonly SchoolSearchResult searchResult2 = new SchoolSearchResult(URN: "223456",
                    EstablishmentName: "A Test School 2",
                    Address: "123 Test Street 2",
                    GenderName: "Girls",
                    ReligiousCharacterName: "Muslim");

    [Fact]
    public async Task SearchAsync_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchTerm = "test school";
        _mockIndexReader.Setup(r => r.SearchAsync(searchTerm, It.IsAny<int>())).ReturnsAsync(new SchoolSearchResults(
            Count: 2,
            Results: new List<SchoolSearchResult> { searchResult1, searchResult2 }));

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object);
        var result = await service.SearchAsync(searchTerm);

        // Assert
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
    public async Task SearchAsync_SearchFieldsNull_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchTerm = "test school";
        _mockIndexReader.Setup(r => r.SearchAsync(searchTerm, It.IsAny<int>())).ReturnsAsync(new SchoolSearchResults(
            Count: 1,
            Results: new List<SchoolSearchResult> {
                new SchoolSearchResult(null, null, null, null, null)
            }));

        // Act
        var service = new SchoolSearchService(_mockIndexReader.Object);
        var result = await service.SearchAsync(searchTerm);

        // Assert
        Assert.Equal(1, result.Count);
        var singleResult = Assert.Single(result.SchoolSearchResults);
        Assert.Null(singleResult.URN);
        Assert.Null(singleResult.EstablishmentName);
        Assert.Null(singleResult.Address);
        Assert.Null(singleResult.GenderName);
        Assert.Null(singleResult.ReligiousCharacterName);
    }
}