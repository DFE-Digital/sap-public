using Dapper;
using Microsoft.Extensions.Logging;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using SAPPub.Infrastructure.Repositories.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public sealed class DapperRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<DapperRepository<T>> _logger;
        private readonly ICodedValueMapper _codedValueMapper;

        public DapperRepository(
            IDbConnection connection,
            ILogger<DapperRepository<T>> logger,
            ICodedValueMapper codedValueMapper)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _codedValueMapper = codedValueMapper ?? throw new ArgumentNullException(nameof(codedValueMapper));
        }

        public Task<T?> ReadAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Task.FromResult<T?>(default);

            // Convention: "Id" parameter name; feature repos can call ReadSingleAsync for non-Id keys.
            return ReadSingleAsync(new { Id = id }, ct);
        }

        public async Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default)
        {
            try
            {
                var sql = DapperHelpers.GetReadMultiple(typeof(T));
                if (string.IsNullOrWhiteSpace(sql))
                    throw new NotSupportedException($"No ReadMultiple query for {typeof(T).Name}");

                var cmd = new CommandDefinition(
                    commandText: sql,
                    parameters: null,
                    transaction: null,
                    commandTimeout: null,
                    commandType: CommandType.Text,
                    flags: CommandFlags.Buffered,
                    cancellationToken: ct);

                var items = (await _connection.QueryAsync<T>(cmd)).ToList();

                _codedValueMapper.Apply(items); // map *_Coded -> numeric + _Reason

                return items;
            }
            catch (OperationCanceledException)
            {
                // Let cancellations propagate (don’t log as error).
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed ReadAllAsync for {Type}", typeof(T).Name);
                return Enumerable.Empty<T>();
            }
        }

        public async Task<T?> ReadSingleAsync(object parameters, CancellationToken ct = default)
        {
            if (parameters is null)
                return default;

            try
            {
                var sql = DapperHelpers.GetReadSingle(typeof(T));
                if (string.IsNullOrWhiteSpace(sql))
                    throw new NotSupportedException($"No ReadSingle query for {typeof(T).Name}");

                var cmd = new CommandDefinition(
                    commandText: sql,
                    parameters: parameters,
                    transaction: null,
                    commandTimeout: null,
                    commandType: CommandType.Text,
                    flags: CommandFlags.Buffered,
                    cancellationToken: ct);

                var item = await _connection.QuerySingleOrDefaultAsync<T>(cmd);

                if (item is not null)
                    _codedValueMapper.Apply(item);

                return item;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // NOTE: don't interpolate params into the message; pass as a structured property.
                _logger.LogError(ex, "Failed ReadSingleAsync for {Type} params={Params}", typeof(T).Name, parameters);
                return default;
            }
        }

        // Not in your target interface, but you still need it to support feature repos calling "many".
        // Keep it as a public method if existing code uses it; otherwise make it internal/private and
        // expose via feature repos only.
        public async Task<IEnumerable<T>> ReadManyAsync(object parameters, CancellationToken ct = default)
        {
            if (parameters is null)
                return Enumerable.Empty<T>();

            try
            {
                var sql = DapperHelpers.GetReadMany(typeof(T));
                if (string.IsNullOrWhiteSpace(sql))
                    throw new NotSupportedException($"No ReadMany query for {typeof(T).Name}");

                var cmd = new CommandDefinition(
                    commandText: sql,
                    parameters: parameters,
                    transaction: null,
                    commandTimeout: null,
                    commandType: CommandType.Text,
                    flags: CommandFlags.Buffered,
                    cancellationToken: ct);

                var items = (await _connection.QueryAsync<T>(cmd)).ToList();

                _codedValueMapper.Apply(items);

                return items;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed ReadManyAsync for {Type} params={Params}", typeof(T).Name, parameters);
                return Enumerable.Empty<T>();
            }
        }
    }
}
