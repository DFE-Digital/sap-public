using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.Core.Entities;
using SAPPub.Core.Entities.Destinations;
using SAPPub.Core.Entities.Gateway;
using SAPPub.Core.Entities.KS4.Absence;
using SAPPub.Core.Entities.KS4.Performance;
using SAPPub.Core.Entities.Performance;
using SAPPub.Core.Interfaces.Repositories.Generic;
using SAPPub.Infrastructure.Mapping.ValueCodes;
using SAPPub.Infrastructure.Repositories.Helpers;
using System.Data.Common;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public class JSONRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _filePath;
        private readonly ILogger<JSONRepository<T>> _logger;

        private readonly ICodedValueMapper _codedValueMapper;
        private readonly JsonSerializerSettings _jsonSettings;

        public JSONRepository(ILogger<JSONRepository<T>> logger,
            ICodedValueMapper codedValueMapper,
            IReasonCodeLookup reasonCodeLookup)
        {
            _logger = logger ?? throw new ArgumentNullException();
            var basePath = AppContext.BaseDirectory;
            _filePath = Path.Combine(basePath, "Data", "Files");
            _codedValueMapper = codedValueMapper ?? throw new ArgumentNullException(nameof(codedValueMapper));
            if (reasonCodeLookup is null)
                throw new ArgumentNullException(nameof(reasonCodeLookup));
            _jsonSettings = new JsonSerializerSettings
            {
                Converters =
                {
                    new CodedDoubleJsonConverter(reasonCodeLookup),
                    new CodedStringJsonConverter(reasonCodeLookup)
                }
            };
        }

        private async Task<string?> ReadFileAsync(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(_filePath, $"{fileName}.json");
                var fileContent = await File.ReadAllTextAsync(fullPath, Encoding.UTF8);
                return fileContent; 
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
                var data = await ReadFileAsync(typeof(T).Name).ConfigureAwait(false);

                var items = JsonConvert.DeserializeObject<IEnumerable<T>>(data ?? "[]", _jsonSettings) ?? Enumerable.Empty<T>();

                if (items.Any())
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
                var data = await ReadFileAsync(typeof(T).Name).ConfigureAwait(false);

                var items = JsonConvert.DeserializeObject<IEnumerable<T>>(data ?? "[]", _jsonSettings) ?? Enumerable.Empty<T>();

                var item = await Filter(typeof(T), items, parameters).ConfigureAwait(false);

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
                var fileData = await ReadFileAsync(typeof(T).Name);
                var items = JsonConvert.DeserializeObject<IEnumerable<T>>(fileData ?? "[]", _jsonSettings) ?? Enumerable.Empty<T>();

                if (items != null)
                {
                    return items;
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

        private static async Task<T?> Filter(Type entityType, IEnumerable<T> items, object parameters)
        {
            return entityType.Name switch
            {
                nameof(Establishment) => items.FirstOrDefault(i => ((Establishment)(object)i).URN == ((dynamic)parameters).Id),
                nameof(EstablishmentAbsence) => items.FirstOrDefault(i => ((EstablishmentAbsence)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS4EstablishmentDestinations) => items.FirstOrDefault(i => ((KS4EstablishmentDestinations)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS5EstablishmentDestinations) => items.FirstOrDefault(i => ((KS4EstablishmentDestinations)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS2EstablishmentPerformance) => items.FirstOrDefault(i => ((KS2EstablishmentPerformance)(object)i).Id == ((dynamic)parameters).Id),
                nameof(EstablishmentPerformance) => items.FirstOrDefault(i => ((EstablishmentPerformance)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS5EstablishmentPerformance) => items.FirstOrDefault(i => ((KS5EstablishmentPerformance)(object)i).Id == ((dynamic)parameters).Id),

                nameof(LAAbsence) => items.FirstOrDefault(i => ((LAAbsence)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS2LAPerformance) => items.FirstOrDefault(i => ((KS2LAPerformance)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS5LAPerformance) => items.FirstOrDefault(i => ((KS5LAPerformance)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS4LADestinations) => items.FirstOrDefault(i => ((KS4LADestinations)(object)i).Id == ((dynamic)parameters).Id),
                nameof(KS5LADestinations) => items.FirstOrDefault(i => ((KS5LADestinations)(object)i).Id == ((dynamic)parameters).Id),
                nameof(LAPerformance) => items.FirstOrDefault(i => ((LAPerformance)(object)i).Id == ((dynamic)parameters).Id),

                nameof(EnglandAbsence) => items.FirstOrDefault(),
                nameof(KS4EnglandDestinations) => items.FirstOrDefault(),
                nameof(KS5EnglandDestinations) => items.FirstOrDefault(),
                nameof(KS2EnglandPerformance) => items.FirstOrDefault(),
                nameof(KS5EnglandPerformance) => items.FirstOrDefault(),
                nameof(EnglandPerformance) => items.FirstOrDefault(),

                nameof(LaUrls) => items.FirstOrDefault(i => ((LaUrls)(object)i).Id == ((dynamic)parameters).Id),
                _ => default(T)
            };
        }
    }
}
