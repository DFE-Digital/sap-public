using FluentAssertions;
using Xunit;

namespace SAPData.Tests.Unit;

public class GenerateIndexesTests
{
    private readonly string _tempDir;
    private readonly string _sqlDir;

    public GenerateIndexesTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        _sqlDir = Path.Combine(_tempDir, "Sql");

        Directory.CreateDirectory(_sqlDir);
    }

    [Fact]
    public void Generates_indexes_sql_file()
    {
        new GenerateIndexes(_sqlDir).Run();

        var path = Path.Combine(_sqlDir, "04_indexes.sql");

        File.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void Writes_expected_header()
    {
        new GenerateIndexes(_sqlDir).Run();

        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_indexes.sql"));

        sql.Should().Contain("04_indexes.sql");
        sql.Should().Contain("Indexes for materialized views");
    }

    [Fact]
    public void Generates_index_for_each_expected_view()
    {
        new GenerateIndexes(_sqlDir).Run();

        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_indexes.sql"));

        sql.Should().Contain("ON public.v_england_destinations");
        sql.Should().Contain("ON public.v_england_performance");
        sql.Should().Contain("ON public.v_establishment");
        sql.Should().Contain("ON public.v_establishment_absence");
        sql.Should().Contain("ON public.v_establishment_destinations");
        sql.Should().Contain("ON public.v_establishment_performance");
        sql.Should().Contain("ON public.v_establishment_workforce");
        sql.Should().Contain("ON public.v_la_destinations");
        sql.Should().Contain("ON public.v_la_performance");
    }

    [Fact]
    public void Uses_urn_for_establishment_index()
    {
        new GenerateIndexes(_sqlDir).Run();

        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_indexes.sql"));

        sql.Should().Contain(
            "CREATE INDEX IF NOT EXISTS idx_v_establishment_urn");
        sql.Should().Contain(
            "ON public.v_establishment (\"URN\")");
    }

    [Fact]
    public void Uses_id_for_non_establishment_views()
    {
        new GenerateIndexes(_sqlDir).Run();

        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_indexes.sql"));

        sql.Should().Contain(
            "CREATE INDEX IF NOT EXISTS idx_v_england_destinations_id");
        sql.Should().Contain(
            "ON public.v_england_destinations (\"Id\")");

        sql.Should().Contain(
            "CREATE INDEX IF NOT EXISTS idx_v_la_performance_id");
        sql.Should().Contain(
            "ON public.v_la_performance (\"Id\")");
    }

    [Fact]
    public void Uses_if_not_exists_for_idempotency()
    {
        new GenerateIndexes(_sqlDir).Run();

        var sql = File.ReadAllText(Path.Combine(_sqlDir, "04_indexes.sql"));

        sql.Should().Contain("CREATE INDEX IF NOT EXISTS");
    }
}
