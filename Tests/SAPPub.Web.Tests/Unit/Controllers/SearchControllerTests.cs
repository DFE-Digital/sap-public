using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Constants;
using SAPPub.Web.Controllers;
using SAPPub.Web.Models.Search;

namespace SAPPub.Web.Tests.Unit.Controllers;

public class SearchControllerTests
{
    private readonly SearchController _controller;
    private readonly Mock<ISchoolSearchService> _mockSchoolSearchService = new();
    private readonly SchoolSearchResultServiceModel _schoolSearchResult1 = new()
    {
        URN = "123456",
        EstablishmentName = "Test School",
        Address = "123 Test Street",
        GenderName = "Mixed",
        ReligiousCharacterName = "None",
        Distance = 0.1,
        StatusCode = 1
    };
    private readonly SchoolSearchResultServiceModel _schoolSearchResult2 = new()
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

    private List<SchoolSearchResultServiceModel> CreateSearchResults(int count)
    {
        var results = new List<SchoolSearchResultServiceModel>();
        for (int i = 1; i <= count; i++)
        {
            var searchResult = new SchoolSearchResultServiceModel
            {
                URN = i.ToString(),
                EstablishmentName = $"A Test School {i}",
                Address = $"123 Test Street {1}",
                GenderName = "Girls",
                ReligiousCharacterName = "None",
                Distance = i,
            };
            results.Add(searchResult);
        }
        return results;
    }

