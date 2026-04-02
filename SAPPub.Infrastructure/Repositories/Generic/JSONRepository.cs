using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.Core.Interfaces.Repositories.Generic;
using System.Text;

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


        public async Task<IEnumerable<T>> ReadAll()
        {
            try
            {
                var fileData = await ReadFileAsync(typeof(T).Name);
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

        public async Task<IEnumerable<T>> ReadPageAsync(int page, int take, CancellationToken ct = default)
        {
            var items = (await ReadAll()).Skip((page - 1) * take).Take(take);
            return items;
        }

        public Task<T?> ReadSingleAsync(object parameters, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        private Task<string> ReadFileAsync(string fileName)
        {
            try
            {
                var fullPath = Path.Combine(_filePath, $"{fileName}.json");
                return File.ReadAllTextAsync(fullPath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Read file {fileName}! - {ex.Message}, {ex}");
                return Task.FromResult(string.Empty);
            }
        }

        Task<bool> IGenericRepository<T>.UpdateAsync(object? updateObject, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<bool> IGenericRepository<T>.WriteAsync(object? writeObject, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
