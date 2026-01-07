using SAPPub.Core.Entities;
using SAPPub.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPPub.Core.Tests.Helpers
{
    [ExcludeFromCodeCoverage]
    public class TextHelpersTests
    {
        [Fact]
        public void ConcatListToString_WorksOnNormalList()
        {
            // Arrange
            var addressList = new List<string>
            {
                "123 Main St",
                "Springfield",
                "IL",
                "62701"
            };

            var expected = "123 Main St, Springfield, IL, 62701";

            // Act
            var result = TextHelpers.ConcatListToString(addressList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatListToString_WorksOnNormalListWithEmpty()
        {
            // Arrange
            var addressList = new List<string>
            {
                "123 Main St",
                "",
                "IL",
                "62701"
            };

            var expected = "123 Main St, IL, 62701";

            // Act
            var result = TextHelpers.ConcatListToString(addressList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConcatListToString_WorksOnNormalListWithAllEmpty()
        {
            // Arrange
            var addressList = new List<string>
            {
                "",
                "",
                string.Empty,
                ""
            };

            var expected = "";

            // Act
            var result = TextHelpers.ConcatListToString(addressList);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("h3ll0","h3ll0")]
        [InlineData("St John's Academy o' fine arts", "St-Johns-Academy-o-fine-arts")]
        [InlineData("Smith School Robinson ++--{}[[];'#:@~,./<>?¬`!£$%^&*()", "Smith-School-Robinson")]
        public void CleanForUrl_RemovesNonAlphaChars(string input, string output)
        {
            // Arrange
            // Act
            var result = TextHelpers.CleanForUrl(input);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(output, result);
        }
    }
}
