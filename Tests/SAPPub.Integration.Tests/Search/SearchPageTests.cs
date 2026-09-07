using SAPPub.Playwright.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAPPub.Integration.Tests.Search
{
    [Collection("Integration Tests")]
    public class SearchPageTests() : BasePageTest()
    {
        private static readonly string _pageRoot = "/search/results";

        [Fact]
        public async Task SimpleSearch_LoadsSuccessfully()
        {
            // Act
            var response = await Page.GotoAsync($"{_pageRoot}?NameSearchTerm=Ark");

            // Assert
            var resultsHeading = Page.GetByTestId("search-results-heading");
            var noResultsId = Page.GetByTestId("no-results-heading");

            Assert.NotNull(response);
            Assert.Equal(200, response.Status);
            Assert.NotNull(resultsHeading);
            Assert.Contains("40", await resultsHeading.First.InnerTextAsync());
            Assert.Null(noResultsId.Description);
        }

        [Theory]
        [InlineData("3", "12")]
        [InlineData("5", "18")]
        [InlineData("10", "23")]
        public async Task SimpleSearch_WithLocation_AndDistance_LoadsSuccessfully(string distance, string expectedCount)
        {
            // Act
            var response = await Page.GotoAsync($"{_pageRoot}?NameSearchTerm=Ark&LocationSearchTerm=W12%207AJ&Distance={distance}");

            // Assert
            var resultsHeading = Page.GetByTestId("search-results-heading");
            var noResultsId = Page.GetByTestId("no-results-heading");

            Assert.NotNull(response);
            Assert.Equal(200, response.Status);
            Assert.NotNull(resultsHeading);
            Assert.Contains(expectedCount, await resultsHeading.First.InnerTextAsync());
            Assert.Null(noResultsId.Description);
        }

        [Theory]
        [InlineData("Primary", "25")]
        [InlineData("Secondary", "23")]
        [InlineData("16 to 19", "18")]
        [InlineData("All-through", "8")]
        public async Task SimpleSearch_WithPhaseFilter_LoadsSuccessfully(string phase, string expectedCount)
        {
            // Act
            var response = await Page.GotoAsync($"{_pageRoot}?NameSearchTerm=Ark&phase={phase}");

            // Assert
            var resultsHeading = Page.GetByTestId("search-results-heading");
            var noResultsId = Page.GetByTestId("no-results-heading");

            Assert.NotNull(response);
            Assert.Equal(200, response.Status);
            Assert.NotNull(resultsHeading);
            Assert.Contains(expectedCount, await resultsHeading.First.InnerTextAsync());
            Assert.Null(noResultsId.Description);
        }

        [Theory]
        [InlineData("Academy")]
        [InlineData("Maintained school")]
        [InlineData("Independent schools")]
        [InlineData("Special school")]
        [InlineData("College")]
        public async Task SimpleSearch_WithTypeFilter_LoadsSuccessfully(string type)
        {
            // Act
            var response = await Page.GotoAsync($"{_pageRoot}?NameSearchTerm=school&schoolType={type}");

            // Assert
            var resultsHeading = Page.GetByTestId("search-results-heading");
            var noResultsId = Page.GetByTestId("no-results-heading");

            Assert.NotNull(response);
            Assert.Equal(200, response.Status);
            Assert.NotNull(resultsHeading);
            Assert.DoesNotContain("0 results", await resultsHeading.First.InnerTextAsync());
            Assert.NotNull(noResultsId);
        }
    }
}
    