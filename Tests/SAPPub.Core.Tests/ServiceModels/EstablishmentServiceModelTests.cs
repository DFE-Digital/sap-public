using SAPPub.Core.ServiceModels;

namespace SAPPub.Core.Tests.ServiceModels;

public class EstablishmentServiceModelTests
{
    [Theory]
    [InlineData("Has Nursery Classes", true)]
    [InlineData("No Nursery Classes", false)]
    [InlineData("Not Applicable", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void HasNurseryProvision_ReturnsExpected(string? nurseryProvisionName, bool expected)
    {
        var model = new EstablishmentServiceModel { NurseryProvisionName = nurseryProvisionName };

        Assert.Equal(expected, model.HasNurseryProvision);
    }
}