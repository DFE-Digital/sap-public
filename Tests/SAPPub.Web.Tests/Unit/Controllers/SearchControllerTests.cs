using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search;
using SAPPub.Web.Controllers;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class SearchControllerTests
{
    private readonly SearchController _controller;
    private readonly Mock<ISchoolSearchService> _mockSchoolSearchService = new();
    private readonly SchoolSearchResultServiceModel _schoolSearchResult1 = new SchoolSearchResultServiceModel
    {
        URN = "123456",
        EstablishmentName = "Test School",
        Address = "123 Test Street",
        GenderName = "Mixed",
        ReligiousCharacterName = "None"
    };
    private readonly SchoolSearchResultServiceModel _schoolSearchResult2 = new SchoolSearchResultServiceModel
    {
        URN = "223456",
        EstablishmentName = "A Test School 2",
        Address = "123 Test Street 2",
        GenderName = "Female",
        ReligiousCharacterName = "Muslim"
    };

    public SearchControllerTests()
    {
        _controller = new SearchController(_mockSchoolSearchService.Object);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    [Fact]
    public async Task Get_SearchResults_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var searchTerm = "test school";
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchTerm)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 2,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel> {
                _schoolSearchResult1, _schoolSearchResult2
            }
        });

        // act
        var result = await _controller.SearchResults(searchTerm);
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchTerm, viewModel.NameSearchTerm);
        Assert.Equal(2, viewModel.SearchResultsCount);
        Assert.Collection(viewModel.SearchResults,
            item =>
            {
                Assert.Equal(_schoolSearchResult1.URN, item.URN);
                Assert.Equal(_schoolSearchResult1.EstablishmentName, item.EstablishmentName);
                Assert.Equal(_schoolSearchResult1.Address, item.Address);
                Assert.Equal(_schoolSearchResult1.GenderName, item.GenderName);
                Assert.Equal(_schoolSearchResult1.ReligiousCharacterName, item.ReligiousCharacter);
            },
            item =>
            {
                Assert.Equal(_schoolSearchResult2.URN, item.URN);
                Assert.Equal(_schoolSearchResult2.EstablishmentName, item.EstablishmentName);
                Assert.Equal(_schoolSearchResult2.Address, item.Address);
                Assert.Equal(_schoolSearchResult2.GenderName, item.GenderName);
                Assert.Equal(_schoolSearchResult2.ReligiousCharacterName, item.ReligiousCharacter);
            });
    }

    [Fact]
    public async Task Get_SearchResults_NoResults_ReturnsExpectedSchoolResultsViewModel()
    {
        // arrange
        var searchTerm = "no results term";
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchTerm)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 0,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel>()
        });

        // act
        var result = await _controller.SearchResults(searchTerm);
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchTerm, viewModel.NameSearchTerm);
        Assert.Equal(0, viewModel.SearchResultsCount);
        Assert.Empty(viewModel.SearchResults);
    }
}
