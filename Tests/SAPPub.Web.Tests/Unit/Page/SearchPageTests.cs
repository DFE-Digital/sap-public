using AngleSharp;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SAPPub.Core.Interfaces.Services;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Common;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Core.ServiceModels.Search.Results;
using SAPPub.Web.Tests.Unit.Page.Infrastructure;


namespace SAPPub.Web.Tests.Unit.Page;

public class SearchPageTests : IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly Mock<ISchoolSearchService> _searchServiceMock;

    public SearchPageTests()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        var accessor = _factory.Services.GetRequiredService<MockAccessor<ISchoolSearchService>>();
        _searchServiceMock = accessor.GetOrCreate();

        var establishmentComparisionServiceAccessor = _factory.Services.GetRequiredService<MockAccessor<IEstablishmentComparisonService>>();
        _= establishmentComparisionServiceAccessor.GetOrCreate();        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Search_ReturnsResults()
    {
        // Arrange
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 2,
                Records = new List<SchoolSearchResultServiceModel>
                {
                    new SchoolSearchResultServiceModel
                    {
                        URN = "123456",
                        EstablishmentName = "Test School",
                        Address = "123 Test Street"
                    },
                    new SchoolSearchResultServiceModel
                    {
                        URN = "654321",
                        EstablishmentName = "Another School",
                        Address = "456 Another Street"
                    }
                },
                PagerInfo = new Pager(2, 1, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?NameSearchTerm=test");
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML with AngleSharp
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        var results = document.QuerySelectorAll(".school-search-result");
        Assert.True(results.Length >= 2, "Expected at least two search results.");
        Assert.Contains("Test School", results[0].TextContent);
        Assert.Contains("Another School", results[1].TextContent);

        // Pagination should NOT be present if all results fit on one page
        var pagination = document.QuerySelector(".govuk-pagination");
        Assert.Null(pagination);
    }
    [Fact]
    public async Task Search_WithManyResults_ShowsPagination()
    {
        // Arrange: 15 results, page size 10, so 2 pages
        var records = new List<SchoolSearchResultServiceModel>();
        for (int i = 1; i <= 15; i++)
        {
            records.Add(new SchoolSearchResultServiceModel
            {
                URN = $"{100000 + i}",
                EstablishmentName = $"School {i}",
                Address = $"{i} Test Street"
            });
        }
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 15,
                Records = records,
                PagerInfo = new Pager(15, 1, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?NameSearchTerm=school");
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML with AngleSharp
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        var results = document.QuerySelectorAll(".school-search-result");
        Assert.Equal(15, results.Length);

        // Pagination should be present
        var pagination = document.QuerySelector(".govuk-pagination");
        Assert.NotNull(pagination);
    }

    [Fact]
    public async Task Search_Pagination_Present_On_Page1Of2()
    {
        // Arrange: 15 results, page size 10, page 1
        var records = new List<SchoolSearchResultServiceModel>();
        for (int i = 1; i <= 10; i++)
        {
            records.Add(new SchoolSearchResultServiceModel
            {
                URN = $"{100000 + i}",
                EstablishmentName = $"School {i}",
                Address = $"{i} Test Street"
            });
        }
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 15,
                Records = records,
                PagerInfo = new Pager(15, 1, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?NameSearchTerm=school&PageNumber=1");
        var html = await response.Content.ReadAsStringAsync();
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        var pagination = document.QuerySelector(".govuk-pagination");
        Assert.NotNull(pagination);
        // No previous link on first page
        var prev = document.QuerySelector(".govuk-pagination__prev");
        Assert.Null(prev);
        // Next link should be present
        var next = document.QuerySelector(".govuk-pagination__next");
        Assert.NotNull(next);
        // Page numbers
        var pageItems = document.QuerySelectorAll(".govuk-pagination__item");
        Assert.Contains(pageItems, item => item.TextContent.Trim() == "1");
        Assert.Contains(pageItems, item => item.TextContent.Trim() == "2");
    }

    [Fact]
    public async Task Search_Pagination_NoNextLink_OnLastPage()
    {
        // Arrange: 15 results, page size 10, page 2 (last page)
        var records = new List<SchoolSearchResultServiceModel>();
        for (int i = 11; i <= 15; i++)
        {
            records.Add(new SchoolSearchResultServiceModel
            {
                URN = $"{100000 + i}",
                EstablishmentName = $"School {i}",
                Address = $"{i} Test Street"
            });
        }
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 15,
                Records = records,
                PagerInfo = new Pager(15, 2, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?NameSearchTerm=school&PageNumber=2");
        var html = await response.Content.ReadAsStringAsync();
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        var pagination = document.QuerySelector(".govuk-pagination");
        Assert.NotNull(pagination);
        // Previous link should be present
        var prev = document.QuerySelector(".govuk-pagination__prev");
        Assert.NotNull(prev);
        // No next link on last page
        var next = document.QuerySelector(".govuk-pagination__next");
        Assert.Null(next);
        // Page numbers
        var pageItems = document.QuerySelectorAll(".govuk-pagination__item");
        Assert.Contains(pageItems, item => item.TextContent.Trim() == "1");
        Assert.Contains(pageItems, item => item.TextContent.Trim() == "2");
    }
    [Fact]
    public async Task Search_ReturnsNoResults()
    {
        // Arrange
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.Success,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 0,
                Records = new List<SchoolSearchResultServiceModel>(),
                PagerInfo = new Pager(0, 1, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?NameSearchTerm=none");       
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML with AngleSharp
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        // No results message should be present
        var noResults = document.QuerySelector("[data-testid='no-results-heading']");
        Assert.NotNull(noResults);
        Assert.Contains("Try another search", noResults.TextContent, StringComparison.OrdinalIgnoreCase);

        // There should be no .school-search-result elements
        var results = document.QuerySelectorAll(".school-search-result");
        Assert.Empty(results);

        // Pagination should not be present
        var pagination = document.QuerySelector(".govuk-pagination");
        Assert.Null(pagination);
    }

    [Fact]
    public async Task Search_InvalidQuery_ShowsError()
    {
        // Arrange
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.InvalidPostcode,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 0,
                Records = new List<SchoolSearchResultServiceModel>(),
                PagerInfo = new Pager(0, 1, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?LocationSearchTerm=invalidpostcode");
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML with AngleSharp
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        // Error message should be present
        Assert.Contains("Enter a full postcode", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Search_PostcodeServiceError_ReturnsNoResults()
    {
        // Arrange
        var expectedResults = new SchoolSearchResultsServiceModel
        {
            Status = SchoolSearchStatus.PostcodeServiceError,
            PagedResponse = new PagedResponse<SchoolSearchResultServiceModel>
            {
                TotalRecords = 0,
                Records = new List<SchoolSearchResultServiceModel>(),
                PagerInfo = new Pager(0, 1, 10)
            }
        };

        _searchServiceMock
            .Setup(s => s.SearchAsync(It.IsAny<SchoolSearchServiceQuery>()))
            .ReturnsAsync(expectedResults);

        // Act
        var response = await _client.GetAsync("/search/results?LocationSearchTerm=validnotfoundpostcode");
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML with AngleSharp
        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(html));

        // Assert
        response.EnsureSuccessStatusCode();
        // No results message should be present
        var noResults = document.QuerySelector("[data-testid='no-results-heading']");
        Assert.NotNull(noResults);
        Assert.Contains("Try another search", noResults.TextContent, StringComparison.OrdinalIgnoreCase);

        // There should be no .school-search-result elements
        var results = document.QuerySelectorAll(".school-search-result");
        Assert.Empty(results);

        // Pagination should not be present
        var pagination = document.QuerySelector(".govuk-pagination");
        Assert.Null(pagination);
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
        _searchServiceMock.Reset(); 
    }
}