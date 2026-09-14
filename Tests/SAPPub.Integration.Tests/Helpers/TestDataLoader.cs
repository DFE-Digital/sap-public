using System.Text.Json;

namespace SAPPub.Integration.Tests.Helpers;

public static class TestDataLoader
{
    public static List<List<JsonElement>> GetTestData(string folder, string fileName)
    {
        var testDataVersion =
            Environment.GetEnvironmentVariable("TEST_DATA_VERSION") ?? "2425";

        var testDataFilePath = Path.Combine(
            AppContext.BaseDirectory,
            $"TestData\\{testDataVersion}",
            $"{folder}\\{fileName}.json");

        if (!File.Exists(testDataFilePath))
        {
            throw new FileNotFoundException(
                $"Test data file not found: {testDataFilePath}");
        }

        var jsonData = File.ReadAllText(testDataFilePath);

        var testCases = JsonSerializer.Deserialize<List<List<JsonElement>>>(
            jsonData) ?? new List<List<JsonElement>>();

        return testCases;
    }
}
