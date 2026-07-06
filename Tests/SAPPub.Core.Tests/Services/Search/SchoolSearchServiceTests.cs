using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Extensions;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.PostcodeSearch;
using SAPPub.Core.ServiceModels.Search;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Core.Services.Search;
using SearchQuery = SAPPub.Core.ServiceModels.Search.InputModels.SchoolSearchServiceQuery;
using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Tests.Services.Search;

public class SchoolSearchServiceTests
{
    private readonly Mock<ISchoolSearchIndexReader> _mockSchoolSearchIndexReader = new();
    private readonly Mock<IPostcodeLookupService> _mockPostcodeLookupService = new();

    private readonly EstablishmentServiceModel establishment1 = new()
    {
        URN = "123456",
        EstablishmentName = "Test School",
        AddressStreet = "123 Test Street",
        GenderName = "Mixed",
        ReligiousCharacterName = "None"
    };

    private readonly EstablishmentServiceModel establishment2 = new()
    {
        URN = "223456",
        EstablishmentName = "A Test School 2",
        AddressStreet = "123 Test Street 2",
        GenderName = "Girls",
        ReligiousCharacterName = "Muslim"
    };

    private List<EstablishmentServiceModel> CreateSearchResults(int count)
    {
        var results = new List<EstablishmentServiceModel>();
        for (int i = 1; i <= count; i++)
        {
            var establishment = new EstablishmentServiceModel
            {
                URN = i.ToString(),
                EstablishmentName = $"A Test School {i}",
                AddressStreet = $"123 Test Street {i}",
                GenderName = "Girls",
                ReligiousCharacterName = "None"
            };
            results.Add(establishment);
        }
        return results;
    }

    private SchoolSearchResults ToSchoolSearchResults(List<EstablishmentServiceModel> establishments)
    {
        return new SchoolSearchResults(
            Count: establishments.Count,
            Results: establishments.Select(e => e.ToSchoolSearchDocument()).ToList()
        );
    }

    [Fact]
    public async Task SearchAsync_ByName_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchQuery = new SearchQuery() { Name = "test school", PageNumber = 1 };
        var establishments = new List<EstablishmentServiceModel> { establishment1, establishment2 };
        var schoolSearchResults = ToSchoolSearchResults(establishments);

        _mockSchoolSearchIndexReader
            .Setup(s => s.SearchAsync(It.IsAny<ServiceModels.Search.InputModels.SearchQuery>(), Constants.PageSize))
            .ReturnsAsync(schoolSearchResults);

        var service = new SchoolSearchService(_mockSchoolSearchIndexReader.Object, _mockPostcodeLookupService.Object);

