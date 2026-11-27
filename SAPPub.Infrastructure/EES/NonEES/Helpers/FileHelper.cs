using CsvHelper;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace SAPPub.Infrastructure.EES.NonEES.Helpers
{
    public interface IFileHelper
    {
        string GetFilePath(string fileName);
        IEnumerable<T> ReadCSV<T>();
    }

    public class FileHelper : IFileHelper
    {
        private readonly string _filePath;
        private readonly ILogger<FileHelper> _logger;

        public FileHelper(ILogger<FileHelper> logger)
        {
            _logger = logger ?? throw new ArgumentNullException();
            var basePath = AppContext.BaseDirectory;
            _filePath = Path.Combine(basePath, "Data", "CSV");
        }

        public string GetFilePath(string fileName)
        {
            var fullPath = Path.Combine(_filePath, fileName);
            if (!File.Exists(fullPath))
            {
                _logger.LogError("File not found: {FilePath}", fullPath);
                throw new FileNotFoundException($"File not found: {fullPath}");
            }
            return fullPath;
        }

        public IEnumerable<T> ReadCSV<T>()
        {
            var tName = typeof(T).Name;

            var fullFilePath = GetFilePath($"{tName}.csv");

            List<T> dataMaps;
            using (var reader = new StreamReader(fullFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                dataMaps = [.. csv.GetRecords<T>()];
            }
            return dataMaps;
        }
    }
}
