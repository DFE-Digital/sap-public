using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;

namespace SAPPub.Core.Helpers;

public class MappingHelper
{
    /// <summary>
    /// Converts British National Grid Easting/Northing to WGS84 Latitude/Longitude
    /// </summary>
    /// <param name="value">Tuple of Easting, and Northing</param>
    /// <returns>Tuple of Latitude, and Longitude</returns>
    public static LatitudeLongitude? ConvertToLatLon(string easting, string northing)
    {
        if (string.IsNullOrWhiteSpace(easting) || string.IsNullOrWhiteSpace(northing))
        {
            return null;
        }

        bool canParseEasting = double.TryParse(easting, out double Easting);
        bool canParseNorthing = double.TryParse(northing, out double Northing);

        if (!canParseEasting || !canParseNorthing)
        {
            return null;
        }

        Cartesian cartesian = GeoUK.Convert.ToCartesian(new Airy1830(),
            new BritishNationalGrid(),
            new EastingNorthing(Easting, Northing));

        Cartesian wgsCartesian = Transform.Osgb36ToEtrs89(cartesian);

        return GeoUK.Convert.ToLatitudeLongitude(new Wgs84(), wgsCartesian);
    }

    public static double? HaversineMiles(double lat1, double lon1, double? lat2, double? lon2)
    {
        if (lat2 is null || lon2 is null)
        {
            return null;
        }
        double ToRad(double d) => Math.PI * d / 180.0;
        const double Rmiles = 3958.7613;
        var dLat = ToRad(lat2.Value - lat1);
        var dLon = ToRad(lon2.Value - lon1);
        var a = Math.Pow(Math.Sin(dLat / 2), 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2.Value)) * Math.Pow(Math.Sin(dLon / 2), 2);
        var c = 2 * Math.Asin(Math.Sqrt(a));
        return Rmiles * c;
    }

    public static double MilesToDegrees(double miles)
    {
        const double metersPerMile = 1609.344;
        const double metersPerDegree = 111_320.0;   // 1 degree latitude in meters
        return miles * metersPerMile / metersPerDegree;
    }
}
