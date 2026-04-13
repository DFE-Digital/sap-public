using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
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
        ReligiousCharacterName = "None",
        Distance = 0.1,
        StatusCode = 1
    };
    private readonly SchoolSearchResultServiceModel _schoolSearchResult2 = new SchoolSearchResultServiceModel
    {
        URN = "223456",
        EstablishmentName = "A Test School 2",
        Address = "123 Test Street 2",
        GenderName = "Female",
        ReligiousCharacterName = "Muslim",
        Distance = 0.4,
        StatusCode = 2,
        ClosedDate = new DateOnly(2026, 01, 01)
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
    public async Task Get_SearchResults_SearchByName_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Name = "test school" };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 2,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel> {
                _schoolSearchResult1, _schoolSearchResult2
            }
        });

        // act
        var result = await _controller.SearchResults(new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location });
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(2, viewModel.SearchResultsCount);
        Assert.Collection(viewModel.SearchResults,
            item =>
            {
                Assert.Equal(_schoolSearchResult1.URN, item.URN);
                Assert.Equal(_schoolSearchResult1.EstablishmentName, item.EstablishmentName);
                Assert.Equal(_schoolSearchResult1.Address, item.Address);
                Assert.Equal(_schoolSearchResult1.GenderName, item.GenderName);
                Assert.Equal(_schoolSearchResult1.ReligiousCharacterName, item.ReligiousCharacter);
                Assert.Equal(_schoolSearchResult1.StatusCode, item.StatusCode);
                Assert.Null(_schoolSearchResult1.ClosedDate);
            },
            item =>
            {
                Assert.Equal(_schoolSearchResult2.URN, item.URN);
                Assert.Equal(_schoolSearchResult2.EstablishmentName, item.EstablishmentName);
                Assert.Equal(_schoolSearchResult2.Address, item.Address);
                Assert.Equal(_schoolSearchResult2.GenderName, item.GenderName);
                Assert.Equal(_schoolSearchResult2.ReligiousCharacterName, item.ReligiousCharacter);
                Assert.Equal(_schoolSearchResult2.StatusCode, item.StatusCode);                
                Assert.Equal(_schoolSearchResult2.ClosedDate!.Value, item.ClosedDate.Value);
                Assert.True(item.ClosedDate.IsAvailable);
            });
    }

    [Fact]
    public async Task Get_SearchResults_SearchByValidLocation_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Location = "NE2 1VF", Distance = 3 };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 2,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel> {
                _schoolSearchResult1, _schoolSearchResult2
            }
        });

        // act
        var result = await _controller.SearchResults(new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location });
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
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
    public async Task Get_SearchResults_SearchByValidLocation_ReturnsSchoolResultsOrderedDistanceAscending()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Location = "NE2 1VF", Distance = 3 };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 2,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel> {
                _schoolSearchResult2, _schoolSearchResult1
            }
        });

        // act
        var result = await _controller.SearchResults(new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location });
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(2, viewModel.SearchResultsCount);
        Assert.Equal(_schoolSearchResult1.URN, viewModel.SearchResults[0].URN);
        Assert.Equal(_schoolSearchResult2.URN, viewModel.SearchResults[1].URN);
    }

    [Fact]
    public async Task Get_SearchResults_SearchByInvalidPostcode_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var location = "invalid";
        var searchQuery = new SchoolSearchServiceQuery() { Location = location, Distance = 3 };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 1,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel>(),
            Status = SchoolSearchStatus.InvalidPostcode
        });


        // act
        var viewResult = await _controller.SearchResults(new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location }) as ViewResult;

        // assert
        Assert.NotNull(viewResult);
        Assert.False(viewResult.ViewData.ModelState.IsValid);
    }

    [Fact]
    public async Task Get_SearchResults_NullFieldsInResults_ReturnsSchoolResultsViewModelWithEmptyFields()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Name = "test school" };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 1,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel> {
                new SchoolSearchResultServiceModel
                {
                    URN = null,
                    EstablishmentName = null,
                    Address = null,
                    GenderName = null,
                    ReligiousCharacterName = null
                }
            }
        });

        // act
        var result = await _controller.SearchResults(new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location });
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(1, viewModel.SearchResultsCount);
        var item = Assert.Single(viewModel.SearchResults);
        Assert.Equal(string.Empty, item.URN);
        Assert.Equal(string.Empty, item.EstablishmentName);
        Assert.Equal(string.Empty, item.Address);
        Assert.Equal(string.Empty, item.GenderName);
        Assert.Equal(string.Empty, item.ReligiousCharacter);
    }

    [Fact]
    public async Task Get_SearchResults_NoResults_ReturnsExpectedSchoolResultsViewModel()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Name = "no results term" };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            Count = 0,
            SchoolSearchResults = new List<SchoolSearchResultServiceModel>()
        });

        // act
        var result = await _controller.SearchResults(new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location });
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(0, viewModel.SearchResultsCount);
        Assert.Empty(viewModel.SearchResults);
    }
}
