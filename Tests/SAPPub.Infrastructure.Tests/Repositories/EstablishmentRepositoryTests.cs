using Moq;
using Npgsql;
using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Core.Interfaces.Services.Search;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Infrastructure.Repositories;

namespace SAPPub.Infrastructure.Tests.Repositories
{
    public class EstablishmentRepositoryTests
    {
        private readonly Mock<IGenericRepository<Establishment>> _mockGenericRepo;
        private readonly Mock<ISearchVisibilityPolicy> _mockSearchVisibilityPolicy;
        private readonly EstablishmentRepository _sut;

        private static NpgsqlDataSource CreateSafeDataSource()
        {
            return NpgsqlDataSource.Create("Host=127.0.0.1;Port=1;Username=x;Password=x;Database=x;Timeout=1;Command Timeout=1");
        }

        public EstablishmentRepositoryTests()
        {
            _mockGenericRepo = new Mock<IGenericRepository<Establishment>>();
            _mockSearchVisibilityPolicy = new Mock<ISearchVisibilityPolicy>();
            _sut = new EstablishmentRepository(
                _mockGenericRepo.Object,
                CreateSafeDataSource(),
                _mockSearchVisibilityPolicy.Object);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ReturnsCorrectItemWhenUrnExists()
        {
            var urn = "123";
            var expected = new Establishment { URN = urn, EstablishmentName = "Found" };

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _sut.GetEstablishmentAsync(urn, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(urn, result.URN);
            Assert.Equal("Found", result.EstablishmentName);

            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEstablishmentAsync_ReturnsNullWhenUrnDoesNotExist()
        {
            var urn = "999";

            _mockGenericRepo
                .Setup(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Establishment?)null);

            var result = await _sut.GetEstablishmentAsync(urn, CancellationToken.None);

            Assert.Null(result);
            _mockGenericRepo.Verify(r => r.ReadAsync(urn, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void BuildSearchSqlParts_IncludeKs5True_DoesNotApplyKs5Filter()
        {
            var query = new SearchQuery { Name = "test" };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: true);

            Assert.DoesNotContain(EstablishmentRepository.Ks5VisibilityPredicate, parts.WhereClause, StringComparison.Ordinal);
        }

        [Fact]
        public void BuildSearchSqlParts_IncludeKs5False_AppliesKs5Filter()
        {
            var query = new SearchQuery { Name = "test" };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: false);

            Assert.Contains(EstablishmentRepository.Ks5VisibilityPredicate, parts.WhereClause, StringComparison.Ordinal);
        }

        [Fact]
        public void BuildSearchSqlParts_NameOnly_BuildsExpectedWhereAndOrder()
        {
            var query = new SearchQuery { Name = "academy" };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: true);

            Assert.Contains(@"""EstablishmentNameFTS"" @@ plainto_tsquery", parts.WhereClause, StringComparison.Ordinal);
            Assert.DoesNotContain(@"""geom"" IS NOT NULL", parts.WhereClause, StringComparison.Ordinal);
            Assert.Equal(@"""EstablishmentName"" ASC", parts.OrderBy);
            Assert.DoesNotContain(@"AS ""Distance""", parts.SelectFields, StringComparison.Ordinal);

            var names = parts.Parameters.ParameterNames.ToHashSet(StringComparer.Ordinal);
            Assert.Contains("searchTerm", names);
            Assert.DoesNotContain("lat", names);
            Assert.DoesNotContain("lng", names);
            Assert.DoesNotContain("distance", names);
        }

        [Fact]
        public void BuildSearchSqlParts_LocationOnly_BuildsExpectedWhereOrderSelectAndParameters()
        {
            var query = new SearchQuery
            {
                Latitude = 51.5074f,
                Longitude = -0.1278f,
                Distance = 10
            };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: true);

            Assert.Contains(@"""geom"" IS NOT NULL", parts.WhereClause, StringComparison.Ordinal);
            Assert.Contains(@"ST_DWithin(""geom""", parts.WhereClause, StringComparison.Ordinal);
            Assert.Equal(@"""Distance"" ASC, ""EstablishmentName"" ASC", parts.OrderBy);
            Assert.Contains(@"AS ""Distance""", parts.SelectFields, StringComparison.Ordinal);

            var names = parts.Parameters.ParameterNames.ToHashSet(StringComparer.Ordinal);
            Assert.Contains("lat", names);
            Assert.Contains("lng", names);
            Assert.Contains("distance", names);

            Assert.Equal(51.5074f, parts.Parameters.Get<float>("lat"), 6);
            Assert.Equal(-0.1278f, parts.Parameters.Get<float>("lng"), 6);
            Assert.Equal(MappingHelper.MilesToMeters(10), parts.Parameters.Get<double>("distance"), 6);
        }

        [Fact]
        public void BuildSearchSqlParts_NameAndLocation_BuildsCombinedFilters()
        {
            var query = new SearchQuery
            {
                Name = "academy",
                Latitude = 52.0f,
                Longitude = -1.0f,
                Distance = 5
            };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: false);

            Assert.Contains(@"""EstablishmentNameFTS"" @@ plainto_tsquery", parts.WhereClause, StringComparison.Ordinal);
            Assert.Contains(@"""geom"" IS NOT NULL", parts.WhereClause, StringComparison.Ordinal);
            Assert.Contains(@"ST_DWithin(""geom""", parts.WhereClause, StringComparison.Ordinal);
            Assert.Contains(EstablishmentRepository.Ks5VisibilityPredicate, parts.WhereClause, StringComparison.Ordinal);
            Assert.Equal(@"""Distance"" ASC, ""EstablishmentName"" ASC", parts.OrderBy);
        }

        [Fact]
        public void BuildSearchSqlParts_PageSizeNull_UsesMaxResults_AndOffsetUsesResolvedPageSize()
        {
            var query = new SearchQuery { Page = 2, PageSize = null };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: true);

            Assert.Equal(10, parts.Parameters.Get<int>("pageSize"));
            Assert.Equal(10, parts.Parameters.Get<int>("offset"));
        }

        [Fact]
        public void BuildSearchSqlParts_PageSizeAboveMax_ClampsToMaxResults()
        {
            var query = new SearchQuery { Page = 1, PageSize = 999 };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: true);

            Assert.Equal(10, parts.Parameters.Get<int>("pageSize"));
            Assert.Equal(0, parts.Parameters.Get<int>("offset"));
        }

        [Fact]
        public void BuildSearchSqlParts_PageSizeBelowOne_ClampsToOne()
        {
            var query = new SearchQuery { Page = 3, PageSize = 0 };

            var parts = EstablishmentRepository.BuildSearchSqlParts(query, maxResults: 10, includeKs5: true);

            Assert.Equal(1, parts.Parameters.Get<int>("pageSize"));
            Assert.Equal(2, parts.Parameters.Get<int>("offset"));
        }
    }
}