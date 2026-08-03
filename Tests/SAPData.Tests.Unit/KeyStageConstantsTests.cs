using Xunit;

namespace SAPData.Unit.Tests;

public class KeyStageConstantsTests
{
    [Fact]
    public void AllKeyStages_Contains_KS2_KS4_KS5()
    {
        // Assert
        Assert.Equal(3, KeyStageConstants.AllKeyStages.Count);
        Assert.Contains("KS2", KeyStageConstants.AllKeyStages);
        Assert.Contains("KS4", KeyStageConstants.AllKeyStages);
        Assert.Contains("KS5", KeyStageConstants.AllKeyStages);
    }

    [Fact]
    public void AllKeyStages_Are_In_Correct_Order()
    {
        // Assert - Order should be KS2, KS4, KS5
        Assert.Equal("KS2", KeyStageConstants.AllKeyStages[0]);
        Assert.Equal("KS4", KeyStageConstants.AllKeyStages[1]);
        Assert.Equal("KS5", KeyStageConstants.AllKeyStages[2]);
    }

    [Fact]
    public void Individual_Constants_Match_Array_Values()
    {
        // Assert
        Assert.Equal(KeyStageConstants.KS2, KeyStageConstants.AllKeyStages[0]);
        Assert.Equal(KeyStageConstants.KS4, KeyStageConstants.AllKeyStages[1]);
        Assert.Equal(KeyStageConstants.KS5, KeyStageConstants.AllKeyStages[2]);
    }

    [Fact]
    public void AllKeyStages_Is_ReadOnly()
    {
        // Assert - Verify the list is read-only by checking it implements IReadOnlyList
        Assert.IsAssignableFrom<IReadOnlyList<string>>(KeyStageConstants.AllKeyStages);
    }
}
