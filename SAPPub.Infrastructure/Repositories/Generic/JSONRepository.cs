using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Infrastructure.Repositories.Generic
{
    public class JSONRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly string _filePath;
        private readonly ILogger<JSONRepository<T>> _logger;

        public JSONRepository(ILogger<JSONRepository<T>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException();
            var basePath = AppContext.BaseDirectory;
            _filePath = Path.Combine(basePath, "Data", "Files");
        }


        public IEnumerable<T> ReadAll()
        {
            try
            {
                var fileData = ReadFile(typeof(T).Name);
                if (!string.IsNullOrWhiteSpace(fileData))
                {
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(fileData) ?? [];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to execute generic readall for {typeof(T).Name}! - {ex.Message}, {ex}");
            }

            return [];
        }

        public Task<IEnumerable<T>> ReadAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<T?> ReadAsync(string id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReadManyAsync(object parameters, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> ReadPageAsync(int page, int take, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<T?> ReadSingleAsync(object parameters, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        private string ReadFile(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(_filePath, $"{fileName}.json");
                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Read file {fileName}! - {ex.Message}, {ex}");
                return string.Empty;
            }
        }
    }
}
