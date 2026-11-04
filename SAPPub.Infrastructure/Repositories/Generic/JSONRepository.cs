using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using SAPPub.Infrastructure.Repositories.Helpers;
using System.Data.Common;
using System.Text;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public class JSONRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _filePath;
        private readonly ILogger<JSONRepository<T>> _logger;

        private readonly ICodedValueMapper _codedValueMapper;

        public JSONRepository(ILogger<JSONRepository<T>> logger,
            ICodedValueMapper codedValueMapper)
        {
            _logger = logger ?? throw new ArgumentNullException();
            var basePath = AppContext.BaseDirectory;
            _filePath = Path.Combine(basePath, "Data", "Files");
            _codedValueMapper = codedValueMapper ?? throw new ArgumentNullException(nameof(codedValueMapper));
        }

        private async Task<T?> ReadandParseFileAsync(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(_filePath, $"{fileName}.json");
                var fileContent = await File.ReadAllTextAsync(fullPath, Encoding.UTF8);
                return JsonConvert.DeserializeObject<T>(fileContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Read file {fileName}! - {ex.Message}, {ex}");
                return null;
            }
        }

        public Task<T?> ReadAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Task.FromResult<T?>(default);

            // Convention: "Id" parameter name; feature repos can call ReadSingleAsync for non-Id keys.

            return ReadSingleAsync(new { Id = id }, ct);
        }

        public async Task<IEnumerable<T>> ReadPageAsync(int page, int take, CancellationToken ct = default)
        {
            var items = (await ReadAll()).Skip((page - 1) * take).Take(take);
            return items;
        }

        public async Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default)
        {
            try
            {
                var sql = DapperHelpers.GetReadMultiple(typeof(T));
                if (string.IsNullOrWhiteSpace(sql))
                    throw new NotSupportedException($"No ReadMultiple query for {typeof(T).Name}");

                using var conn = await _dataSource.OpenConnectionAsync(ct).ConfigureAwait(false);

                var cmd = new DapperCommandBuilder()
                    .WithCommandText(sql)
                    .Build(ct);

                var items = (await conn.QueryAsync<T>(cmd).ConfigureAwait(false)).ToList();

                if (items.Count > 0)
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
                var item = await ReadandParseFileAsync(typeof(T).Name).ConfigureAwait(false);

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
                // Keep params out of the formatted message; log as a structured property.
                _logger.LogError(ex, "Failed ReadSingleAsync for {Type} paramsType={ParamsType}", typeof(T).Name, parameters.GetType().Name);
                return default;
            }
        }


        public async Task<IEnumerable<T>> ReadAll()
        {
            try
            {
                var fileData = await ReadandParseFileAsync(typeof(T).Name);
                if (fileData != null)
                {
                    return [fileData];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to execute generic readall for {typeof(T).Name}! - {ex.Message}, {ex}");
            }

            return [];
        }

        Task<bool> IGenericRepository<T>.WriteAsync(object? writeObject, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericRepository<T>.UpdateAsync(object? updateObject, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReadManyAsync(object parameters, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

    }
}
