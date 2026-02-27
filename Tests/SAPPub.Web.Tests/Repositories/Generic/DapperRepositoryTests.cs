using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Npgsql;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using SAPPub.Infrastructure.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SAPPub.Infrastructure.Tests.Repositories.Generic
{
    public class DapperRepositoryTests
    {
        // A type that (very likely) has NO SQL mapping in DapperHelpers,
        // so DapperHelpers.GetReadSingle/GetReadMany/GetReadMultiple returns empty
        // -> NotSupportedException -> caught -> log error -> return default/empty.
        public sealed class UnmappedEntity
        {
            public string? Id { get; set; }
        }

        private static NpgsqlDataSource CreateSafeDataSource()
        {
            // Safe because our tests below do not actually open a connection
            // (we target early return / missing SQL branches).
            return NpgsqlDataSource.Create("Host=127.0.0.1;Port=1;Username=x;Password=x;Database=x;Timeout=1;Command Timeout=1");
        }

        private static DapperRepository<UnmappedEntity> CreateSut(
            NpgsqlDataSource? dataSource = null,
            Mock<ILogger<DapperRepository<UnmappedEntity>>>? logger = null,
            Mock<ICodedValueMapper>? mapper = null)
        {
            return new DapperRepository<UnmappedEntity>(
                dataSource ?? CreateSafeDataSource(),
                (logger ?? new Mock<ILogger<DapperRepository<UnmappedEntity>>>()).Object,
                (mapper ?? new Mock<ICodedValueMapper>()).Object);
        }

        [Fact]
        public void Ctor_WithNullDataSource_Throws()
        {
            var logger = NullLogger<DapperRepository<UnmappedEntity>>.Instance;
            var mapper = new Mock<ICodedValueMapper>();

            Assert.Throws<ArgumentNullException>(() =>
                new DapperRepository<UnmappedEntity>(null!, logger, mapper.Object));
        }

        [Fact]
        public void Ctor_WithNullLogger_Throws()
        {
            var ds = CreateSafeDataSource();
            var mapper = new Mock<ICodedValueMapper>();

            Assert.Throws<ArgumentNullException>(() =>
                new DapperRepository<UnmappedEntity>(ds, null!, mapper.Object));
        }

        [Fact]
        public void Ctor_WithNullMapper_Throws()
        {
            var ds = CreateSafeDataSource();
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();

            Assert.Throws<ArgumentNullException>(() =>
                new DapperRepository<UnmappedEntity>(ds, logger.Object, null!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ReadAsync_WithNullOrWhitespaceId_ReturnsNull(string? id)
        {
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();
            var mapper = new Mock<ICodedValueMapper>();
            var sut = CreateSut(logger: logger, mapper: mapper);

            var result = await sut.ReadAsync(id!, CancellationToken.None);

            Assert.Null(result);

            // No DB call should happen; also no error logging expected.
            VerifyNoErrorLogs(logger);
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadSingleAsync_WithNullParameters_ReturnsNull_AndDoesNotLog()
        {
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();
            var mapper = new Mock<ICodedValueMapper>();
            var sut = CreateSut(logger: logger, mapper: mapper);

            var result = await sut.ReadSingleAsync(parameters: null, ct: CancellationToken.None);

            Assert.Null(result);

            VerifyNoErrorLogs(logger);
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadManyAsync_WithNullParameters_ReturnsEmpty_AndDoesNotLog()
        {
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();
            var mapper = new Mock<ICodedValueMapper>();
            var sut = CreateSut(logger: logger, mapper: mapper);

            var result = await sut.ReadManyAsync(parameters: null, ct: CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);

            VerifyNoErrorLogs(logger);
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadAllAsync_WhenNoSqlMapping_ReturnsEmpty_AndLogsError()
        {
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();
            var mapper = new Mock<ICodedValueMapper>();
            var sut = CreateSut(logger: logger, mapper: mapper);

            var result = await sut.ReadAllAsync(CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);

            VerifyErrorLoggedOnce(logger);
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadSingleAsync_WhenNoSqlMapping_ReturnsNull_AndLogsError()
        {
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();
            var mapper = new Mock<ICodedValueMapper>();
            var sut = CreateSut(logger: logger, mapper: mapper);

            // parameters is non-null so it enters try, hits missing SQL mapping, throws NotSupportedException, logs error.
            var result = await sut.ReadSingleAsync(new { Id = "123" }, CancellationToken.None);

            Assert.Null(result);

            VerifyErrorLoggedOnce(logger);
            mapper.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ReadManyAsync_WhenNoSqlMapping_ReturnsEmpty_AndLogsError()
        {
            var logger = new Mock<ILogger<DapperRepository<UnmappedEntity>>>();
            var mapper = new Mock<ICodedValueMapper>();
            var sut = CreateSut(logger: logger, mapper: mapper);

            var result = await sut.ReadManyAsync(new { Some = "Param" }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);

            VerifyErrorLoggedOnce(logger);
            mapper.VerifyNoOtherCalls();
        }

        private static void VerifyNoErrorLogs(Mock<ILogger<DapperRepository<UnmappedEntity>>> logger)
        {
            logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }

        private static void VerifyErrorLoggedOnce(Mock<ILogger<DapperRepository<UnmappedEntity>>> logger)
        {
            logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
