using Moq;
using SAPPub.Core.Entities;
using SAPPub.Core.Interfaces.Repositories;
using SAPPub.Core.ServiceModels.Search.InputModels;
using SAPPub.Infrastructure.PostgresSearch;

namespace SAPPub.Infrastructure.Tests.PostgresSearch
{
    public class PostgresSearchIndexReaderTests
    {
        private readonly Mock<IEstablishmentRepository> _mockRepo;
        private readonly PostgresSchoolSearchIndexReader _sut;

        private readonly Establishment FakeEstablishmentOne = new()
        {
            URN = "1",
            UKPRN = "2",
            LAId = "3",
            EstablishmentNumber = "4",
            EstablishmentName = "Fake School One"
        };
        private readonly Establishment FakeEstablishmentTwo = new()
        {
            URN = "10",
            UKPRN = "20",
            LAId = "30",
            EstablishmentNumber = "40",
            EstablishmentName = "Fake School Two"
        };
    

        public PostgresSearchIndexReaderTests()
        {
            _mockRepo = new Mock<IEstablishmentRepository>();
            _sut = new PostgresSchoolSearchIndexReader(_mockRepo.Object);
        }
   

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public async Task SearchAsync_ReturnsEmpty_ForNullOrWhiteSpace(string? input)
        {
            _mockRepo.Setup(r => r.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<int>(), default))
                .ReturnsAsync((Enumerable.Empty<Establishment>(), 0));

            var results = await _sut.SearchAsync(new SearchQuery { Name = input! });

            Assert.Empty(results.Results);
        }

        [Fact]
        public async Task SearchAsync_PartialWords_returns_AllMatches()
        {
            var establishments = new List<Establishment>
            {
                FakeEstablishmentOne,
                FakeEstablishmentTwo
            };

            _mockRepo.Setup(r => r.SearchAsync(It.IsAny<SearchQuery>(), It.IsAny<int>(), default))
                .ReturnsAsync((establishments, establishments.Count));

            var result = await _sut.SearchAsync(new SearchQuery { Name = "fa" });

            Assert.Equal(2, result.Count);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 4)]
        public async Task SearchAsync_LocationAndDistance_ReturnsResults(int distance, int expectedCount)
        {
            var establishments = new List<Establishment>();
            for (int i = 0; i < expectedCount; i++)
            {
                establishments.Add(new Establishment { URN = (1000 + i).ToString(), EstablishmentName = $"School {i}" });
            }

            _mockRepo.Setup(r => r.SearchAsync(It.Is<SearchQuery>(q => q.Distance == distance), It.IsAny<int>(), default))
                .ReturnsAsync((establishments, establishments.Count));

            var result = await _sut.SearchAsync(new SearchQuery { Distance = distance, Latitude = 54.979671f, Longitude = -1.611639f });

            Assert.Equal(expectedCount, result.Count);
        }

        [Theory]
        [InlineData("School", 1, 2)]
        [InlineData("School", 3, 3)]
        [InlineData("Academy", 3, 1)]
        [InlineData("Academy", 1, 0)]
        [InlineData("non-present name", 1, 0)]
        public async Task SearchAsync_NameAndLocation_ReturnsResults(string searchName, int distance, int expectedCount)
        {
            var establishments = new List<Establishment>();
            for (int i = 0; i < expectedCount; i++)
            {
                establishments.Add(new Establishment { URN = (2000 + i).ToString(), EstablishmentName = $"{searchName} {i}" });
            }

            _mockRepo.Setup(r => r.SearchAsync(It.Is<SearchQuery>(q => q.Name == searchName && q.Distance == distance), It.IsAny<int>(), default))
                .ReturnsAsync((establishments, establishments.Count));

            var result = await _sut.SearchAsync(new SearchQuery { Name = searchName, Distance = distance, Latitude = 54.979671f, Longitude = -1.611639f });

            Assert.Equal(expectedCount, result.Count);
        }
    }
}