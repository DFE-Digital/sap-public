using Xunit;

namespace SAPData.Tests.Unit;

public class GenerateIndexesTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _sqlDir;
    private readonly string _sqlPath;
    private readonly string _sql;

    public GenerateIndexesTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        _sqlDir = Path.Combine(_tempDir, "Sql");
        Directory.CreateDirectory(_sqlDir);

        // Arrange once for all tests in this class
        new GenerateIndexes(_sqlDir).Run();

        _sqlPath = Path.Combine(_sqlDir, "04_indexes.sql");
        _sql = File.ReadAllText(_sqlPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    [Fact]
    public void Generates_indexes_sql_file()
    {
        Assert.True(File.Exists(_sqlPath));
    }

    [Fact]
    public void Writes_expected_header()
    {
        Assert.Contains("04_indexes.sql", _sql);
        Assert.Contains("Indexes for materialized views", _sql);
    }

    [Fact]
    public void Generates_index_for_each_expected_view()
    {
        Assert.Contains("ON public.v_england_destinations", _sql);
        Assert.Contains("ON public.v_england_performance", _sql);
        Assert.Contains("ON public.v_establishment", _sql);
        Assert.Contains("ON public.v_establishment_absence", _sql);
        Assert.Contains("ON public.v_establishment_destinations", _sql);
        Assert.Contains("ON public.v_establishment_performance", _sql);
        Assert.Contains("ON public.v_establishment_workforce", _sql);
        Assert.Contains("ON public.v_la_destinations", _sql);
        Assert.Contains("ON public.v_la_performance", _sql);
    }

    [Fact]
    public void Uses_urn_for_establishment_index()
    {
        Assert.Contains("CREATE INDEX IF NOT EXISTS idx_v_establishment_urn", _sql);
        Assert.Contains("ON public.v_establishment (\"URN\")", _sql);
    }

    [Fact]
    public void Uses_id_for_non_establishment_views()
    {
        Assert.Contains("CREATE INDEX IF NOT EXISTS idx_v_england_destinations_id", _sql);
        Assert.Contains("ON public.v_england_destinations (\"Id\")", _sql);

        Assert.Contains("CREATE INDEX IF NOT EXISTS idx_v_la_performance_id", _sql);
        Assert.Contains("ON public.v_la_performance (\"Id\")", _sql);
    }

    [Fact]
    public void Uses_if_not_exists_for_idempotency()
    {
        Assert.Contains("CREATE INDEX IF NOT EXISTS", _sql);
    }
}