    public SearchControllerTests()
    {
        _controller = new SearchController(_mockSchoolSearchService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task Get_SearchResults_SearchByName_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Name = "test school" };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            { 
                TotalRecords = 2,
                Records =
                [
                    _schoolSearchResult1,
                    _schoolSearchResult2
                ],
                PagerInfo = new Pager(2, 1, 10)
            }
        });

        var searchParamsModel = new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location };

        // act
        var result = await _controller.SearchResults(searchParamsModel);
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

        Assert.NotNull(viewModel.Pagination);
        Assert.Equal(2, viewModel.Pagination.PagerInfo.TotalItems);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.CurrentPage);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.TotalPages);
        Assert.Equal(RouteConstants.SearchResults, viewModel.Pagination.RouteName);
        Assert.Equal(2, viewModel.Pagination.RouteAttributes.Count);
        Assert.Equal(searchParamsModel.NameSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.NameSearchTerm)]);
        Assert.Equal(searchParamsModel.LocationSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.LocationSearchTerm)]);
    }

    [Theory]
    [InlineData(10, 1)]
    [InlineData(20, 2)]
    [InlineData(25, 3)]
    [InlineData(100, 10)]
    public async Task Get_SearchResults_SearchByName_ReturnsSchoolResultsViewModel_With_Pagination(int records, int expectedPages)
    {
        // arrange
        var expectedSearchResults = CreateSearchResults(records);
        var searchQuery = new SchoolSearchServiceQuery() { Name = "test school" };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = expectedSearchResults.Count,
                Records = expectedSearchResults,
                PagerInfo = new Pager(expectedSearchResults.Count, 1, 10)
            }
        });

        var searchParamsModel = new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location };

        // act
        var result = await _controller.SearchResults(searchParamsModel);
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(expectedSearchResults.Count, viewModel.SearchResultsCount);

        foreach (var searchResult in viewModel.SearchResults)
        {
            var expectedSearchResult = expectedSearchResults.FirstOrDefault(r => r.URN == searchResult.URN);
            Assert.NotNull(expectedSearchResult);

            Assert.Equal(expectedSearchResult.URN, searchResult.URN);
            Assert.Equal(expectedSearchResult.EstablishmentName, searchResult.EstablishmentName);
            Assert.Equal(expectedSearchResult.Address, searchResult.Address);
            Assert.Equal(expectedSearchResult.GenderName, searchResult.GenderName);
            Assert.Equal(expectedSearchResult.ReligiousCharacterName, searchResult.ReligiousCharacter);
            Assert.Equal(expectedSearchResult.StatusCode, searchResult.StatusCode);
            Assert.Null(expectedSearchResult.ClosedDate);
        }

        Assert.NotNull(viewModel.Pagination);
        Assert.Equal(expectedSearchResults.Count, viewModel.Pagination.PagerInfo.TotalItems);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.CurrentPage);
        Assert.Equal(expectedPages, viewModel.Pagination.PagerInfo.TotalPages);
        Assert.Equal(RouteConstants.SearchResults, viewModel.Pagination.RouteName);
        Assert.Equal(2, viewModel.Pagination.RouteAttributes.Count);
        Assert.Equal(searchParamsModel.NameSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.NameSearchTerm)]);
        Assert.Equal(searchParamsModel.LocationSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.LocationSearchTerm)]);
    }

    [Fact]
    public async Task Get_SearchResults_SearchByValidLocation_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Location = "NE2 1VF", Distance = 3 };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 2,
                Records =
                [
                    _schoolSearchResult1,
                    _schoolSearchResult2
                ],
                PagerInfo = new Pager(2, 1, 10)
            }
        });

        var searchParamsModel = new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location };

        // act
        var result = await _controller.SearchResults(searchParamsModel);
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

        Assert.NotNull(viewModel.Pagination);
        Assert.Equal(2, viewModel.Pagination.PagerInfo.TotalItems);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.CurrentPage);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.TotalPages);
        Assert.Equal(RouteConstants.SearchResults, viewModel.Pagination.RouteName);
        Assert.Equal(2, viewModel.Pagination.RouteAttributes.Count);
        Assert.Equal(searchParamsModel.NameSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.NameSearchTerm)]);
        Assert.Equal(searchParamsModel.LocationSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.LocationSearchTerm)]);
    }

    [Fact]
    public async Task Get_SearchResults_SearchByValidLocation_ReturnsSchoolResultsOrderedDistanceAscending()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Location = "NE2 1VF", Distance = 3 };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 2,
                Records =
                [
                    _schoolSearchResult2,
                    _schoolSearchResult1
                ],
                PagerInfo = new Pager(2, 1, 10)
            }            
        });
        var searchParamsModel = new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location };

        // act
        var result = await _controller.SearchResults(searchParamsModel);
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(2, viewModel.SearchResultsCount);
        Assert.Equal(_schoolSearchResult1.URN, viewModel.SearchResults[0].URN);
        Assert.Equal(_schoolSearchResult2.URN, viewModel.SearchResults[1].URN);

        Assert.NotNull(viewModel.Pagination);
        Assert.Equal(2, viewModel.Pagination.PagerInfo.TotalItems);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.CurrentPage);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.TotalPages);
        Assert.Equal(RouteConstants.SearchResults, viewModel.Pagination.RouteName);
        Assert.Equal(2, viewModel.Pagination.RouteAttributes.Count);
        Assert.Equal(searchParamsModel.NameSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.NameSearchTerm)]);
        Assert.Equal(searchParamsModel.LocationSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.LocationSearchTerm)]);
    }

    [Fact]
    public async Task Get_SearchResults_SearchByInvalidPostcode_ReturnsSchoolResultsViewModel()
    {
        // arrange
        var location = "invalid";
        var searchQuery = new SchoolSearchServiceQuery() { Location = location, Distance = 3 };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 0,
                Records = [],
                PagerInfo = new Pager(0, 1, 10)
            },
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
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {                
                Records =
                [
                    new SchoolSearchResultServiceModel
                    {
                        URN = null,
                        EstablishmentName = null,
                        Address = null,
                        GenderName = null,
                        ReligiousCharacterName = null
                    }
                ],
                PagerInfo = new Pager(1, 1, 10)
            }
        });
        var searchParamsModel = new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location };

        // act
        var result = await _controller.SearchResults(searchParamsModel);
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

        Assert.NotNull(viewModel.Pagination);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.TotalItems);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.CurrentPage);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.TotalPages);
        Assert.Equal(RouteConstants.SearchResults, viewModel.Pagination.RouteName);
        Assert.Equal(2, viewModel.Pagination.RouteAttributes.Count);
        Assert.Equal(searchParamsModel.NameSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.NameSearchTerm)]);
        Assert.Equal(searchParamsModel.LocationSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.LocationSearchTerm)]);
    }

    [Fact]
    public async Task Get_SearchResults_NoResults_ReturnsExpectedSchoolResultsViewModel()
    {
        // arrange
        var searchQuery = new SchoolSearchServiceQuery() { Name = "no results term" };
        _mockSchoolSearchService.Setup(s => s.SearchAsync(searchQuery)).ReturnsAsync(new SchoolSearchResultsServiceModel
        {
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                Records = [],
                PagerInfo = new Pager(0, 1, 10)
            }
        });
        var searchParamsModel = new SearchParamsModel() { NameSearchTerm = searchQuery.Name, LocationSearchTerm = searchQuery.Location };

        // act
        var result = await _controller.SearchResults(searchParamsModel);
        var viewModel = ((ViewResult)result).Model as SearchResultsViewModel;

        // assert
        Assert.NotNull(viewModel);
        Assert.Equal(searchQuery.Name, viewModel.SearchParams.NameSearchTerm);
        Assert.Equal(0, viewModel.SearchResultsCount);
        Assert.Empty(viewModel.SearchResults);

        Assert.NotNull(viewModel.Pagination);
        Assert.Equal(0, viewModel.Pagination.PagerInfo.TotalItems);
        Assert.Equal(1, viewModel.Pagination.PagerInfo.CurrentPage);
        Assert.Equal(0, viewModel.Pagination.PagerInfo.TotalPages);
        Assert.Equal(RouteConstants.SearchResults, viewModel.Pagination.RouteName);
        Assert.Equal(2, viewModel.Pagination.RouteAttributes.Count);
        Assert.Equal(searchParamsModel.NameSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.NameSearchTerm)]);
        Assert.Equal(searchParamsModel.LocationSearchTerm, viewModel.Pagination.RouteAttributes[nameof(searchParamsModel.LocationSearchTerm)]);
    }
}