        // Act
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(2, result.PagedResponse.TotalRecords);
        Assert.Equal(2, result.PagedResponse.PagerInfo.TotalItems);
        Assert.Equal(searchQuery.PageNumber, result.PagedResponse.PagerInfo.CurrentPage);
        Assert.Equal(Constants.PageSize, result.PagedResponse.PagerInfo.PageSize);
        Assert.Equal(1, result.PagedResponse.PagerInfo.TotalPages);
        Assert.Collection(result.PagedResponse.Records,
            item =>
            {
                Assert.Equal(establishment1.URN, item.URN);
                Assert.Equal(establishment1.EstablishmentName, item.EstablishmentName);
                Assert.Equal(establishment1.Address, item.Address);
                Assert.Equal(establishment1.GenderName, item.GenderName);
                Assert.Equal(establishment1.ReligiousCharacterName, item.ReligiousCharacterName);
            },
            item =>
            {
                Assert.Equal(establishment2.URN, item.URN);
                Assert.Equal(establishment2.EstablishmentName, item.EstablishmentName);
                Assert.Equal(establishment2.Address, item.Address);
                Assert.Equal(establishment2.GenderName, item.GenderName);
                Assert.Equal(establishment2.ReligiousCharacterName, item.ReligiousCharacterName);
            });
    }

    [Theory]
    [InlineData(10, 1)]
    [InlineData(20, 2)]
    [InlineData(25, 3)]
    [InlineData(100, 10)]
    public async Task SearchAsync_ByName_ReturnsExpectedServiceModel_With_Pagination(int records, int expectedPages)
    {
        // Arrange
        var expectedSearchResults = CreateSearchResults(records);
        var searchQuery = new SearchQuery() { Name = "test", PageNumber = 1 };
        var schoolSearchResults = ToSchoolSearchResults(expectedSearchResults);

        _mockSchoolSearchIndexReader
            .Setup(s => s.SearchAsync(It.IsAny<SAPPub.Core.ServiceModels.Search.InputModels.SearchQuery>(), Constants.PageSize))
            .ReturnsAsync(schoolSearchResults);

        var service = new SchoolSearchService(_mockSchoolSearchIndexReader.Object, _mockPostcodeLookupService.Object);

        // Act
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(expectedSearchResults.Count, result.PagedResponse.TotalRecords);
        Assert.Equal(expectedSearchResults.Count, result.PagedResponse.PagerInfo.TotalItems);
        Assert.Equal(searchQuery.PageNumber, result.PagedResponse.PagerInfo.CurrentPage);
        Assert.Equal(Constants.PageSize, result.PagedResponse.PagerInfo.PageSize);
        Assert.Equal(expectedPages, result.PagedResponse.PagerInfo.TotalPages);
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
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = distance, PageNumber = 1 };
        var allEstablishments = new List<EstablishmentServiceModel>
        {
            new EstablishmentServiceModel { URN = "123456", EstablishmentName = "Test School", AddressStreet = "123 Test Street", GenderName = "Mixed", ReligiousCharacterName = "None" },
            new EstablishmentServiceModel { URN = "223456", EstablishmentName = "A Test School 2", AddressStreet = "123 Test Street 2", GenderName = "Girls", ReligiousCharacterName = "Muslim" }
        };
        var filteredEstablishments = allEstablishments.Where(e => expectedUrns.Contains(e.URN)).ToList();
        var schoolSearchResults = ToSchoolSearchResults(filteredEstablishments);

        _mockPostcodeLookupService.Setup(p => p.GetLatitudeAndLongitudeAsync(searchQuery.Location))
            .ReturnsAsync(new PostcodeResponseModel
            {
                Status = 200,
                Result = new PostcodeResultModel { Latitude = searchLatitude, Longitude = searchLongitude, Postcode = searchQuery.Location }
            });

        _mockSchoolSearchIndexReader
            .Setup(s => s.SearchAsync(It.Is<SAPPub.Core.ServiceModels.Search.InputModels.SearchQuery>(
                q => q.Latitude == searchLatitude && q.Longitude == searchLongitude && q.Distance == distance), Constants.PageSize))
            .ReturnsAsync(schoolSearchResults);

        var service = new SchoolSearchService(_mockSchoolSearchIndexReader.Object, _mockPostcodeLookupService.Object);

        // Act
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(expectedUrns.Length, result.PagedResponse.TotalRecords);
        Assert.Equal(expectedUrns.Length, result.PagedResponse.PagerInfo.TotalItems);
        Assert.Equal(searchQuery.PageNumber ?? 1, result.PagedResponse.PagerInfo.CurrentPage);
        Assert.Equal(Constants.PageSize, result.PagedResponse.PagerInfo.PageSize);
        var resultUrns = result.PagedResponse.Records.Select(r => r.URN).ToArray();
        Assert.Equal(expectedUrns, resultUrns);
    }

    [Fact]
    public async Task SearchAsync_ByPostcodeLocationNotRecognised_ErrorReturnedByService_ReturnsExpectedStatus()
    {
        // Arrange
        var searchPostcode = "NE2 3RR";
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = 5, PageNumber = 1 };
        _mockPostcodeLookupService.Setup(p => p.GetLatitudeAndLongitudeAsync(searchQuery.Location))
            .ReturnsAsync(new PostcodeResponseModel
            {
                Status = 404,
                Error = "Postcode not found"
            });

        var service = new SchoolSearchService(_mockSchoolSearchIndexReader.Object, _mockPostcodeLookupService.Object);

        // Act
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.PostcodeNotFound, result.Status);
        Assert.Equal(0, result.PagedResponse.TotalRecords);
        Assert.Equal(0, result.PagedResponse.PagerInfo.TotalItems);
        Assert.Equal(searchQuery.PageNumber ?? 1, result.PagedResponse.PagerInfo.CurrentPage);
        Assert.Equal(Constants.PageSize, result.PagedResponse.PagerInfo.PageSize);
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
        var searchQuery = new SearchQuery() { Location = searchPostcode, Distance = 5, PageNumber = 1 };

        var service = new SchoolSearchService(_mockSchoolSearchIndexReader.Object, _mockPostcodeLookupService.Object);

        // Act
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.InvalidPostcode, result.Status);
        Assert.Equal(0, result.PagedResponse.TotalRecords);
        Assert.Equal(0, result.PagedResponse.PagerInfo.TotalItems);
        Assert.Equal(searchQuery.PageNumber ?? 1, result.PagedResponse.PagerInfo.CurrentPage);
        Assert.Equal(Constants.PageSize, result.PagedResponse.PagerInfo.PageSize);
    }

    [Fact]
    public async Task SearchAsync_ResultsFieldsNull_ReturnsExpectedServiceModel()
    {
        // Arrange
        var searchQuery = new SearchQuery() { Name = "test school", PageNumber = 1 };
        var establishments = new List<EstablishmentServiceModel> { new EstablishmentServiceModel() };
        var schoolSearchResults = ToSchoolSearchResults(establishments);

        _mockSchoolSearchIndexReader
            .Setup(s => s.SearchAsync(It.IsAny<SAPPub.Core.ServiceModels.Search.InputModels.SearchQuery>(), Constants.PageSize))
            .ReturnsAsync(schoolSearchResults);

        var service = new SchoolSearchService(_mockSchoolSearchIndexReader.Object, _mockPostcodeLookupService.Object);

        // Act
        var result = await service.SearchAsync(searchQuery);

        // Assert
        Assert.Equal(SchoolSearchStatus.Success, result.Status);
        Assert.Equal(1, result.PagedResponse.PagerInfo.TotalItems);
        var singleResult = Assert.Single(result.PagedResponse.Records);
        Assert.True(string.IsNullOrEmpty(singleResult.URN));
        Assert.True(string.IsNullOrEmpty(singleResult.EstablishmentName));
        Assert.True(string.IsNullOrEmpty(singleResult.Address));
        Assert.True(string.IsNullOrEmpty(singleResult.GenderName));
        Assert.True(string.IsNullOrEmpty(singleResult.ReligiousCharacterName));
    }
}