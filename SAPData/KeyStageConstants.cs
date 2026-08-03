namespace SAPData;

/// <summary>
/// Central definition of supported key stages.
/// </summary>
public static class KeyStageConstants
{
    /// <summary>
    /// All supported key stages: KS2, KS4, KS5
    /// </summary>
    public static readonly IReadOnlyList<string> AllKeyStages = new[] { KS2, KS4, KS5 };

    public const string KS2 = "KS2";
    public const string KS4 = "KS4";
    public const string KS5 = "KS5";
}
