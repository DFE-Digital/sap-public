using SAPPub.Core.Enums;
using SAPPub.Web.Areas.Profiles.Helpers;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Tests.Unit.Areas.Profiles.Helpers;

public class PupilCharacteristicRouteHelperTests
{
    public static IEnumerable<object[]> PupilCharacteristicRouteMappings =>
    [
        [PupilCharacteristic.AllPupils, RouteConstants.PrimaryPupilCharacteristicsAllPupils],
        [PupilCharacteristic.DisadvantagedPupils, RouteConstants.PrimaryPupilCharacteristicsDisadvantagedPupils],
        [PupilCharacteristic.PupilsWithEnglishAsAnAdditionalLanguage, RouteConstants.PrimaryPupilCharacteristicsEnglishAsAdditionalLanguage],
        [PupilCharacteristic.GirlsAndBoys, RouteConstants.PrimaryPupilCharacteristicsGirlsAndBoys],
        [PupilCharacteristic.PupilsWithDifferentStartingPoints, RouteConstants.PrimaryPupilCharacteristicsDifferentStartingPoints],
        [PupilCharacteristic.NonMobilePupils, RouteConstants.PrimaryPupilCharacteristicsNonMobilePupils],
    ];

    [Theory]
    [MemberData(nameof(PupilCharacteristicRouteMappings))]
    public void ToRouteName_ReturnsExpectedRoute(PupilCharacteristic characteristic, string expectedRouteName)
    {
        // Act
        var result = PupilCharacteristicRouteHelper.ToRouteName(characteristic);

        // Assert
        Assert.Equal(expectedRouteName, result);
    }

    [Fact]
    public void ToRouteName_ReturnsNull_ForUndefinedValue()
    {
        // Act
        var result = PupilCharacteristicRouteHelper.ToRouteName((PupilCharacteristic)999);

        // Assert
        Assert.Null(result);
    }
}
