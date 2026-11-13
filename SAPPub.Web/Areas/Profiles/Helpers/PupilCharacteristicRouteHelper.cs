using SAPPub.Core.Enums;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Helpers;

public static class PupilCharacteristicRouteHelper
{
    public static string? ToRouteName(PupilCharacteristic characteristic) => characteristic switch
    {
        PupilCharacteristic.AllPupils => RouteConstants.PrimaryPupilCharacteristicsAllPupils,
        PupilCharacteristic.DisadvantagedPupils => RouteConstants.PrimaryPupilCharacteristicsDisadvantagedPupils,
        PupilCharacteristic.PupilsWithEnglishAsAnAdditionalLanguage => RouteConstants.PrimaryPupilCharacteristicsEnglishAsAdditionalLanguage,
        PupilCharacteristic.GirlsAndBoys => RouteConstants.PrimaryPupilCharacteristicsGirlsAndBoys,
        PupilCharacteristic.PupilsWithDifferentStartingPoints => RouteConstants.PrimaryPupilCharacteristicsDifferentStartingPoints,
        PupilCharacteristic.NonMobilePupils => RouteConstants.PrimaryPupilCharacteristicsNonMobilePupils,
        _ => null
    };
}
