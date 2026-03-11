using GeoUK;
using GeoUK.Coordinates;
using GeoUK.Ellipsoids;
using GeoUK.Projections;

namespace SAPPub.Web.Helpers
{
    public class MappingHelper
    {
        /// <summary>
        /// Converts British National Grid Easting/Northing to WGS84 Latitude/Longitude
        /// </summary>
        /// <param name="value">Tuple of Easting, and Northing</param>
        /// <returns>Tuple of Latitude, and Longitude</returns>
        public static LatitudeLongitude? ConvertToLatLong(string easting, string northing)
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
    }
}
