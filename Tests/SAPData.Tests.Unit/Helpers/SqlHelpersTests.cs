using System.Reflection;
using Xunit;

namespace SAPData.Unit.Tests.Helpers;

public class SqlHelpersTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
    private readonly string _path;

    public SqlHelpersTests()
    {
        Directory.CreateDirectory(_dir);
        _path = Path.Combine(_dir, "00_cleanup.sql");
    }

    public void Dispose()
    {
        if (Directory.Exists(_dir))
        {
            Directory.Delete(_dir, recursive: true);
        }
    }

    [Fact]
    public void SelectiveMode_DropsRebuiltTables_And_StaleTablesButKeepsCurrentOnes()
    {
        // Arrange/Act
        var sql = WriteCleanup(["t_gias_new"], ["t_gias_new", "t_manual_keep"], false );

        // Assert
        Assert.Contains("tables_to_rebuild text[] := ARRAY['t_gias_new'];", sql);
        Assert.Contains("tables_to_keep text[] := ARRAY['t_manual_keep'];", sql);
        Assert.Contains("AND tablename LIKE 't\\_%' ESCAPE '\\'", sql);
        Assert.Contains("AND (tablename = ANY(tables_to_rebuild) OR NOT (tablename = ANY(tables_to_keep)))", sql);
        Assert.Contains("DROP TABLE IF EXISTS %I.%I CASCADE", sql);
    }

    [Fact]
    public void RebuildAllMode_DropsAllTables()
    {
        // Arrange/Act
        var sql = WriteCleanup(["t_a", "t_b"], ["t_a", "t_b"], true);

        // Assert
        Assert.Contains("AND tablename LIKE 't\\_%' ESCAPE '\\'", sql);
        Assert.DoesNotContain("tables_to_keep)))", sql);
    }

    [Fact]
    public void EmptyListsProductsEmptyTextArrays()
    {
        // Arrange/Act
        var sql = WriteCleanup([], [], false);

        // Assert
        Assert.Contains("tables_to_rebuild text[] := ARRAY[]::text[];", sql);
        Assert.Contains("tables_to_keep text[] := ARRAY[]::text[];", sql);
    }

    private string WriteCleanup(string[] rebuilt, string[] known, bool rebuildAll)
    { 
        var type = Assembly.Load("SAPData").GetType("SAPData.Helpers.SqlHelpers", throwOnError: true);
        var method = type!.GetMethod("WriteCleanupSql", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);

        method!.Invoke(null, [_path, rebuilt, known, rebuildAll]);

        return File.ReadAllText(_path);
    }
}
