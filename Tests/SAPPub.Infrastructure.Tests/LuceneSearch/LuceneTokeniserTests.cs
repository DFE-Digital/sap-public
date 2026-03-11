using SAPPub.Infrastructure.LuceneSearch;

namespace SAPPub.Infrastructure.Tests.LuceneSearch;

public class LuceneTokeniserTests
{
    [Fact]
    public void Tokenise_ReturnsEmpty_ForNullOrWhitespace()
    {
        using var ctx = new LuceneIndexContext();
        var sut = new LuceneTokeniser(ctx);

        Assert.Empty(sut.Tokenise(null!));
        Assert.Empty(sut.Tokenise(""));
        Assert.Empty(sut.Tokenise("   "));
    }

    [Fact]
    public void Tokenise_ProducesLowercase()
    {
        using var ctx = new LuceneIndexContext();
        var sut = new LuceneTokeniser(ctx);

        var tokens = sut.Tokenise("PETER").ToList();

        Assert.Contains("peter", tokens);
    }

    [Fact]
    public void Tokenise_Removes_possessives()
    {
        using var ctx = new LuceneIndexContext();
        var sut = new LuceneTokeniser(ctx);

        var tokens = sut.Tokenise("Peter's").ToList();

        Assert.Contains("peter", tokens);
    }
}
