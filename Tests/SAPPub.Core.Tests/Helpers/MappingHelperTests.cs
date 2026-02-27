using SAPPub.Core.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace SAPPub.Core.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class MappingHelperTests
    {
        [Fact]
        public void ConvertEastNorthToLatLong()
        {
            // We're ultimately testing an external library, so just a quick test.
            // Arrange
            var easting = "530047";
            var northing = "179945";

            var longitudeExpected = -0.12769745130735563;
            var latitudeExpected = 51.503485756703007;
            // Act
            var result = MappingHelper.ConvertToLatLon(easting, northing);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(latitudeExpected, result.Latitude);
            Assert.Equal(longitudeExpected, result.Longitude);
        }

    }
}
