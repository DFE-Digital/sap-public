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
            "TestData",
            testDataVersion,
            folder,
            $"{fileName}.json");

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

    public static List<T> Load<T>(string folder, string fileName)
    {
        var testDataVersion =
            Environment.GetEnvironmentVariable("TEST_DATA_VERSION") ?? "2425";

        var path = Path.Combine(
            AppContext.BaseDirectory,
            "TestData",
            testDataVersion,
            folder,
            $"{fileName}.json");

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"Test data file not found: {path}");
        }

        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<List<T>>(
                   json,
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true,
                       PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
                   })
               ?? [];
    }

    public static TheoryData<T> LoadTheoryData<T>(
    string folder,
    string fileName)
    {
        var theoryData = new TheoryData<T>();

        foreach (var item in Load<T>(folder, fileName))
        {
            theoryData.Add(item);
        }

        return theoryData;
    }

    public static TheoryData<T1, T2> LoadTheoryData<T1, T2>(
        string folder,
        string fileName1,
        string fileName2)
    {
        var theoryData = new TheoryData<T1, T2>();

        var items1 = Load<T1>(folder, fileName1);
        var items2 = Load<T2>(folder, fileName2);

        for (int i = 0; i < Math.Min(items1.Count, items2.Count); i++)
        {
            theoryData.Add(items1[i], items2[i]);
        }

        return theoryData;
    }
}
